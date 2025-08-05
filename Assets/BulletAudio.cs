using System;
using JamSeed.Runtime;
using UnityEngine;

public class BulletAudio : MonoBehaviour
{
    [SerializeField] private AudioClip clip;

    private void Start()
    {
        SoundManager.Instance.PlaySe(clip);
    }
}
