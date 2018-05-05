using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour {

    public enum PlayerID { P1,P2,P3};
    public Enums.Players playerID;
   
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
    [HideInInspector]
    public bool grounded;
    private bool stunned;

    //components
    private Rigidbody rb;
    private Collider colliderComponent;

    private Vector3 extraMovement;

    private float movementCurrentElapsedTime;

    public UnityEvent finishMovCB;
    // Use this for initialization
    void Start() {
        rb = GetComponent<Rigidbody>();
        colliderComponent = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!stunned)
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
                UpdateStates(Time.deltaTime * currentSpecialMovement.timeScaler);

                ProcessVelocitySpecialMovement();

                FinishMovement();

                //ProcessInputs.
            }
            GetInputsSPecialMov();


        }
        CheckGrounded();


    }

    private void UpdateStates(float v)
    {
        movementCurrentElapsedTime += v;
    }

    private bool controlLock = false;
    public bool GetControlLock()
    {
        controlLock = false;
        for (int i = 0; i < currentSpecialMovement.controlIntervals.Length; i++)
        {
            if (movementCurrentElapsedTime > currentSpecialMovement.controlIntervals[i].start && movementCurrentElapsedTime < currentSpecialMovement.controlIntervals[i].end)
                controlLock = true;
        }
        return controlLock;
    }

    void UpdateStates()
    {
        movementCurrentElapsedTime = Time.deltaTime* currentSpecialMovement.timeScaler;
    }

    void CheckGrounded()
    {

        RaycastHit hit;
        grounded = false;
       // if (rb.velocity.y <=0)
        grounded = Physics.Raycast( transform.position - new Vector3(0, colliderComponent.bounds.extents.y*0.8f, 0), Vector3.down, out hit, 0.1f);
        if (grounded)
        {
            if (stunned)
            {
                stunned = false;

            }
        }
    }

    void FinishMovement()
    {
        if ( GetElapsedTime() >1)
        {
            previousSpecialMovement = currentSpecialMovement;
            FunkManager.S_INSTANCE.ModifyPoints(currentSpecialMovement.pointsOnSuccess);
            FunkManager.CompleteAction(currentSpecialMovement.action, playerID);
            currentSpecialMovement = null;
            finishMovCB.Invoke();

        }
    }

    void ResetVariables()
    {
        previousSpecialMovement = currentSpecialMovement;
        currentSpecialMovement = null;


    }

    void ProcessInputsSpecialMovement()
    {
        //if
    }

    float GetElapsedTime()
    {
        return movementCurrentElapsedTime;
    }

    void ProcessVelocitySpecialMovement()
    {
        
        velocity = specialMovementInitialVec * currentSpecialMovement.movementBehasviour.Evaluate(GetElapsedTime()) * currentSpecialMovement.speedModifier;
        
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
        if (playerID == Enums.Players.Player1)
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button1))
                queuedMov = inputBAction;

            if (Input.GetKeyDown(KeyCode.Joystick1Button0))
                queuedMov = inputAAction;

            if (Input.GetKeyDown(KeyCode.Joystick1Button2))
                queuedMov = inputXAction;
        }
        if (playerID == Enums.Players.Player2)
        {

            if (Input.GetKeyDown(KeyCode.Joystick2Button1))
                queuedMov = inputBAction;

            if (Input.GetKeyDown(KeyCode.Joystick2Button0))
                queuedMov = inputAAction;

            if (Input.GetKeyDown(KeyCode.Joystick2Button2))
                queuedMov = inputXAction;
        }
        if (playerID == Enums.Players.Player3)
        {

            if (Input.GetKeyDown(KeyCode.Joystick3Button1))
                queuedMov = inputBAction;

            if (Input.GetKeyDown(KeyCode.Joystick3Button0))
                queuedMov = inputAAction;

            if (Input.GetKeyDown(KeyCode.Joystick3Button2))
                queuedMov = inputXAction;
        }

        if (queuedMov != null)
            if (currentSpecialMovement == null)
                LoadSpecialMovement(queuedMov);
    }

    void GetAxis()
    {
        if (!controlling)
            return;
        if (playerID == Enums.Players.Player1)
        {
            inputX = Input.GetAxis("Horizontal");
            inputY = Input.GetAxis("Vertical");
        }
        if (playerID == Enums.Players.Player2)
        {

            inputX = Input.GetAxis("Horizontal2");
            inputY = Input.GetAxis("Vertical2");
        }
        if (playerID == Enums.Players.Player3)
        {

            inputX = Input.GetAxis("Horizontal3");
            inputY = Input.GetAxis("Vertical3");
        }
    }

    void GetVelocity()
    {
        velocity = new Vector3(0, 0, 0);
        
        float curve = movementBehaviour.Evaluate(curveParser);
        velocity +=  (Quaternion.AngleAxis(-45, Vector3.up) * Vector3.right)  * inputX;
        velocity +=  (Quaternion.AngleAxis(-45, Vector3.up) * Vector3.forward  )  * inputY;
        velocity.Normalize();
        velocity.x *= speedX;
        velocity.z *= speedY;
        velocity *= curve;

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
            ResetTime();

            velocity.y = currentSpecialMovement.jumpVelocity;
            rb.velocity = velocity;
            extraMovement = velocity.normalized;
            specialMovementInitialVec = velocity.normalized;
        }
    }

    void ResetTime()
    {
        movementCurrentElapsedTime = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Player")
        {

            Debug.Log("Lose Point");
            ResetVariables();
            stunned = true;
            Vector3 vel;
            grounded = false;
            velocity = Vector3.zero;
            extraMovement = Vector3.zero;
            vel = (  transform.position - collision.gameObject.transform.position).normalized * 2;
            vel.y = 2;
            rb.velocity = vel;

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        
    }


}
