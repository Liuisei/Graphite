using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    private int baseDamage;
    public Weapon(int baseDamage)
    {
        this.baseDamage = baseDamage;
    }
    public void DealDamage(GameObject target , int amount)
    {
        //IHasHp hpTargerget = target.GetComponent<IHasHp>();
        //if (hpTargerget != null)
        //{
        //    hpTargerget.TakeDamage(amount, target);
        //}

    }

    public void TryDealDamage(GameObject attacker , GameObject target)
    {
        IHasHp hp = target.GetComponent<IHasHp>();
        if (hp == null) return;

        IHasHp attakerTeam = attacker.GetComponent<IHasHp>();
        IHasHp targetTame = target.GetComponent<IHasHp>();
        if (targetTame != null  && targetTame != null && attakerTeam.TeamID == attakerTeam.TeamID)
        {
            Debug.Log("味方なので無効");
            return;
        }

        hp.ChangeHP(baseDamage,target);
    }


}
