using UnityEngine;
using UnityEngine.UI;

public class fever : MonoBehaviour
{
    [SerializeField, Header("始まるときの音")] AudioClip StartFeverSE;
    [SerializeField, Header("フィーバー中になる音")] AudioClip FeverModeSE;

    // AudioSourceをキャッシュするための変数
    private AudioSource audioSource;
    private float FeverMode = 1.0f;
    private Image imageComponent;
    float Fever;
    // フィーバー中にゲージが減る速度
    [SerializeField, Header("フィーバー中のゲージ減少速度")]
    // フィーバー中かどうかを判定するフラグ
    private bool isFever = false;
    // 現在のフィーバーゲージの値を保持（0.0f ~ 1.0f）
    private float feverPoint = 0f;
    private float feverDecreaseSpeed = 0.1f;
    private void Start()
    {
        imageComponent = GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();
        // 初期化
        feverPoint = 0f;
        isFever = false;
        UpdateFeverGauge();

        if (imageComponent == null)
        {
            Debug.LogError("Imageコンポーネントが見つかりません！");
        }

    }

    /// <summary>
    /// 他のスクリプトから呼び出してフィーバーゲージを増やすための関数
    /// </summary>
    /// <param name="amount">増やす量</param>
    public void AddFeverPoint(float amount)
    {
        // すでにフィーバー中ならゲージを増やさない
        if (isFever) return;

        feverPoint += amount;

        // ゲージが最大値を超えたらフィーバー開始
        if (feverPoint >= 1.0f)
        {
            feverPoint = 1.0f;
            StartFever();
        }

        // UIに反映
        UpdateFeverGauge();
    }

    private void Update()
    {
        // フィーバー中でなければ処理をしない
        if (!isFever)
        {
            return;
        }

        // --- フィーバー中の処理 ---
        // 時間経過でフィーバーポイントを減らす
        feverPoint -= feverDecreaseSpeed * Time.deltaTime;

        // ゲージが0になったらフィーバー終了
        if (feverPoint <= 0f)
        {
            feverPoint = 0f;
            EndFever();
        }

        //ここにプレイヤーに当たったアイテムの値が渡ってくる。

    }
    /// <summary>
    /// フィーバーを終了する処理
    /// </summary>
    private void EndFever()
    {
        isFever = false;
        Debug.Log("フィーバー終了");

        // BGMを停止
        audioSource.Stop();
        audioSource.loop = false;

        // ここに、フィーバー終了時の処理（速度を元に戻すなど）を記述する
    } /// <summary>
      /// フィーバーを開始する処理
      /// </summary>
    private void StartFever()
    {
        isFever = true;
        Debug.Log("フィーバースタート！");

        // フィーバー開始音を再生
        audioSource.PlayOneShot(StartFeverSE);

        // フィーバー中のBGMをループ再生
        audioSource.clip = FeverModeSE;
        audioSource.loop = true;
        audioSource.Play();

        // ここに、移動速度アップなどフィーバー中の特殊効果を記述する
    }
    /// <summary>
    /// ImageのFill Amountを更新する
    /// </summary>
    private void UpdateFeverGauge()
    {
        if (imageComponent != null)
        {
            imageComponent.fillAmount = feverPoint;
        }
    }
}
