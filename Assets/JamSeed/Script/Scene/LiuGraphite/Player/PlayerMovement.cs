using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody rb;
    private Vector2 moveInput;

    public Action onPlayerClone;
    public Action onFire;
    public bool IsMove = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void OnEnable()
    {
        // InGameScene の Input を使う
        var input = InGameScene.Instance.InputActions;

        input.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnDisable()
    {
        // イベント解除（クローン再生成時に多重登録しないため）
        var input = InGameScene.Instance?.InputActions;
        if (input != null)
        {
            input.Player.Move.performed -= ctx => moveInput = ctx.ReadValue<Vector2>();
            input.Player.Move.canceled -= ctx => moveInput = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if (!IsMove) return;

        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y) * moveSpeed;
        Vector3 velocity = move;
        velocity.y = rb.linearVelocity.y;
        rb.linearVelocity = velocity;

    }
}
