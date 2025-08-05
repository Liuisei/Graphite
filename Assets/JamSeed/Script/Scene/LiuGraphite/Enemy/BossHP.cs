using System;
using UnityEngine;
using UnityEngine.UI;

public class BossHP : MonoBehaviour,IHasHp
{
    [SerializeField] private int maxHP = 100;
    [SerializeField] private Slider hpSlider;

    private int currentHP = 100;

    public event Action OnHPChanged;

    public int CurrentHP => currentHP;
    public int MaxHP => maxHP;

    public int HP => currentHP;

    public int TeamID => 1;

    private void Awake()
    {
        currentHP = maxHP;
        UpdateHPBar();
    }
    public void UpdateHPBar()
    {
        if (hpSlider != null)
        {
            Debug.LogWarning($"UpdateHPBar called with currentHP: {currentHP}, maxHP: {maxHP}");
            hpSlider.value = (float)currentHP/(float)maxHP;
        }
    }

    private void Die()
    {
        Debug.Log("Boss defeated!");
        // 死亡処理をここに書く
    }

    public void ChangeHP(int amount, GameObject attacker)
    {
        Debug.LogWarning($"ChangeHP called with amount: {amount}, attacker: {attacker?.name} {currentHP} {maxHP}");
        currentHP -= amount;
        if (currentHP < 0)
        {
            currentHP = 0;
        }
        else if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }

        UpdateHPBar();
    }
}
