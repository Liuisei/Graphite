using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private float _duration = 3f;
    private float _currentTime;

    private void Start()
    {
        _currentTime = _duration;
    }

    private void Update()
    {
        _currentTime -= Time.deltaTime;
        if (_currentTime <= 0f)
        {
            Destroy(gameObject);
        }
    }

    public void Refresh()
    {
        _currentTime = _duration;
        Debug.Log("Shield duration refreshed!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            Destroy(other.gameObject);
        }
    }
}