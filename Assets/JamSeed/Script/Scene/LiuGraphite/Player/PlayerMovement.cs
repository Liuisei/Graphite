using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float fireCooldown = 0.2f;

    private Rigidbody rb;
    private Vector2 moveInput;
    private bool isFiring;
    private float nextFireTime;

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

        input.Player.Attack.performed += _ => isFiring = true;
        input.Player.Attack.canceled += _ => isFiring = false;

        input.Player.Jump.performed += _ => PlayerClone();
    }

    private void OnDisable()
    {
        // イベント解除（クローン再生成時に多重登録しないため）
        var input = InGameScene.Instance?.InputActions;
        if (input != null)
        {
            input.Player.Move.performed -= ctx => moveInput = ctx.ReadValue<Vector2>();
            input.Player.Move.canceled -= ctx => moveInput = Vector2.zero;
            input.Player.Attack.performed -= _ => isFiring = true;
            input.Player.Attack.canceled -= _ => isFiring = false;
            input.Player.Jump.performed -= _ => PlayerClone();
        }
    }

    private void FixedUpdate()
    {
        if (!IsMove) return;

        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y) * moveSpeed;
        Vector3 velocity = move;
        velocity.y = rb.linearVelocity.y;
        rb.linearVelocity = velocity;

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
        onFire?.Invoke();
    }
}
