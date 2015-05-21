using UnityEngine;
using System.Collections;

public class ChatDisplay : MonoBehaviour {

    public ChatBuffer ChatBuff;
    private Vector2 scrollPosition;
    public static bool IsChatting = false;
    public static int ChatBoxWidth = Screen.width / 3;
    public static int ChatBoxHeight = Screen.height / 3;
    public static ChattingState ChatState = ChattingState.NoUsername;


    void Start() {
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
        if (System.String.IsNullOrEmpty(ChatBuff.Host)) {
            ChatBuff.Host = PlayerStats.GetPlayerName();
            ChatBuff.AddLine(ChatBuff.Host + " Has Joined", true);
        }
        else {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUILayout.Width(ChatBoxWidth), GUILayout.Height(ChatBoxHeight));
            GUILayout.Label(ChatBuff.TextOutput());
            GUILayout.EndScrollView();
        }


    }

    void ChatMessageSent(string message) {
        scrollPosition.y = Mathf.Infinity;//set scroll position equal to the bottom of the view area
    }
}
