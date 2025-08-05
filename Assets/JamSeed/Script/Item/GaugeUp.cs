using JamSeed.Runtime;
using UnityEngine;

public class GaugeUp : MonoBehaviour
{
    [SerializeField] private float increaseAmount = 0.1f;
    [SerializeField] private float tweenDuration = 0.3f;
    [SerializeField] AudioClip[] audioClips;

    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        SoundManager.Instance.PlaySe(audioClips[0]);
        InGameScene.Instance._playerDataLiu.fibarCloneCurrentTime += increaseAmount;
        InGameScene.Instance._playerDataLiu.OnPlayerDataChanged.Invoke();
        Destroy(gameObject);
    }
}
