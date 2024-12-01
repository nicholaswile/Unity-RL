using System;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSystem : MonoBehaviour
{

    [SerializeField]
    private List<Checkpoint> _checkpoints;

    [SerializeField]
    private List<DriverController> _drivers;

    // A list of int for each agent training on the track - they may be at different checkpoints
    private List<int> _next_checkpoint;

    // Events that the agent subscribes to
    public event EventHandler Hit_Correct_Checkpoint;
    public event EventHandler Hit_Wrong_Checkpoint;


    private void Awake()
    {

        // Gets all checkpoints in scene and adds them to list
        Checkpoint[] checkpoints = FindObjectsOfType<Checkpoint>();

        // Gets all driver agents in scene
        DriverController[] drivers = FindObjectsOfType<DriverController>();
        
        _checkpoints = new List<Checkpoint>();

        foreach (Checkpoint checkpoint in checkpoints)
        {
            _checkpoints.Add(checkpoint);
            checkpoint.Set_Checkpoint(this);
        }

        Debug.Log($"Found {_checkpoints.Count} checkpoints");

        _next_checkpoint = new List<int>();
        _drivers = new List<DriverController>();

        foreach (DriverController driver in drivers)
        {
            _drivers.Add(driver);
            _next_checkpoint.Add(0);
        }

        Debug.Log($"Found {_next_checkpoint.Count} drivers");

    }

    // Tells which checkpoint the vehicle passes thru
    public void Hit_Checkpoint(Checkpoint checkpoint, DriverController driver)
    {
        
        int next_index = _next_checkpoint[_drivers.IndexOf(driver)];

        int hit_index = _checkpoints.IndexOf(checkpoint);

        if (hit_index == next_index)
        {
            Debug.Log($"[CORRECT] Hit checkpoint #{hit_index}");

            _next_checkpoint[_drivers.IndexOf(driver)] = (next_index + 1) % _checkpoints.Count;

            Hit_Correct_Checkpoint?.Invoke(this, EventArgs.Empty);
        }

        else
        {
            Debug.Log($"[ERROR] Hit wrong checkpoint #{hit_index} (expected #{next_index})");

            Hit_Wrong_Checkpoint?.Invoke(this, EventArgs.Empty);
        }
    }
}
