using Framework.UI.Structure.Base.View;
using Game.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Screens.Replay
{
    public class ReplayPage : Page<ReplayPageModel>
    {
        [SerializeField] private Text _currentScoreText;
        [SerializeField] private Text _bestScoreText;

        public override void OnEnter()
        {
            base.OnEnter();

            _currentScoreText.text = GameController.Instance.GameSession.CurrentScore.ToString();
            _bestScoreText.text = GameController.Instance.GameSession.BestScore.ToString();
        }
    }
}