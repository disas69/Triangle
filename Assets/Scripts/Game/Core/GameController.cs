using Framework.Tools.Gameplay;
using Framework.UI.Structure;
using Framework.Utils;
using Game.Audio.Structure;
using Game.Core.Structure;
using Game.Input.Structure;
using Game.Path;
using Game.Triangle;
using Game.UI.Screens.Play;
using Game.UI.Screens.Replay;
using Game.UI.Screens.Start;
using UnityEngine;
using Zenject;

namespace Game.Core
{
    public class GameController : MonoBehaviour
    {
        private GameSession _gameSession;
        private GameSnapshot _gameSnapshot;
        private StateMachine<GameState> _gameStateMachine;
        private TriangleController _triangleController;

        [Inject] private IInputProvider _inputProvider;
        [Inject] private INavigationProvider _navigationProvider;
        [Inject] private IAudioEffectsProvider _audioEffectsProvider;

        [SerializeField] private TriangleController _trianglePrefab;
        [SerializeField] private PathController _pathController;
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _dynamicObjects;
        [SerializeField] private GameVariables _gameVariables;
        [SerializeField] private GameEvents _gameEvents;

        private void Start()
        {
            UnityEngine.Input.multiTouchEnabled = false;

            _gameSession = new GameSession(_gameVariables.CurrentScore, _gameVariables.BestScore,
                _gameEvents.Passed10LinesEvent, _gameEvents.BestScoreBeatenEvent);
            
            _gameSnapshot = new GameSnapshot(_dynamicObjects);
            _gameSnapshot.SaveState();

            _gameStateMachine = CreateStateMachine();
            _inputProvider.PointerDown += OnPointerDown;

            IdleState();
        }

        private StateMachine<GameState> CreateStateMachine()
        {
            var stateMachine = new StateMachine<GameState>(GameState.Idle);
            stateMachine.AddTransition(GameState.Idle, GameState.Play, PlayState);
            stateMachine.AddTransition(GameState.Play, GameState.Stop, StopState);
            stateMachine.AddTransition(GameState.Stop, GameState.Idle, IdleState);

            return stateMachine;
        }

        private void OnPointerDown(Vector3 vector)
        {
            if (_gameSession.IsPlaying)
            {
                return;
            }

            if (_gameSession.ReadyToPlay)
            {
                _gameStateMachine.SetState(GameState.Play);
            }
            else
            {
                _gameSnapshot.RestoreState();
                _gameStateMachine.SetState(GameState.Idle);
            }
        }

        private void OnTriangleLineTouched()
        {
            _gameStateMachine.SetState(GameState.Stop);
        }

        private void OnTriangleLinePassed()
        {
            _gameSession.IncreaseScore();
        }

        private void IdleState()
        {
            _gameSession.Idle();
            _pathController.Idle();
            _audioEffectsProvider.Play();
            _navigationProvider.OpenScreen<StartPage>();

            SpawnTriangle();
        }

        private void PlayState()
        {
            _gameSession.Play();
            _pathController.Play();
            _navigationProvider.OpenScreen<PlayPage>();
        }

        private void StopState()
        {
            _gameSession.Stop();
            _pathController.Stop();
            _audioEffectsProvider.Stop();
            _navigationProvider.OpenScreen<ReplayPage>();
        }

        private void SpawnTriangle()
        {
            if (_triangleController != null)
            {
                _triangleController.LinePassed -= OnTriangleLinePassed;
                _triangleController.LineTouched -= OnTriangleLineTouched;

                Destroy(_triangleController.gameObject);
            }

            _triangleController = Instantiate(_trianglePrefab, transform);
            _triangleController.SetupInput(_inputProvider);

            _triangleController.transform.position = Vector3.zero;
            _triangleController.LinePassed += OnTriangleLinePassed;
            _triangleController.LineTouched += OnTriangleLineTouched;

            var cameraFollower = _camera.GetComponent<TargetFollower>();
            if (cameraFollower != null)
            {
                cameraFollower.SetTarget(_triangleController.transform);
            }
        }

        private void OnDestroy()
        {
            _gameSession.GameDataKeeper.Save();
            _inputProvider.PointerDown -= OnPointerDown;

            if (_triangleController != null)
            {
                _triangleController.LinePassed -= OnTriangleLinePassed;
                _triangleController.LineTouched -= OnTriangleLineTouched;
            }
        }
    }
}