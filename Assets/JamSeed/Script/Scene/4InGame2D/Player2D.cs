using UnityEngine;

public class Player2D : MonoBehaviour
{
    private InputSystem_Actions controls;
    private Vector2 moveInput;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint1;
    [SerializeField] Transform firePoint2;
    [SerializeField] Transform firePoint3;
    [SerializeField] Transform firePoint4;
    [SerializeField] Transform firePoint5;
    [SerializeField] private Transform target;
    [SerializeField] private Transform target1;
    [SerializeField] private Transform target2;
    [SerializeField] float shootInterval = 0.2f;
    [SerializeField] private int level = 0;
    [SerializeField] GameObject PlayerClone;
    private int playerHp = 5;

    [Header("バリア設定")]
    [SerializeField] private GameObject barrierBlock; // バリアの見た目（Cube）
    private bool isBarrierActive = false;
    private float barrierDuration = 5f;
    private float barrierTimer = 0f;
    private int barrierHP = 3;

    private bool isShooting = false;
    private float shootTimer = 0f;

    private void Awake()
    {
        controls = new InputSystem_Actions();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Player.Attack.started += ctx =>
        {
            isShooting = true;
            shootTimer = 0f;
        };

        controls.Player.Attack.performed += ctx => Shoot();

        controls.Player.Attack.canceled += ctx =>
        {
            isShooting = false;
        };

        controls.Player.Jump.performed += ctx =>
        {
            Instantiate(PlayerClone, transform.position, Quaternion.Euler(0f, -90f, 0f));
            ActivateBarrier(); // ジャンプでバリア発動（テスト用）
        };
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Update()
    {
        Move();

        if (isShooting)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootInterval)
            {
                shootTimer = 0f;
                Shoot();
            }
        }

        // バリア時間処理
        if (isBarrierActive)
        {
            barrierTimer += Time.deltaTime;
            if (barrierTimer >= barrierDuration)
            {
                DeactivateBarrier();
            }
        }
    }

    private void Move()
    {
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y) * moveSpeed;
        transform.position += move * Time.deltaTime;
    }

    public void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firePoint1.position, firePoint1.rotation);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.SetDirection(-firePoint1.forward);
            Destroy(bulletObj, 5f);
        }

        GameObject bulletObj2 = Instantiate(bulletPrefab, firePoint2.position, firePoint2.rotation);
        Bullet bullet2 = bulletObj2.GetComponent<Bullet>();
        if (bullet2 != null)
        {
            bullet2.SetDirection(-firePoint2.forward);
            Destroy(bulletObj2, 5f);
        }

        if (level == 1)
        {
            GameObject bulletObj3 = Instantiate(bulletPrefab, firePoint5.position, Quaternion.identity);
            Bullet bullet3 = bulletObj3.GetComponent<Bullet>();
            if (bullet3 != null)
            {
                Vector3 direction = -firePoint5.forward;
                direction.y = 0;
                direction.Normalize();
                bullet3.SetDirection(direction);
                Destroy(bulletObj3, 5f);
            }

            GameObject bulletObj4 = Instantiate(bulletPrefab, firePoint4.position, Quaternion.identity);
            Bullet bullet4 = bulletObj4.GetComponent<Bullet>();
            if (bullet4 != null)
            {
                Vector3 direction = (target1.position - firePoint4.position).normalized;
                direction.y = 0;
                bullet4.SetDirection(direction);
                Destroy(bulletObj4, 5f);
            }

            GameObject bulletObj5 = Instantiate(bulletPrefab, firePoint5.position, Quaternion.identity);
            Bullet bullet5 = bulletObj5.GetComponent<Bullet>();
            if (bullet5 != null)
            {
                Vector3 direction = (target.position - firePoint5.position).normalized;
                direction.y = 0;
                bullet5.SetDirection(direction);
                Destroy(bulletObj5, 5f);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            TryBlockDamage();
            if (!isBarrierActive)
            {
                playerHp--;
                if (playerHp < 0)
                {
                    Debug.Log("ゲームオーバー");
                }
            }

            // バリアがなければダメージを受ける
            //TakeDamage(1);
        }
    }

    public void ActivateBarrier()
    {
        isBarrierActive = true;
        barrierTimer = 0f;
        barrierHP = 3;

        if (barrierBlock != null)
            barrierBlock.SetActive(true); // 表示ON

        Debug.Log("バリア発動！");
    }

    private void DeactivateBarrier()
    {
        isBarrierActive = false;

        if (barrierBlock != null)
            barrierBlock.SetActive(false); // 表示OFF

        Debug.Log("バリア終了！");
    }

    public bool IsBarrierActive()
    {
        return isBarrierActive;
    }

    public bool TryBlockDamage()
    {
        if (isBarrierActive)
        {
            barrierHP--;
            if (barrierHP <= 0)
            {
                DeactivateBarrier();
            }
            return true; // バリアでブロックした
        }
        return false; // バリアなし、ダメージ通す
    }
}
