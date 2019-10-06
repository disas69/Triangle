using UnityEngine;
using UnityEngine.UI;

namespace Framework.Utils.Debug
{
    public class QualitySettingsTest : MonoBehaviour
    {
        [SerializeField] private Text _output;
        [SerializeField] private Button _increaseButton;
        [SerializeField] private Button _decreaseButton;

        private void Awake()
        {
            _increaseButton.onClick.AddListener(IncreaseLevel);
            _decreaseButton.onClick.AddListener(DecreaseLevel);

            UpdateOutput();
        }

        private void IncreaseLevel()
        {
            QualitySettings.IncreaseLevel(true);
            UpdateOutput();
        }

        private void DecreaseLevel()
        {
            QualitySettings.DecreaseLevel(true);
            UpdateOutput();
        }

        private void UpdateOutput()
        {
            var current = QualitySettings.GetQualityLevel();
            var levelName = QualitySettings.names[current];

            _output.text = $"Current Quality level: {levelName}";
        }
    }
}