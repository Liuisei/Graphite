using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class StarWarsCreditController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image _creditImage;   // ← Sprite表示用に変更
    [SerializeField] private Image _endImage;

    [Header("Credit Settings")]
    [SerializeField] private Sprite[] _creditSprites;  // ← Sprite配列に変更
    [SerializeField] private float _scrollDistance = 100f;
    [SerializeField] private float _scrollDuration = 3f;
    [SerializeField] private float _delayBetweenCredits = 1f;
    [SerializeField] private Vector3 _startPosition;

    public async UniTaskVoid PlayCreditsAsync()
    {
        _creditImage.transform.localPosition = _startPosition;
        _endImage.gameObject.SetActive(false);
        _creditImage.gameObject.SetActive(true);

        foreach (Sprite creditSprite in _creditSprites)
        {
            _creditImage.sprite = creditSprite;
            _creditImage.transform.localPosition = _startPosition;

            Vector3 targetPosition = _startPosition + new Vector3(0, 0, _scrollDistance);

            float elapsed = 0f;
            while (elapsed < _scrollDuration)
            {
                float t = elapsed / _scrollDuration;
                _creditImage.transform.localPosition = Vector3.Lerp(_startPosition, targetPosition, t);
                elapsed += Time.deltaTime;
                await UniTask.Yield();
            }

            await UniTask.Delay(System.TimeSpan.FromSeconds(_delayBetweenCredits));
        }

        _creditImage.gameObject.SetActive(false);
        _endImage.gameObject.SetActive(true);
    }
}