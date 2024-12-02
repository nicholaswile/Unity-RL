// Nicholas Wile

using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting.FullSerializer;

public class RaceAgent : Agent
{

    // Reference to checkpoint system (used for rewards)
    [SerializeField]
    private CheckpointSystem _checkpoint_system;
    
    // Start position
    [SerializeField]
    private Vector3 _start_position;
    private Quaternion _start_rotation;

    // Reference to the driver component of the agent (used to control movement)
    private DriverController _driver;


    private void Awake()
    {
        _start_position = transform.localPosition;
        _start_rotation = transform.localRotation;
        _driver = GetComponent<DriverController>();
    }
    

    private void Start()
    {
        _checkpoint_system.Hit_Correct_Checkpoint += Hit_Correct_Checkpoint;
        _checkpoint_system.Hit_Wrong_Checkpoint += Hit_Wrong_Checkpoint;
    }
    

    private void Hit_Correct_Checkpoint(object sender, HitCheckpointEventArgs e)
    {

        if (e.driver_transform == transform)
        {
            Debug.Log("[REWARD] Driver hit correct checkpoint");
            AddReward(+1.0f);
        }
    }


    private void Hit_Wrong_Checkpoint(object sender, HitCheckpointEventArgs e)
    {

        if (e.driver_transform == transform)
        {
            Debug.Log("[PUNISH] Driver hit incorrect checkpoint");
            AddReward(-1.0f);
        }
    }


    // Resets the episode to the start state to train the agent again with additional trials
    public override void OnEpisodeBegin()
    {

        // Stop driver
        _driver.Set_Velocity(0.0f);
     
        // Randomize the initial state for dynamic learning
        transform.localPosition = _start_position + new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-3.0f, +3.0f));
        transform.localRotation = _start_rotation;

        // Reset the checkpoints of this driver
        _checkpoint_system.Reset_Checkpoints(_driver);

    }

    
    // Observe environment and collect data for AI to make decision on actions
    public override void CollectObservations(VectorSensor sensor)
    {
        // The direction towards the next checkpoint
        Vector3 next_checkpoint = _checkpoint_system.Get_Next_Checkpoint(_driver).transform.forward;
        float dot = Vector3.Dot(transform.forward, next_checkpoint);

        // Goal is to teach the agent to face the same direction as the checkpoint
        sensor.AddObservation(dot);
    }


    // Agent performs an action
    public override void OnActionReceived(ActionBuffers actions)
    {

        // Teach agent to accelerate
        // Accelerate is a boolean button press, thus digital (discrete)
        bool accelerate = false;
        switch(actions.DiscreteActions[0]) 
        {
            case 0:     accelerate = false;      break; // Don't accelerate
            case 1:     accelerate = true;      break; // Accelerate
        }

        // Teach agent to brake
        // Braking is a boolean button press, thus digital (discrete)
        bool brake = false;
        switch(actions.DiscreteActions[1])
        {
            case 0:     brake = false;           break; // Don't brake
            case 1:     brake = true;           break; // Brake
        }

        // Teach agent to steer
        // Turning could be digital or analog, I choose analog to represent axis
        float turn = actions.ContinuousActions[0];

        // Let agent control driver
        _driver.Set_Inputs(accelerate, brake, turn);

    }


    // When behavior type is set to heuristic, enables user input for testing actions
    // Sets accelerate, brake, and turn actions according to player input and passes them to the OnActionReceived() function
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discrete_actions = actionsOut.DiscreteActions;
        ActionSegment<float> continuous_actions = actionsOut.ContinuousActions;

        // The Fire1 button in Unity is mapped to the left ctrl key, left mouse button, and the square button on my USB controller
        bool accelerate = Input.GetButton("Fire1"); 

        // The Fire2 button in Unity is mapped to the left alt key, right mouse button, and the x button on my USB controller
        bool brake = Input.GetButton("Fire2"); 

        // Horizontal and vertical axes are mapped to the WASD keys, analog sticks on USB controllers, etc.
        float turn_input = Input.GetAxis("Horizontal");

        discrete_actions[0] = (accelerate ? 1 : 0);
        discrete_actions[1] = (brake ? 1 : 0);
        continuous_actions[0] = turn_input;

    }


    // Rewards or penalizes agent depending on what it collides with in the scene
    // Then ends the trial and starts the episode again
    private void OnTriggerEnter(Collider other)
    {

        // If agent falls off the track, it collides with a trigger tagged 'wall', penalize
        if (other.TryGetComponent(out Wall _))
        {
            AddReward(-10.0f);
            Debug.Log("[LOSE] Agent fell off the track!");
            EndEpisode();
        }


    }


}
