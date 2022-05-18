using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Authenticating;
using FishNet.Transporting;
using FishNet.Managing.Server;
using FishNet.Broadcast;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ChatManager : MonoBehaviour
{
    public List<ChatMessage> Chat = new List<ChatMessage>();
    public List<string> ChatTest = new List<string>();

    private bool _isServer;

    //public NetworkLeader _Leader;

    [Header("ChatSettings")]
    public int MaxMessages = 25;
    public bool ChatSelected = false;
    [SerializeField] GameObject inputField;
    public GameObject TextObject;
    public GameObject ViewPanel;

    [Header("UI")]
    public GameObject TextBackground;

    public void EnableChat(bool isserver)
    {
        _isServer = isserver;

        if (_isServer)
            InstanceFinder.ServerManager.RegisterBroadcast<ChatBroadcast>(OnServerChatBroadcast, false);
        else
            InstanceFinder.ClientManager.RegisterBroadcast<ChatBroadcast>(OnClientChatBroadcast);

        Debug.Log("Started as " + _isServer);
    }
    private void OnDisable()
    {
        if (_isServer && InstanceFinder.ServerManager != null)
            InstanceFinder.ServerManager.UnregisterBroadcast<ChatBroadcast>(OnServerChatBroadcast);
        else if (InstanceFinder.ClientManager != null)
            InstanceFinder.ClientManager.UnregisterBroadcast<ChatBroadcast>(OnClientChatBroadcast);
    }   

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (ChatSelected) //if the chat is already selected
            {
                string MessageToSend = inputField.GetComponent<TMPro.TMP_InputField>().text;

                if (MessageToSend == "")
                {
                    DisableChatObject();
                    return;
                }

                SendChatMessage(MessageToSend);
                DisableChatObject();
            }
            else //if the chat is not selected we enable it
            {
                EnableChatObject();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape)) //if we want to quit the chat without sending the message
        {
            DisableChatObject();
        }
    }

    private void DisableChatObject()
    {
        inputField.GetComponent<TMPro.TMP_InputField>().text = "";
        EventSystem.current.SetSelectedGameObject(null);
        TextBackground.SetActive(false);
        ChatSelected = false;
    }
    private void EnableChatObject()
    {
        inputField.GetComponent<TMPro.TMP_InputField>().text = "";
        EventSystem.current.SetSelectedGameObject(inputField);
        TextBackground.SetActive(true);
        ChatSelected = true;
    }

    //send chat message broadcast from client to server
    public void SendChatMessage(string text)
    {
        //Client won't send their username, server will already know it.
        ChatBroadcast msg = new ChatBroadcast()
        {
            Message = text,
            FontColor = Color.white
        };

        Debug.Log("Sending Message " + text);
        InstanceFinder.ClientManager.Broadcast(msg);
    }

    //Upon revieivng broadcast from client (on server)
    public void OnServerChatBroadcast(NetworkConnection conn, ChatBroadcast msg)
    {
        //simplfying on getting first object
        NetworkObject nob = conn.FirstObject;

        //if the client wasn't spawned (doesn't have it's object)
        if (nob == null)
            return;

        //Getting name
        msg.Username = "Client";//_Leader.GetName(conn);

        //Send Broadcast
        Debug.Log("sending back to clients " + msg.Message);
        InstanceFinder.ServerManager.Broadcast(nob, msg, false);
    }

    //revieving chat broadcast back from server (on broadcast)
    private void OnClientChatBroadcast(ChatBroadcast obj)
    {
        Debug.Log("Revieved " + obj.Message);
        PrintToChat(obj.Username, obj.Message, obj.FontColor);
    }

    //printing recived chat broadcast to chat.
    private void PrintToChat(string Username, string Message, Color _color)
    {
        if (Chat.Count >= MaxMessages)
        {
            Destroy(Chat[0].textObject.gameObject);
            Chat.Remove(Chat[0]);
        }
        if (ChatTest.Count >= MaxMessages)
            ChatTest.Remove(ChatTest[0]);

        ChatMessage NewMessage = new ChatMessage();
        NewMessage.color = _color;
        NewMessage.username = Username;
        NewMessage.message = Message;

        GameObject newText = Instantiate(TextObject, ViewPanel.transform);
        NewMessage.textObject = newText.GetComponent<TMPro.TextMeshProUGUI>();
        NewMessage.textObject.text = NewMessage.message;

        Chat.Add(NewMessage);
        ChatTest.Add(Message);
    }
}

public struct ChatBroadcast : IBroadcast
{
    public string Username;
    public string Message;
    public Color FontColor;
}

public class ChatMessage
{
    public string username;
    public string message;
    
    public TMPro.TextMeshProUGUI textObject;

    public Color color;
}
