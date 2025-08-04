using JamSeed.Runtime;
using System.Collections;
using UnityEngine;

public class BossManager : MonoBehaviour
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
    public AudioClip houdan;

    [Header("Thunder")]
    public GameObject thunderWarningPrefab;
    public GameObject thunderPrefab;
    public Transform thunderFirePoint;
    public float thunderWarningTime = 3f;
    public float thunderRate = 5f;
    public float thunderTimer = 0f;
    public AudioClip thunder;


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
        HandleThunder();
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
            SoundManager.Instance.PlaySe(houdan);
            Instantiate(bulletPrefab, firePoint.position, rotation);
        }
    }

    public void HandleThunder()
    {
        if (!hasEntered) return;

        thunderTimer += Time.deltaTime;
        if(thunderTimer >= thunderRate)
        {
            thunderTimer = 0f;
            StartThunderAttack();
        }
    }

    private void StartThunderAttack()
    {
        StartCoroutine(ThunderAttackCoroutine());
    }

    private IEnumerator ThunderAttackCoroutine()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null) yield break;

        Vector3 dir = (player.transform.position - thunderFirePoint.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        //Step 1 : Warning
        GameObject preview = Instantiate(thunderWarningPrefab, thunderFirePoint.position, rotation);
        Destroy(preview, thunderWarningTime);

        yield return new WaitForSeconds(thunderWarningTime);

        //Step 2 : Thunder
        SoundManager.Instance.PlaySe(thunder);
        Instantiate(thunderPrefab, thunderFirePoint.position, rotation);

    }

}
