using System.Collections;
using System.Collections.Generic;
using Framework.Extensions;
using UnityEngine;

namespace Framework.Utils.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class CameraColorChanger : MonoBehaviour
    {
        private UnityEngine.Camera _camera;
        private int _currentIndex;
        private Color _currentColor;
        private Color _nextColor;
        private Coroutine _lerpCoroutine;

        [SerializeField] private bool _changeOnStart;
        [SerializeField] private float _lerpTime;
        [SerializeField] private List<Color> _colors;

        private void Awake()
        {
            _camera = GetComponent<UnityEngine.Camera>();
            _currentIndex = 0;

            if (_changeOnStart)
            {
                ChangeColor();
            }
        }

        public void ChangeColor()
        {
            this.SafeStopCoroutine(_lerpCoroutine);
            _currentColor = _camera.backgroundColor;

            if (_colors.Count > 1)
            {
                var newIndex = Random.Range(0, _colors.Count);
                if (newIndex == _currentIndex)
                {
                    if (++newIndex >= _colors.Count)
                    {
                        newIndex = 0;
                    }
                }

                _currentIndex = newIndex;
                _nextColor = _colors[newIndex];
                _lerpCoroutine = StartCoroutine(LerpColor());
            }
        }

        private IEnumerator LerpColor()
        {
            var time = 0f;
            while (time < _lerpTime)
            {
                _camera.backgroundColor = Color.Lerp(_currentColor, _nextColor, time / _lerpTime);
                time += Time.deltaTime;
                yield return null;
            }

            _camera.backgroundColor = _nextColor;
        }
    }
}