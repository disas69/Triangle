using Framework.Localization;
using Framework.UI.Notifications;
using Framework.UI.Notifications.Model;
using Framework.UI.Structure.Base.View;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Screens.Play
{
    public class PlayPage : Page<PlayPageModel>
    {
        private bool _bestScoreShown;

        [SerializeField] private Text _currentScoreText;

        public override void OnEnter()
        {
            base.OnEnter();

            _bestScoreShown = false;
            UpdateScoreValue(Model.CurrentScore.Value);
            Model.CurrentScore.ValueChanged += UpdateScoreValue;
            NotificationManager.Show(new TextNotification(LocalizationManager.GetString("FollowTheLine")), 2.5f);
        }

        public override void OnExit()
        {
            base.OnExit();

            Model.CurrentScore.ValueChanged -= UpdateScoreValue;
            NotificationManager.HideAll();
        }

        [UsedImplicitly]
        public void ShowNewBestScoreNotification()
        {
            if (_bestScoreShown)
            {
                return;
            }

            _bestScoreShown = true;
            NotificationManager.Show(new TextNotification(LocalizationManager.GetString("NewBestScore")), 2.5f);
        }

        private void UpdateScoreValue(int value)
        {
            _currentScoreText.text = value.ToString();
            _currentScoreText.gameObject.SetActive(value > 0);
        }
    }
}