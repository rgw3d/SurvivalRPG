using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnControl : Photon.MonoBehaviour {

    public GameObject PlayerPrefab;
    public GameObject CameraPrefab;
    public List<GameObject> EnemyTypes;
    private static List<string> _enemyNames = new List<string>();

    public GameObject Player;

	void Start () {
        Player = null;
        DelegateHolder.OnMapGenerated += SpawnPlayers;
        DelegateHolder.OnMapGenerated += SpawnEnemies;

        foreach (GameObject i in EnemyTypes) {
            _enemyNames.Add(i.name);
        }
        
	}

    public void SpawnPlayers(bool isHost) {
        if (isHost && Player == null) {//Only spawn a player/call the RPC if this is the Host who has generated the map and there is not one already
            Player = PhotonNetwork.Instantiate(PlayerPrefab.name, GenerateMap.Map.roomList[0].GetCenter(), Quaternion.identity, 0);
            GameObject playerCamera = Instantiate(CameraPrefab) as GameObject;
            playerCamera.transform.parent = Player.transform;//set the camera to be a child of the player
            playerCamera.transform.localPosition = new Vector3(0, 0, -10);
            photonView.RPC("PlacePlayer", PhotonTargets.OthersBuffered, GenerateMap.Map.roomList[0].GetCenter());//Call all clients
        }
        else if (isHost && Player != null) {
            Player.transform.position = GenerateMap.Map.roomList[0].GetCenter();//just set the locaiton - don't create a new player
            photonView.RPC("PlacePlayer", PhotonTargets.OthersBuffered, GenerateMap.Map.roomList[0].GetCenter());//Call all clients
        }

    }

    [RPC]
    void PlacePlayer(Vector2 position) {
        if (Player == null) {
            Player = PhotonNetwork.Instantiate(PlayerPrefab.name, position, Quaternion.identity, 0);
            GameObject playerCamera = Instantiate(CameraPrefab) as GameObject;
            playerCamera.transform.parent = Player.transform;//set the camera to be a child of the player
            playerCamera.transform.localPosition = new Vector3(0, 0, -10);
        }
        else {
            Player.transform.position = position;
        }
    }


    public void SpawnEnemies(bool isHost) {
        if (isHost) {//to spawn enemy, just call PhotonNetwork.Instantiate() to do it
			//PhotonNetwork.Instantiate(EnemyTypes[0].name, GenerateMap.Map.roomList[6].GetCenter(), Quaternion.identity, 0);
		    //how are we going to spawn enemies? we will spawn them accros the network! and only the server moves them
            //that is not bad
            //we can make that happen
        }
    }
    public static void SpawnNewEnemies(int location = 6, int type = 0) {
        Debug.Log("Spawning new enemies");
            PhotonNetwork.Instantiate(_enemyNames[type], GenerateMap.Map.roomList[location].GetCenter(), Quaternion.identity, 0);
    }

}
