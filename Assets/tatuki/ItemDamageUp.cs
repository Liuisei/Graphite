using System;
using Unity.VisualScripting;
using UnityEngine;

public class ItemDamageUp : MonoBehaviour
{
    [SerializeField]PlayerTest1 _playerTest1;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerTest1.AddDamage(5);
        }
    }
}
