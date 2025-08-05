using UnityEngine;
using UnityEngine.UI;

public class BossHP : MonoBehaviour
{
    [SerializeField] private int maxHP = 100;
    [SerializeField] private Slider hpSlider;

    private int currentHP;

    public int CurrentHP => currentHP;
    public int MaxHP => maxHP;

    private void Awake()
    {
        currentHP = maxHP;

        if (hpSlider == null)
        {
            Debug.LogError("HPスライダーがセットされていません！");
        }
        else
        {
            hpSlider.maxValue = maxHP;
            hpSlider.value = currentHP;
        }
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0) return;

        currentHP = Mathf.Max(currentHP - amount, 0);
        UpdateHPBar();

        if (currentHP == 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (amount <= 0) return;

        currentHP = Mathf.Min(currentHP + amount, maxHP);
        UpdateHPBar();
    }

    private void UpdateHPBar()
    {
        if (hpSlider != null)
        {
            hpSlider.value = currentHP;
        }
    }

    private void Die()
    {
        Debug.Log("Boss defeated!");
        // 死亡処理をここに書く
    }
}
