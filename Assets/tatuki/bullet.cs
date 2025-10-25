using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected float speed = 10f;
    [SerializeField] protected float lifetime = 3f;
   protected Vector3 Velocity;
 protected okawari okawari;

    protected virtual void Start()
    {
        okawari = FindAnyObjectByType<okawari>();
        Destroy(gameObject, lifetime); // 一定時間後に削除
       Velocity = new (0 ,0, -1);

    }

    protected void Update()
    {
        // 前方向(Z軸)にまっすぐ進む
        transform.Translate(Velocity* speed * Time.deltaTime);
    }

    protected virtual void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("EnemyBullet"))
        {

            okawari.TakeOkawari();
            okawari.GetOkawari();


        Destroy(this.gameObject);
        }
    }
}
