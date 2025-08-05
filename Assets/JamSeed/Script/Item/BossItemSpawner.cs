using UnityEngine;
using System.Collections;

public class BossItemSpawner : MonoBehaviour
{
    [System.Serializable]
    public class BossItemType
    {
        [Header("アイテム設定")] public GameObject prefab; // アイテムプレハブ
        public string itemName; // アイテム名
        public float speed = 5f; // 移動速度
        public float lifeTime = 15f; // 生存時間（通常より長め）

        [Header("ドロップ確率")] [Range(0f, 100f)] public float dropChance = 50f; // ドロップ確率
    }

    [Header("ボス設定")] public Transform bossTransform; // ボスのTransform（親）
    public Transform dropPoint; // ドロップ地点（ボスの子オブジェクト）

    [Header("ドロップアイテム")] [Tooltip("ボスがドロップする特別なアイテム")]
    public BossItemType[] bossItems = new BossItemType[2]
    {
        new BossItemType { itemName = "レベルアップ", dropChance = 30f },
        new BossItemType { itemName = "バリア", dropChance = 20f }
    };

    [Header("スポーン範囲設定")] public float spawnRadius = 2f; // ドロップ地点周辺のスポーン半径
    public Vector2 spawnOffset = Vector2.zero; // ドロップ地点からのオフセット

    [Header("自動ドロップ設定")] public bool enableAutoDrop = false; // 自動ドロップを有効にするか
    public float autoDropInterval = 5f; // 自動ドロップ間隔

    private bool isAutoDropping = false;

    void Start()
    {
        // ボスTransformが設定されていない場合は親を使用
        if (bossTransform == null)
            bossTransform = transform.parent;

        // ドロップ地点が設定されていない場合は自分のTransformを使用
        if (dropPoint == null)
            dropPoint = transform;

        // 自動ドロップを開始
        if (enableAutoDrop)
        {
            StartAutoDrop();
        }
    }

    void Update()
    {
        // ボスが存在しない場合は自動ドロップを停止
        if (bossTransform == null && isAutoDropping)
        {
            StopAutoDrop();
        }
    }

    // 手動でアイテムをドロップ
    public void DropItems()
    {
        foreach (var item in bossItems)
        {
            // ドロップ確率をチェック
            if (Random.Range(0f, 100f) <= item.dropChance)
            {
                DropItem(item);
            }
        }
    }

    // 特定のアイテムを強制ドロップ
    public void DropSpecificItem(int itemIndex)
    {
        if (itemIndex >= 0 && itemIndex < bossItems.Length)
        {
            DropItem(bossItems[itemIndex]);
        }
    }

    // 全アイテムを強制ドロップ
    public void DropAllItems()
    {
        foreach (var item in bossItems)
        {
            DropItem(item);
        }
    }

    void DropItem(BossItemType itemType)
    {
        if (itemType.prefab == null)
        {
            Debug.LogWarning($"{itemType.itemName} のプレハブが設定されていません！");
            return;
        }

        // ドロップ位置を計算
        Vector3 dropPosition = GetRandomDropPosition();

        // アイテムをスポーン
        GameObject droppedItem = Instantiate(itemType.prefab, dropPosition, Quaternion.identity);

        // アイテムの移動設定
        SetupItemMover(droppedItem, itemType);

        Debug.Log($"ボスドロップ: {itemType.itemName} をドロップしました！");
    }

    Vector3 GetRandomDropPosition()
    {
        // ドロップ地点の位置を取得
        Vector3 basePosition = dropPoint.position;

        // オフセットを適用
        basePosition += new Vector3(spawnOffset.x, spawnOffset.y, 0f);

        // 円形範囲内でランダム位置を生成
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;

        return new Vector3(
            basePosition.x + randomCircle.x,
            basePosition.y + randomCircle.y,
            basePosition.z
        );
    }

    void SetupItemMover(GameObject item, BossItemType itemType)
    {
        // ItemMoverコンポーネントを取得または追加
        ItemMover mover = item.GetComponent<ItemMover>();
        if (mover == null)
        {
            mover = item.AddComponent<ItemMover>();
        }

        // ボスドロップアイテムの設定
        mover.SetSpeed(itemType.speed);
        mover.SetLifeTime(itemType.lifeTime);
        mover.SetDirection(Vector2.left); // 右から左へ移動
    }

    // 自動ドロップを開始
    public void StartAutoDrop()
    {
        if (!isAutoDropping && bossTransform != null)
        {
            isAutoDropping = true;
            StartCoroutine(AutoDropCoroutine());
        }
    }

    // 自動ドロップを停止
    public void StopAutoDrop()
    {
        isAutoDropping = false;
    }

    IEnumerator AutoDropCoroutine()
    {
        while (isAutoDropping && bossTransform != null)
        {
            yield return new WaitForSeconds(autoDropInterval);

            if (isAutoDropping)
            {
                DropItems();
            }
        }
    }

    // ドロップ確率を動的に変更
    public void SetDropChance(int itemIndex, float chance)
    {
        if (itemIndex >= 0 && itemIndex < bossItems.Length)
        {
            bossItems[itemIndex].dropChance = Mathf.Clamp(chance, 0f, 100f);
        }
    }

    // ボスの状態に応じてドロップ設定を変更
    public void SetBossPhase(int phase)
    {
        switch (phase)
        {
            case 1: // フェーズ1：通常
                SetDropChance(0, 20f); // レベルアップ 20%
                SetDropChance(1, 10f); // バリア 10%
                break;

            case 2: // フェーズ2：中程度
                SetDropChance(0, 30f); // レベルアップ 30%
                SetDropChance(1, 20f); // バリア 20%
                break;

            case 3: // フェーズ3：高確率
                SetDropChance(0, 50f); // レベルアップ 50%
                SetDropChance(1, 40f); // バリア 40%
                break;
        }
    }

    // デバッグ用：スポーン範囲を可視化
    void OnDrawGizmosSelected()
    {
        if (dropPoint == null)
            dropPoint = transform;

        // ドロップ地点を緑で表示
        Gizmos.color = Color.green;
        Vector3 center = dropPoint.position + new Vector3(spawnOffset.x, spawnOffset.y, 0f);
        Gizmos.DrawWireSphere(center, 0.2f);

        // スポーン範囲を青で表示
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(center, spawnRadius);

        // ボスとの接続線を表示
        if (bossTransform != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(bossTransform.position, center);
        }
    }
}