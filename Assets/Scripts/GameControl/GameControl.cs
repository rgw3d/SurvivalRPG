using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour {

    public static int Difficulty = 5; //Out of 0-10
    public static string FilePath = "C/path/to/file";
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

    private ChatBuffer _chatClient;
    private string _chatUsername = "UserName";
    private Vector2 scrollPosition;
    public bool IsChatting = false;
    

    


	void Start () {
        DontDestroyOnLoad(this);
        _chatClient = FindObjectOfType<ChatBuffer>();
	}
	
	void Update () {
	    //use this to control what music is playing
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
            IsChatting = true;
        }
        if (Input.GetKey(KeyCode.Return)) {

        }
        
	}

    void OnGUI() {
        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal(GUILayout.MinHeight(3 * Screen.height / 4));
                GUILayout.EndHorizontal();
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
                }
                else {
                    StartCoroutine(PhotonNetworkManager.TextPopup(.5f));
                }
            }
        }
        else {
            _chatClient.SuspendInput = !IsChatting;
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.MinWidth(Screen.width));
            GUILayout.Label(_chatClient.TextOutput());
            GUILayout.EndScrollView();
            //GUILayout.Box(_chatClient.TextOutput(), GUILayout.MinWidth(Screen.width));
        }
    }
}
