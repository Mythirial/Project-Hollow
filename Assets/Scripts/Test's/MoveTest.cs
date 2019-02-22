﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Test Player script Will be rewritten after test purpose is completed
/// 
/// Bug: Jumping & Dashing doesn't work
/// jumping only worky with default layer and but grounded function is always returns true then
/// 
/// </summary>
public class MoveTest : MonoBehaviour
{
    //private Animator animator;

    [SerializeField]
    private float moveSpeed = 7.5f;
    [SerializeField]
    private float turnSpeed = 15f;
    [SerializeField]
    private float jumpHeight = 6f;
    [SerializeField]
    private float groundDistance = 0.5f;
    [SerializeField]
    private float dashDistance = 5f;
    [SerializeField]
    private float fallMultiplier = 2.5f;

    public LayerMask Ground;

    private Rigidbody _body;
    private Vector3 _inputs = Vector3.zero;
    private bool _isGrounded = true;
    private Transform _groundChecker;

    private void Awake()
    {
        //animator = GetComponentInChildren<Animator>();
        _body = GetComponent<Rigidbody>();
        _groundChecker = transform;
    }

    private void Update()
    {
        MOVE();
    }

    private void FixedUpdate()
    {
        MOVEPhysics();
    }

    /// <summary>
    /// MOVE (Subject to change)
    /// Basis of the movement system
    /// </summary>
    private void MOVE()
    {
        //animator.SetFloat("Speed", movement.magnitude);

        _isGrounded = Physics.CheckSphere(_groundChecker.position, groundDistance, Ground, QueryTriggerInteraction.Ignore);

        _inputs = Vector3.zero;
        _inputs.x = Input.GetAxis("Horizontal");
        _inputs.z = Input.GetAxis("Vertical");

        if (_inputs != Vector3.zero)
            transform.forward = _inputs;

        var movement = new Vector3(_inputs.x, 0, _inputs.z);

        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _body.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
        }

        if (Input.GetButtonDown("Dash"))
        {
            Vector3 dashVelocity = Vector3.Scale(transform.forward, dashDistance * new Vector3((Mathf.Log(1f / ((Time.deltaTime * _body.drag) + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * _body.drag + 1)) / -Time.deltaTime)));
            _body.AddForce(dashVelocity, ForceMode.VelocityChange);
        }

        if (movement.magnitude > 0)
        {
            Quaternion newDirection = Quaternion.LookRotation(movement);

            transform.rotation = Quaternion.Slerp(transform.rotation, newDirection, Time.deltaTime * turnSpeed);
        }
    }
    /// <summary>
    /// MOVE physics function
    /// </summary>
    void MOVEPhysics()
    {
        _body.MovePosition(_body.position + _inputs * moveSpeed * Time.fixedDeltaTime);
        _body.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
    }
}
