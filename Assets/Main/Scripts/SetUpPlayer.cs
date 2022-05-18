using System.Collections;
using System.Collections.Generic;
using FishNet.Component.Animating;
using UnityEngine;

public class SetUpPlayer : MonoBehaviour
{
    public int Element;
    public GameObject[] Characters;
    public AnimatorLand animatorMaster;

    public void GetPlayerReady()
    {
        foreach (GameObject Character in Characters)
        {
            Character.SetActive(false);
        }
        Characters[Element].SetActive(true);

        GetAnimations();
    }

    private void GetAnimations()
    {
        Characters[Element].GetComponent<NetworkAnimator>().SetAnimator(Characters[Element].GetComponent<Animator>());
        animatorMaster.SetAnimator(Characters[Element].GetComponent<Animator>());
    }

    //private void Start()
    //{
    //    GetPlayerReady();
    //}
}
