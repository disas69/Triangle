using System.Collections;
using Framework.Extensions;
using UnityEngine;

namespace Game.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioEffects : MonoBehaviour
    {
        private AudioSource _audioSource;
        private float _initialPitch;
        private Coroutine _pitchCoroutine;

        [SerializeField] private bool _fadeInOnAwake;
        [SerializeField] private float _fadeInSpeed;
        [SerializeField] private float _pitchChangeSpeed;

        public void Play()
        {
            this.SafeStopCoroutine(_pitchCoroutine);
            _pitchCoroutine = StartCoroutine(ChangePitch(_initialPitch));
        }

        public void Stop()
        {
            this.SafeStopCoroutine(_pitchCoroutine);
            _pitchCoroutine = StartCoroutine(ChangePitch(0f));
        }

        protected void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _initialPitch = _audioSource.pitch;

            if (_fadeInOnAwake)
            {
                StartCoroutine(FadeIn());
            }
        }

        private IEnumerator FadeIn()
        {
            _audioSource.volume = 0;
            yield return new WaitForSeconds(1f);

            while (_audioSource.volume < 1f)
            {
                _audioSource.volume += _fadeInSpeed * Time.deltaTime;
                yield return null;
            }

            _audioSource.volume = 1f;
        }

        private IEnumerator ChangePitch(float value)
        {
            if (_audioSource.pitch > value)
            {
                while (_audioSource.pitch > value)
                {
                    var pitchValue = _audioSource.pitch - _pitchChangeSpeed * Time.deltaTime;
                    _audioSource.pitch = Mathf.Max(value, pitchValue);
                    yield return null;
                }
            }
            else
            {
                while (_audioSource.pitch < value)
                {
                    _audioSource.pitch += _pitchChangeSpeed * Time.deltaTime;
                    yield return null;
                }
            }

            _audioSource.pitch = value;
            _pitchCoroutine = null;
        }
    }
}