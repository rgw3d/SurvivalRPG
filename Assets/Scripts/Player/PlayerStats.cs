using UnityEngine;
using System.Collections;

public class PlayerStats: MonoBehaviour {

    public enum CharacterClass {
        Fighter = 0,
        Mage = 1,
        Healer = 2,
        Shrek = 3
    }

    /* Get Methods */
    public static string GetPlayerName(){
        return PhotonNetworkManager.selectedPlayerName;
    }

    public static int GetMaxHealth(){
        return PlayerPrefs.GetInt(GameControl.PLAYERMAXHEALTHKEY + PhotonNetworkManager.selectedPlayerName);
    }

    public static int GetMaxMana(){
        return PlayerPrefs.GetInt(GameControl.PLAYERMAXMANAKEY + PhotonNetworkManager.selectedPlayerName);
    }

    public static int GetBaseAttackValue(){
        return PlayerPrefs.GetInt(GameControl.PLAYERATTACKKEY + PhotonNetworkManager.selectedPlayerName);
    }

    public static int GetBaseDefenseValue(){
        return PlayerPrefs.GetInt(GameControl.PLAYERDEFENSEKEY + PhotonNetworkManager.selectedPlayerName);
    }

    public static float GetBaseMovementSpeed(){
        return PlayerPrefs.GetFloat(GameControl.PLAYERMOVEMENTKEY + PhotonNetworkManager.selectedPlayerName);
    }

    public static CharacterClass GetPlayerClass(){
        return (CharacterClass)PlayerPrefs.GetInt(GameControl.PLAYERCLASSKEY + PhotonNetworkManager.selectedPlayerName);
    }

   
    /* Set Methods */
    public static void SetMaxHealth(int maxHealth) {
        PlayerPrefs.SetInt(GameControl.PLAYERMAXHEALTHKEY + PhotonNetworkManager.selectedPlayerName,maxHealth);
    }

    public static void SetMaxMana(int maxMana) {
        PlayerPrefs.SetInt(GameControl.PLAYERMAXMANAKEY + PhotonNetworkManager.selectedPlayerName, maxMana);
    }

    public static void SetBaseAttackValue(int baseAttack) {
        PlayerPrefs.SetInt(GameControl.PLAYERATTACKKEY + PhotonNetworkManager.selectedPlayerName, baseAttack);
    }

    public static void SetBaseDefenseValue(int baseDefense) {
        PlayerPrefs.SetInt(GameControl.PLAYERDEFENSEKEY + PhotonNetworkManager.selectedPlayerName,baseDefense);
    }

}
