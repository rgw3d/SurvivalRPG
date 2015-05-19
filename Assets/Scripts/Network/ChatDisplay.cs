using UnityEngine;
using System.Collections;

public class ChatDisplay : MonoBehaviour {

    private ChatBuffer _chatClient;
    private Vector2 scrollPosition;
    public static bool IsChatting = false;
    public static int ChatBoxWidth = Screen.width / 3;
    public static int ChatBoxHeight = Screen.height / 3;
    public static ChattingState ChatState = ChattingState.NoUsername;


    void Start() {
        _chatClient = FindObjectOfType<ChatBuffer>();
        DelegateHolder.OnChatMessageSent += ChatMessageSent;
    }

    public enum ChattingState {
        NoUsername,
        ChatClosedButShowing,
        ChatOpenAndTyping,
    }

    void OnGUI() {
        GUILayout.BeginArea(new Rect(0, Screen.height - ChatBoxHeight, ChatBoxWidth, ChatBoxHeight));
        GUILayout.BeginVertical();
        if (PhotonNetwork.room != null) {
            try {
                ChatClient();
            }
            catch (System.ArgumentException e) {
                Debug.Log(e.Message);
                // Debug.Log(e.StackTrace);
            }
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();


    }
    void ChatClient() {
        if (_chatClient.Host.Equals("")) {
            _chatClient.Host = PlayerStats.PlayerName;
            _chatClient.AddLine(_chatClient.Host + " Has Joined", true);
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
