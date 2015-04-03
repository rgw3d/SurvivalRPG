using UnityEngine;
using System.Collections;

public class PlayerStats {

	public int MaxHealth;
	public int CurrentHealth;
	public int MaxMana;
	public int CurrentMana;
	public int AttackDamage;
	public int ArmorValue;
	public float MovementSpeed;
    private CharacterClass _characterClass;

    public enum CharacterClass {
        op_fighter = 0,
        squishy_mage = 1,
        usless_other_than_the_fact_that_they_can_heal_healer = 2
    }

    public CharacterClass getClass() {
        return _characterClass;
    }

    public int GetHealth() {
        return CurrentHealth;
    }
    public void ChangeHealth(int change) {
        CurrentHealth += change;
        if (CurrentHealth <= 0) {
            //This calls a delegat saying that the player is dead. ouch
        }
        if (CurrentHealth >= MaxHealth) {
            CurrentHealth = MaxHealth;
        }
    }

	public string getSerializedStats(){
		string serial = MaxHealth + "," + CurrentHealth + "," + MaxMana + "," + CurrentMana + "," + AttackDamage + "," + ArmorValue + "," + MovementSpeed;
		return serial;
	}

}
