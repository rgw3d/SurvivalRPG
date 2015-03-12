using UnityEngine;
using System.Collections;

public class Ladder : MonoBehaviour {

	public StevensMapRenderer mapRend;
	public StevensMapGeneration mapGen;

	// Use this for initialization
	void Start () {
		GameObject map = GameObject.FindGameObjectWithTag("Map Controller") as GameObject;
		mapGen = map.GetComponent<StevensMapGeneration>();
		mapRend = map.GetComponent<StevensMapRenderer>();
	}

	void OnTriggerEnter2D(Collider2D collider){
		mapGen.generateMap();
		mapRend.reRenderMap();
	}
}
