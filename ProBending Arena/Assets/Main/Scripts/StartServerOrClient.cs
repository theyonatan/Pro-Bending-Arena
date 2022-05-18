using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Managing;
using FishNet.Transporting;

public class StartServerOrClient : MonoBehaviour
{
    private NetworkManager _networkManager;

    private void Start()
    {
        _networkManager = InstanceFinder.NetworkManager;
        
        if (_networkManager == null)
        {
            Debug.Log("Can't find network Manager");
            return;
        }
    }

    public void StartServer(string IP, ushort Port)
    {
        if (_networkManager == null)
            return;

        IP = IP.Remove(IP.Length - 1);

        if (IP.Length < 7 && Port == 0)
        {
            _networkManager.TransportManager.Transport.SetServerBindAddress("127.0.0.1", IPAddressType.IPv4);
            _networkManager.TransportManager.Transport.SetPort(7770);

            _networkManager.ServerManager.StartConnection();
        }

        _networkManager.TransportManager.Transport.SetServerBindAddress(IP, IPAddressType.IPv4);
        _networkManager.TransportManager.Transport.SetPort(Port);

        _networkManager.ServerManager.StartConnection();
    }

    public void StartClient(string IP, ushort Port)
    {
        if (_networkManager == null)
            return;

        IP.Replace(" ", "");

        if (IP.Length < 7 && Port == 0)
        {
            _networkManager.TransportManager.Transport.SetClientAddress("127.0.0.1");
            _networkManager.TransportManager.Transport.SetPort(7770);

            _networkManager.ClientManager.StartConnection();
        }

        _networkManager.TransportManager.Transport.SetClientAddress(IP);
        _networkManager.TransportManager.Transport.SetPort(Port);

        _networkManager.ClientManager.StartConnection();
    }

    
}
