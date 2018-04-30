using Framework.UI.Structure.Base.View;
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

            _currentScoreText.text = Model.CurrentScore.Value.ToString();
            _bestScoreText.text = Model.BestScore.Value.ToString();
        }
    }
}