using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class AnimatorLand : NetworkBehaviour
{
    public bool SinglePlayer;

    [SerializeField] GravityFalls Falls;
    [SerializeField] InputDirector Director;
    [SerializeField] Animator Any;
    private float WalkVertical;
    private float WalkHoritzontal;

    private void Update()
    {
        if (!base.IsOwner && !SinglePlayer)
            return;
        //Debug.Log(WalkHoritzontal + " " + WalkVertical);
        if (Director.enabled)
        {
            //if (Falls.IsFalling())
            //{
            //    Any.SetFloat("WalkVertical", 0);
            //    Any.SetFloat("WalkHorizontal", 0);
            //}
            //else
            //{
            //    Any.SetFloat("WalkVertical", WalkHoritzontal);
            //    Any.SetFloat("WalkHorizontal", WalkVertical);
            //}
            Any.SetFloat("WalkVertical", WalkHoritzontal);
            Any.SetFloat("WalkHorizontal", WalkVertical);
        }
    }

    public void GetState(float Vertical, float Horizontal)
    {
        WalkVertical = Vertical;
        WalkHoritzontal = Horizontal;
    }

    public void TriggerJump()
    {
        Any.SetTrigger("Jump");
    }

    public void TriggerFall()
    {
        Any.SetTrigger("Falling");
    }

    public void TriggerLanding()
    {
        Any.SetTrigger("Land");
    }

    public void SetAnimator(Animator ani)
    {
        Any = ani;
    }




}


