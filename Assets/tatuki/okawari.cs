using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class okawari : MonoBehaviour
{
    [SerializeField] Image _okawariimage;
    [SerializeField] Image _feverGauge;
    public float okawarigauge = 0f;
    public float feverGauge = 0f;
    public bool IsFever = false;
    private float fillduration = 3f;
    private Coroutine _fillCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        _okawariimage.fillAmount = 0.1f;
        _feverGauge.fillAmount = 0.1f;
    }
    private void Update()
    {
        // 毎フレーム、時間経過でゲージを増やす
        okawarigauge += Time.deltaTime / fillduration;
        okawarigauge = Mathf.Clamp01(okawarigauge); // 0〜1の範囲に制限

        ChangeOkawari(); // UI反映
    }

   

    public void TakeOkawari()
    {
        okawarigauge += 0.1f;
        okawarigauge = Mathf.Clamp(okawarigauge, 0f, 1f);
        ChangeOkawari();
    }

    public void TakeFiverGauge()
    {
        if(IsFever)return;
        feverGauge += 0.34f;
        feverGauge = Mathf.Clamp(feverGauge, 0f, 1f);
        if (feverGauge >= 1f) IsFever = true;
    }

    public void FeverReset()
    {
      if(IsFever)  IsFever = false;
        feverGauge = 0.1f;
        ChangeFiverGauge();
    }

    public void ChangeFiverGauge()
    {
        _feverGauge.fillAmount = feverGauge;
    }

    public void ChangeOkawari()
    {
        _okawariimage.fillAmount = okawarigauge;
    }

    public float GetOkawari() => okawarigauge;
    public float GetFeverGauge() => feverGauge;
}