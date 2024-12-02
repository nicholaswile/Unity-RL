using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    // Place on the target the camera should follow
    [SerializeField]
    private Transform _target;

    [SerializeField]
    private Vector3 _offset;

    private void Update()
    {
        transform.localPosition = _target.localPosition + _offset;
    }
}
