using Framework.Events;
using Framework.Variables;
using Game.Core.Data;

namespace Game.Core.Structure
{
    public class GameSession
    {
        private readonly IntVariable _currentScore;
        private readonly IntVariable _bestScore;
        private readonly Event _passed10LinesEvent;
        private readonly Event _bestScoreBeatenEvent;

        public bool ReadyToPlay { get; private set; }
        public bool IsPlaying { get; private set; }
        public GameDataKeeper GameDataKeeper { get; private set; }

        public int CurrentScore
        {
            get { return _currentScore.Value; }
        }

        public int BestScore
        {
            get { return _bestScore.Value; }
        }

        public GameSession(IntVariable currentScore, IntVariable bestScore, Event passed10LinesEvent = null, Event bestScoreBeatenEvent = null)
        {
            _currentScore = currentScore;
            _bestScore = bestScore;

            _passed10LinesEvent = passed10LinesEvent;
            _bestScoreBeatenEvent = bestScoreBeatenEvent;

            GameDataKeeper = new GameDataKeeper();
            GameDataKeeper.Load();
        }

        public void Idle()
        {
            _currentScore.Value = 0;
            _bestScore.Value = GameDataKeeper.Data.BestScore;

            ReadyToPlay = true;
            IsPlaying = false;
        }

        public void Play()
        {
            ReadyToPlay = false;
            IsPlaying = true;
        }

        public void Stop()
        {
            ReadyToPlay = false;
            IsPlaying = false;

            if (_currentScore.Value > _bestScore.Value)
            {
                _bestScore.Value = _currentScore.Value;
                GameDataKeeper.Data.BestScore = _bestScore.Value;
                GameDataKeeper.Save();
            }
        }

        public void IncreaseScore()
        {
            ++_currentScore.Value;

            if (_currentScore.Value % 10 == 0 && _passed10LinesEvent != null)
            {
                _passed10LinesEvent.Fire();
            }

            if (_bestScore.Value > 0)
            {
                if (_currentScore.Value > _bestScore.Value && _bestScoreBeatenEvent != null)
                {
                    _bestScoreBeatenEvent.Fire();
                }
            }
        }
    }
}