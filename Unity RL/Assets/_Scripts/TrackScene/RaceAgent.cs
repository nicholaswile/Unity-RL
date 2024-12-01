// Nicholas Wile

using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class RaceAgent : Agent
{


    // Reference to target (goal)
    [SerializeField]
    private Transform _coin_agent;

    // Agent speed
    [SerializeField]
    private float _move_speed = 3.0f;

    // UI displays agent accuracy
    private UI _ui;

    // Start position
    [SerializeField]
    private Vector3 _start_position;
    private Quaternion _start_rotation;


    private void Awake()
    {
        _ui = FindObjectOfType<UI>();
        _start_position = transform.localPosition;
        _start_rotation = transform.localRotation;
    }


    // Resets the episode to the start state to train the agent again with additional trials
    public override void OnEpisodeBegin()
    {

        // Randomize the initial state for dynamic learning
        // transform.localPosition = new Vector3(Random.Range(-3.0f, -1.0f), 1.5f, Random.Range(-3.0f, 3.0f));
        // _coin_agent.localPosition = new Vector3(Random.Range(1.0f, 3.0f), 1.5f, Random.Range(-3.0f, 3.0f));
        transform.localPosition = _start_position;
        transform.localRotation = _start_rotation;

    }

    
    // Observe environment and collect data for AI to make decision on actions
    public override void CollectObservations(VectorSensor sensor)
    {

        // Passing (x, y, z) position of player and target
        // 6 floats observed in total, 3 for player and 3 for target
        sensor.AddObservation(transform.localPosition); 
        sensor.AddObservation(_coin_agent.localPosition);

    }


    // Agent performs an action
    public override void OnActionReceived(ActionBuffers actions)
    {

        // Actions may be passed by the player using heuristics or inferenced by the agent based on observations
        float move_x = actions.ContinuousActions[0];
        float move_z = actions.ContinuousActions[1];

        // Face agent in direction it advances
        Vector3 move_direction = new Vector3(move_x, 0, move_z).normalized;
        if (move_direction != Vector3.zero)
        {
            Quaternion rotation_direction = Quaternion.LookRotation(move_direction);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation_direction, _move_speed * Time.deltaTime);
        }

        // Move agent to a new position
        transform.localPosition += _move_speed * Time.deltaTime * new Vector3(move_x, 0, move_z);

    }


    // When behavior type is set to heuristic, enables user input for testing actions
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuous_actions = actionsOut.ContinuousActions;

        // Sets the x and z actions according to player input and passes them to the OnActionReceived() function
        // Horizontal and vertical axes include the WASD keys, up/down/left/right arrows, analog sticks on USB controllers, etc.
        continuous_actions[0] = Input.GetAxisRaw("Horizontal");
        continuous_actions[1] = Input.GetAxisRaw("Vertical");
    }


    // Rewards or penalizes agent depending on what it collides with in the scene
    // Then ends the trial and starts the episode again
    // And changes color of floor to show a win or loss
    private void OnTriggerEnter(Collider other)
    {

        // If agent collides with wall, penalize
        if (other.TryGetComponent(out Wall _))
        {
            Lose(-1.0f, "Hit Wall");
            _ui.total_losses++;
        }

        // If agent collides with coin, reward
        if (other.TryGetComponent(out Coin _))
        {
            Win(+1.0f, "Got Coin");
            _ui.total_wins++;
        }

    }
    

    private void Win(float reward, string msg)
    {
        Debug.Log("[WIN] " + msg);

        AddReward(reward);

        // EndEpisode();
    }


    private void Lose(float reward, string msg)
    {
        Debug.Log("[LOSE] " + msg);

        SetReward(reward);

        EndEpisode();
    }
}
