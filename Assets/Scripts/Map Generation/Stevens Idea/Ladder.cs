using UnityEngine;
using System.Collections;

public class Ladder : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D collider){
        DelegateHolder.TriggerGenerateAndRenderMap();
	}
}
