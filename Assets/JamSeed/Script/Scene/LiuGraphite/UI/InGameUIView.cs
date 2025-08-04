using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIView : MonoBehaviour
{
    public Slider[] _hpbar;

    public TextMeshProUGUI _LVText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
             SetHPBar(InGameScene.Instance._playerDataLiu.CurrentPlayerHP); // Example: Set HP bar to full (10 HP)
        SetLVText(InGameScene.Instance._playerDataLiu._playerLevel.ToString()); // Example: Set LV text to 1
    }


    /// <summary>
    /// 0 - 10
    /// </summary>
    /// <param name="hp"></param>
    public void SetHPBar(int hp)
    {
        Debug.Log($"SetHPBar: {hp}");
        for (int i = 0; i < hp/2; i++)
        {
            _hpbar[i].value = 1;
        }

        if (hp % 2 == 1)
        {
            _hpbar[0].value = 0.5f;
        }
    }

    public void SetLVText(string lv)
    {
        _LVText.text = lv;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
