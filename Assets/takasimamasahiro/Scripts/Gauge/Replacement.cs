using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Replacement : MonoBehaviour
{
    [SerializeField,Header("毎秒プラスされる値")] float PlasGauge = 0.05f;
    private UnityEngine.UI.Image imageComponent;
    float currentFillAmount;
    private void Start()
    {// このスクリプトがアタッチされているゲームオブジェクトからImageコンポーネントを取得
         imageComponent = GetComponent<UnityEngine.UI.Image>();
        currentFillAmount = 0;
        if (imageComponent != null)
        {
            // 現在のfillAmount値を取得してコンソールに出力
            currentFillAmount = imageComponent.fillAmount;
            Debug.Log("現在のFill Amount: " + currentFillAmount);

            // 例：fillAmountの値を半分にする
            // imageComponent.fillAmount = 0.5f;
        }
        else
        {
            Debug.LogError("Imageコンポーネントが見つかりません！");
        }
    }
    private void FixUpdate()
    {
        if (currentFillAmount <= 1)
        {
            //ここに満タンになった通知
            currentFillAmount = 0;//ゼロに戻す
        }
        else
        {
            currentFillAmount += PlasGauge;
        }
    }
}
