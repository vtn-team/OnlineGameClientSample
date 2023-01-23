using Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePack;
using System;
using System.Text;
using TMPro;

public class WebSocketManager : MonoBehaviour
{
    [SerializeField] string _host = "ws://localhost:3000/"; //ws://35.72.48.33:3000/
    [SerializeField] GameObject _playerPrefab;
    [SerializeField] TextMeshProUGUI _chatText;
    [SerializeField] TMP_InputField _chatInput;

    public bool IsJoin { get; private set; } = false;

    WebSocketCli client = new WebSocketCli();

    List<string> ChatLog = new List<string>();
    Queue<ISendData> DataQueue = new Queue<ISendData>();
    Queue<ChatMsgResult> ChatQueue = new Queue<ChatMsgResult>();

    void Start()
    {
        _chatText.text = "";
        Connect();
    }

    void Update()
    {
        if (ChatQueue.Count > 0)
        {
            foreach (var msg in ChatQueue)
            {
                ChatLog.Add($"{msg.Name}: {msg.Message}");
            }
            while(ChatLog.Count > 12)
            {
                ChatLog.RemoveAt(0);
            }
            _chatText.text = string.Join("\n", ChatLog);
            ChatQueue.Clear();
        }

        if (DataQueue.Count == 0) return;

        var data = DataQueue.Dequeue();
        client.Send(JsonUtility.ToJson(data));
    }


    class ServerResult
    {
        public string UserId;
        public string Target;
        public string Command = "Join";
        public string Data;
    }
    class ChatMsgResult
    {
        public string UserId;
        public string Name;
        public string Message;
    }

    interface ISendData
    {

    }

    class UserJoin : ISendData
    {
        public string UserId;
        public string Command = "Join";
        public string Name;
    }

    class ChatMsg : ISendData
    {
        public string UserId;
        public string Name;
        public string Command = "ChatMsg";
        public string Message;
    }

    void Connect()
    {
        client.Connect(_host, Message);
    }

    void Message(byte[] msg)
    {
        ServerResult data = null;
        try
        {
            //data = MessagePackSerializer.Deserialize<ServerResult>(msg);
            string json = Encoding.UTF8.GetString(msg);
            data = JsonUtility.FromJson<ServerResult>(json);
        }
        catch(Exception ex)
        {
            Debug.Log(ex.Message);
        }

        if (data == null) return;

        try
        {
            Debug.Log(data.Command);
            switch (data.Command)
            {
                //サーバ入室
                case "JoinCall":
                    {
                        UserJoin join = new UserJoin() { Name = GameManager.SaveData.Name, UserId = GameManager.SaveData.UUID };
                        DataQueue.Enqueue(join);
                        IsJoin = true;
                    }
                    break;

                //チャット
                case "ChatMsg":
                    {
                        ChatMsgResult chatMsg = JsonUtility.FromJson<ChatMsgResult>(data.Data);
                        ChatQueue.Enqueue(chatMsg);
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }

    }



    public void SendChat()
    {
        if (_chatInput.text == "") return;
        string msg = _chatInput.text;

        ChatMsg chat = new ChatMsg() { Message = msg, Name = GameManager.SaveData.Name, UserId = GameManager.SaveData.UUID };
        DataQueue.Enqueue(chat);
    }
}
