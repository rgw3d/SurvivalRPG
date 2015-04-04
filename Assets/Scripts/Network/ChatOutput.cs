using UnityEngine;
using System.Collections;

public class ChatOutput : MonoBehaviour {

    public ChatBuffer Output;

    public void Awake() {
        Output = FindObjectOfType<ChatBuffer>();
    }

    public void InitScreen() {
        Output.AddText("Basic Chat room");
    }

    public bool PrintInput() {
        return true;
    }

    public void ParseInput(string original) {
        Output.AddLine(original);
    }
}
