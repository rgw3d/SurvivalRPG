using UnityEngine;
using System.Collections;

public class StevensMapRenderer : MonoBehaviour {
	
	public GameObject walkable;
	public GameObject background;
	
	private GameObject[,] spriteArray;

	// Use this for initialization
	void Start () {
	
		StevensMapGeneration map = GetComponent<StevensMapGeneration>();
		Debug.Log(map.mapWidth);
	}
	// Update is called once per frame
	void Update () {
	
	}
}