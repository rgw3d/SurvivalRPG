using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChatOutput : MonoBehaviour {

    public ChatBuffer Output;

    public string[] AcceptedCommands = new[] { "spawnenemy" };
    public string[] AcceptedOptions = new[] { "["};
    List<string> Options;

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
        if (original.Length > 0) {

            Debug.Log(original.Substring(1));
            Debug.Log(original);
            if (original[0] == '/') {
                original = original.Substring(1);
                Options = new List<string>();
                string command = GetCommand(original);
                //Debug.Log("got the command: " +command);
                if (!command.Equals("") && original.IndexOf(" ") > 0) {
                    //Debug.Log("good command...now parsing options");
                    WashHands(original.Substring(original.IndexOf(" ")));
                    ParseCommands(command);
                }
                else {
                    Output.AddLine("Bad command");
                    Debug.Log("bad command " + !command.Equals("") + "  " + original.IndexOf(" "));
                }
            }
        }
    }

    private void ParseCommands(string command) {
        if (command.Equals(AcceptedCommands[0])) {//spawn enemy
            if (Options.Count == 2) {//two commands command
                try {
                    int type = int.Parse(Options[0]); //first is type
                    int number = int.Parse(Options[1]); // second is number
                    for (int i = 0; i < number; i++) {
                        SpawnControl.SpawnNewEnemies(UnityEngine.Random.Range(0, 30 - 1), type);
                        //Debug.Log("Spawning enemeis");
                    }
                }
                catch (Exception e) {
                    Output.AddLine("Incorrect Syntax: SpawnEnemy -type -number");
                    Debug.Log(e.Message + "\n" + e.StackTrace);
                }

            }
            else {
                Output.AddLine("Incorrect Syntax: SpawnEnemy -type -number");
            }
        }
    }

    public string GetCommand(string original) {
        string command = original.IndexOf(" ") > 0 ? (original.Substring(0, original.IndexOf(" ")).TrimStart()) : original.Trim();
        Debug.Log("parsed command: " + command);
        command = command.Trim();
        foreach (string comd in AcceptedCommands) {
            if (comd.Trim() == command) {
                return command;
            }
        }
        return "";
    }

    void WashHands(string original) {
        original = original.Trim();
        if (original.Length == 0) {
            return;
        }
        while (original.Length > 0) {

            if (original.IndexOf(" ", System.StringComparison.Ordinal) > 0) {
                Options.Add(original.Substring(0, original.IndexOf(" ", System.StringComparison.Ordinal)).Trim());
                original = original.Substring(original.IndexOf(" ", System.StringComparison.Ordinal));
            }
            else {
                Options.Add(original.Trim());
                original = "";
            }
        }

    }

   
}
