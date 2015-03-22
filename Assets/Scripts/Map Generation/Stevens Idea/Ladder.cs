using UnityEngine;
using System.Collections;

public class Ladder : MonoBehaviour {

	public StevensMapGeneration mapGen;

	// Use this for initialization
	void Start () {
        mapGen = GameObject.FindGameObjectWithTag("Map Controller").GetComponent<StevensMapGeneration>();
	}

	void OnTriggerEnter2D(Collider2D collider){
        mapGen.GenerateAndDisplayMap();
	}
}
