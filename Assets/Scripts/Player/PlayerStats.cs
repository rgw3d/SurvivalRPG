using UnityEngine;
using System.Collections;

public class PlayerStats: MonoBehaviour {

	public string playerName;
	public int MaxHealth;
	public int CurrentHealth;
	public int MaxMana;
	public int CurrentMana;
	public int AttackDamage;
	public int ArmorValue;
	public float MovementSpeed;
    private CharacterClass _characterClass;

    public enum CharacterClass {
        Fighter = 0,
        Mage = 1,
        Healer = 2
    }

    public static CharacterClass IntToCharacterClass(int classInt){
        switch (classInt) {
            case 0:
                return CharacterClass.Fighter;
            case 1:
                return CharacterClass.Mage;
            case 2:
                return CharacterClass.Healer;
            default:
                return CharacterClass.Fighter;
        }
        
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
