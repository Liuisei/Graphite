using UnityEngine;

public class PlayerClone : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint1;
    public Transform firePoint2;
    public Transform firePoint3;
    public Transform firePoint4;
    public Transform firePoint5;
    public Transform target;
    public Transform target1;
    public Transform target2;
    public float shootInterval = 2f;
    public int level = 0;
  
   


    private float shootTimer = 0f;


    private void Start()
    {
       
        Destroy(gameObject, 5f); // 5秒後に消滅
    }

    private void Update()
    {
       
      
        shootTimer += Time.deltaTime;
        if (shootTimer > shootInterval)
        {

            Debug.Log($"{shootTimer} / {shootInterval}"); // ←ここに移動
            shootTimer = 0f;
            Shoot();

        }
    }
    public void SetLevel(int lvl)
    {
        level = lvl;
    }


    public void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firePoint1.position, firePoint1.rotation);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.SetDirection(-firePoint1.forward);
            Destroy(bulletObj, 5f);// 右方向に飛ばす例
        }
        GameObject bulletObj2 = Instantiate(bulletPrefab, firePoint2.position, firePoint2.rotation);
        Bullet bullet2 = bulletObj2.GetComponent<Bullet>();
        if (bullet2 != null)
        {
            bullet2.SetDirection(-firePoint2.forward);
            Destroy(bulletObj2, 5f);
        }
        if (level == 1)
        {
            GameObject bulletObj3 = Instantiate(bulletPrefab, firePoint5.position, Quaternion.identity);
            Bullet bullet3 = bulletObj3.GetComponent<Bullet>();
            if (bullet3 != null)
            {
                Vector3 direction = -firePoint5.forward;  // firePoint3の向いている方向（Z+方向）
                direction.y = 0;                         // Yを0にしてXZ平面に固定
                direction.Normalize();                   // 正規化

                bullet3.SetDirection(direction);
                Destroy(bulletObj3, 5f);
            }


            GameObject bulletObj4 = Instantiate(bulletPrefab, firePoint4.position, Quaternion.identity); // 回転不要なら identity
            Bullet bullet4 = bulletObj4.GetComponent<Bullet>();
            if (bullet4 != null)
            {
                // ターゲット方向ベクトル（正規化）
                Vector3 direction = (target1.position - firePoint4.position).normalized;
                direction.y = 0;
                bullet4.SetDirection(direction);
                Destroy(bulletObj4, 5f);
            }


            GameObject bulletObj5 = Instantiate(bulletPrefab, firePoint5.position, Quaternion.identity); // 回転不要なら identity
            Bullet bullet5 = bulletObj5.GetComponent<Bullet>();
            if (bullet5 != null)
            {
                // ターゲット方向ベクトル（正規化）
                Vector3 direction = (target.position - firePoint5.position).normalized;
                direction.y = 0;
                bullet5.SetDirection(direction);
                Destroy(bulletObj5, 5f);
            }

        }
    }
}