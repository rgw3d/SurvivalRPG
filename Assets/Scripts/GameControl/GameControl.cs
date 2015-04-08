using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour {

    public static int Difficulty = 5; //Out of 0-10
    //Music when we have it
    //Any other settings
    public KeyCode ExitKey = KeyCode.Escape;
    public const string TITLESCREEN = "TitleScreen";
    public const string PLAYSCREEN = "PlayScreen";

    public const string PLAYERNAMESKEY = "PLAYERNAMESKEY";
    public const string PLAYERCLASSKEY = "PLAYERCLASSKEY";
    public const string PLAYERMAXHEALTHKEY = "PLAYERMAXHEALTHKEY";
    public const string PLAYERMAXMANAKEY = "PLAYERMAXMANAKEY";
    public const string PLAYERDEFENSEKEY = "PLAYERDEFENSEKEY";
    public const string PLAYERATTACKKEY = "PLAYERATTACKKEY";
	public const string PLAYERMOVEMENTKEY = "PLAYERMOVEMENTKEY";

    private ChatBuffer _chatClient;
    private string _chatUsername = "UserName";
    private Vector2 scrollPosition;
    public static bool IsChatting = false;
    public static int ChatBoxWidth = Screen.width/3;
    public static ChattingState ChatState = ChattingState.NoUsername;
    

	void Start () {
        DontDestroyOnLoad(this);
        _chatClient = FindObjectOfType<ChatBuffer>();
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
        if (Input.GetKey(KeyCode.T)) {
            ChatState = ChattingState.ChatOpenAndTyping;
        }
        
	}

    void OnGUI() {
        GUILayout.BeginArea(new Rect(0, 4 * Screen.height/6, Screen.width, Screen.height));
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                    if(PhotonNetwork.room != null)
                        ChatClient();
                GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        GUILayout.EndArea();


    }
    void ChatClient() {
        if (_chatClient.Host.Equals("")) {
            GUILayout.Label("Chat Username: ");
            _chatUsername = GUILayout.TextField(_chatUsername);
            if (GUILayout.Button("Join ChatRoom")) {
                if (!_chatUsername.Equals("UserName") && !_chatUsername.Equals("")) {
                    _chatClient.Host = _chatUsername;
                    ChatState = ChattingState.ChatClosedButShowing;
                }
                else {
                    StartCoroutine(PhotonNetworkManager.TextPopup(.5f));
                }
            }
        }
        else {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(ChatBoxWidth));
            GUILayout.Label(_chatClient.TextOutput());
            GUILayout.EndScrollView();
        }
    }
}
