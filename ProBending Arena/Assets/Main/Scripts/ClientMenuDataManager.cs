using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientMenuDataManager : MonoBehaviour
{
    private MenuManager menuManager;
    public PlayerData _Data;

    private void Awake()
    {
        menuManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<MenuManager>();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            _Data = menuManager.LocalData;
            //Debug.Log(_Data.PlayerName);
        }
    }


}
