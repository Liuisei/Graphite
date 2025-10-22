using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JamSeed.Runtime;

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
    private float direction = -1f;

    [Header("Normal Attack (P1&P2)")] //Phase 1 & 2
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float normalShootRate = 1f;
    public float[] shotAngles;
    public AudioClip houdan;

    [Header("Thunder (P1&P2)")] //Phase 1 & 2
    public GameObject thunderPrefab;
    public Transform thunderFirePoint;
    public float thunderWarningTime = 3f;
    public float thunderSpeed = 15f;
    public float thunderRate = 5f;
    public AudioClip thunderClip;

    [Header("Barrage Attack (P2)")] //Phase 2
    public float barrageRate = 7f;
    public int bulletCount = 24; //number of mines

    [Header("Rotation Attack (P3)")] //Phase 3
    public float minFireCooldown = 0.025f; //min cooldown between 2 mines 
    public float maxFireCooldown = 0.1f; //max cooldown between 2 mines
    public float spinDuration = 1f; //DUration of the spin
    public Transform[] spinFirePoints;

    [Header("Phases")]
    public List<BossPhaseParts> shields;              // Phase 1
    public List<BossPhaseParts> lampionAndMouth;      // Phase 2
    public BossPhaseParts body;                       // Phase 3
    private BossPhase currentPhase = BossPhase.Phase1_Shields;

    [Header("Phase 2 Intensification")]
    public float phase2FireRateMultiplier = 0.5f;
    public float phase2ThunderRateMultiplier = 0.5f;
    public float phase2BarrageRateMultiplier = 0.5f;


    private bool hasEntered = false;
    private bool isImmerged = false;

    private Coroutine verticalCoroutine;

    void Start()
    {
        InitPhase(shields);
        verticalCoroutine = StartCoroutine(VerticalCycle());
        Invoke("EnterPhase1", 1); //delay x secondes before attacking

    }

    void Update()
    {
        HandleMovement();
    }

    #region Movement (Horizontal & vertical)
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

    // Stop the coroutine
    public void StopVerticalMovement()
    {
        if (verticalCoroutine != null)
        {
            StopCoroutine(verticalCoroutine);
            verticalCoroutine = null;
            isImmerged = false;
            StartCoroutine(MoveToY(0.75f, 1.5f));
        }
    }

    // Cycle of inside/outside the ocean
    private IEnumerator VerticalCycle()
    {
        while (currentPhase == BossPhase.Phase1_Shields || currentPhase == BossPhase.Phase2_LampionAndMouth)
        {
            yield return new WaitForSeconds(4f); // Tempo before start

            // Descente en Lerp vers Y = -1.5
            yield return StartCoroutine(MoveToY(-1.5f, 1.5f)); // // Y position, time for going to this position
            isImmerged = true;
            yield return new WaitForSeconds(4f); // Stay X secondes inside the ocean
            // Remontée en Lerp vers Y = 0.75
            yield return StartCoroutine(MoveToY(0.75f, 1.5f)); // Y position, time for going to this position
            isImmerged = false;
            yield return new WaitForSeconds(11f); // Stay X secondes outside the ocean
        }
    }

    //Smooth the vertical movement
    private IEnumerator MoveToY(float targetY, float duration)
    {
        float startY = transform.position.y;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newY = Mathf.Lerp(startY, targetY, elapsed / duration);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            yield return null;
        }

        // Last position
        transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
    }
    #endregion

    #region PHASE 1
    private void EnterPhase1()
    {
        StartCoroutine(NormalAttackCoroutine());
        StartCoroutine(ThunderAttackCoroutine());
    }
    private void ExitPhase1()
    {
        StopCoroutine(NormalAttackCoroutine());
        StopCoroutine(ThunderAttackCoroutine());
    }
    #endregion

    #region PHASE 2
    private void EnterPhase2()
    {
        StartCoroutine(BarrageAttackCoroutine());
    }
    private void ExitPhase2()
    {
        StopCoroutine(BarrageAttackCoroutine());
    }

    private void IntensifyPhase2()
    {
        normalShootRate *= phase2FireRateMultiplier;
        thunderRate *= phase2ThunderRateMultiplier;
        barrageRate *= phase2BarrageRateMultiplier;
        Debug.Log("[BossManager] Phase 2 intensified!");
    }
    #endregion

    #region PHASE 3
    private void EnterPhase3()
    {
        ExitPhase1();
        ExitPhase2();
        StartCoroutine(MoveToY(0.5f, 1.5f));
        StartCoroutine(SpinAttackCoroutine());
    }
    private void ExitPhase3()
    {
        StopAllCoroutines();
    }
    #endregion

    #region Phase logic
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
                    EnterPhase2();
                }
                break;

            case BossPhase.Phase2_LampionAndMouth:
                lampionAndMouth.Remove(part);
                if (lampionAndMouth.Count <= 1)
                {
                    StopVerticalMovement();
                    IntensifyPhase2();
                }

                if (lampionAndMouth.Count == 0)
                {
                    Debug.Log("[BossManager] Phase 2 over → Phase 3");
                    currentPhase = BossPhase.Phase3_Body;
                    EnterPhase3();
                    body.OnDestroyed += OnFinalPartDestroyed;
                }
            break;
        }
    }

    private void OnFinalPartDestroyed(BossPhaseParts part)
    {
        Debug.Log("[BossManager] BOSS DOWN !");
        ExitPhase3();
    }
    #endregion

    #region Normal shoot (Phase 1+2)
    IEnumerator NormalAttackCoroutine()
    {
        while (currentPhase == BossPhase.Phase1_Shields || currentPhase == BossPhase.Phase2_LampionAndMouth)
        {
            while (isImmerged)
                yield return null;

            foreach (float angle in shotAngles)
            {
                Quaternion rotation = Quaternion.Euler(0f, angle, 0f);
                SoundManager.Instance.PlaySe(houdan);
                Instantiate(bulletPrefab, firePoint.position, rotation);
            }
            yield return new WaitForSeconds(normalShootRate);
        }
        
    }
    #endregion

    #region Thunder (Phase 1+2)
    private IEnumerator ThunderAttackCoroutine()
    {
        GameObject target = GameObject.FindGameObjectWithTag("Player");
        if (target == null) yield break;

        while (currentPhase == BossPhase.Phase1_Shields || currentPhase == BossPhase.Phase2_LampionAndMouth)
        {
            GameObject thunder = Instantiate(thunderPrefab, thunderFirePoint.position, Quaternion.identity);

            float timer = 0f;
            Vector3 targetPos = Vector3.zero;

            while (timer < thunderWarningTime)
            {
                if (target != null)
                {
                    targetPos = target.transform.position;
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

            float traveled = 0f;
            while (traveled < 50f)
            {
                float move = thunderSpeed * Time.deltaTime;
                thunder.transform.position += launchDir * move;
                traveled += move;
                yield return null;
            }

            Destroy(thunder);
            yield return new WaitForSeconds(thunderRate);
        }
    }
    #endregion

    #region Barrage Attack (Phase2)
    private IEnumerator BarrageAttackCoroutine()
    {
        while (currentPhase == BossPhase.Phase2_LampionAndMouth)
        {
            while (isImmerged)
                yield return null;

            float angleStep = 360f / bulletCount;
            for (int i = 0; i < bulletCount; i++)
            {
                float angle = i * angleStep;
                Quaternion rotation = Quaternion.Euler(0f, angle, 0f);
                Instantiate(bulletPrefab, body.transform.position, rotation);
            }
            yield return new WaitForSeconds(barrageRate);
        }
    }
    #endregion

    #region Rotation Attack (Phase 3)
    private IEnumerator SpinAttackCoroutine()
    {
        while (currentPhase == BossPhase.Phase3_Body)
        {
            int direction = (Random.value > 0.5f) ? 1 : -1; //clockwise or anti-clockwise
            Quaternion startY = Quaternion.Euler(0f, 90f, 0f); //origin position
            float elapsed = 0f;
            float nextFireTime = 0f;
            float fireTimer = 0f;
            float spinSpeed = 360f;

            while (elapsed < spinDuration)
            {
                elapsed += Time.deltaTime;
                float targetY = direction * spinSpeed * Time.deltaTime;
                transform.Rotate(0f, targetY, 0f, Space.Self);

                // Allow fire only when spinning
                fireTimer += Time.deltaTime;
                if (fireTimer >= nextFireTime)
                {
                    fireTimer = 0f;
                    foreach (var point in spinFirePoints)
                    {
                        SoundManager.Instance.PlaySe(houdan);
                        Instantiate(bulletPrefab, point.position, point.rotation);
                    }
                    nextFireTime = Random.Range(minFireCooldown, maxFireCooldown);
                }
                yield return null;
            }

            transform.rotation = startY; //Return in the original position (90 Y)
            yield return new WaitForSeconds(1f); // Little break between 2 spins
        }
    }
    #endregion
    
}
