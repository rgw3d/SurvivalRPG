using UnityEngine;
using System.Collections;

public class ChatOutput : MonoBehaviour {

    public ChatBuffer Output;

    public void Awake() {
        Output = FindObjectOfType<ChatBuffer>();
    }

    public abstract void ParseInput(string input);
    public abstract void InitScreen();
    public abstract bool PrintInput();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
