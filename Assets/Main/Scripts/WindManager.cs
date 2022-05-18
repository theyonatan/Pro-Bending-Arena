using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindManager : MonoBehaviour
{
    public Dictionary<int, PlayerData> PlayersForwardData;
    public int LocalScoreBlue;
    public int LocalScoreRed;

    private void Start()
    { //Destroy If Server Retreat To Menu
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Menu")
            Destroy(gameObject);
    }

    public Dictionary<int, PlayerData> GetData()
    {
        return PlayersForwardData;
    }

    public void SetData(Dictionary<int, PlayerData> data)
    {
        PlayersForwardData = data;
    }

    public void RemovePlayer(int Id)
    {
        PlayersForwardData.Remove(Id);
    }
}
