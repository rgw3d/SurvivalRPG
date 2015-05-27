using UnityEngine;
using System.Collections;

public class Ladder : Photon.MonoBehaviour {

	void OnTriggerEnter2D(Collider2D collider){
        if (collider.tag.Equals("Player")) {
            //if (photonView.isMine)
                GameControl.ClearMap();
            //else
             //   photonView.RPC("ClearMap", PhotonTargets.MasterClient);
            PlayerStats.SavePlayerScore();
            photonView.RPC("SavePlayerScore", PhotonTargets.Others);
            DelegateHolder.TriggerGenerateAndRenderMap();
            photonView.RPC("IncreaseXP", PhotonTargets.All, 100);
        }
	}

    [RPC]
    public void SavePlayerScore() {
        PlayerStats.SavePlayerScore();
    }

    [RPC]
    public void ClearMap() {
        GameControl.ClearMap();
    }

    [RPC]
    public void IncreaseXP(int amt) {
        PlayerStats.PlayerScore += amt;
    }
}
