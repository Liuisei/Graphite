using UnityEngine;

public class Player : MonoBehaviour
{
    private InputSystem_Actions controls; // 自動生成されたクラス
    private Vector2 moveInput; // 入力は2Dベクトルで扱う
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 5f;

    private Rigidbody rb; // 3D用

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Z回転など不要な動きを固定（横スクロール想定）
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;

        // InputActions 初期化
        controls = new InputSystem_Actions();

        // 移動入力
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        // ジャンプ入力
        controls.Player.Jump.performed += ctx => Jump();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Update()
    {
        // 移動（XZ平面ではなくXY平面で動かすなら transform.right / up を使う）
        Vector3 velocity = rb.linearVelocity;
        velocity.x = moveInput.x * moveSpeed;
        rb.linearVelocity = velocity;
    }

    private void Jump()
    {
        // 地面にいるときだけジャンプ（簡易判定）
        if (Mathf.Abs(rb.linearVelocity.y) < 0.01f)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
