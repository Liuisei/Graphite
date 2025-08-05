using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour, IHasHp
{
    [Header("Movement")]
    public float moveSpeed = 1f;
    public float minY = -5f;
    public float maxY = 5f;
    public float startReturnY = 5f;

    [SerializeField] private int maxHP = 100;
    private int currentHP;

    public int HP => currentHP;
    public int MaxHP => maxHP;
    public int TeamID => 3; // チームID
    public void ChangeHP(int amount, GameObject attacker)
    {
        throw new NotImplementedException();
    }

    public event Action OnHPChanged;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    public float fireTimer = 0f;
    public float[] shotAngles;

    private bool hasEntered = false;
    private float direction = -1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleShooting();
    }
    public void TakeDamage(int amount)
    {
        currentHP = Mathf.Clamp(currentHP - amount, 0, maxHP);
        Debug.Log(currentHP);
        Debug.Log($"[Player] Took damage: {amount} → HP: {currentHP}/{maxHP}");

        if (currentHP <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Debug.Log("[Player] 死亡しました");
        // 死亡処理（アニメーション、リスポーンなど）
        SceneManager.LoadScene("3OutGame");
    }
    void HandleMovement()
    {
        if (!hasEntered)
        {
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

            if (transform.position.y <= startReturnY)
            {
                hasEntered = true;
            }
        }
        else
        {
            transform.Translate(Vector3.up * direction * moveSpeed * Time.deltaTime);

            if (transform.position.y >= maxY - 0.01f && direction > 0f)
            {
                direction = -1f;
            }
            else if (transform.position.y <= minY + 0.01f && direction < 0f)
            {
                direction = 1f;
            }
        }
    }

    void HandleShooting()
    {
        if (!hasEntered) return;

        fireTimer += Time.deltaTime;
        if(fireTimer >= 1f / fireRate)
        {
            fireTimer = 0f;
            Shoot();
        }
    }

    void Shoot()
    {
        foreach (float angle in shotAngles)
        {
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

            Instantiate(bulletPrefab, firePoint.position, rotation);
        }
    }
}
