using Framework.Signals;
using Game.Core.Data;

namespace Game.Core.Structure
{
    public class GameSession
    {
        private int _currentScore;
        private int _bestScore;

        public bool ReadyToPlay { get; private set; }
        public bool IsPlaying { get; private set; }
        public GameDataKeeper GameData { get; }

        public int CurrentScore
        {
            get { return _currentScore; }
            private set
            {
                _currentScore = value;
                SignalsManager.Broadcast("CurrentScoreChanged", _currentScore);
            }
        }

        public int BestScore
        {
            get { return _bestScore; }
            private set
            {
                _bestScore = value;
                SignalsManager.Broadcast("BestScoreChanged", _bestScore);
            }
        }

        public GameSession()
        {
            GameData = new GameDataKeeper();
            GameData.Load();
        }

        public void Idle()
        {
            CurrentScore = 0;
            BestScore = GameData.Data.BestScore;

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

            if (CurrentScore > BestScore)
            {
                BestScore = CurrentScore;
                GameData.Data.BestScore = BestScore;
                GameData.Save();
            }
        }

        public void IncreaseScore()
        {
            ++CurrentScore;

            //Use variable from configuration here
            if (CurrentScore % 10 == 0)
            {
                SignalsManager.Broadcast("PassedLineSection");
            }

            if (BestScore > 0)
            {
                if (CurrentScore > BestScore)
                {
                    SignalsManager.Broadcast("BestScoreBeaten");
                }
            }
        }
    }
}