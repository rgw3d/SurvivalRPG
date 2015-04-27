using UnityEngine;
using System.Collections;

public class MeleeSword: MonoBehaviour {

	public GameObject player;

	void Start(){

	}

	void FixedUpdate(){
		transform.position = new Vector2(player.transform.position.x - PlayerControl.leftAttackOffset, player.transform.position.y);
	}

}
