﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float speedX, speedY;
    public AnimationCurve movementBehaviour;

    private float curveParser;

    private float inputX, inputY;

    private Vector3 velocity;

    private SpecialMovement previousSpecialMovement;
    public SpecialMovement currentSpecialMovement;


    public SpecialMovement inputBAction;
    public SpecialMovement inputAAction;
    public SpecialMovement inputXAction;

    private bool controlling = true;

    // special movement variables
    private Vector3 specialMovementInitialVec;

    //states 
    public bool grounded;

    //components
    private Rigidbody rb;
    private Collider colliderComponent;

    private Vector3 extraMovement;
    // Use this for initialization
    void Start() {
        rb = GetComponent<Rigidbody>();
        colliderComponent = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentSpecialMovement == null)
        {
            if (grounded)
            {
                GetAxis();
                ProcessInputs();
                GetVelocity();
            }
            Move();
        }
        else
        {
            currentSpecialMovement.UpdateStates(Time.deltaTime * currentSpecialMovement.timeScaler);
           
            ProcessVelocitySpecialMovement();
            FinishMovement();
           
            //ProcessInputs.
        }
        CheckGrounded();
        GetInputsSPecialMov();


    }

    void CheckGrounded()
    {
        RaycastHit hit;
        grounded = Physics.Raycast( transform.position - new Vector3(0, colliderComponent.bounds.extents.y*0.8f, 0), Vector3.down, out hit, 0.1f);

        Debug.Log(grounded);

    }

    void FinishMovement()
    {
        if ( currentSpecialMovement.GetElapsedTime() >1)
        {
            previousSpecialMovement = currentSpecialMovement;
            currentSpecialMovement = null;
        }
    }

    void ProcessInputsSpecialMovement()
    {
        //if
    }

    void ProcessVelocitySpecialMovement()
    {
        
        velocity = specialMovementInitialVec * currentSpecialMovement.movementBehasviour.Evaluate(currentSpecialMovement.GetElapsedTime()) * currentSpecialMovement.speedModifier;
        
        if (currentSpecialMovement.allowSteering)
        {
            GetAxis();

            velocity += (Quaternion.AngleAxis(-45, Vector3.up) * Vector3.right) * inputX;
            velocity += (Quaternion.AngleAxis(-45, Vector3.up) * Vector3.forward) * inputY;
           
        }
        Vector3 extraMovementResultant = Vector3.zero;
        if (currentSpecialMovement.applyHurricane)
        {
            extraMovement = Quaternion.AngleAxis(currentSpecialMovement.steeringAngle, Vector3.up) * extraMovement;
            extraMovementResultant = extraMovement;
            if (Vector3.Dot(extraMovement, velocity) > 0)
            {
                extraMovementResultant *= (1 + Vector3.Dot(extraMovement, velocity)) * 1.4f;
            }


            velocity = Quaternion.AngleAxis(UnityEngine.Random.Range(-10, 10), Vector3.up) * velocity;
            velocity += extraMovementResultant ;
        }

        velocity.y = rb.velocity.y;
        rb.velocity = velocity;
    }

    private void GetInputsSPecialMov()
    {
        SpecialMovement queuedMov = null;
        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
            queuedMov = inputBAction;

        if (Input.GetKeyDown(KeyCode.Joystick1Button0))
            queuedMov = inputAAction;
        
        if (Input.GetKeyDown(KeyCode.Joystick1Button2))
            queuedMov = inputXAction;

        if (queuedMov != null)
            if (currentSpecialMovement == null)
                LoadSpecialMovement(queuedMov);
    }

    void GetAxis()
    {
        if (!controlling)
            return;
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");
    }

    void GetVelocity()
    {
        velocity = new Vector3(0, 0, 0);
        
        float curve = movementBehaviour.Evaluate(curveParser);
        velocity +=  (Quaternion.AngleAxis(-45, Vector3.up) * Vector3.right) * speedX * inputX;
        velocity +=  (Quaternion.AngleAxis(-45, Vector3.up) * Vector3.forward  ) * inputY * speedY;
        velocity.Normalize();
        velocity *= curve;
       // velocity.y = rb.velocity.y;

    }

    void ProcessInputs()
    {
        float inputGatherer = (Mathf.Abs(inputX) + Mathf.Abs(inputY)) / 
             (2*( Mathf.Abs(inputX) + Mathf.Abs(inputY) ) + (0.01f ));
        

        if (Mathf.Abs(inputX )> 0.3f || Mathf.Abs(inputY) > 0.3f)
            curveParser += inputGatherer * Time.deltaTime ;
        if (curveParser > 1) curveParser -= 1;
    }

    private void Move()
    {
        //float velY = rb.velocity.y;
        velocity.y = rb.velocity.y;
        rb.velocity = velocity;
        
    }


    private void OnGUI()
    {
        Debug.DrawRay(transform.position, rb.velocity);
    }

    public void LoadSpecialMovement(SpecialMovement movement)
    {
        if (movement.velocityBiggerThan < rb.velocity.magnitude)
        {
            previousSpecialMovement = currentSpecialMovement;
            currentSpecialMovement = movement;
            currentSpecialMovement.ResetTime();

            velocity.y = currentSpecialMovement.jumpVelocity;
            rb.velocity = velocity;
            extraMovement = velocity.normalized;
            specialMovementInitialVec = velocity.normalized;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void OnCollisionExit(Collision collision)
    {
        
    }


}