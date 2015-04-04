using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ChatBuffer : MonoBehaviour {

    private string _chatUsername;

    public ChatBuffer(string userName) {
        _chatUsername = userName;
    }

    private string _buffer = "";
    private string _inputBuffer = "";
    public int _scrollIndex;
    private int _inputIndex;
    private readonly List<string> _inputHistory = new List<string>();

    public const int ScreenHeight = 24;
    public int ScreenWidth;

    public string Host = "";
    public string Prompt = "";

    public const int MaxBufferLength = 10000;

    public bool SuspendInput;
    public bool HidePrompt;

    public ConsoleOutputBase GetOutput;


    public void Start() {

        ScreenWidth = Mathf.RoundToInt(53 / 1.731f * Screen.width / Screen.height);
        HidePrompt = true;
    }

    public void Update() {
        AddInput(Input.inputString);

        _text.text = ScrollBuffer(TextOutput(), _scrollIndex);

        if (Input.GetKeyDown(KeyCode.PageUp) || Input.GetKeyDown(KeyCode.F1)) {
            _scrollIndex += ScreenHeight - 5;

            var lines = TextOutput().Split('\n').ToArray();
            if (_scrollIndex >= lines.Length - ScreenHeight) {
                _scrollIndex = lines.Length - ScreenHeight - 1;
            }
        }

        
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
            if (c == "\b"[0]) {
                if (_inputBuffer.Length != 0) {
                    _inputBuffer = _inputBuffer.Substring(0, _inputBuffer.Length - 1);
                }
            }
            else if (c == "\n"[0] || c == "\r"[0]) {
                if (GetOutput.PrintInput()) {
                    AddLine(InputTextOutput(true));
                }

                _inputHistory.Reverse();
                _inputHistory.Add(_inputBuffer);
                _inputHistory.Reverse();
                _inputIndex = -1;

                GetOutput.ParseInput(_inputBuffer);

                _inputBuffer = "";
            }
            else {
                _inputBuffer += char.ToLower(c);
            }
        }
    }

    public string TextOutput() {
        var output = _buffer;

        output += "\n";

        if (!SuspendInput) {
            output += InsertLineBreaks(InputTextOutput());
        }
        else if (Prompt != "" && !HidePrompt) {
            output += Prompt;
        }

        return output;
    }

    public void Clear() {
        _buffer = "";
    }

    public string InputTextOutput(bool final = false) {
        var input = _inputBuffer;
        var caret = ((Time.time * 2) % 2) > 1 && !final ? "|" : "";
        return String.Format("{0}{1} {2}{3}", Host, Prompt == "" ? ">" : Prompt, input, caret);
    }

    public void AddText(string text) {
        _buffer += text;

        _buffer = InsertLineBreaks(_buffer);
    }

    public string AttemptCenter(string text, out int paddingLength) {
        text = text.Trim();

        var leftPadding = (ScreenWidth - text.Length) / 2;

        if (leftPadding < 0) {
            paddingLength = 0;
            return text;
        }

        var padding = "";
        for (var i = 0; i < leftPadding; i++) {
            padding += " ";
        }

        paddingLength = leftPadding;
        return padding + text;
    }

    public int AddLine(string text = "", bool attemptCenter = false, int padding = 0) {
        ;
        var paddingLength = 0;

        _buffer += "\n";

        for (var i = 0; i < padding; i++) {
            _buffer += " ";
        }

        _buffer += attemptCenter ? AttemptCenter(text, out paddingLength) : text;

        _buffer = InsertLineBreaks(_buffer);
        return paddingLength;
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

    public void ClearHistory() {
        _inputHistory.Clear();
    }



}
