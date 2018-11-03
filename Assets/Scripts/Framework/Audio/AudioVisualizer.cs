using System.Collections.Generic;
using UnityEngine;

namespace Framework.Audio
{
    public class AudioVisualizer : MonoBehaviour
    {
        private List<GameObject> _gameObjects;

        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private int _spectrumSize;
        [SerializeField] private float _minScale;
        [SerializeField] private float _maxMultiplier;
        [SerializeField] private bool _useChildObjects;

        private void Start()
        {
            _gameObjects = new List<GameObject>(_spectrumSize);

            if (_useChildObjects)
            {
                var renderers = transform.GetComponentsInChildren<Renderer>();
                for (int i = 0; i < renderers.Length; i++)
                {
                    _gameObjects.Add(renderers[i].gameObject);
                }
            }
            else
            {
                for (int i = 0; i < _spectrumSize; i++)
                {
                    var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.localPosition = new Vector3(i, 0f, 0f);
                    cube.transform.SetParent(transform);
                    _gameObjects.Add(cube);
                }
            }
        }

        private void Update()
        {
            if (!_audioSource.isPlaying || _audioSource.pitch < 1f)
            {
                return;
            }

            var spectrumData = new float[_spectrumSize];
            _audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.Rectangular);

            for (int i = 0; i < _gameObjects.Count; i++)
            {
                var gameObj = _gameObjects[i];
                var localScale = gameObj.transform.localScale;
                var newLocalScale = new Vector3(localScale.x, spectrumData[i] * _maxMultiplier + _minScale, localScale.z);
                gameObj.transform.localScale = newLocalScale;
            }
        }
    }
}