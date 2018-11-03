using UnityEngine;
using UnityEngine.EventSystems;

namespace Framework.Utils.UI
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(BoxCollider))]
    public class BoxColliderResizer : UIBehaviour
    {
        private RectTransform _rectTransform;
        private BoxCollider _boxCollider;

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
                _boxCollider = GetComponent<BoxCollider>();
            }

            Vector3 size = _rectTransform.rect.size;
            size.z = _boxCollider.size.z;

            _boxCollider.size = size;
        }
    }
}