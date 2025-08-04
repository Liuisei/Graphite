using UnityEngine;

public class EnemyBulletSettings : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;

    private void Start()
    {
        Destroy(gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        //damage
    }
}
