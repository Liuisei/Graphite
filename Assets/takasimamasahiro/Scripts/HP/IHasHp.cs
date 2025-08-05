    using UnityEngine;

    public interface IHasHp
    {
        int HP { get; }
        int MaxHP { get; }

        int TeamID { get; }

        void TakeDamage(int amount , GameObject attacker);//攻撃者の情報付き
        void Heal(int amount);

        event System.Action OnHPChanged;
    }
