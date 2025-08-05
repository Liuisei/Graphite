using UnityEngine;

public class ItemMover : MonoBehaviour
{
    [Header("移動設定")]
    public float MoveSpeed = 5f;
    public Vector2 MoveDirection = Vector2.left;

    [Header("生存時間")]
    public float lifeTime = 10;
    private float timer = 0f;

    private void Start()
    {
        timer = 0;
    }
    private void Update()
    {
        MoveItem();

        CheckLifeTime();
    }
    void MoveItem()
    {
        // 指定された方向と速度で移動
        Vector3 movement = MoveDirection.normalized * MoveSpeed * Time.deltaTime;
        transform.Translate(movement);
    }
    void CheckLifeTime()
    {
        timer += Time.deltaTime;

        if (timer >= lifeTime)
        {
            DestroyItem();
        }
    }
    void DestroyItem()
    {
        // アイテムを破棄（エフェクトやサウンドを追加することも可能）
        Destroy(gameObject);
    }

    // 外部からの設定メソッド
    public void SetSpeed(float speed)
    {
        MoveSpeed = speed;
    }

    public void SetDirection(Vector2 direction)
    {
        MoveDirection = direction;
    }

    public void SetLifeTime(float time)
    {
        lifeTime = time;
    }

    // 画面外に出た時の処理（オプション）
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("GameArea"))
        {
            DestroyItem();
        }
    }
}
