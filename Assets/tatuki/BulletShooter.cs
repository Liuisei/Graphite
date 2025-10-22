using UnityEngine;
using System.Collections;

public class BulletShooter : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireInterval = 0.2f; // 連射間隔
    [SerializeField] private float shootDuration = 3f;  // 撃ち続ける時間

    public void Start()
    {
        StartShooting();
    }
    public void StartShooting()
    {
        StartCoroutine(ShootForSeconds());
    }

    private IEnumerator ShootForSeconds()
    {
        float elapsed = 0f;

        while (elapsed < shootDuration)
        {
            if (bulletPrefab != null && firePoint != null)
            {
                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            }

            yield return new WaitForSeconds(fireInterval);
            elapsed += fireInterval;
        }
        Destroy(this.gameObject);
    }
}
