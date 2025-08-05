using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StarWarsCreditController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _creditText;
    [SerializeField] private Image _endImage;

    [Header("Credit Settings")]
    [SerializeField] private string[] _creditNames;
    [SerializeField] private float _scrollDistance = 100f;
    [SerializeField] private float _scrollDuration = 3f;
    [SerializeField] private float _delayBetweenCredits = 1f;
    [SerializeField] private Vector3 _startPosition;

    public async UniTaskVoid PlayCreditsAsync()
    {
        _creditText.transform.localPosition = _startPosition;
        _endImage.gameObject.SetActive(false);
        _creditText.gameObject.SetActive(true);

        foreach (string creditName in _creditNames)
        {
            _creditText.text = creditName;
            _creditText.transform.localPosition = _startPosition;

            Vector3 targetPosition = _startPosition + new Vector3(0, 0, _scrollDistance);

            float elapsed = 0f;
            while (elapsed < _scrollDuration)
            {
                float t = elapsed / _scrollDuration;
                _creditText.transform.localPosition = Vector3.Lerp(_startPosition, targetPosition, t);
                elapsed += Time.deltaTime;
                await UniTask.Yield();
            }

            await UniTask.Delay(System.TimeSpan.FromSeconds(_delayBetweenCredits));
        }

        // 最後の演出
        _creditText.gameObject.SetActive(false);
        _endImage.gameObject.SetActive(true);
    }
}