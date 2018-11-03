using Framework.Audio.Configuration;
using Framework.Extensions;
using Framework.Tools.Gameplay;
using UnityEngine;

namespace Framework.Audio
{
    public class AudioPlayer : MonoBehaviour
    {
        private Pool<AudioSource> _audioSourcePool;

        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private int _audioPoolCapacity;
        [SerializeField] private AudioStorage _audioStorage;

        private void Awake()
        {
            _audioSourcePool = new Pool<AudioSource>(_audioSource, transform, _audioPoolCapacity);
        }

        public void Play(string audioKey)
        {
            var audioClip = _audioStorage.GetAudioClip(audioKey);
            if (audioClip != null)
            {
                var audioSource = _audioSourcePool.GetNext();
                audioSource.clip = audioClip;
                audioSource.Play();
                this.WaitUntil(() => !audioSource.isPlaying, () => _audioSourcePool.Return(audioSource));
            }
        }
    }
}