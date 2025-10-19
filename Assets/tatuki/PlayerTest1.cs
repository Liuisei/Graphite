using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTest1 : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] private float _speed = 1.0f;

    [Header("Shoot Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject copyPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float spreadAngle = 15f; // 左右の角度差
    [SerializeField] private int hp = 3;
    [SerializeField] private LifeGauge LifeGauge;

    private Rigidbody _rigidbody;
    private Vector2 moveInput;
    public bool IsMove = true;

    [SerializeField] private PlayerInput playerInput;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        LifeGauge.SetLifeGauge(hp);
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
        if (ctx.performed)
        {
            Fire3Way();
        }
    }

    private void OnCopyInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
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

        // 弾を生成
        GameObject bullet = Instantiate(copyPrefab, firePoint.position, firePoint.rotation);
        Debug.Log("Bullet fired!");
    }
    private void Fire3Way()
    {
        if (bulletPrefab == null || firePoint == null) return;

        // 3方向（-1:左, 0:正面, 1:右）
        for (int i = -1; i <= 1; i++)
        {
            Quaternion rot = Quaternion.Euler(0, i * spreadAngle, 0);
            Vector3 dir = rot * firePoint.forward;
            dir.y = 0; // 水平に固定

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(dir));
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
