using UnityEngine;
using System.Collections;

public class PlayerStats {

	public int maxHealth;
	public int currentHealth;
	public int maxMana;
	public int currentMana;
	public int attackDamage;
	public int armorValue;
	public float speed;

	public string getSerializedStats(){
		string serial = maxHealth + "," + currentHealth + "," + maxMana + "," + currentMana + "," + attackDamage + "," + armorValue + "," + speed;
		return serial;
	}

}
