using UnityEngine;
using System.Collections;
using System;

public class GameControl : MonoBehaviour {

    public static int Difficulty = 5; //Out of 0-10
    public KeyCode ExitKey = KeyCode.Escape;
    public const string TITLESCREEN = "TitleScreen";
    public const string PLAYSCREEN = "PlayScene";

    public const string PLAYERNAMESKEY = "PLAYERNAMESKEY";
    public const string PLAYERCLASSKEY = "PLAYERCLASSKEY";
    public const string PLAYERMAXHEALTHKEY = "PLAYERMAXHEALTHKEY";
    public const string PLAYERMAXMANAKEY = "PLAYERMAXMANAKEY";
    public const string PLAYERDEFENSEKEY = "PLAYERDEFENSEKEY";
    public const string PLAYERATTACKKEY = "PLAYERATTACKKEY";
	public const string PLAYERMOVEMENTKEY = "PLAYERMOVEMENTKEY";

    private ChatBuffer _chatClient;
    private Vector2 scrollPosition;
    public static bool IsChatting = false;
    public static int ChatBoxWidth = Screen.width/3;
    public static int ChatBoxHeight = Screen.height / 3;
    public static ChattingState ChatState = ChattingState.NoUsername;
    

	void Start () {
        DontDestroyOnLoad(this);
        _chatClient = FindObjectOfType<ChatBuffer>();
        _chatClient.Host = PlayerStats.PlayerName;
        _chatClient.AddLine(_chatClient.Host + " Has Joined", true);
        DelegateHolder.OnChatMessageSent += ChatMessageSent;
        DelegateHolder.OnPlayerHasConnected += PlayerConnected;
        DelegateHolder.OnPlayerHasDisconnected += PlayerDisconnected;
    }

    public void PlayerConnected() {
    }

    public void PlayerDisconnected() {
    }

    public enum ChattingState {
        NoUsername,
        ChatClosedButShowing,
        ChatOpenAndTyping,
    }
	
	void Update () {
        if (Input.GetKey(ExitKey)) {
            if (Application.loadedLevelName.Equals(TITLESCREEN)) {//escape key for when on Title Screen
                Application.Quit();
            }
            if (Application.loadedLevelName.Equals(PLAYSCREEN)) {//escape key for when in play screen
                PhotonNetwork.LeaveRoom();
                Application.LoadLevel(TITLESCREEN);
            }
        }
        if (Input.GetKey(KeyCode.T) || Input.GetKey(KeyCode.Slash)) {
            ChatState = ChattingState.ChatOpenAndTyping;

        }
        
	}

    void OnGUI() {
        GUILayout.BeginArea(new Rect(0, Screen.height - ChatBoxHeight, ChatBoxWidth, ChatBoxHeight));
        GUILayout.BeginVertical();
        if (PhotonNetwork.room != null) {
            try {
                ChatClient();
            }
            catch (ArgumentException e) {
                Debug.Log(e.Message);
                // Debug.Log(e.StackTrace);
            }
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();


    }
    void ChatClient() {
        if (_chatClient.Host.Equals("")) {

        }
        else {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUILayout.Width(ChatBoxWidth), GUILayout.Height(ChatBoxHeight));
            GUILayout.Label(_chatClient.TextOutput());
            GUILayout.EndScrollView();
        }


    }

    void ChatMessageSent(string message) {
        scrollPosition.y = Mathf.Infinity;//set scroll position equal to the bottom of the view area
    }
}
