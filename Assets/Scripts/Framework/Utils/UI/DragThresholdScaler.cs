using UnityEngine;
using UnityEngine.EventSystems;

namespace Framework.Utils.UI
{
    [RequireComponent(typeof(EventSystem))]
    public class DragThresholdScaler : MonoBehaviour
    {
        private const float InchToMillimeterRatio = 1 / 25.4f;

        [SerializeField]
        private float _dragThresholdInMillimeters = 0.3f;

        private void Start()
        {
            var eventSystem = GetComponent<EventSystem>();
            var threshold = _dragThresholdInMillimeters * InchToMillimeterRatio * Screen.dpi;
            eventSystem.pixelDragThreshold = Mathf.RoundToInt(threshold);
        }
    }
}