using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerBullet1 : MonoBehaviour, IHasHp
{
    [Header("HP設定")]
    public int maxHP = 100; // 最大HPを設定
    private int currentHP;

    [Header("移動設定")]
    public float speed = 10f; // 弾丸の速度
    public Vector3 direction = Vector3.forward; // 移動方向

    [Header("攻撃設定")]
    public int damage = 20; // 与えるダメージ

    [Header("自動破棄設定")]
    public float lifeTime = 5f; // 自動消滅時間

    private int teamID = 1; // チームIDを設定（プレイヤー弾丸）
    private Rigidbody rb;
    private bool isInitialized = false;

    // IHasHpの実装
    public int HP => currentHP;
    public int MaxHP => maxHP;
    public int TeamID => teamID;
    public event Action OnHPChanged;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 初期化がまだされていない場合はデフォルト初期化
        if (!isInitialized)
        {
            Initialize(direction.normalized, speed);
        }

        // 一定時間後に自動破棄
        Destroy(gameObject, lifeTime);
    }

    /// <summary>
    /// 弾丸を初期化する
    /// </summary>
    /// <param name="moveDirection">移動方向（正規化される）</param>
    /// <param name="bulletSpeed">弾丸の速度</param>
    /// <param name="bulletDamage">与えるダメージ（オプション）</param>
    public void Initialize(Vector3 moveDirection, float bulletSpeed, int bulletDamage = -1)
    {
        // HP初期化
        currentHP = maxHP;

        // 移動設定
        direction = moveDirection.normalized;
        speed = bulletSpeed;

        // ダメージ設定（-1の場合はデフォルト値を使用）
        if (bulletDamage >= 0)
        {
            damage = bulletDamage;
        }

        // Rigidbodyが取得できている場合は即座に移動開始
        if (rb != null)
        {
            rb.linearVelocity = direction * speed;
        }

        isInitialized = true;

        // HPChanged イベントを発火
        OnHPChanged?.Invoke();
    }

    void FixedUpdate()
    {
        // Rigidbodyで移動（Initialize後に確実に動作）
        if (rb != null && isInitialized)
        {
            rb.linearVelocity = direction * speed;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // IHasHpインターフェースを持つオブジェクトかチェック
        IHasHp target = other.GetComponent<IHasHp>();

        if (target != null)
        {
            // 自分と違うチームかどうかチェック
            if (target.TeamID != this.TeamID)
            {
                // ダメージを与える
                target.TakeDamage(damage, gameObject);

                // 自分もダメージを受ける（貫通しない弾丸の場合）
                TakeDamage(currentHP, other.gameObject); // 一撃で破壊
            }
        }
    }

    public void Heal(int amount)
    {
        if (amount <= 0) return;

        currentHP = Mathf.Min(currentHP + amount, maxHP);
        OnHPChanged?.Invoke();
    }

    public void TakeDamage(int amount, GameObject attacker)
    {
        if (amount <= 0) return;

        currentHP = Mathf.Max(currentHP - amount, 0);
        OnHPChanged?.Invoke();

        // HPが0になったら破棄
        if (currentHP <= 0)
        {
            OnDestroy();
        }
    }

    void OnDestroy()
    {
        // 破棄時の処理（エフェクト、音声などを追加可能）
        Debug.Log($"PlayerBullet destroyed at {transform.position}");
    }

    // デバッグ用：弾丸の移動方向を可視化
    void OnDrawGizmos()
    {
        if (isInitialized)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(transform.position, direction * 2f);

            // HP状態を色で表現
            float hpRatio = (float)currentHP / maxHP;
            Gizmos.color = Color.Lerp(Color.red, Color.green, hpRatio);
            Gizmos.DrawWireSphere(transform.position, 0.2f);
        }
    }
}