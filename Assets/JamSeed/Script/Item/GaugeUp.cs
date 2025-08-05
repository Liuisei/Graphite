using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GaugeUp : MonoBehaviour
{
    [SerializeField] private float increaseAmount = 0.1f;
    [SerializeField] private float tweenDuration = 0.3f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        InGameScene.Instance._playerDataLiu.fibarCloneCurrentTime += increaseAmount;
        InGameScene.Instance._playerDataLiu.OnPlayerDataChanged.Invoke();

        // foreach (Transform child in other.transform.GetComponentsInChildren<Transform>())
        // {
        //     if (child.CompareTag("Gauge"))
        //     {
        //         Slider gaugeSlider = child.GetComponent<Slider>();
        //         if (gaugeSlider != null)
        //         {
        //             float targetValue = Mathf.Clamp01(gaugeSlider.value + increaseAmount);
        //
        //             // DOTweenでスムーズにスライダーを増加
        //             gaugeSlider.DOValue(targetValue, tweenDuration).SetEase(Ease.OutCubic);
        //
        //             Destroy(gameObject);
        //             return;
        //         }
        //         else
        //         {
        //             Debug.LogWarning("Gauge タグはあるが Slider コンポーネントが見つかりませんでした。");
        //         }
        //     }
        // }
        //
        // Debug.LogWarning("Player の子オブジェクトに Gauge タグが見つかりませんでした。");
    }
}
