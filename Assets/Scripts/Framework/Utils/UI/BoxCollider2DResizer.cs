using UnityEngine;
using UnityEngine.EventSystems;

namespace Framework.Utils.UI
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class BoxCollider2DResizer : UIBehaviour
    {
        private RectTransform _rectTransform;
        private BoxCollider2D _boxCollider;

        protected override void Start()
        {
            base.Start();
            Resize();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            Resize();
        }

        private void Resize()
        {
            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
            }

            if (_boxCollider == null)
            {
                _boxCollider = GetComponent<BoxCollider2D>();
            }

            _boxCollider.size = _rectTransform.rect.size;
        }
    }
}