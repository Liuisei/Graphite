using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float fireCooldown = 0.2f; // 発射間隔（秒）
    

    private Rigidbody rb;
    private InputSystem_Actions inputActions;
    private Vector2 moveInput;
    private bool isFiring;
    private float nextFireTime; // 次に撃てる時間

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        inputActions = new InputSystem_Actions();

        // 移動
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        // 発射（押したら開始 / 離したら停止）
        inputActions.Player.Attack.performed += _ => isFiring = true;
        inputActions.Player.Attack.canceled += _ => isFiring = false;

        // クローン発射
        inputActions.Player.Jump.performed += _ => PlayerClone();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void FixedUpdate()
    {
        // 移動処理
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y) * moveSpeed;
        Vector3 velocity = move;
        velocity.y = rb.linearVelocity.y;
        rb.linearVelocity = velocity;

        // 発射処理（クールタイム付き）
        if (isFiring && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireCooldown;
        }
    }

    private void PlayerClone()
    {
        Debug.Log("PlayerClone 発射！");
    }

    private void Fire()
    {
        Debug.Log("Fire 発射！");
    }
}
