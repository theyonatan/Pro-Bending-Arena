using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FishNet.Object;
public class MovingPlayer : NetworkBehaviour
{
    public bool SinglePlayer;

    public float Speed;

    public Transform Target;
    private Vector3 PlayerMovement;
    public Vector2 InputMovement;

    CharacterController CharacterController;

    private Vector3 GravityMovement;
    [SerializeField] private GravityFalls gravityfalls;

    [SerializeField] private InputDirector Director;

    // Start is called before the first frame update
    void Start()
    {
        CharacterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!base.IsOwner && !SinglePlayer)
            return;
        if (Director.enabled) //So Online Players Don't OverRide.
        {
            //GravityMovement = gravityfalls.GravityMovement; //Get Gravitation Change.
            Movement(); //Calculate The Movement.

            //Debug.Log(GravityMovement); //Check Gravity
            CharacterController.Move(PlayerMovement); // + GravityMovement); //Move the player
        }
    }

    private void Movement() //Calculate Movement.
    {
        PlayerMovement = Target.transform.forward * (Speed * InputMovement.y) * Time.deltaTime;
        PlayerMovement += Target.transform.right * (Speed * InputMovement.x) * Time.deltaTime;

    }

    public void GetInputMovement(Vector2 inputMovement) //Move Input From InputDirector To Here.
    {
        InputMovement = inputMovement;
    }
}
