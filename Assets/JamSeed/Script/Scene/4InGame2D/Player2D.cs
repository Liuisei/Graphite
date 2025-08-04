using UnityEngine;

public class Player2D : MonoBehaviour
{
    private InputSystem_Actions controls; // 自動生成されたクラス
    private Vector2 moveInput; // 入力は2Dベクトルで扱う
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint1;
    [SerializeField] Transform firePoint2;
    [SerializeField] Transform firePoint3;
    [SerializeField] Transform firePoint4;
    [SerializeField] Transform firePoint5;
    [SerializeField] private Transform target;// Inspectorからターゲットを指定しておく
    [SerializeField] private Transform target1;
    [SerializeField] private Transform target2;
    [SerializeField] float shootInterval = 0.2f;
    [SerializeField] private int level = 0;
    [SerializeField] GameObject PlayerClone;
    private bool isShooting = false;
    private float shootTimer = 0;
    //[SerializeField] float jumpForce = 5f;

    private void Awake()
    {
        // InputActions 初期化
        controls = new InputSystem_Actions();

        // 移動入力
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        // 攻撃入力
        controls.Player.Attack.started += ctx =>
        {
            isShooting = true;
            shootTimer = 0f; // すぐ撃つ
        };

        controls.Player.Attack.performed += ctx => Shoot();

        controls.Player.Attack.canceled += ctx =>
        {
            isShooting = false;
        };

        controls.Player.Jump.performed += ctx =>
        {
            Instantiate(PlayerClone, transform.position, Quaternion.identity);
        };

        // ジャンプ入力
        // controls.Player.Jump.performed += ctx => Jump();
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
            bullet.SetDirection(firePoint1.right); // 右方向に飛ばす例
        }
        GameObject bulletObj2 = Instantiate(bulletPrefab, firePoint2.position, firePoint2.rotation);
        Bullet bullet2 = bulletObj2.GetComponent<Bullet>();
        if (bullet2 != null)
        {
            bullet2.SetDirection(firePoint2.right);
        }
        if (level == 1)
        {
            GameObject bulletObj3 = Instantiate(bulletPrefab, firePoint5.position, Quaternion.identity);
            Bullet bullet3 = bulletObj3.GetComponent<Bullet>();
            if (bullet3 != null)
            {
                Vector3 direction = firePoint5.right;  // firePoint3の向いている方向（Z+方向）
                direction.y = 0;                         // Yを0にしてXZ平面に固定
                direction.Normalize();                   // 正規化

                bullet3.SetDirection(direction);
            }


            GameObject bulletObj4 = Instantiate(bulletPrefab, firePoint4.position, Quaternion.identity); // 回転不要なら identity
            Bullet bullet4 = bulletObj4.GetComponent<Bullet>();
            if (bullet4 != null)
            {
                // ターゲット方向ベクトル（正規化）
                Vector3 direction = (target2.position - firePoint4.position).normalized;
                direction.y = 0;
                bullet4.SetDirection(direction);
            }


            GameObject bulletObj5 = Instantiate(bulletPrefab, firePoint5.position, Quaternion.identity); // 回転不要なら identity
            Bullet bullet5 = bulletObj5.GetComponent<Bullet>();
            if (bullet5 != null)
            {
                // ターゲット方向ベクトル（正規化）
                Vector3 direction = (target.position - firePoint5.position).normalized;
                direction.y = 0;
                bullet5.SetDirection(direction);
            }

        }



    }
}
