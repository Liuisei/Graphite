using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 moveDirection;

    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction.normalized;
    }

    void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
    }
}