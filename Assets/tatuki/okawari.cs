using System;
using UnityEngine;
using UnityEngine.UI;

 public class okawari : MonoBehaviour
{
    [SerializeField] Slider slider;
   public float gauge = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        slider.value = gauge;
    }

    public void TakeOkawari()
    {
        gauge += 0.1f;
        gauge = Mathf.Clamp(gauge, 0f, 1f);
        slider.value  = gauge;
    }

     public float GetOkawari() => gauge;


}
