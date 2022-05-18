using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FishNet.Object;

public class InputDirector : NetworkBehaviour
{
    public bool SinglePlayer;

    [Header("Director")]
    //Inputs
    public InputMaster Director;

    //PlayerMovement
    [Header("Player Movement")]
    [SerializeField] private MovingPlayer PlayerMovement;
    private Vector2 MovementStrength = new Vector2(0, 0);
    //MouseLook
    [Header("Camera")]
    [SerializeField] private CameraController CamController;
    Vector2 mouseInput;

    [SerializeField] AnimatorLand CharacterAnimator;

    //Gravity
    [Header("Gravity")]
    [SerializeField] GravityFalls Gravityfalls;

    private void Awake()
    {
        Director = new InputMaster();
            
        //Shoot;
        Director.Player.Shoot.performed += _ => Shoot();

        //MoveMent;
        Director.Player.Movement.started += x => MovementStrength = x.ReadValue<Vector2>();
        Director.Player.Movement.performed += x => MovementStrength = x.ReadValue<Vector2>();
        Director.Player.Movement.canceled += x => MovementStrength = x.ReadValue<Vector2>();

        //Mouse;
        Director.Player.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        Director.Player.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();

        //Jump
        Director.Player.Jumping.performed += ctx => Gravityfalls.Jump();

        Debug.Log("Done Orginizing Controls");
    }


    private void Update()
    {
        if (!base.IsOwner && !SinglePlayer)
            return;

        PlayerMovement.GetInputMovement(MovementStrength);
        CamController.GetMouseInput(mouseInput);
        Debug.Log(mouseInput.x + " " + mouseInput.y);

        CharacterAnimator.GetState(MovementStrength.y, MovementStrength.x);
    }

    private void Shoot()
    {
        Debug.Log("Damn they shot");
    }

    

    private void OnEnable()
    {
        Director.Enable();
    }
    private void OnDisable()
    {
        Director.Disable();
    }

}
