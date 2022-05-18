using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Object;

public class MainSceneManager : NetworkBehaviour
{
    public GameObject _Leader;
    // Start is called before the first frame update
    private void Start()
    {
        if (!IsServer)
            return;
        GameObject go = Instantiate(_Leader);
        Spawn(go);
    }
}
