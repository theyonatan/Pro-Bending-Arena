using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    public PlayerData LocalData;

    [Header("Data")]
    public bool CollectData = true;

    [SerializeField] private TMP_Text Name;
    //[SerializeField] private int Id = 20;
    [SerializeField] private Slider ElementPicker;
    [SerializeField] private int Team;

    [SerializeField] private TMP_Text _Port;
    [SerializeField] private TMP_Text _IP;
    public int Port;
    public string Ip;

    public bool EmptyIp;
    public bool EmptyPort;

    public GameObject MainCanvas;
    public GameObject PlayCanvas;

    public StartServerOrClient _GameStarter;

    public ClientMenuDataManager _clientManager;

    // Start is called before the first frame update
    void Start()
    {
        _clientManager = GameObject.FindGameObjectWithTag("ClientLeader").GetComponent<ClientMenuDataManager>();

        LocalData = new PlayerData("New Player", 20, -1, 0);

        if (_IP.text == "")
            Ip = "";
        if (_Port.text == "")
            Port = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "Menu")
            return;

        if (_IP.text == "")
            Ip = "";
        if (_Port.text == "")
            Port = 0;

        LocalData.SetName(Name.text);
        LocalData.SetElement((int)ElementPicker.value);

    }

    public void GameScreen()
    {
        MainCanvas.SetActive(!MainCanvas.activeSelf);
        PlayCanvas.SetActive(!PlayCanvas.activeSelf);
    }

    public void SettingsScreen()
    {
        Debug.Log("Not Yet!!!!!!!!");
    }

    public void PressedServer()
    {
        ushort _port = 0;
        string _Ip = Ip;
        
        if (_Port.text != "")
        {
            string NewPort = _Port.text.Remove(_Port.text.Length - 1);

            if (int.TryParse(NewPort, out int _Result))
                _port = (ushort)_Result;
        }

        if (_IP.text != "")
            _Ip = _IP.text;

        Debug.Log("Last " + _port);
        CollectData = false;

        Debug.Log("Starting a server on " + _Ip + " : " + _port);
        _GameStarter.StartServer(_Ip, _port);
    }

    public void PressedClient()
    {
        ushort _port = 0;
        string _Ip = _IP.text;

        if (_Port.text != "")
        {
            string NewPort = _Port.text.Remove(_Port.text.Length - 1);

            if (int.TryParse(NewPort, out int _Result))
                _port = (ushort)_Result;
        }
        if (_IP.text != "")
            _Ip = _IP.text;

        CollectData = false;

        Debug.Log("Connecting to " + _Ip + " : " + _port);
        _GameStarter.StartClient(_Ip, _port);
    }

    private void SetUpClient()
    {
        CollectData = false;
        _clientManager._Data = LocalData;
    }
}
