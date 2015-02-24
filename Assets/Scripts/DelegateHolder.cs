using UnityEngine;
using System.Collections;


public delegate float ChangePlayerStat(StatType statType, float amountChange);
public delegate void PlayerAttack(bool isAttacking);

public class DelegateHolder {
}
