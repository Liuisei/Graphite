using UnityEngine;

public class PlayerClone : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint1;
    public Transform firePoint2;
    public Transform firePoint4;
    public Transform firePoint5;
    public Transform target;
    public Transform target2;
    public float shootInterval = 2f;
    public int level = 0;
    public bool Ishot = false;

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

    void Shoot()
    {
        Debug.Log($"{shootTimer}{shootInterval}");

        GameObject bulletObj = Instantiate(bulletPrefab, firePoint1.position, firePoint1.rotation);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.SetDirection(firePoint1.right);
        }

        GameObject bulletObj2 = Instantiate(bulletPrefab, firePoint2.position, firePoint2.rotation);
        Bullet bullet2 = bulletObj2.GetComponent<Bullet>();
        if (bullet2 != null)
        {
            bullet2.SetDirection(firePoint2.right);
        }

        if (level == 1)
        {
            GameObject bulletObj4 = Instantiate(bulletPrefab, firePoint4.position, Quaternion.identity);
            Bullet bullet4 = bulletObj4.GetComponent<Bullet>();
            if (bullet4 != null)
            {
                Vector3 direction = (target2.position - firePoint4.position).normalized;
                direction.y = 0;
                bullet4.SetDirection(direction);
            }

            GameObject bulletObj5 = Instantiate(bulletPrefab, firePoint5.position, Quaternion.identity);
            Bullet bullet5 = bulletObj5.GetComponent<Bullet>();
            if (bullet5 != null)
            {
                Vector3 direction = (target.position - firePoint5.position).normalized;
                direction.y = 0;
                bullet5.SetDirection(direction);
            }
        }
    }
}
