// the player movement script
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovementGrappling : MonoBehaviour
{
    //all the variables
    [Header("Movement")] //speed 
    private float moveSpeed;//useless not needed
    public float walkSpeed; // how fast will you walk
    public float sprintSpeed;// how fast will you run
    public float swingSpeed;// how fast will you be while grappling 

    public float groundDrag;

    [Header("Jumping")] //jump
    public float jumpForce;//how high will you jump
    public float jumpCooldown; // when can you jump
    public float airMultiplier; // how fast are you travling while falling in the are 
    bool readyToJump; // when can you jump

    [Header("Crouching")]
    public float crouchSpeed; // how fast do you crouch 
    public float crouchYScale; // how low down will you crouch 
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space; // key to press when you want to jump
    public KeyCode sprintKey = KeyCode.LeftShift; // key to press when you want to sprint 
    public KeyCode crouchKey = KeyCode.LeftControl; // key to press when you want to crouch

    [Header("Ground Check")]
    public float playerHeight; // how tall is the player
    public LayerMask whatIsGround; // what is the ground 
    bool grounded; //are you on the ground

    [Header("Slope Handling")]
    public float maxSlopeAngle; // how big of a slope can you go down on 
    private RaycastHit slopeHit; // what will happen when you hit a slope
    private bool exitingSlope; // are you on a slope right now?

    [Header("Camera Effects")]
    public PlayerCam cam; //get the player camera
    public float grappleFov = 95f; // the field of view for the character 

    public Transform orientation; // the orientation of the camera 

    float horizontalInput; //forwards/backword movement  (W and S)
    float verticalInput; //left/right movement (A and D)

    Vector3 moveDirection; //which direction are you facing 

    Rigidbody rb;//rigidbody pulgin (for collision)

    public MovementState state; // are you walking, sprinting, etc
    public enum MovementState
    {
        // what type of movement state are you in
        freeze, //not moving
        grappling, //grappling
        swinging, //grappling
        walking, //walking
        sprinting, //sprinting
        crouching, //crouching
        air // are you on nothing (air)
    }

    public bool freeze; //if you are frozen nothing really matters 

    public bool activeGrapple; //are you grappling
    public bool swinging; //are you grappling

    private void Start() //when the game starts:
    {
        rb = GetComponent<Rigidbody>(); // get rigidbody pulgin (for collision)
        rb.freezeRotation = true; //freeze if on freeze mode

        readyToJump = true;//you are ready to jump

        startYScale = transform.localScale.y;//camera transformations
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround); 
        //if you are on the ground then set player hight

        //do the following functions
        MyInput(); 
        SpeedControl();
        StateHandler();

        // handle drag
        if (grounded && !activeGrapple)
            rb.drag = groundDrag; //physics for grappling
        else
            rb.drag = 0;

        //TextStuff();
    }

    private void FixedUpdate() //happens every tick
    {
        MovePlayer();//a function 
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal"); //move forward/backwords
        verticalInput = Input.GetAxisRaw("Vertical"); // move left/right

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded) //if the jump button is pressed then jump if you are on the ground and are ready to jump
        {
            readyToJump = false; // if this is true set ready to jump at false

            Jump(); // do the jump function

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // start crouch
        if (Input.GetKeyDown(crouchKey)) // if crouch has been press then:
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z); //set the hight of the player if crouched 
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse); //not used (feture will be added)
        }

        // stop crouch
        if (Input.GetKeyUp(crouchKey)) // if crouch has been pressed then:
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z); //set back to normal hight 
        }
    }

    private void StateHandler()
    {
        //set the mode you are in and set the speed of each of the states
        // Mode - Freeze
        if (freeze)
        {
            state = MovementState.freeze;
            moveSpeed = 0;
            rb.velocity = Vector3.zero;
        }

        // Mode - Grappling
        else if (activeGrapple)
        {
            state = MovementState.grappling;
            moveSpeed = sprintSpeed;
        }

        // Mode - Swinging
        else if (swinging)
        {
            state = MovementState.swinging;
            moveSpeed = swingSpeed;
        }

        // Mode - Crouching
        else if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }

        // Mode - Sprinting
        else if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }

        // Mode - Walking
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }

        // Mode - Air
        else
        {
            state = MovementState.air;
        }
    }

    private void MovePlayer() // move the player at a speed 
    {
        if (activeGrapple) return; // if you are grappling do not do this code
        if (swinging) return; // if you are grappling do not do this code

        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //the movement speed if you are on a slope, on ground or in air 
        // on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        else if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        // turn gravity off while on slope
        rb.useGravity = !OnSlope();
    }

    private void SpeedControl() 
    //speed control (this code makes it so the person doesn't get to much speed (it could break the game))
    {
        if (activeGrapple) return; // if you are grappling do not do this code

        // limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump() // jump physics 
    {
        exitingSlope = true; // you are not on a slope

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); //jump speed

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse); //allows the camera to move up 
    }
    private void ResetJump() //you can now jump
    {
        readyToJump = true;//you can jump

        exitingSlope = false;//you are not on a slope
    }

    private bool enableMovementOnNextTouch; // lets you move normaly after done jump
    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight) // trajectory for when you try to grapple etc while jumping 
    {
        activeGrapple = true; // you are grappling 

        // trajectory for grappling while moving 
        velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight); 
        Invoke(nameof(SetVelocity), 0.1f);

        Invoke(nameof(ResetRestrictions), 3f);
    }

    private Vector3 velocityToSet;
    private void SetVelocity()
    {
        enableMovementOnNextTouch = true;
        rb.velocity = velocityToSet;

        cam.DoFov(grappleFov); //not used
    }

    public void ResetRestrictions()
    {
        activeGrapple = false;
        cam.DoFov(85f); //not used
    }

    private void OnCollisionEnter(Collision collision) //what will happen when you collide into something
    {
        if (enableMovementOnNextTouch)
        {
            enableMovementOnNextTouch = false; //you can not move through the wall 
            ResetRestrictions();
        }
    }

    private bool OnSlope() //what to do when you hit a slope 
    {
        // trajectory for slope
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection() //where will you go when you hit a slope
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight) //jump trajectory 
    {
        // all the jump math calulations for gravity and phsics (I would try to explain but I dont have a physics degree)
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity) 
            + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }

    #region Text & Debugging

    #endregion
    //end of the code
}
