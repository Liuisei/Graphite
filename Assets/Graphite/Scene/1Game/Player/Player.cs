using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private InputSystem_Actions inputActions;
    [SerializeField] private float accel = 10f;      // 加速力
    [SerializeField] private float maxSpeed = 5f;    // 最高速度
    [SerializeField] private float drag = 2f;        // 減速（抵抗）

    private Vector2 moveInput;
    [SerializeField] private Rigidbody rb;
     
    void Awake()
    {
        if (rb == null)
        {
            Debug.LogError("E001 Rigidbody component is not assigned.");
        }
        rb.useGravity = false;
        rb.linearDamping = drag;

        // Input Actions の準備
        inputActions = new InputSystem_Actions();
        inputActions.Enable();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    void FixedUpdate()
    {
        // 入力 → XZ 方向ベクトル
        Vector3 inputDir = new Vector3(moveInput.x, 0f, moveInput.y);

        // 加速（物理）
        rb.AddForce(inputDir * accel, ForceMode.Acceleration);

        // 最高速度を制限
        Vector3 vel = rb.linearVelocity;
        Vector3 flatVel = new Vector3(vel.x, 0, vel.z);

        if (flatVel.magnitude > maxSpeed)
        {
            flatVel = flatVel.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(flatVel.x, rb.linearVelocity.y, flatVel.z);
        }
    }
}
