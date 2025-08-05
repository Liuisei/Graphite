using JamSeed.Runtime;
using System.Collections;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 1f;
    public float minZ = -5f;
    public float maxZ = 5f;
    public float startReturnZ = 5f;


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
    public AudioClip thunderClip;


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
            transform.position += new Vector3(0, 0, direction * moveSpeed * Time.deltaTime);

            if (transform.position.z <= startReturnZ)
            {
                hasEntered = true;
            }
        }
        else
        {
            transform.position += new Vector3(0, 0, direction * moveSpeed * Time.deltaTime);

            if (transform.position.z >= maxZ - 0.01f && direction > 0f)
            {
                direction = -1f;
            }
            else if (transform.position.z <= minZ + 0.01f && direction < 0f)
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
            Quaternion rotation = Quaternion.Euler(0f, angle, 0f);
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

        Vector3 start = thunderFirePoint.position;
        Vector3 end = player.transform.position;
        Vector3 direction = (end - start).normalized;
        float distance = Vector3.Distance(start, end);
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);


        // Step 1: Warning
        GameObject warning = Instantiate(thunderWarningPrefab, start, rotation);
        // Ajuste la scale du warning (par exemple sur l'axe X si ton prefab est aligné horizontalement)
        warning.transform.localScale = new Vector3(distance, 1, 1);
        Destroy(warning, thunderWarningTime);

        yield return new WaitForSeconds(thunderWarningTime);

        // Step 2: Thunder
        SoundManager.Instance.PlaySe(thunderClip);
        GameObject thunder = Instantiate(thunderPrefab, start, rotation);
        thunder.transform.localScale = new Vector3(distance, 1, 1);
    }


}
