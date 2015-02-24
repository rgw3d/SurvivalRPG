using UnityEngine;
using System.Collections;


public delegate float ChangePlayerStat(StatType statType, float amountChange);
public delegate void PlayerAttack(bool isAttacking);

public static class DelegateHolder {


    public static event ChangePlayerStat OnPlayerStatChange;
    public static event PlayerAttack OnPlayerAttack;


    public static void TriggerPlayerStatChange(StatType statType, float amountChange) {
        OnPlayerStatChange(statType, amountChange);
    }

    public static void TriggerPlayerAttack(bool isAttacking) {
        OnPlayerAttack(isAttacking);
    }


}
