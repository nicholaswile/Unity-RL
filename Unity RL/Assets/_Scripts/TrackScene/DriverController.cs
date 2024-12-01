using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverController : MonoBehaviour
{
    // Speed variables
    [SerializeField]
    private float _max_velocity = 20.0f;
    [SerializeField]
    private float _acceleration = 5.0f;
    [SerializeField]
    private float _braking = 10.0f;

    // Turning variables
    [SerializeField]
    private float _turn_speed = 45.0f;

    private float _current_velocity = 0.0f;

    // Reference to rigid body physics object on agent in simulation
    private Rigidbody _rb;

    // UI displays agent velocity
    private UI _ui;

    public void Set_Velocity(float velocity)
    {
        _current_velocity = velocity;
    }

    private void Awake()
    {
        _ui = FindObjectOfType<UI>();
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Get input from the player
        bool accelerate = Input.GetButton("Fire1"); 
        bool brake = Input.GetButton("Fire2"); 
        float turn_input = Input.GetAxis("Horizontal");

        // Apply acceleration or braking
        if (accelerate)
        {
            // Kinematic equation: final velocity = initial velocity + acceleration * time
            _current_velocity += _acceleration * Time.deltaTime;

            // Vehicle has reached its top speed
            _current_velocity = Mathf.Min(_current_velocity, _max_velocity);
        }

        else if (brake)
        {
            _current_velocity -= _braking * Time.deltaTime;

            // Vehicle has slowed to a stop (no reversing)
            _current_velocity = Mathf.Max(_current_velocity, 0);
        }

        else
        {
            // Vehicle slows down when not accelerating
            if (_current_velocity > 0)
            {
                _current_velocity -= _braking * Time.deltaTime / 2; 
                _current_velocity = Mathf.Max(_current_velocity, 0);
            }

            else if (_current_velocity < 0)
            {
                _current_velocity += _braking * Time.deltaTime / 2;
                _current_velocity = Mathf.Min(_current_velocity, 0);
            }
        }

        // Translate vehicle
        Vector3 movement = transform.forward * _current_velocity;
        _rb.velocity = new Vector3(movement.x, _rb.velocity.y, movement.z);

        // Turn vehicle
        float turn_amount = turn_input * _turn_speed * Time.deltaTime;
        Quaternion turn = Quaternion.Euler(0, turn_amount, 0);
        _rb.MoveRotation(_rb.rotation * turn);

        _ui.velocity = _current_velocity;
    }
}