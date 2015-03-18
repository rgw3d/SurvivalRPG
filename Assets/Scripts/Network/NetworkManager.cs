using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

    public StevensMapGeneration MapGenerationScript;

    private const string typeName = "UniqueGameNameWendel";
    //private const string gameName = "Room Name Test";
    public string gameName = "Example Room Name ";
    private HostData[] hostList;

	void Start () {
	//MasterServer.ipAddress = "127.0.0.1";
        MapGenerationScript = FindObjectOfType<StevensMapGeneration>();
	}

    void Awake() {
        DontDestroyOnLoad(this);//dont kill this object :)
    }
	
	// Update is called once per frame
	void Update () {
	
	}

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
        Network.Instantiate(MapGenerationScript.player, new Vector3(29, 20, 0), Quaternion.identity, 0);
    }
    void OnDisconnectedFromServer() {
        Debug.Log("Disconnected for whatever reason");
    }

    void OnGUI() {
        if (!Network.isClient && !Network.isServer) {
            if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server")) {
                StartServer();
                MapGenerationScript.StartGeneration();
                //Application.LoadLevel(1);
            }

            gameName = GUI.TextField(new Rect( 400, 100, 250, 25), gameName, 25);

            if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
                RefreshHostList();

            if (hostList != null) {
                for (int i = 0; i < hostList.Length; i++) {
                    if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName)) {
                        JoinServer(hostList[i]);
                        
                    }
                }
            }
        }
    }

}
