using UnityEngine;

public interface IHasHp
{
    int HP { get; }
    int MaxHP { get; }

    int TeamID { get; }

    void ChangeHP(int amount , GameObject attacker);//攻撃者の情報付き


    event System.Action OnHPChanged;
}
