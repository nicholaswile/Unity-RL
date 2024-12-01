using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSystem : MonoBehaviour
{

    [SerializeField]
    private List<Checkpoint> _checkpoints;

    private void Awake()
    {

        Checkpoint[] checkpoints = FindObjectsOfType<Checkpoint>();
        foreach (var checkpoint in checkpoints)
        {
            _checkpoints.Add(checkpoint);
        }

        Debug.Log($"Found {_checkpoints.Count} checkpoints");

    }
}
