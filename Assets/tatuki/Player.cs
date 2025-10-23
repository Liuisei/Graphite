using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerTest1 : MonoBehaviour
{
    [Header("Move Settings")] [SerializeField]
    private float _speed = 1.0f;

    [FormerlySerializedAs("bulletPrefab")] [Header("Shoot Settings")] [SerializeField]
    private GameObject bulletPrefab1;

    [SerializeField] private GameObject bulletPrefab2;
    [SerializeField] private GameObject beamPrefab;

    [SerializeField] private int level = 1;
    [SerializeField] private GameObject copyPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float spreadAngle = 15f; // 左右の角度差
    [SerializeField] private int hp = 3;
    [SerializeField] private LifeGauge LifeGauge;

private okawari _okawari;
    private Rigidbody _rigidbody;
    private Vector2 moveInput;
    public bool IsMove = true;

    [SerializeField] private PlayerInput playerInput;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        LifeGauge.SetLifeGauge(hp);
        _okawari =  FindAnyObjectByType<okawari>();
    }

    private void OnEnable()
    {
        playerInput.actions["Move"].performed += OnMove;
        playerInput.actions["Move"].canceled += OnMove;
        playerInput.actions["Attack"].performed += OnFireInput;
        playerInput.actions["Jump"].performed += OnCopyInput;
    }

    private void OnDisable()
    {
        playerInput.actions["Move"].performed -= OnMove;
        playerInput.actions["Move"].canceled -= OnMove;
        playerInput.actions["Attack"].performed -= OnFireInput;
        playerInput.actions["Jump"].performed -= OnCopyInput;
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    private void OnFireInput(InputAction.CallbackContext ctx)
    {
        //FireBullet();
        switch (level)
        {
            case 1:
                Fire2Way();
                break;
            case 2:
                Fire3Way();
                break;
            case 3:
                Beam();
                break;
            default:
                Debug.LogWarning("未対応のレベル: " + level);
                break;
        }
    }

    private void OnCopyInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed&&_okawari.GetOkawari() ==1)
        {
            copyBullet();
        }
    }

    private void FixedUpdate()
    {
        if (!IsMove) return;

        // W/S → X軸, A/D → Z軸
        Vector3 move = new Vector3(moveInput.y, 0f, -moveInput.x) * _speed;
        Vector3 velocity = move;
        velocity.y = _rigidbody.linearVelocity.y;
        _rigidbody.linearVelocity = velocity;
    }


    private void copyBullet()
    {
        if (copyPrefab == null || firePoint == null) return;
        Vector3 point1 = new Vector3(firePoint.position.x, 0.1f, firePoint.position.z - 0.2f);
        // 弾を生成
        GameObject bullet = Instantiate(copyPrefab, point1, firePoint.rotation);
        Debug.Log("Bullet fired!");
    }

    private void Beam()
    {
        Vector3 position = new Vector3(firePoint.position.x,0.5f, firePoint.position.z - 4f);
        GameObject beam = Instantiate(beamPrefab, position, firePoint.rotation);
        beam.transform.localScale = new Vector3(1, 1, 10);
        Destroy(beam, 0.2f);
    }

    private void Fire2Way()
    {
        if (bulletPrefab1 == null || firePoint == null) return;

        // 左右に少し離した発射位置を計算
        Vector3 point1 = new Vector3(firePoint.position.x - 0.3f, 0.1f, firePoint.position.z);
        Vector3 point2 = new Vector3(firePoint.position.x + 0.3f, 0.1f, firePoint.position.z);
        // 左方向の弾
        Instantiate(bulletPrefab1, point1, firePoint.rotation);

        // 右方向の弾
        Instantiate(bulletPrefab1, point2, firePoint.rotation);
    }

    private void Fire3Way()
    {
        if (bulletPrefab2 == null || firePoint == null) return;

        // 3方向（-1:左, 0:正面, 1:右）
        for (int i = -1; i <= 1; i++)
        {
            Quaternion rot = Quaternion.Euler(0, i * spreadAngle, 0);
            Vector3 dir = rot * firePoint.forward;
            dir.y = 0; // 水平に固定
            Vector3 position = new Vector3(firePoint.position.x, 0.1f, firePoint.position.z );
            GameObject bullet = Instantiate(bulletPrefab2, firePoint.position, Quaternion.LookRotation(dir));
        }


        Debug.Log(" 3Way Shot!");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            LifeGauge.SetLifeGauge2(1);
        }
    }
}