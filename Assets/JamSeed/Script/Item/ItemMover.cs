using UnityEngine;

public class ItemMover : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 5f;           // 移動速度
    public Vector2 moveDirection = Vector2.left;  // 移動方向（左向き）

    [Header("生存時間")]
    public float lifeTime = 10f;           // 生存時間（0以下で無限）
    public bool useLifeTime = true;        // 生存時間を使用するか

    [Header("画面外削除")]
    public bool destroyOffScreen = true;   // 画面外で削除するか
    public float offScreenBuffer = 2f;     // 画面外判定のバッファ

    private float timer = 0f;
    private Camera mainCamera;

    void Start()
    {
        // 初期化
        timer = 0f;
        mainCamera = Camera.main;

        // メインカメラが見つからない場合は最初のカメラを使用
        if (mainCamera == null)
        {
            mainCamera = FindObjectOfType<Camera>();
        }
    }

    void Update()
    {
        // アイテムを移動
        MoveItem();

        // 生存時間をチェック
        if (useLifeTime && lifeTime > 0)
        {
            CheckLifeTime();
        }

        // 画面外チェック
        if (destroyOffScreen)
        {
            CheckOffScreen();
        }
    }

    void MoveItem()
    {
        // 指定された方向と速度で移動
        Vector3 movement = moveDirection.normalized * moveSpeed * Time.deltaTime;
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

    void CheckOffScreen()
    {
        if (mainCamera == null) return;

        // カメラの境界を取得
        Vector3 screenPos = mainCamera.WorldToViewportPoint(transform.position);

        // 画面外に出たかチェック（バッファ考慮）
        float buffer = offScreenBuffer / 10f; // ビューポート座標用に調整
        if (screenPos.x < -buffer || screenPos.x > 1 + buffer ||
            screenPos.y < -buffer || screenPos.y > 1 + buffer)
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
        moveSpeed = speed;
    }

    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction;
    }

    public void SetLifeTime(float time)
    {
        lifeTime = time;
        useLifeTime = time > 0;
    }

    public void DisableLifeTime()
    {
        useLifeTime = false;
    }

    public void EnableOffScreenDestroy(bool enable)
    {
        destroyOffScreen = enable;
    }

    // コライダーベースの削除（オプション）
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("GameArea"))
        {
            DestroyItem();
        }
    }
}