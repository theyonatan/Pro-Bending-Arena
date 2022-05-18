using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Object;

public class LobbyManager : NetworkBehaviour
{
    public GameObject Button;

    public GameObject _Leader;
    public GameObject Instantiated_Leader;
    // Start is called before the first frame update
    private void Start()
    {
        if (!IsServer)
        {
            Button.SetActive(false);
            return;
        }
        GameObject go = Instantiate(_Leader);
        Spawn(go);

        Instantiated_Leader = go;
    }
    public void OnStartedGame()
    {
        Instantiated_Leader.GetComponent<NetworkLeader>().LoadGameScene();
    }

    //public void PrintNewPlayer(PlayerData data)
    //{
    //    //print PlayerData;
    //    Debug.Log("Ok Printed Player Data!");
    //}
}
