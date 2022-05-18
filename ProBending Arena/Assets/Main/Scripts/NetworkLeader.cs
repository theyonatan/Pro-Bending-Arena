using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Object;
using FishNet.Managing.Scened;
using FishNet.Transporting;
using FishNet.Managing;
using FishNet.Connection;
using FishNet.Managing.Server;
using FishNet.Managing.Client;
using System;
using UnityEngine.SceneManagement;

public class NetworkLeader : NetworkBehaviour
{
    #region Items
    [Header("General Settings")]
    public bool Offline;
    
    [Header("Server Settings")]
    private bool GameInProgress;
    public int MaxPlayers = 6;

    [Header("NetworkLeaders")]
    private NetworkManager _NetworkManager;
    private FishNet.Managing.Scened.SceneManager _NetworkSceneManager;
    private UnityEngine.SceneManagement.SceneManager LocalSceneManager;
    private ServerManager _ServerManager;
    private ClientManager _ClientManager;

    [Header("MenuData")]
    private MenuManager menuManager;
    private LobbyManager lobbyManager;
    
    [Header("Player Data")]
    private Dictionary<int, PlayerData> Players;
    public Vector3[] SpawnPositions;

    public PlayerData _Data;

    public GameObject Player;

    [Header("Scene Switching")]
    public NetworkObject[] MenuToLobby;
    [SerializeField] GameObject SceneSwitching;
    public ClientMenuDataManager _clientManager;
    public WindManager _windController;
    public int PlayersReadyForGame = 0;
    #endregion;

    #region FirstUp
    private void Awake()
    { //before first frame
        Players = new Dictionary<int, PlayerData>();

        menuManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<MenuManager>();
        _ServerManager = InstanceFinder.ServerManager;
        _ClientManager = InstanceFinder.ClientManager;
        _NetworkManager = InstanceFinder.NetworkManager;
        _NetworkSceneManager = InstanceFinder.SceneManager;

        _Data = new PlayerData("Empty Name", 0, 0, 2);

        _ServerManager.OnServerConnectionState += OnServerConnectionStateChanged;
        _ServerManager.OnRemoteConnectionState += OnClientConnectionChanged;
        _ClientManager.OnClientConnectionState += _ClientManager_OnClientConnectionState;
        _NetworkSceneManager.OnClientPresenceChangeEnd += NetworkSceneManager_OnLoadEnd;
    }

    private void OnEnable()
    {

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Lobby")
        {
            if (InstanceFinder.IsServer)
                return;

            _ClientManager = InstanceFinder.ClientManager;

            _clientManager = GameObject.FindGameObjectWithTag("ClientLeader").GetComponent<ClientMenuDataManager>();
            _Data = _clientManager._Data;

            //SendDataToServer(_Data);

            return;
        }
    }

    private void OnDestroy()
    { //after last frame
        //_ServerManager.OnServerConnectionState -= OnServerConnectionStateChanged;
        //_ServerManager.OnRemoteConnectionState -= OnClientConnectionChanged;
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Lobby")
        {
            _ServerManager.OnServerConnectionState -= OnServerConnectionStateChanged;
            _ServerManager.OnRemoteConnectionState -= OnClientConnectionChanged;
            _ClientManager.OnClientConnectionState -= _ClientManager_OnClientConnectionState;
            _NetworkSceneManager.OnClientPresenceChangeEnd -= NetworkSceneManager_OnLoadEnd;
        }
    }

    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //LoadLobbyScene();
            //GameObject go = Instantiate(SceneSwitching, new Vector3(0, 0, 0), Quaternion.identity);
            //Spawn(go);
            OnLoadGame();
        }
    }

    #region Server/ClientActions
    private void OnServerConnectionStateChanged(ServerConnectionStateArgs args)
    { //when server connection changed
        if (args.ConnectionState == LocalConnectionStates.Started)
        { //server started
            SetUpServer();
        }
        if (args.ConnectionState == LocalConnectionStates.Stopped)
        { //server ended
            // Do something...
        }
    } 

    private void OnClientConnectionChanged(NetworkConnection connection, RemoteConnectionStateArgs args)
    {
        //Started client(called on server)
        if (args.ConnectionState == RemoteConnectionStates.Started)
        {
            Debug.Log(args.ConnectionId + " - New Player Has Joined The Server!");
            //GetPlayerLobbyData(connection);
            //Spawn(gameObject, connection);
        }
        if (args.ConnectionState == RemoteConnectionStates.Stopped)
        {
            int LeavingId = args.ConnectionId;
            Players.Remove(LeavingId);
            //if (GameObject.FindGameObjectWithTag("Wind"))
            //    _windController.RemovePlayer(LeavingId);

            Debug.Log(args.ConnectionId + " - Has Disconnected");
        }
    }

    private void _ClientManager_OnClientConnectionState(ClientConnectionStateArgs obj)
    { //on client: new client joined the server
        if (obj.ConnectionState == LocalConnectionStates.Started)
        {
            Debug.Log("Hey");
            //SetUpClient();
            //SendDataToServer(_Data);
        }
        if (obj.ConnectionState == LocalConnectionStates.Stopped)
        {
            Debug.Log("I Left am client");
        }
    }

    #endregion

    #region ServerCommands
    private void SetUpServer()
    {
        if (!IsServer)
            return;

        Players = new Dictionary<int, PlayerData>();

        Debug.Log("Server started succesfully!");

        LoadLobbyScene();
    }

    private void AddNewPlayer(PlayerData data)
    {
        data.SetId((ulong)Players.Count);

        if (data.ClientId <= 3)
            data.SetTeam(1);
        else if (data.ClientId <= 6)
            data.SetTeam(2);
        else
            data.SetTeam(8);

        Players.Add(Players.Count, data);

        Debug.Log("Adding a new Player " + data.PlayerName);

        SetLobbyData(data);
        PrintNewPlayerLocal(data);
    }

    private void PrintNewPlayerLocal(PlayerData data)
    {
        //Print new player locally
    }

    public void StartGame()
    {
        _windController.SetData(Players);
        GameInProgress = true;

        Debug.Log("Starting Game");

        LoadGameScene();
    }

    public string GetName(NetworkConnection conn)
    {
        if (IsClient)
            return "";

        return Players[conn.ClientId].PlayerName;
    }

    public int GetTeam(NetworkConnection conn)
    {
        if (IsClient)
            return 0;

        return Players[conn.ClientId].Team;
    }

    private void UnsubscribeFromEvents()
    {
        _ServerManager.OnServerConnectionState -= OnServerConnectionStateChanged;
        _ServerManager.OnRemoteConnectionState -= OnClientConnectionChanged;
        _ClientManager.OnClientConnectionState -= _ClientManager_OnClientConnectionState;
        _NetworkSceneManager.OnClientPresenceChangeEnd -= NetworkSceneManager_OnLoadEnd;
    }
    #endregion

    #region SceneSwitching
    public void LoadLobbyScene()
    {
        if (!IsServer)
            return;

        UnsubscribeFromEvents();

        SceneLoadData sceneData = new SceneLoadData("Lobby")
        {
            ReplaceScenes = ReplaceOption.All,
        };

        if (Offline)
            return;

        _NetworkSceneManager.LoadGlobalScenes(sceneData);
    }

    public void LoadGameScene()
    {
        UnsubscribeFromEvents();

        if (!IsServer)
            return;

        SceneLoadData sceneData = new SceneLoadData("Main");
        sceneData.ReplaceScenes = ReplaceOption.All;

        Debug.Log("Loading scene main");
        _NetworkSceneManager.LoadGlobalScenes(sceneData);
    }

    private void NetworkSceneManager_OnLoadEnd(ClientPresenceChangeEventArgs args)
    { //Loaded New Scene
        Debug.Log("Loaded Scene " + args.Scene.name);

        if (!IsServer)
            return;

        switch (args.Scene.name)
        { //On Client Joined Scene
            case "Lobby":
                OnLoadLobby(args.Connection);
                break;
            case "Main":
                Debug.Log("Loaded Main");
                OnLoadGame();
                break;
        }
        Debug.Log(args.Scene.name);
    }

    private void OnLoadLobby(NetworkConnection conn)
    {
        _windController = GameObject.FindGameObjectWithTag("Wind").GetComponent<WindManager>();
        GetPlayerLobbyData(conn);
    }

    private void OnLoadGame()
    {
        Debug.Log("CHOO");
        _windController = GameObject.FindGameObjectWithTag("Wind").GetComponent<WindManager>();
        if (Players == null || Players.Count == 0)
        {
            Debug.Log("Resetting!");
            Players = new Dictionary<int, PlayerData>();
            Players = _windController.GetData();
        }
        Debug.Log(PlayersReadyForGame);
        PlayersReadyForGame++;

        if (PlayersReadyForGame == Players.Count)
        {
            StartGameMain();
            Debug.Log("We Are All Ready");
        }
    }
    #endregion

    #region GameMain
    public void StartGameMain()
    {
        int CurrentA = 0;
        int CurrentB = 3;

        for (int i = 0; i < Players.Count; i++)
        {
            if (Players[i].Team == 0)
            {
                Players[i].SpawnPosition = SpawnPositions[CurrentA];
                CurrentA++;
            }
            else
            {
                Players[i].SpawnPosition = SpawnPositions[CurrentB];
                CurrentB++;
            }
            Debug.Log(Players[i].PlayerName);
        }

        Debug.Log("Spawning");
        SpawnPlayers();
    }

    public void SpawnPlayers()
    {
        foreach (var item in Players)
        {
            GameObject NewPlayer = Instantiate(Player, Players[item.Key].SpawnPosition, Quaternion.identity);
            Spawn(NewPlayer, InstanceFinder.ServerManager.Clients[item.Key]);
        }
    }
    #endregion

    #region Rpcs
    [ServerRpc(RequireOwnership = false)]
    public void SendDataToServer(PlayerData data, NetworkConnection conn = null)
    { //send local player data to server
        if (!IsServer)
            return;

        AddNewPlayer(data);

        Debug.Log("New Player Joined! " + data.PlayerName);

        _windController = GameObject.FindGameObjectWithTag("Wind").GetComponent<WindManager>();
        _windController.SetData(Players);

        SetLobbyData(data);
    }

    [ObserversRpc] public void SetLobbyData(PlayerData data)
    {
        if (IsServer)
            return;

        Debug.Log("I am an observer and i Have Recieved Player " + data.PlayerName);
    }

    [TargetRpc] public void GetPlayerLobbyData(NetworkConnection conn)
    {
        if (IsServer)
            return;

        ClientMenuDataManager _dataHolder;
        _dataHolder = GameObject.FindGameObjectWithTag("ClientLeader").GetComponent<ClientMenuDataManager>();

        _Data = _dataHolder._Data;

        SendDataToServer(_Data);
        TestRpc("Testing The Rpc");
    }

    [ServerRpc(RequireOwnership =false)]
    public void TestRpc(string ok)
    {
        Debug.LogError(ok);
    }
    #endregion
}
