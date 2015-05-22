﻿using UnityEngine;
using System.Collections;

public class Ladder : Photon.MonoBehaviour {

	void OnTriggerEnter2D(Collider2D collider){
        if (collider.tag.Equals("Player")) {
            GameControl.ClearMap();
            PlayerStats.SavePlayerScore();
            photonView.RPC("SavePlayerScore", PhotonTargets.Others);
            DelegateHolder.TriggerGenerateAndRenderMap();
        }
	}

    [RPC]
    public void SavePlayerScore() {
        PlayerStats.SavePlayerScore();
    }
}
