using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Framework.Utils.UI
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollRectSnapping : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        private RectTransform _container;
        private int _currentElement;
        private bool _dragging;
        private int _elementsCount;
        private List<Vector2> _elementsPositions;
        private int _fastSwipeThresholdMaxLimit;
        private bool _horizontal;
        private bool _lerp;
        private float _lerpTime;
        private Vector2 _lerpTo;
        private ScrollRect _scrollRectComponent;
        private RectTransform _scrollRectRect;
        private Vector2 _startPosition;
        private float _timeStamp;

        public int StartingElement;
        public int FastSwipeThresholdDistance = 100;
        public float FastSwipeThresholdTime = 0.3f;
        public float LerpTime;
        public float MaxVelocityMagnitude;

        public void Init()
        {
            _scrollRectComponent = GetComponent<ScrollRect>();
            _scrollRectRect = _scrollRectComponent.viewport;
            _container = _scrollRectComponent.content;
            _elementsCount = _container.childCount;
            _elementsPositions = new List<Vector2>();

            if (_scrollRectComponent.horizontal && !_scrollRectComponent.vertical)
            {
                _horizontal = true;
            }
            else if (!_scrollRectComponent.horizontal && _scrollRectComponent.vertical)
            {
                _horizontal = false;
            }
            else
            {
                UnityEngine.Debug.LogWarning("Confusing setting of horizontal/vertical direction. Default set to horizontal.");
                _horizontal = true;
            }

            _lerp = false;

            SetElementsPositions();
            SetElement(StartingElement);
        }

        public void SetCurrentElementIndex(int index)
        {
            SetElement(index);
        }

        public int GetCurrentElementIndex()
        {
            return _currentElement;
        }

        private void Update()
        {
            if (_lerp)
            {
                var decelerate = _lerpTime / LerpTime;
                _scrollRectComponent.velocity = Vector2.ClampMagnitude(_scrollRectComponent.velocity, MaxVelocityMagnitude);
                _container.anchoredPosition = Vector2.Lerp(_container.anchoredPosition, _lerpTo, decelerate);

                if (Vector2.SqrMagnitude(_container.anchoredPosition - _lerpTo) < 0.25f)
                {
                    _container.anchoredPosition = _lerpTo;
                    _scrollRectComponent.velocity = Vector2.zero;
                    _lerp = false;
                }

                _lerpTime += Time.unscaledDeltaTime;
            }
        }

        public void OnBeginDrag(PointerEventData aEventData)
        {
            _lerp = false;
            _dragging = false;
        }

        public void OnDrag(PointerEventData aEventData)
        {
            if (!_dragging)
            {
                _dragging = true;
                _timeStamp = Time.unscaledTime;
                _startPosition = _container.anchoredPosition;
            }
        }

        public void OnEndDrag(PointerEventData aEventData)
        {
            float difference;
            if (_horizontal)
            {
                difference = _startPosition.x - _container.anchoredPosition.x;
            }
            else
            {
                difference = -(_startPosition.y - _container.anchoredPosition.y);
            }

            if (Time.unscaledTime - _timeStamp < FastSwipeThresholdTime && Mathf.Abs(difference) > FastSwipeThresholdDistance &&
                Mathf.Abs(difference) < _fastSwipeThresholdMaxLimit)
            {
                if (difference > 0)
                {
                    NextElement();
                }
                else
                {
                    PreviousElement();
                }
            }
            else
            {
                LerpToPage(GetNearestElement());
            }

            _dragging = false;
        }

        private void SetElementsPositions()
        {
            var width = 0;
            var height = 0;
            var offsetX = 0;
            var offsetY = 0;
            var containerWidth = 0;
            var containerHeight = 0;

            if (_horizontal)
            {
                width = (int) _scrollRectRect.rect.width;
                offsetX = width / 2;
                containerWidth = width * _elementsCount;
                _fastSwipeThresholdMaxLimit = width;
            }
            else
            {
                height = (int) _scrollRectRect.rect.height;
                offsetY = height / 2;
                containerHeight = height * _elementsCount;
                _fastSwipeThresholdMaxLimit = height;
            }

            var newSize = new Vector2(containerWidth, containerHeight);
            _container.sizeDelta = newSize;
            var newPosition = new Vector2(containerWidth / 2f, containerHeight / 2f);
            _container.anchoredPosition = newPosition;

            _elementsPositions.Clear();

            for (var i = 0; i < _elementsCount; i++)
            {
                var child = _container.GetChild(i).GetComponent<RectTransform>();
                Vector2 childPosition;
                if (_horizontal)
                {
                    childPosition = new Vector2(i * width - containerWidth / 2 + offsetX, 0f);
                }
                else
                {
                    childPosition = new Vector2(0f, -(i * height - containerHeight / 2 + offsetY));
                }
                child.anchoredPosition = childPosition;
                _elementsPositions.Add(-childPosition);
            }
        }

        private void SetElement(int aPageIndex)
        {
            aPageIndex = Mathf.Clamp(aPageIndex, 0, _elementsCount - 1);
            _container.anchoredPosition = _elementsPositions[aPageIndex];
            _currentElement = aPageIndex;
        }

        private void LerpToPage(int aPageIndex)
        {
            aPageIndex = Mathf.Clamp(aPageIndex, 0, _elementsCount - 1);
            _lerpTo = _elementsPositions[aPageIndex];
            _lerp = true;
            _lerpTime = 0f;
            _currentElement = aPageIndex;
        }

        private void NextElement()
        {
            LerpToPage(_currentElement + 1);
        }

        private void PreviousElement()
        {
            LerpToPage(_currentElement - 1);
        }

        private int GetNearestElement()
        {
            var currentPosition = _container.anchoredPosition;
            var distance = float.MaxValue;
            var nearestElement = _currentElement;

            for (var i = 0; i < _elementsPositions.Count; i++)
            {
                var testDist = Vector2.SqrMagnitude(currentPosition - _elementsPositions[i]);
                if (testDist < distance)
                {
                    distance = testDist;
                    nearestElement = i;
                }
            }

            return nearestElement;
        }
    }
}