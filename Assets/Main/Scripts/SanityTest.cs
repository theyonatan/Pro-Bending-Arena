using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Managing.Server;

public sealed class SanityTest : MonoBehaviour
{
    private void Start()
    {
        InstanceFinder.ServerManager.OnRemoteConnectionState += (connection, args) =>
        {
            {
                Debug.Log($"Connection ({connection.ClientId}): {args.ConnectionState}");
            }
        };
    }
}