using UnityEngine;
using System.Collections;

public class SpawnControl : Photon.MonoBehaviour {

    public GameObject playerPrefab;
    public GameObject cameraPrefab;

	
	void Start () {
        DelegateHolder.OnMapGenerated += SpawnPlayers;
	}

    public void SpawnPlayers(bool isHost) {
        if (isHost) {//Only spawn a player/call the RPC if this is the Host who has generated the map
            GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, GenerateMap.Map.roomList[0].GetCenter(), Quaternion.identity, 0);
            GameObject playerCamera = Instantiate(cameraPrefab) as GameObject;
            playerCamera.transform.parent = player.transform;//set the camera to be a child of the player
            playerCamera.transform.localPosition = new Vector3(0, 0, -10);

            photonView.RPC("PlacePlayer", PhotonTargets.OthersBuffered, GenerateMap.Map.roomList[0].GetCenter());//Call all clients
        }

    }

    [RPC]
    void PlacePlayer(Vector2 position) {
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(position.x, position.y), Quaternion.identity, 0);
        GameObject playerCamera = Instantiate(cameraPrefab) as GameObject;
        playerCamera.transform.parent = player.transform;//set the camera to be a child of the player
        playerCamera.transform.localPosition = new Vector3(0, 0, -10);

    }
}
