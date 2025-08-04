using UnityEngine;

public class Player2D : MonoBehaviour
{
    private InputSystem_Actions controls; // 自動生成されたクラス
    private Vector2 moveInput; // 入力は2Dベクトルで扱う
    [SerializeField] float moveSpeed = 5f;
    //[SerializeField] float jumpForce = 5f;

    private void Awake()
    {
        // InputActions 初期化
        controls = new InputSystem_Actions();

        // 移動入力
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        // ジャンプ入力
       // controls.Player.Jump.performed += ctx => Jump();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Update()
    {
        Move();
    }

    private void Jump()
    {
       
    }
    private void Move()
    {
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y) * moveSpeed;
        transform.position += move * Time.deltaTime;
    }
    
}
