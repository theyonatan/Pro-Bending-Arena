using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class GravityFalls : NetworkBehaviour
{
    [Header("Main Stuff")]
    [SerializeField] InputDirector Director;
    [SerializeField] CharacterController CC;
    [SerializeField] AnimatorLand ZLandAnYMator;

    public bool SinglePlayer;

    [Header("Gravity")]
    public float Gravity;

    public float BaseGravity;
    public float JumpingGravity;

    [Header("Jumping")]
    public float JumpHeight;

    public bool JumpingTriggered;
    public bool FallingTriggered;


    [Header("GroundCheck")]
    public bool isGrounded;
    public Transform groundCheck;
    public float groundDistance;
    public LayerMask groundMask;

    public Vector3 Velocity;


    public bool IsGrounded()
    {

        //isGrounded = SphereCheck.IsTouchingGrounded;
        //TouchingObject = SphereCheck.otherObject;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        return isGrounded; // This checks if the Player is hitting the ground through The Character Controller Default Ground Check. I Don't Want That so I made My Own GroundCheck() Using A Sphere RayCast.;

        //isGrounded = Physics.CheckSphere(groundCheck.position + OffSet, groundDistance, groundMask);
        
        //Debug.Log(groundCheck.position + " - " + LayerMask.LayerToName(groundMask));
        //return isGrounded;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(groundCheck.position + OffSet, groundDistance);
    //}

    //public bool IsFalling()
    //{
    //    if (fallingSpeed < fallingThreashold)
    //    {
    //        return true;
    //    }

    //    return false;
    //}


    //private void CalculateGravity()
    //{
    //    if (IsGrounded() && !JumpingTriggered)
    //    {
    //        CurrentGravity = 0;
    //    }
    //    else
    //    {
    //        if (CurrentGravity > MaxGravity)
    //        {
    //            CurrentGravity -= Gravity * Time.deltaTime;
    //        }
    //    }

    //    GravityMovement = GravityDirection * -CurrentGravity * Time.deltaTime;
    //}

    //public void CalculateFalling()
    //{
    //    fallingSpeed = transform.InverseTransformDirection(CC.velocity).y; // Get The Falling Speed.;

    //    if (IsFalling() && !IsGrounded() && !JumpingTriggered && !FallingTriggered && fallingSpeed > 0.35f) //If the player just walks off ; . 
    //    {
    //        FallingTriggered = true;
    //        ZLandAnYMator.TriggerFall();
    //    }

    //    if (FallingTriggered && IsGrounded() && fallingSpeed < -0.5f )
    //    {
    //        FallingTriggered = false;
    //        JumpingTriggered = false;
    //        ZLandAnYMator.TriggerLanding();
    //        //transform.position = new Vector3(0f, TouchingObject.transform.position.y, 0f);
    //    }
    //}

    //public void ApplyJumpForce()
    //{
    //    CurrentGravity = JumpingForce;
    //}
    
    void StayOnGround()
    {
        if (isGrounded && Velocity.y < 0)
        {
            Velocity.y = -2f;
        }
    }
    public void Jump()
    {
        if (isGrounded)
        {
            JumpingTriggered = true;
            ZLandAnYMator.TriggerJump();
            Velocity.y = Mathf.Sqrt(JumpHeight * -2f * Gravity);
        }
    }
    private void Update()
    {
        if (!base.IsOwner && !SinglePlayer)
            return;

        if (Director.enabled)
        {
            IsGrounded();
            StayOnGround();

            Velocity.y += Gravity * Time.deltaTime;
            CC.Move(Velocity * Time.deltaTime);
            //CalculateGravity();
            //CalculateFalling();
            if (isGrounded && FallingTriggered)
            {
                FallingTriggered = false;
                JumpingTriggered = false;
                ZLandAnYMator.TriggerLanding();
            }

            if (!isGrounded && Velocity.y >= 0)
            {
                TriggerFalling();
            }

            if (isGrounded)
            {
                Gravity = BaseGravity;
            }
        }

    }

    void TriggerFalling()
    {
        FallingTriggered = true;
        Gravity = JumpingGravity;
    }
}


