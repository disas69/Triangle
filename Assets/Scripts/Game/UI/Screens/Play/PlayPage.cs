using Framework.UI.Structure.Base.View;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Screens.Play
{
    public class PlayPage : Page<PlayPageModel>
    {
        [SerializeField] private Text _currentScoreText;
        [SerializeField] private Text _newBestScoreText;

        public override void OnEnter()
        {
            base.OnEnter();

            _newBestScoreText.gameObject.SetActive(false);
            UpdateScoreValue(Model.CurrentScore.Value);
            Model.CurrentScore.ValueChanged += UpdateScoreValue;
        }

        public override void OnExit()
        {
            base.OnExit();

            Model.CurrentScore.ValueChanged -= UpdateScoreValue;
        }

        public void ShowNewBestScoreNotification()
        {
            _newBestScoreText.gameObject.SetActive(true);
        }

        private void UpdateScoreValue(int value)
        {
            _currentScoreText.text = value.ToString();
            _currentScoreText.gameObject.SetActive(value > 0);
        }
    }
}