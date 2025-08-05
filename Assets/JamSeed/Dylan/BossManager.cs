using JamSeed.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public enum BossPhase
    {
        Phase1_Shields,
        Phase2_LampionAndMouth,
        Phase3_Body
    }

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
    public float thunderSpeed = 15f;
    public float thunderRate = 5f;
    public float thunderTimer = 0f;
    public AudioClip thunderClip;

    [Header("Phases")]
    public List<BossPhaseParts> shields;              // Phase 1
    public List<BossPhaseParts> lampionAndMouth;      // Phase 2
    public BossPhaseParts body;                       // Phase 3

    private bool hasEntered = false;
    private float direction = -1f;
    private BossPhase currentPhase = BossPhase.Phase1_Shields;

    void Start()
    {
        InitPhase(shields);
    }

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
        if (fireTimer >= 1f / fireRate)
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
        if (thunderTimer >= thunderRate)
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

        float timer = 0f;
        Vector3 targetPos = Vector3.zero;

        while (timer < thunderWarningTime)
        {
            if (player != null)
            {
                targetPos = player.transform.position;
                thunder.transform.position = thunderFirePoint.position;

                Vector3 dir = targetPos - thunder.transform.position;
                dir.y = 0f;

                if (dir.sqrMagnitude > 0.001f)
                {
                    float baseRotationY = -79f;
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

    // ------------------------ PHASE SYSTEM ------------------------
    private void InitPhase(List<BossPhaseParts> parts)
    {
        foreach (var part in parts)
        {
            part.OnDestroyed += OnPartDestroyed;
        }
    }

    private void OnPartDestroyed(BossPhaseParts part)
    {
        switch (currentPhase)
        {
            case BossPhase.Phase1_Shields:
                shields.Remove(part);
                if (shields.Count == 0)
                {
                    Debug.Log("[BossManager] Phase 1 over → Phase 2");
                    currentPhase = BossPhase.Phase2_LampionAndMouth;
                    InitPhase(lampionAndMouth);
                }
                break;

            case BossPhase.Phase2_LampionAndMouth:
                lampionAndMouth.Remove(part);
                if (lampionAndMouth.Count == 0)
                {
                    Debug.Log("[BossManager] Phase 2 over → Phase 3");
                    currentPhase = BossPhase.Phase3_Body;
                    body.OnDestroyed += OnFinalPartDestroyed;
                }
                break;
        }
    }

    private void OnFinalPartDestroyed(BossPhaseParts part)
    {
        Debug.Log("[BossManager] BOSS DOWN !");
    }
}
