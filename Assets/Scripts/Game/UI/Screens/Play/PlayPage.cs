using DG.Tweening;
using Framework.Input;
using Framework.Localization;
using Framework.Signals;
using Framework.Tasking;
using Framework.UI.Notifications;
using Framework.UI.Notifications.Model;
using Framework.UI.Structure.Base.View;
using Game.Core;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI.Screens.Play
{
    public class PlayPage : Page<PlayPageModel>
    {
        private bool _bestScoreShown;
        private Task _showTutorialTask;

        [SerializeField] private Text _currentScoreText;
        [SerializeField] private CanvasGroup _tutorial;

        public override void OnEnter()
        {
            base.OnEnter();

            _bestScoreShown = false;
            _tutorial.alpha = 0f;

            UpdateScoreValue(0);
            SignalsManager.Register("CurrentScoreChanged", UpdateScoreValue);

            _showTutorialTask = new WaitTask(this, GameController.Instance.Settings.ActivationTime, () =>
            {
                if (InputEventProvider.Instance.PointersCount == 0)
                {
                    ShowTutorial();
                }
            });
            _showTutorialTask.Start();

            InputEventProvider.Instance.PointerDown += OnPointerDown;
            InputEventProvider.Instance.PointerUp += OnPointerUp;
        }

        private void OnPointerDown(PointerEventData eventData)
        {
            _showTutorialTask.Stop();
            ActivateTutorial(false);
        }

        private void OnPointerUp(PointerEventData eventData)
        {
            if (InputEventProvider.Instance.PointersCount == 0)
            {
                _showTutorialTask = new WaitTask(this, GameController.Instance.Settings.ActivationTime, ShowTutorial);
                _showTutorialTask.Start();
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            InputEventProvider.Instance.PointerDown -= OnPointerDown;
            InputEventProvider.Instance.PointerUp -= OnPointerUp;

            _showTutorialTask.Stop();
            _tutorial.DOPause();

            NotificationManager.HideAll();
            SignalsManager.Unregister("CurrentScoreChanged", UpdateScoreValue);
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

        private void ShowTutorial()
        {
            NotificationManager.Show(new TextNotification(LocalizationManager.GetString("FollowTheLine")), 2.5f);
            ActivateTutorial(true);
        }

        private void ActivateTutorial(bool isActive)
        {
            _tutorial.DOPause();
            _tutorial.DOFade(isActive ? 1f : 0f, 2.5f);
        }
    }
}