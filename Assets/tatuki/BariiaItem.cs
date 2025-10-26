using System;
using UnityEngine;

public class BariiaItem : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            Destroy(other.gameObject);
        }
    }
}
