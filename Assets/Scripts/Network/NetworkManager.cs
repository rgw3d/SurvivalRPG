﻿using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	//MasterServer.ipAddress = "127.0.0.1";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private const string typeName = "UniqueGameNameWendel";
    private const string gameName = "Room Name Test";
    private HostData[] hostList;

    private void StartServer() {
        Network.InitializeServer(2, 25000, !Network.HavePublicAddress());
        MasterServer.RegisterHost(typeName, gameName);
    }

    private void RefreshHostList() {
        MasterServer.RequestHostList(typeName);
    }

    void OnMasterServerEvent(MasterServerEvent msEvent) {
        if (msEvent == MasterServerEvent.HostListReceived)
            hostList = MasterServer.PollHostList();
    }

    private void JoinServer(HostData hostData) {
        Network.Connect(hostData);
    }


    void OnServerInitialized() {
        Debug.Log("Server Initializied");
    }
    void OnConnectedToServer() {
        Debug.Log("Server Joined");
    }

    void OnGUI() {
        if (!Network.isClient && !Network.isServer) {
            if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
                StartServer(); if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
                StartServer();

            if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
                RefreshHostList();

            if (hostList != null) {
                for (int i = 0; i < hostList.Length; i++) {
                    if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
                        JoinServer(hostList[i]);
                }
            }
        }
    }

}
