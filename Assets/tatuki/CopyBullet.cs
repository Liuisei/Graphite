using UnityEngine;

public class CopyBullet : Bullet
{
    protected override void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            // おかわり呼ばない！
            Destroy(gameObject);
        }
    }
}
