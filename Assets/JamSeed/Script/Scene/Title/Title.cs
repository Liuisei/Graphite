using JamSeed.Foundation;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
<<<<<<< HEAD
using UnityEngine.SceneManagement; // DOTween
=======
using JamSeed.Runtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
>>>>>>> 1da1f75117199e031dc37cdb3b885d9a8eba100b

public class Title : SceneSingleton<Title>
{
    [SerializeField] LiteButton startButton;
<<<<<<< HEAD
=======
    [SerializeField] LiteButton _creditButton;
    [SerializeField] private Image _creditsUI;
    [SerializeField] private AudioClip _clickSound;
    [SerializeField] private StarWarsCreditController _starWarsCreditController;
>>>>>>> 1da1f75117199e031dc37cdb3b885d9a8eba100b

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

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
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

            Debug.Log("Start Button Hovered");
        });

        startButton.AddOnExit(() =>
        {
            isHovering = false;

            startButton.transform
                .DOScale(Vector3.one, normalScaleDuration)
                .SetEase(Ease.OutBack);

            startButton.image
                .DOColor(normalColor, colorChangeDuration);

            Debug.Log("Start Button Exited");
        });
    }

    private async UniTaskVoid To2DInGame()
    {
        await UniTask.Delay(500); // 1秒待機
        SceneManager.LoadScene("4InGame2D");
    }

<<<<<<< HEAD
=======
    // Creditの表示
    private void OnClickCreditButton()
    {
        SoundManager.Instance.PlaySe(_clickSound);
        _creditsUI.gameObject.SetActive(true);
        LiteButton creditsBackButton = _creditsUI.GetComponentInChildren<LiteButton>();
        creditsBackButton.AddOnClick(OnClickCreditBackButton);
        _starWarsCreditController.gameObject.SetActive(true);
        _starWarsCreditController.PlayCreditsAsync().Forget();
    }

    // Creditの非表示
    private void OnClickCreditBackButton()
    {
        SoundManager.Instance.PlaySe(_clickSound);
        _creditsUI.gameObject.SetActive(false);
        _starWarsCreditController.gameObject.SetActive(false);
    }
>>>>>>> 1da1f75117199e031dc37cdb3b885d9a8eba100b
}
