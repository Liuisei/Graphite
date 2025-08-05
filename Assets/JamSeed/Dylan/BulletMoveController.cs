using JamSeed.Runtime;
using UnityEngine;

public class BulletMoveController : MonoBehaviour
{
    public float speed = 10f;
    [SerializeField] private AudioClip _audioClip;

    private void Start()
    {
        SoundManager.Instance.PlaySe(_audioClip);
        Destroy(gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-Vector3.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
