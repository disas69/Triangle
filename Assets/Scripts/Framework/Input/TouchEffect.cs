using UnityEngine;
using UnityEngine.EventSystems;

namespace Framework.Input
{
    public class TouchEffect : MonoBehaviour
    {
        private Camera _mainCamera;
        private RectTransform _rootRectTransform;
        private GameObject _touchEffect;

        [SerializeField] private InputEventProvider _inputProvider;
        [SerializeField] private GameObject _touchEffectPrefab;

        private void Start()
        {
            _mainCamera = Camera.main;
            _rootRectTransform = GetComponent<RectTransform>();

            _inputProvider.PointerDown += OnPointerDown;
            _inputProvider.Drag += OnDrag;
            _inputProvider.PointerUp += OnPointerUp;
        }

        private void OnPointerDown(PointerEventData eventData)
        {
            if (_touchEffect == null)
            {
                _touchEffect = Instantiate(_touchEffectPrefab, transform);
            }
            else
            {
                _touchEffect.gameObject.SetActive(true);
            }

            var viewportPoint = _mainCamera.ScreenToViewportPoint(eventData.position);
            var point = new Vector2((viewportPoint.x * _rootRectTransform.rect.size.x) - (_rootRectTransform.rect.size.x * 0.5f),
                (viewportPoint.y * _rootRectTransform.rect.size.y) - (_rootRectTransform.rect.size.y * 0.5f));

            _touchEffect.transform.localPosition = point;
        }

        private void OnDrag(PointerEventData eventData)
        {
            if (_touchEffect != null && _touchEffect.gameObject.activeSelf)
            {
                var viewportPoint = _mainCamera.ScreenToViewportPoint(eventData.position);
                var point = new Vector2((viewportPoint.x * _rootRectTransform.rect.size.x) - (_rootRectTransform.rect.size.x * 0.5f),
                    (viewportPoint.y * _rootRectTransform.rect.size.y) - (_rootRectTransform.rect.size.y * 0.5f));

                _touchEffect.transform.localPosition = point;
            }
        }

        private void OnPointerUp(PointerEventData eventData)
        {
            if (_touchEffect != null)
            {
                _touchEffect.gameObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            _inputProvider.PointerDown -= OnPointerDown;
            _inputProvider.Drag -= OnDrag;
            _inputProvider.PointerUp -= OnPointerUp;
        }
    }
}