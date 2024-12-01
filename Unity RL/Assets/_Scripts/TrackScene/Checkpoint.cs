using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    private CheckpointSystem _checkpoint_system;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called");
        if (other.TryGetComponent<DriverController>(out DriverController driver))
        {
            Debug.Log("Hit checkpoint");

            // Notify the checkpoint system class that this checkpoint has been hit
            _checkpoint_system.Hit_Checkpoint(this, driver);
        }
    }

    // Checkpoint system class notifies this checkpoint that it has been added to the list of checkpoints
    public void Set_Checkpoint(CheckpointSystem checkpoint_system)
    {
        _checkpoint_system = checkpoint_system;
    }
    
}
