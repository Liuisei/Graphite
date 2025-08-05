using UnityEngine;

public class BossPhaseParts : MonoBehaviour, IHasHp
{
    [SerializeField] public int maxHP = 50;
    private int currentHP;

    public int HP => currentHP;
    public int MaxHP => maxHP;
    public int TeamID => 3; 

    public event System.Action OnHPChanged;
    public event System.Action<BossPhaseParts> OnDestroyed;

    private void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int amount, GameObject attacker)
    {
        currentHP = Mathf.Clamp(currentHP - amount, 0, maxHP);
        OnHPChanged?.Invoke();

        if (currentHP <= 0)
        {
            HandleDestruction();
        }
    }

    public void Heal(int amount)
    {
        currentHP = Mathf.Clamp(currentHP + amount, 0, maxHP);
        OnHPChanged?.Invoke();
    }

    private void HandleDestruction()
    {
        Debug.Log($"[BossPhasePart] {name} destroyed");
        OnDestroyed?.Invoke(this);
        Destroy(gameObject);
    }
}
