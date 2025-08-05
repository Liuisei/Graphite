using UnityEngine;
using System.Collections;

public class ItemSpoon : MonoBehaviour 
{
    [Header("スポーン設定")]
    public GameObject itemPrefab;           // スポーンするアイテムのプレハブ
    public Transform spawnArea;             // スポーンエリアを定義するTransform
    public float spawnInterval = 2f;        // スポーン間隔（秒）
    public float itemSpeed = 5f;           // アイテムの移動速度

    [Header("スポーンエリア設定")]
    public Vector2 spawnAreaSize = new Vector2(10f, 6f);  // スポーンエリアのサイズ

    [Header("アイテム設定")]
    public float itemLifeTime = 10;

    private bool isSpawning = true;
    void Start()
    {
        // スポーンエリアが設定されていない場合は自分のTransformを使用
        if (spawnArea == null)
            spawnArea = transform;

        // スポーンを開始
        StartCoroutine(SpawnItems());
    }
    IEnumerator SpawnItems()
    {
        while (isSpawning)
        {
            SpawnItem();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnItem()
    {

        if (itemPrefab == null)
        {
            Debug.LogWarning("アイテムプレハブが設定されていません！");
            return;
        }
        // スポーンエリアの右端でランダムなY座標を計算
        Vector3 spawnPosition = GetRandomSpawnPosition();

        // アイテムをスポーン
        GameObject spawnedItem = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);

        // アイテムに移動コンポーネントを追加（もしなければ）
        ItemMover mover = spawnedItem.GetComponent<ItemMover>();
        if (mover == null)
        {
            mover = spawnedItem.AddComponent<ItemMover>();
        }

        // 移動速度と生存時間を設定
        mover.SetSpeed(itemSpeed);
        mover.SetLifeTime(itemLifeTime);

    }
    Vector3 GetRandomSpawnPosition()
    {
        // スポーンエリアの中心位置
        Vector3 centerPosition = spawnArea.position;

        // 右端のX座標
        float rightEdgeX = centerPosition.x + (spawnAreaSize.x / 2f);

        // ランダムなY座標（上端から下端まで）
        float randomY = Random.Range(
            centerPosition.y - (spawnAreaSize.y / 2f),
            centerPosition.y + (spawnAreaSize.y / 2f)
        );

        return new Vector3(rightEdgeX, randomY, centerPosition.z);
    }

    // スポーンの開始/停止
    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnItems());
        }
    }


    // スポーン間隔を変更
    public void SetSpawnInterval(float interval)
    {
        spawnInterval = interval;
    }

    // アイテム速度を変更
    public void SetItemSpeed(float speed)
    {
        itemSpeed = speed;
    }

    // デバッグ用：スポーンエリアを可視化
    void OnDrawGizmosSelected()
    {
        if (spawnArea == null)
            spawnArea = transform;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(spawnArea.position, new Vector3(spawnAreaSize.x, spawnAreaSize.y, 0.1f));

        // スポーン位置（右端）を赤で表示
        Gizmos.color = Color.red;
        Vector3 rightEdge = new Vector3(
            spawnArea.position.x + (spawnAreaSize.x / 2f),
            spawnArea.position.y,
            spawnArea.position.z
        );
        Gizmos.DrawLine(
            new Vector3(rightEdge.x, rightEdge.y - spawnAreaSize.y / 2f, rightEdge.z),
            new Vector3(rightEdge.x, rightEdge.y + spawnAreaSize.y / 2f, rightEdge.z)
        );
    }

}
