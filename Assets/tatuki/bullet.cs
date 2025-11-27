using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected float speed = 10f;
    [SerializeField] protected float lifetime = 3f;

    [SerializeField] protected PlayerTest1  playerTest;
    protected Vector3 Velocity;
    protected okawari okawari;
    protected int damage = 0;
    protected virtual void Start()
    {

        ChangeDamage();
        okawari = FindAnyObjectByType<okawari>();
        Destroy(gameObject, lifetime); // 一定時間後に削除
        Velocity = new(-1, 0, 0);
    }

    protected void Update()
    {
        // 前方向(Z軸)にまっすぐ進む
        transform.Translate(Velocity * speed * Time.deltaTime);
        transform.localRotation = new Quaternion(0f, 180f, 0f, 1f);
    }

    protected void ChangeDamage()
    {
        damage = playerTest.Damage;
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