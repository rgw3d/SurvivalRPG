﻿using UnityEngine;
using System.Collections;

public class PlayerStats: MonoBehaviour {

	public static string PlayerName;
	public static int MaxHealth;
	public static int CurrentHealth;
	public static int MaxMana;
	public static int CurrentMana;
	public static int AttackDamage;
	public static int ArmorValue;
	public static float MovementSpeed;
    private CharacterClass _characterClass;

	void Awake(){
        PlayerName = PhotonNetworkManager.selectedPlayerName;
        MaxHealth = PlayerPrefs.GetInt(GameControl.PLAYERMAXHEALTHKEY + PhotonNetworkManager.selectedPlayerName);
		CurrentHealth = MaxHealth;
		MaxMana = PlayerPrefs.GetInt(GameControl.PLAYERMAXMANAKEY + PhotonNetworkManager.selectedPlayerName);
		CurrentMana = MaxMana;
		AttackDamage = PlayerPrefs.GetInt(GameControl.PLAYERATTACKKEY + PhotonNetworkManager.selectedPlayerName);
		ArmorValue = PlayerPrefs.GetInt(GameControl.PLAYERDEFENSEKEY + PhotonNetworkManager.selectedPlayerName);
		_characterClass = (CharacterClass)PlayerPrefs.GetInt(GameControl.PLAYERCLASSKEY + PhotonNetworkManager.selectedPlayerName);
		MovementSpeed = PlayerPrefs.GetFloat(GameControl.PLAYERMOVEMENTKEY + PhotonNetworkManager.selectedPlayerName);

	}


    public enum CharacterClass {
        Fighter = 0,
        Mage = 1,
        Healer = 2,
		Shrek = 3
    }

    public static CharacterClass IntToCharacterClass(int classInt){
        switch (classInt) {
            case 0:
                return CharacterClass.Fighter;
            case 1:
                return CharacterClass.Mage;
            case 2:
                return CharacterClass.Healer;
			case 3:
				return CharacterClass.Shrek;
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
