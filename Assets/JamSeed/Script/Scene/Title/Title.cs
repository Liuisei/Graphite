using JamSeed.Foundation;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using JamSeed.Runtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // DOTween

public class Title : SceneSingleton<Title>
{
    [SerializeField] LiteButton startButton;
    [SerializeField] LiteButton _creditButton;
    [SerializeField] private Image _creditsUI;
    [SerializeField] private AudioClip _clickSound;

    // アニメーション時間
    [SerializeField] float hoverScaleDuration = 0.2f;
    [SerializeField] float normalScaleDuration = 0.2f;
    [SerializeField] float colorChangeDuration = 0.15f;

    // スケール値
    [SerializeField] float hoverScale = 1.2f;

    // 色
    [SerializeField] Color hoverColor = Color.white;
    Color normalColor;
    [SerializeField] Color pressedColor = Color.black;

    // マウスが今ボタン上にあるか
    bool isHovering = false;

    //操作不可な状態にするためのフラグ
    bool isLocked = false;

    void Start()
    {
        normalColor = startButton.image.color;

        startButton.AddOnClick(() =>
        {
            if (isLocked) return; // すでにロックされているなら何もしない
            SoundManager.Instance.PlaySe(_clickSound);
            startButton.image
                .DOColor(pressedColor, colorChangeDuration)
                .OnComplete(() =>
                {
                    // マウスがまだ上なら hoverColor、外なら normalColor
                    var targetColor = isHovering ? hoverColor : normalColor;
                    startButton.image.DOColor(targetColor, colorChangeDuration);
                });
            isLocked = true; // ボタンをロック
            To2DInGame().Forget();
        });

        startButton.AddOnEnter(() =>
        {
            isHovering = true;

            startButton.transform
                .DOScale(Vector3.one * hoverScale, hoverScaleDuration)
                .SetEase(Ease.OutBack);

            startButton.image
                .DOColor(hoverColor, colorChangeDuration);
        });

        _creditButton.AddOnEnter(() =>
        {
            isHovering = true;

            _creditButton.transform
                .DOScale(Vector3.one * hoverScale, hoverScaleDuration)
                .SetEase(Ease.OutBack);

            _creditButton.image
                .DOColor(hoverColor, colorChangeDuration);
        });

        startButton.AddOnExit(() =>
        {
            isHovering = false;

            startButton.transform
                .DOScale(Vector3.one, normalScaleDuration)
                .SetEase(Ease.OutBack);

            startButton.image
                .DOColor(normalColor, colorChangeDuration);

        });

        _creditButton.AddOnExit(() =>
        {
            isHovering = false;

            _creditButton.transform
                .DOScale(Vector3.one, normalScaleDuration)
                .SetEase(Ease.OutBack);

            _creditButton.image
                .DOColor(normalColor, colorChangeDuration);

        });

        _creditButton.AddOnClick(OnClickCreditButton);
    }


    private async UniTaskVoid To2DInGame()
    {
        await UniTask.Delay(500); // 1秒待機
        SceneManager.LoadScene("4InGame2D");
    }

    // Creditの表示
    private void OnClickCreditButton()
    {
        SoundManager.Instance.PlaySe(_clickSound);
        _creditsUI.gameObject.SetActive(true);
        LiteButton creditsBackButton = _creditsUI.GetComponentInChildren<LiteButton>();
        creditsBackButton.AddOnClick(OnClickCreditBackButton);
    }

    // Creditの非表示
    private void OnClickCreditBackButton()
    {
        SoundManager.Instance.PlaySe(_clickSound);
        _creditsUI.gameObject.SetActive(false);
    }
}
