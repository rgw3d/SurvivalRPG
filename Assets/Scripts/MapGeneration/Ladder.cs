using UnityEngine;
using System.Collections;

public class Ladder : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D collider){
        if (collider.tag.Equals("Player")) {
            GameControl.ClearMap();
            DelegateHolder.TriggerGenerateAndRenderMap();
        }
	}
}
