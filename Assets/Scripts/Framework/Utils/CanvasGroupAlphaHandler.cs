using System.Collections;
using UnityEngine;

namespace Framework.Utils
{
    public class CanvasGroupAlphaHandler : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        private float _initialAlphaValue;

        [SerializeField] private float _targetAlphaValue;
        [SerializeField] private float _changeSpeed;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _initialAlphaValue = _canvasGroup.alpha;
        }

        private void OnEnable()
        {
            _canvasGroup.alpha = _initialAlphaValue;
            StartCoroutine(ChangeAlpha());
        }

        private IEnumerator ChangeAlpha()
        {
            if (_initialAlphaValue > _targetAlphaValue)
            {
                while (_canvasGroup.alpha > _targetAlphaValue)
                {
                    _canvasGroup.alpha -= _changeSpeed * Time.deltaTime;
                    yield return null;
                }
            }
            else
            {
                while (_canvasGroup.alpha < _targetAlphaValue)
                {
                    _canvasGroup.alpha += _changeSpeed * Time.deltaTime;
                    yield return null;
                }
            }

            _canvasGroup.alpha = _targetAlphaValue;
        }
    }
}