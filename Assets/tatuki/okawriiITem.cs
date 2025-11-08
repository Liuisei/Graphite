using System;
using UnityEngine;

public class okawriiITem : MonoBehaviour
{
    protected okawari _okawari;

   protected void Start()
    {
        _okawari = FindAnyObjectByType<okawari>();
    }

    protected virtual void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _okawari.okawarigauge += 0.5f;
            _okawari.ChangeOkawari();
            Destroy(this.gameObject);
        }
    }
}
