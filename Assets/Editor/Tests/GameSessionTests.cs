using Framework.Variables;
using Game.Core.Structure;
using NUnit.Framework;
using UnityEngine;

namespace Editor.Tests
{
    public class GameSessionTests
    {
        private const int CurrentScoreIncreaseIterations = 3;
        private const int BestScoreIncreaseIterations = 5;
        private const int BestScoreIncreaseIterationsForSaveLoadTest = 7;

        [Test]
        public void _1_Current_Score_Increase()
        {
            GameDataEditor.ClearData();

            var currentScore = ScriptableObject.CreateInstance<IntVariable>();
            var bestScore = ScriptableObject.CreateInstance<IntVariable>();
            var gameSession = new GameSession(currentScore, bestScore);
            gameSession.Idle();

            for (int i = 0; i < CurrentScoreIncreaseIterations; i++)
            {
                gameSession.IncreaseScore();
            }

            Assert.AreEqual(gameSession.CurrentScore, CurrentScoreIncreaseIterations,
                "GameSession's CurrentScore property was calculated incorrectly");

            GameDataEditor.ClearData();
        }

        [Test]
        public void _2_Best_Score_Increase()
        {
            GameDataEditor.ClearData();

            var currentScore = ScriptableObject.CreateInstance<IntVariable>();
            var bestScore = ScriptableObject.CreateInstance<IntVariable>();
            var gameSession = new GameSession(currentScore, bestScore);
            gameSession.Idle();

            for (int i = 0; i < BestScoreIncreaseIterations; i++)
            {
                gameSession.IncreaseScore();
            }

            gameSession.Stop();
            Assert.AreEqual(gameSession.BestScore, BestScoreIncreaseIterations,
                "GameSession's BestScore property was calculated incorrectly");

            GameDataEditor.ClearData();
        }

        [Test]
        public void _3_Best_Score_Save()
        {
            GameDataEditor.ClearData();

            var currentScore = ScriptableObject.CreateInstance<IntVariable>();
            var bestScore = ScriptableObject.CreateInstance<IntVariable>();
            var gameSession = new GameSession(currentScore, bestScore);
            gameSession.Idle();

            for (int i = 0; i < BestScoreIncreaseIterationsForSaveLoadTest; i++)
            {
                gameSession.IncreaseScore();
            }

            gameSession.Stop();
        }

        [Test]
        public void _4_Best_Score_Load()
        {
            var currentScore = ScriptableObject.CreateInstance<IntVariable>();
            var bestScore = ScriptableObject.CreateInstance<IntVariable>();
            var gameSession = new GameSession(currentScore, bestScore);
            gameSession.Idle();

            Assert.AreEqual(gameSession.BestScore, BestScoreIncreaseIterationsForSaveLoadTest,
                "GameSession's BestScore property was saved incorrectly. This test should be executed after \"BestScoreSave\" test");

            GameDataEditor.ClearData();
        }

        [Test]
        public void _5_Idle_State()
        {
            GameDataEditor.ClearData();

            var currentScore = ScriptableObject.CreateInstance<IntVariable>();
            var bestScore = ScriptableObject.CreateInstance<IntVariable>();
            var gameSession = new GameSession(currentScore, bestScore);
            gameSession.Idle();

            Assert.AreEqual(gameSession.ReadyToPlay, true);
            Assert.AreEqual(gameSession.IsPlaying, false);

            GameDataEditor.ClearData();
        }

        [Test]
        public void _5_Play_State()
        {
            GameDataEditor.ClearData();

            var currentScore = ScriptableObject.CreateInstance<IntVariable>();
            var bestScore = ScriptableObject.CreateInstance<IntVariable>();
            var gameSession = new GameSession(currentScore, bestScore);
            gameSession.Play();

            Assert.AreEqual(gameSession.ReadyToPlay, false);
            Assert.AreEqual(gameSession.IsPlaying, true);

            GameDataEditor.ClearData();
        }

        [Test]
        public void _5_Stop_State()
        {
            GameDataEditor.ClearData();

            var currentScore = ScriptableObject.CreateInstance<IntVariable>();
            var bestScore = ScriptableObject.CreateInstance<IntVariable>();
            var gameSession = new GameSession(currentScore, bestScore);
            gameSession.Stop();

            Assert.AreEqual(gameSession.ReadyToPlay, false);
            Assert.AreEqual(gameSession.IsPlaying, false);

            GameDataEditor.ClearData();
        }
    }
}