using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTest1 : MonoBehaviour
{
    [SerializeField] private float _speed = 1.0f;

    private Rigidbody _rigidbody;
    private Vector2 moveInput;

    public Action Onfire;
    public Action OnPlayerClone;
    public bool IsMove;

   
    private InputSystem_Actions _input;
    private void Awake()
    {
       _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
       

    }

    private  void OnEnable()
    {
        _input = new InputSystem_Actions();

        _input.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        _input.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnDisable()
    {
        _input.Player.Move.performed -= ctx => moveInput = ctx.ReadValue<Vector2>();
        _input.Player.Move.canceled -= ctx => moveInput = Vector2.zero;
    }

    private void FixedUpdate()
    {
        if(!IsMove) return;

        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y)*_speed;
        Vector3 velocity = move;

    }


}
