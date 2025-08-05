using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIView : MonoBehaviour
{
    [SerializeField] private Slider[] _hpbar;         // HPバーのスライダー配列（5個想定）
    [SerializeField] private TextMeshProUGUI _LVText; // レベル表示用テキスト

    private const int MAX_HP_BARS = 5;       // HPバーの最大数
    private const int HP_PER_FULL_BAR = 2;   // 1つのHPバーが表すHP量（2）

    void Start()
    {
        // 参照の安全チェックを行い、問題なければイベント登録とUI初期更新
        if (ValidateReferences())
        {
            InGameScene.Instance._playerDataLiu.OnPlayerDataChanged += UpdateUI;
            UpdateUI();
        }
    }

    void OnDestroy()
    {
        // イベントの登録解除（メモリリーク防止）
        if (InGameScene.Instance != null && InGameScene.Instance._playerDataLiu != null)
        {
            InGameScene.Instance._playerDataLiu.OnPlayerDataChanged -= UpdateUI;
        }
    }

    // 参照のnullチェックなどの検証処理
    private bool ValidateReferences()
    {
        if (InGameScene.Instance == null)
        {
            Debug.LogError("InGameScene.Instance が null です！");
            return false;
        }

        if (InGameScene.Instance._playerDataLiu == null)
        {
            Debug.LogError("PlayerDataLiu が null です！");
            return false;
        }

        if (_hpbar == null || _hpbar.Length != MAX_HP_BARS)
        {
            Debug.LogError($"HPバー配列は正確に {MAX_HP_BARS} 個必要です！");
            return false;
        }

        if (_LVText == null)
        {
            Debug.LogError("レベル表示テキストが null です！");
            return false;
        }

        return true;
    }

    // プレイヤーデータの変化に合わせてUI更新
    private void UpdateUI()
    {
        if (!ValidateReferences()) return;

        SetHPBar(InGameScene.Instance._playerDataLiu.CurrentPlayerHP);
        SetLVText(InGameScene.Instance._playerDataLiu.playerLevel.ToString());
    }

    /// <summary>
    /// HPバーを更新する（HPは0〜10）
    /// 1バーあたり2HP：満タン＝2HP、半分＝1HP、空＝0HP
    /// </summary>
    /// <param name="hp">現在HP値</param>
    public void SetHPBar(int hp)
    {
        Debug.Log($"SetHPBar: {hp}");

        // HPを0〜最大HP範囲に制限
        hp = Mathf.Clamp(hp, 0, MAX_HP_BARS * HP_PER_FULL_BAR);

        int fullHeartCount = hp / HP_PER_FULL_BAR;  // 満タンバーの数
        int halfHeartCount = hp % HP_PER_FULL_BAR;  // 半分バーの数（0か1）
        int i = 0;

        // 満タンバーを設定
        for (; i < fullHeartCount && i < MAX_HP_BARS; i++)
        {
            _hpbar[i].value = 1f;
        }

        // 半分バーを設定
        if (halfHeartCount > 0 && i < MAX_HP_BARS)
        {
            _hpbar[i].value = 0.5f;
            i++;
        }

        // 残りのバーは空に設定
        for (; i < MAX_HP_BARS; i++)
        {
            _hpbar[i].value = 0f;
        }
    }

    // レベルテキストをセット
    public void SetLVText(string lv)
    {
        if (_LVText != null)
        {
            _LVText.text = $"LV {lv}";
        }
    }
}
