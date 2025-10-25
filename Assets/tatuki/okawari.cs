using System;
using UnityEngine;
using UnityEngine.UI;

 public class okawari : MonoBehaviour
{
    [SerializeField] Image _okawariimage;
   public float gauge = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        _okawariimage.fillAmount = 0.1f;
    }

    public void  TakeOkawari()
    {

        gauge += 0.1f;
        gauge = Mathf.Clamp(gauge, 0f, 1f);
        ChangeOkawari();


    }

    public void ChangeOkawari()
    {
        _okawariimage.fillAmount = gauge;
    }

     public float GetOkawari() => gauge;


}
