using UnityEngine;
using System.Collections;

public class ChatOutput : MonoBehaviour {

    public ChatBuffer Output;

    public void Start() {
        Output = FindObjectOfType<ChatBuffer>();
        DelegateHolder.OnChatMessageSent += ParseInput;
    }

    public void InitScreen() {
        Output.AddLine("Basic Chat room");
    }

    public bool PrintInput() {
        return true;
    }

    public void ParseInput(string original) {
        //Output.AddLine(original);
        //do any commands
    }
}
