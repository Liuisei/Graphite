using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 1f;
    public float minY = -5f;
    public float maxY = 5f;
    public float startReturnY = 5f;


    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    public float fireTimer = 0f;
    public float[] shotAngles;

    private bool hasEntered = false;
    private float direction = -1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleShooting();
    }

    void HandleMovement()
    {
        if (!hasEntered)
        {
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

            if (transform.position.y <= startReturnY)
            {
                hasEntered = true;
            }
        }
        else
        {
            transform.Translate(Vector3.up * direction * moveSpeed * Time.deltaTime);

            if (transform.position.y >= maxY - 0.01f && direction > 0f)
            {
                direction = -1f;
            }
            else if (transform.position.y <= minY + 0.01f && direction < 0f)
            {
                direction = 1f;
            }
        }
    }

    void HandleShooting()
    {
        if (!hasEntered) return;

        fireTimer += Time.deltaTime;
        if(fireTimer >= 1f / fireRate)
        {
            fireTimer = 0f;
            Shoot();
        }
    }

    void Shoot()
    {
        foreach (float angle in shotAngles)
        {
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

            Instantiate(bulletPrefab, firePoint.position, rotation);
        }
    }
}
