using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Managing.Scened;

public class PrintObjectNetworkId : MonoBehaviour
{
    NetworkObject LocalObject;

    // Update is called once per frame
    void Update()
    {
        LocalObject = GetComponent<NetworkObject>();
        Debug.Log(" Object: " + LocalObject.ObjectId + " Prefab: " + LocalObject.PrefabId);
    }
}
