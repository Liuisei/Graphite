using UnityEngine;

public class ThunderSettings : MonoBehaviour
{
    public float duration = 5f;
    public int damage = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, duration);
    }

    private void OnTriggerEnter(Collider other)
    {
        IHasHp target = other.GetComponent<IHasHp>();
        if (target != null && target.TeamID == 1) // player ID
        {
            target.TakeDamage(damage, gameObject); // Attacker
        }
            Destroy(gameObject);
    }
}
