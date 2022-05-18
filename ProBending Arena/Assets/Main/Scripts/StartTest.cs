using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Managing.Server;
using FishNet.Managing.Client;

public class StartTest : MonoBehaviour
{
    [SerializeField] GameObject Menu;

    ChatManager chatManager;

    ServerManager _Server;
    ClientManager _Client;

    public bool StartedServer = false;
    public bool StartedClient = false;

    private void Awake()
    {
        chatManager = GameObject.FindGameObjectWithTag("ChatManager").GetComponent<ChatManager>();

        _Server = InstanceFinder.ServerManager;
        _Client = InstanceFinder.ClientManager;
    }

    public void StartClient()
    {
        if (!StartedClient)
            _Client.StartConnection();
        else
            _Client.StopConnection();
        StartedClient = !StartedClient;
        chatManager.EnableChat(false);

        Destroy(Menu);
    }

    public void StartServer()
    {
        if (!StartedServer)
            _Server.StartConnection();
        else
            _Server.StopConnection(false);
        StartedServer = !StartedServer;
        chatManager.EnableChat(true);

        Destroy(Menu);
    }
}
