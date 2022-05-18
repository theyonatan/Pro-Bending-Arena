using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public string PlayerName;
    //public string PlayerName
    //{
    //    get { return _playerName; }
    //    set { _playerName = value; }
    //}
    public ulong ClientId;
    public int Element;
    public int Team;
    public int Scene;
    public Vector3 SpawnPosition;

    public PlayerData(string playerData, ulong id, int Elementi, int team)
    {
        PlayerName = playerData;
        ClientId = id;
        Element = Elementi;
        Team = team;
    }
    
    public PlayerData()
    {
        PlayerName = "Empty";
        ClientId = 22;
        Element = 10;
        Team = 6;
        SpawnPosition = new Vector3(0, 0, 0);
    }
    public void ChangeScene(int Index)
    {
        Scene = Index;
    }

    public void SetName(string Name)
    {
        PlayerName = Name;
    }

    public void SetElement(int element)
    {
        Element = element;
    }

    public void SetTeam(int team)
    {
        Team = team;
    }

    public void SetId(ulong id)
    {
        ClientId = id;
    }

}































