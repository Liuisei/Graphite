using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    [Header("スポーナー設定")]
    [Tooltip("通常アイテムスポーナー（ゲージアップ用）")]
    public ItemSpawner normalItemSpawner;

    [Tooltip("ボスアイテムスポーナー（レベルアップ・バリア用）")]
    public BossItemSpawner bossItemSpawner;

    [Header("ゲーム状態")]
    public bool isGameActive = true;

    void Start()
    {
        InitializeSpawners();
    }

    void InitializeSpawners()
    {
        // 通常スポーナーを開始
        if (normalItemSpawner != null)
        {
            normalItemSpawner.StartSpawning();
            Debug.Log("通常アイテムスポーン開始（ゲージアップ）");
        }

        // ボススポーナーは手動制御（自動ドロップは通常無効）
        if (bossItemSpawner != null)
        {
            Debug.Log("ボスアイテムスポーナー準備完了（レベルアップ・バリア）");
        }
    }

    // ゲーム状態の制御
    public void StartGame()
    {
        isGameActive = true;

        if (normalItemSpawner != null)
            normalItemSpawner.StartSpawning();
    }

    public void StopGame()
    {
        isGameActive = false;

        if (normalItemSpawner != null)
            normalItemSpawner.StopSpawning();

        if (bossItemSpawner != null)
            bossItemSpawner.StopAutoDrop();
    }

    public void PauseGame()
    {
        if (normalItemSpawner != null)
            normalItemSpawner.StopSpawning();

        if (bossItemSpawner != null)
            bossItemSpawner.StopAutoDrop();
    }

    public void ResumeGame()
    {
        if (isGameActive)
        {
            if (normalItemSpawner != null)
                normalItemSpawner.StartSpawning();
        }
    }

    // ボス関連のイベント処理
    public void OnBossAppear()
    {
        Debug.Log("ボス出現 - 特別アイテムドロップ開始可能");

        // 必要に応じてボスの自動ドロップを開始
        // if (bossItemSpawner != null)
        //     bossItemSpawner.StartAutoDrop();
    }

    public void OnBossDefeat()
    {
        Debug.Log("ボス撃破 - 最終アイテムドロップ");

        // ボス撃破時に確実にアイテムをドロップ
        if (bossItemSpawner != null)
        {
            bossItemSpawner.DropAllItems();
            bossItemSpawner.StopAutoDrop();
        }
    }

    public void OnBossDamaged()
    {
        // ボスがダメージを受けた時にアイテムドロップ
        if (bossItemSpawner != null)
        {
            bossItemSpawner.DropItems();
        }
    }

    // 難易度調整
    public void SetDifficulty(int level)
    {
        // 通常アイテムの出現頻度調整
        if (normalItemSpawner != null)
        {
            float interval = Mathf.Lerp(3f, 1f, level / 10f); // レベルが上がると出現頻度アップ
            normalItemSpawner.SetSpawnInterval(interval);
        }

        // ボスアイテムの出現確率調整
        if (bossItemSpawner != null)
        {
            bossItemSpawner.SetBossPhase(Mathf.Min(level / 3, 3)); // 3レベルごとにフェーズアップ
        }
    }

    // 特別イベント
    public void TriggerBonusRain()
    {
        // ボーナスタイム：アイテムを大量スポーン
        if (normalItemSpawner != null)
        {
            for (int i = 0; i < 5; i++)
            {
                normalItemSpawner.SpawnSpecificItem(0); // ゲージアップを5個
            }
        }
    }

    public void TriggerBossRage()
    {
        // ボス激怒：特別アイテムを確実にドロップ
        if (bossItemSpawner != null)
        {
            bossItemSpawner.SetDropChance(0, 100f); // レベルアップ確定
            bossItemSpawner.SetDropChance(1, 80f);  // バリア高確率
            bossItemSpawner.DropItems();
        }
    }

    // デバッグ用メソッド
    [ContextMenu("Test Normal Item Spawn")]
    public void TestNormalSpawn()
    {
        if (normalItemSpawner != null)
            normalItemSpawner.SpawnSpecificItem(0);
    }

    [ContextMenu("Test Boss Item Drop")]
    public void TestBossDrop()
    {
        if (bossItemSpawner != null)
            bossItemSpawner.DropAllItems();
    }

    [ContextMenu("Test Bonus Rain")]
    public void TestBonusRain()
    {
        TriggerBonusRain();
    }
}
