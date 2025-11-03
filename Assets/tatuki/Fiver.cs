using System;
using UnityEngine;

public class Fiver : okawriiITem
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _okawari.TakeFiverGauge();
            _okawari.ChangeFiverGauge();
            Destroy(gameObject);
        }
    }
}
