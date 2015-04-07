using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChatBuffer : Photon.MonoBehaviour {

    private string _buffer = "";
    private string _inputBuffer = "";
    public int _scrollIndex;

    public const int ScreenHeight = 24;
    public int ScreenWidth;

    public string Host = "";
    public string Prompt = ">";

    public const int MaxBufferLength = 10000;

    public bool SuspendInput;
    public bool HidePrompt;

    public ChatOutput GetOutput;

    public void Start() {
        GetOutput = FindObjectOfType<ChatOutput>();
        ScreenWidth = Mathf.RoundToInt(Screen.width);
        HidePrompt = true;
    }

    public void Update() {
        if (!SuspendInput) 
            AddInput(Input.inputString);
        

            //_text.text = ScrollBuffer(TextOutput(), _scrollIndex);
            //TODO add something that will actually display the text


            // Truncate buffer if longer than buffer limit
            while (_buffer.Length > 10000) {
                _buffer = _buffer.Substring(_buffer.IndexOf('\n'));
            }
        
    }

    public void AddInput(string text) {
        if (SuspendInput) {
            return;
        }

        foreach (var c in text) {
            if (c == "\b"[0]) {//backspace
                if (_inputBuffer.Length != 0) {
                    _inputBuffer = _inputBuffer.Substring(0, _inputBuffer.Length - 1);
                }
            }
            else if (c == "\n"[0] || c == "\r"[0]) {//new line/ return
                if (GetOutput.PrintInput()) {
                    AddLine(InputTextOutput(true), true);
                }

                GetOutput.ParseInput(_inputBuffer);//commands

                _inputBuffer = "";
            }
            else {//just add it to the _inputBuffer
                _inputBuffer += char.ToLower(c);
            }
        }
    }

    public string TextOutput() {
        var output = _buffer;

        output += "\n";

        if (!SuspendInput) {// Not Suspending Input, display typing
            output += InsertLineBreaks(InputTextOutput());
        }

        return output;
    }

    public void Clear() {
        _buffer = "";
    }

    public string InputTextOutput(bool final = false) {
        var input = _inputBuffer;
        var caret = ((Time.time * 2) % 2) > 1 && !final ? "|" : "";
        return String.Format("{0}{1} {2}{3}", Host, Prompt, input, caret);
    }

    public void AddText(string text) {
        _buffer += text;

        _buffer = InsertLineBreaks(_buffer);
    }

    public void AddLine(string text = "", bool sendRPC = false) {

        if(sendRPC)
            photonView.RPC("RecieveChatText", PhotonTargets.Others, text);

        _buffer += "\n";

        _buffer +=  text;
        _buffer = InsertLineBreaks(_buffer);
        
    }


    public string InsertLineBreaks(string text) {
        var lastLineStart = text.LastIndexOf('\n');
        var lineLength = 0;
        for (var i = lastLineStart; i < text.Length; i++, lineLength++) {
            if (lineLength == ScreenWidth) {
                text = text.Insert(i, "\n");
                lineLength = 0;
            }
        }

        return text;
    }

    [RPC]
    void RecieveChatText(string text) {
        AddLine(text);
    }




}
