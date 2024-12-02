using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    // Place on the target the camera should follow
    [SerializeField]
    private Transform _target;

    [SerializeField]
    private List<Transform> _locations;
    private Transform _camera_location;

    [SerializeField]
    private float _smoothing = 0.5f;

    private int _location_index = 0;

    private void Awake()
    {
        _camera_location = _locations[0];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _location_index = (_location_index + 1)%_locations.Count;
        }

        _camera_location = _locations[_location_index];

    }

    private void FixedUpdate()
    {
        Vector3 change = _camera_location.position * (1-_smoothing) + transform.position * _smoothing;

        transform.position = new Vector3(change.x, transform.position.y, change.z);

        transform.LookAt(_target);



    }
}
