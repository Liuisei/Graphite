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
    public GameObject thunderPrefab;
    public Transform thunderFirePoint;
    public float thunderWarningTime = 3f;
    public float thunderSpeed = 15f;        // vitesse de l'éclair une fois lancé
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

        GameObject thunder = Instantiate(thunderPrefab, thunderFirePoint.position, Quaternion.identity);

        // On ne touche pas à la rotation Z ici, c'est dans l'enfant Visual dans le prefab
        // thunder.transform.rotation = Quaternion.Euler(0f, 0f, -90f); // <-- supprimé

        float timer = 0f;
        Vector3 targetPos = Vector3.zero;

        while (timer < thunderWarningTime)
        {
            if (player != null)
            {
                targetPos = player.transform.position;
                thunder.transform.position = thunderFirePoint.position;

                Vector3 dir = targetPos - thunder.transform.position;
                dir.y = 0f; // On veut rotation horizontale uniquement

                if (dir.sqrMagnitude > 0.001f)
                {
                    float baseRotationY = -79f; // Valeur à ajuster selon ce que tu trouves dans le prefab

                    float angleY = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
                    float correctedY = angleY - baseRotationY;

                    thunder.transform.rotation = Quaternion.Euler(0f, correctedY, -90f);
                }
            }
            timer += Time.deltaTime;
            yield return null;
        }

        Vector3 launchDir = (targetPos - thunder.transform.position).normalized;

        while (true)
        {
            thunder.transform.position += launchDir * thunderSpeed * Time.deltaTime;
            yield return null;

            if (Vector3.Distance(thunderFirePoint.position, thunder.transform.position) > 50f)
            {
                Destroy(thunder);
                yield break;
            }
        }
    }

}
