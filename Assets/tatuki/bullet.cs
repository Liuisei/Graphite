using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 3f;
    Vector3 Velocity;
 private okawari okawari;

    private void Start()
    {
        okawari = FindAnyObjectByType<okawari>();
        Destroy(gameObject, lifetime); // 一定時間後に削除
       Velocity = new (0 ,0, -1);

    }

    private void Update()
    {
        // 前方向(Z軸)にまっすぐ進む
        transform.Translate(Velocity* speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            okawari.TakeOkawari();
            okawari.GetOkawari();
            Debug.Log(okawari.GetOkawari());
        Destroy(this.gameObject);
        }
    }
}
