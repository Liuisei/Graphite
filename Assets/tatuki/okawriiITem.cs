using System;
using UnityEngine;

public class okawriiITem : MonoBehaviour
{
    private okawari _okawari;

    private void Start()
    {
        _okawari = FindAnyObjectByType<okawari>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _okawari.gauge += 0.5f;
            _okawari.ChangeOkawari();
            Destroy(this.gameObject);
        }
    }
}
