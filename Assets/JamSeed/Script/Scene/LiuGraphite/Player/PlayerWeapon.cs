using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] BulletOrigin bulletPrefab; // 弾のプレハブ
    [SerializeField] int speed = 10; // 弾の速度
    [SerializeField] int damage = 20; // 弾のダメージ


    [SerializeField] GameObject bulletPrefab2;//レーザー
    [SerializeField] Transform firepoint;
    [SerializeField] Transform[] fireVectals; // 発射方向

    public WeaponLevel weaponLevel = WeaponLevel.Bullet;


    public void PlayerFire()
    {
        switch (weaponLevel)
        {
            case WeaponLevel.Bullet:
                FireLevel1();
                break;
            case WeaponLevel.Bullet2:
                FireLevel2();
                break;
            case WeaponLevel.Laser:
                FireLevel3();
                break;
            default:
                break;
        }
    }

    public void FireLevel1()
    {
        for (int i = 0; i < 3; i++)
        {
            BulletOrigin bullet = Instantiate(bulletPrefab, firepoint.position, Quaternion.identity);
            //bullet.transform.forward = fireVectals[i].position - firepoint.position; // 向きを設定
            bullet.Initialize((fireVectals[i].position - firepoint.position), speed, damage);
        }
    }
    public void FireLevel2()
    {
        for (int i = 0; i < 5; i++)
        {
            BulletOrigin bullet = Instantiate(bulletPrefab, firepoint.position, Quaternion.identity);
            bullet.transform.forward = fireVectals[i].position - firepoint.position; // 向きを設定
            bullet.Initialize((fireVectals[i].position - firepoint.position), speed, damage);
        }
    }
    public void FireLevel3()
    {

    }

    public enum WeaponLevel
    {
        Bullet,
        Bullet2,
        Laser
    }
}


