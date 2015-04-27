using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnControl : Photon.MonoBehaviour {

    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject cameraPrefab;

    public List<List<GameObject>> EnemyParties;//for the programmer to decide what types of groups of enemies you will see
	
	void Start () {
        DelegateHolder.OnMapGenerated += SpawnPlayers;
        DelegateHolder.OnMapGenerated += SpawnEnemies;
		DelegateHolder.OnMapGenerated += SpawnAbilities;
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


    public void SpawnEnemies(bool isHost) {
        if (isHost) {//to spawn enemy, just call PhotonNetwork.Instantiate() to do it
			PhotonNetwork.Instantiate(enemyPrefab.name, GenerateMap.Map.roomList[6].GetCenter(), Quaternion.identity, 0);
		    //how are we going to spawn enemies? we will spawn them accros the network! and only the server moves them
            //that is not bad
            //we can make that happen
        }
    }

	public void SpawnAbilities(bool isHost){
		if(isHost){
		

		}
	} 
}
