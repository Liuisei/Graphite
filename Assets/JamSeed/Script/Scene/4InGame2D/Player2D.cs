using UnityEngine;

public class Player2D : MonoBehaviour
{
    private InputSystem_Actions controls; // 自動生成されたクラス
    private Vector2 moveInput; // 入力は2Dベクトルで扱う
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint1;
    [SerializeField] Transform firePoint2;
    [SerializeField] float shootInterval = 0.2f;
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
        controls.Player.Attack.performed += ctx => Shoot();
        controls.Player.Attack.started += ctx =>
        {
            isShooting = true;
            shootTimer = 0f; // すぐ撃つなら0でOK
        };

        controls.Player.Attack.canceled += ctx =>
        {
            isShooting = false;
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

    private void Jump()
    {
       
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
    }



}
