using System;
using UnityEngine;
using UnityEngine.UI;

public class okawari : MonoBehaviour
{
    [SerializeField] Image _okawariimage;
    [SerializeField] Image _feverGauge;
    public float okawarigauge = 0f;
    public float feverGauge = 0f;
    public bool IsFever = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        _okawariimage.fillAmount = 0.1f;
        _feverGauge.fillAmount = 0.1f;
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
        feverGauge = 0f;
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