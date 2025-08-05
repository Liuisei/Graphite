using JamSeed.Foundation;
using UnityEngine;

namespace JamSeed.Runtime
{
    public class SoundManager : SingletonMono<SoundManager>
    {
        [Header("Audio Component")]
        [SerializeField] private AudioSource[] _seSource;
        // BGMは複数再生することはあまりないので複数のObjectはつくらない
        [SerializeField] private AudioSource _bgmSource;

        // BGMを再生する
        public void PlayBgm(AudioClip clip)
        {
            _bgmSource.clip = clip;
            _bgmSource.Play();
        }

        // BGMを止める
        public void StopBgm()
        {
            _bgmSource.Stop();
        }

        // SEを再生する
        public void PlaySe(AudioClip clip)
        {
            foreach (AudioSource source in _seSource)
            {
                if (!source.isPlaying)
                {
                    source.PlayOneShot(clip);
                    return;
                }
            }

            Debug.LogWarning("No se played");
        }
    }
}
