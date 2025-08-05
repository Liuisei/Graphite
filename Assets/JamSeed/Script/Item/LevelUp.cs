using UnityEngine;

public class LevelUp : MonoBehaviour
{
    [SerializeField] private int increaseAmount = 1;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        var data = InGameScene.Instance._playerDataLiu;

        // 現在のレベルを取得
        int currentLevel = (int)data.playerLevel;
        int maxLevel = System.Enum.GetValues(typeof(PlayerDataLiu.PlayerLevel)).Length - 1;

        // 増加後のレベルを Clamp
        int nextLevel = Mathf.Clamp(currentLevel + increaseAmount, 0, maxLevel);

        // レベルが変化した場合のみ更新
        if (nextLevel != currentLevel)
        {
            data.playerLevel = (PlayerDataLiu.PlayerLevel)nextLevel;

            Debug.Log($"レベルアップ！ → {data.playerLevel}");
            InGameScene.Instance._playerDataLiu.OnPlayerDataChanged.Invoke();
        }

        // 自分を破棄（1回きりのアイテムとして）
        Destroy(gameObject);
    }
}
