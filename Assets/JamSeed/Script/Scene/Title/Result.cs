using Cysharp.Threading.Tasks;
using DG.Tweening;
using JamSeed.Runtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviour
{
    [SerializeField] LiteButton _retryButton;
    [SerializeField] LiteButton _titleButton;
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
        normalColor = _retryButton.image.color;

        _retryButton.AddOnClick(() =>
        {
            if (isLocked) return; // すでにロックされているなら何もしない
            SoundManager.Instance.PlaySe(_clickSound);
            _retryButton.image
                .DOColor(pressedColor, colorChangeDuration)
                .OnComplete(() =>
                {
                    // マウスがまだ上なら hoverColor、外なら normalColor
                    var targetColor = isHovering ? hoverColor : normalColor;
                    _retryButton.image.DOColor(targetColor, colorChangeDuration);
                });
            isLocked = true; // ボタンをロック
            To2DInGame().Forget();
        });

        _retryButton.AddOnEnter(() =>
        {
            isHovering = true;

            _retryButton.transform
                .DOScale(Vector3.one * hoverScale, hoverScaleDuration)
                .SetEase(Ease.OutBack);

            _retryButton.image
                .DOColor(hoverColor, colorChangeDuration);
        });

        _titleButton.AddOnEnter(() =>
        {
            isHovering = true;

            _titleButton.transform
                .DOScale(Vector3.one * hoverScale, hoverScaleDuration)
                .SetEase(Ease.OutBack);

            _titleButton.image
                .DOColor(hoverColor, colorChangeDuration);
        });

        _retryButton.AddOnExit(() =>
        {
            isHovering = false;

            _retryButton.transform
                .DOScale(Vector3.one, normalScaleDuration)
                .SetEase(Ease.OutBack);

            _retryButton.image
                .DOColor(normalColor, colorChangeDuration);

        });

        _titleButton.AddOnExit(() =>
        {
            isHovering = false;

            _titleButton.transform
                .DOScale(Vector3.one, normalScaleDuration)
                .SetEase(Ease.OutBack);

            _titleButton.image
                .DOColor(normalColor, colorChangeDuration);

        });

        _titleButton.AddOnClick(OnClickTitleButton);
    }


    private async UniTaskVoid To2DInGame()
    {
        await UniTask.Delay(500); // 1秒待機
        SceneManager.LoadScene("4InGame2D");
    }

    // Creditの表示
    private void OnClickTitleButton()
    {
        SoundManager.Instance.PlaySe(_clickSound);
        SceneManager.LoadScene("2Title");
    }
}