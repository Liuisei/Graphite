using System.Collections;
using UnityEngine;

    public class ItemSpawner : MonoBehaviour
    {
        [System.Serializable]
    public class ItemType
    {
        [Header("アイテム設定")]
        public GameObject prefab;              // アイテムプレハブ
        public string itemName;                // アイテム名（デバッグ用）
        public float speed = 5f;               // 移動速度
        public float lifeTime = 10f;           // 生存時間

        [Header("スポーン確率")]
        [Range(0f, 100f)]
        public float spawnWeight = 33.3f;      // スポーン確率の重み
    }

    [Header("アイテム種類")]
    [Tooltip("通常スポーン用アイテム（ゲージアップなど）")]
    public ItemType[] itemTypes = new ItemType[1]
    {
        new ItemType { itemName = "ゲージアップ", spawnWeight = 100f }
    };

    [Header("スポーン設定")]
    public Transform spawnArea;             // スポーンエリアを定義するTransform
    public float spawnInterval = 2f;        // スポーン間隔（秒）

    [Header("スポーンエリア設定")]
    public Vector3 spawnAreaSize = new Vector3(10f,0f, 6f);  // スポーンエリアのサイズ

    private bool isSpawning = true;

    void Start()
    {
        // スポーンエリアが設定されていない場合は自分のTransformを使用
        if (spawnArea == null)
            spawnArea = transform;

        // アイテム設定の初期化
        InitializeItemTypes();

        // スポーンを開始
        StartCoroutine(SpawnItems());
    }

    void InitializeItemTypes()
    {
        // デフォルト名を設定（プレハブが設定されている場合）
        for (int i = 0; i < itemTypes.Length; i++)
        {
            if (itemTypes[i].prefab != null && string.IsNullOrEmpty(itemTypes[i].itemName))
            {
                itemTypes[i].itemName = itemTypes[i].prefab.name;
            }
        }
    }

    IEnumerator SpawnItems()
    {
        while (isSpawning)
        {
            SpawnRandomItem();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnRandomItem()
    {
        // 重み付きランダム選択でアイテムタイプを決定
        ItemType selectedType = GetRandomItemType();

        if (selectedType == null || selectedType.prefab == null)
        {
            Debug.LogWarning("有効なアイテムプレハブが設定されていません！");
            return;
        }

        // スポーン位置を決定
        Vector3 spawnPosition = GetRandomSpawnPosition();

        // アイテムをスポーン
        GameObject spawnedItem = Instantiate(selectedType.prefab, spawnPosition, Quaternion.identity);

        // アイテムに移動コンポーネントを設定
        SetupItemMover(spawnedItem, selectedType);

        Debug.Log($"通常スポーン: {selectedType.itemName} をスポーンしました");
    }

    void SetupItemMover(GameObject item, ItemType itemType)
    {
        // ItemMoverコンポーネントを取得または追加
        ItemMover mover = item.GetComponent<ItemMover>();
        if (mover == null)
        {
            mover = item.AddComponent<ItemMover>();
        }

        // 各アイテムタイプの設定を適用
        mover.SetSpeed(itemType.speed);
        mover.SetLifeTime(itemType.lifeTime);
        mover.SetDirection(Vector2.left); // 右から左へ
    }

    ItemType GetRandomItemType()
    {
        // 総重みを計算
        float totalWeight = 0f;
        foreach (var item in itemTypes)
        {
            if (item.prefab != null)
                totalWeight += item.spawnWeight;
        }

        if (totalWeight <= 0f)
            return null;

        // ランダム値を生成
        float randomValue = Random.Range(0f, totalWeight);

        // 重み付きランダム選択
        float currentWeight = 0f;
        foreach (var item in itemTypes)
        {
            if (item.prefab != null)
            {
                currentWeight += item.spawnWeight;
                if (randomValue <= currentWeight)
                {
                    return item;
                }
            }
        }

        // フォールバック（最初の有効なアイテム）
        foreach (var item in itemTypes)
        {
            if (item.prefab != null)
                return item;
        }

        return null;
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 center = spawnArea.position;

        float rightEdgeX = center.x + (spawnAreaSize.x / 2f);

        float randomY = Random.Range(
            center.y - (spawnAreaSize.y / 2f),
            center.y + (spawnAreaSize.y / 2f)
        );

        float randomZ = Random.Range(
            center.z - (spawnAreaSize.z / 2f),
            center.z + (spawnAreaSize.z / 2f)
        );

        return new Vector3(rightEdgeX, randomY, randomZ);
    }

    // 公開メソッド
    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnItems());
        }
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }

    public void SetSpawnInterval(float interval)
    {
        spawnInterval = interval;
    }

    public void SpawnSpecificItem(int itemIndex)
    {
        if (itemIndex >= 0 && itemIndex < itemTypes.Length)
        {
            var itemType = itemTypes[itemIndex];
            if (itemType.prefab != null)
            {
                Vector3 spawnPosition = GetRandomSpawnPosition();
                GameObject spawnedItem = Instantiate(itemType.prefab, spawnPosition, Quaternion.identity);
                SetupItemMover(spawnedItem, itemType);
            }
        }
    }

    // スポーン確率を動的に変更
    public void SetItemSpawnWeight(int itemIndex, float weight)
    {
        if (itemIndex >= 0 && itemIndex < itemTypes.Length)
        {
            itemTypes[itemIndex].spawnWeight = weight;
        }
    }

    // デバッグ用：スポーンエリアを可視化
    void OnDrawGizmosSelected()
    {
        if (spawnArea == null)
            spawnArea = transform;

        // スポーンエリア全体
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(spawnArea.position, spawnAreaSize);

        // 右端スポーン位置をZ方向にライン表示
        Gizmos.color = Color.red;
        Vector3 rightEdgeCenter = new Vector3(
            spawnArea.position.x + (spawnAreaSize.x / 2f),
            spawnArea.position.y,
            spawnArea.position.z
        );

        Vector3 topZ = rightEdgeCenter + new Vector3(0, spawnAreaSize.y / 2f, spawnAreaSize.z / 2f);
        Vector3 bottomZ = rightEdgeCenter + new Vector3(0, -spawnAreaSize.y / 2f, -spawnAreaSize.z / 2f);
        Gizmos.DrawLine(topZ, bottomZ);
    }
}