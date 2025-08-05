using UnityEngine;
using UnityEngine.UI;



public class PlayerCont : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public PlayerWeapon playerWeapon;

    //ui
    public Slider sliderClone;
    public Slider sliderFebarl;

    private void OnEnable()
    {
        InGameScene.Instance._playerDataLiu.OnPlayerDataChanged += UpdateUISlider;
    }

    private void OnDisable()
    {
        if (InGameScene.Instance != null && InGameScene.Instance._playerDataLiu != null)
        {
            InGameScene.Instance._playerDataLiu.OnPlayerDataChanged -= UpdateUISlider;
        }
    }

    private void UpdateUISlider()
    {
        sliderClone.value = InGameScene.Instance._playerDataLiu.cloneCurrentTime / InGameScene.Instance._playerDataLiu.cloneCooldownTime;
        sliderFebarl.value = InGameScene.Instance._playerDataLiu.fibarCloneCurrentTime / InGameScene.Instance._playerDataLiu.fibarCloneCooldownTime;
    }

}
