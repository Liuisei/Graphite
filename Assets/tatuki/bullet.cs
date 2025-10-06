using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 3f;

    private void Start()
    {
        Destroy(gameObject, lifetime); // 一定時間後に削除
    }

    private void Update()
    {
        // 前方向(Z軸)にまっすぐ進む
        transform.Translate(Vector3.forward * -speed * Time.deltaTime);
    }
}
