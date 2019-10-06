using Framework.Extensions;
using Framework.Input;
using Framework.Tasking;
using Framework.Tools.Gameplay;
using Framework.Tools.Singleton;
using Framework.UI;
using Framework.Utils.Positioning;
using Game.Audio;
using Game.Core.Structure;
using Game.Path;
using Game.Triangle;
using Game.UI.Screens.Play;
using Game.UI.Screens.Replay;
using Game.UI.Screens.Start;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Core
{
    public class GameController : MonoSingleton<GameController>
    {
        private GameSession _gameSession;
        private PlayerController _player;
        private PositionsSnapshot _positionsSnapshot;
        private StateMachine<GameState> _stateMachine;
        private Task _playerActivationTask;

        [SerializeField] private PlayerController _playerPrefab;
        [SerializeField] private PathController _pathController;
        [SerializeField] private AudioEffects _audioEffects;
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _scene;
        [SerializeField] private GameSettings _settings;

        public GameSettings Settings => _settings;
        public GameSession Session => _gameSession;
        public PathController Path => _pathController;

        private void Start()
        {
            _gameSession = new GameSession();
            _positionsSnapshot = new PositionsSnapshot(_scene);
            _positionsSnapshot.Save();
            _stateMachine = CreateStateMachine();

            InputEventProvider.Instance.PointerDown += OnPointerDown;

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

        private void OnPointerDown(PointerEventData pointerEventData)
        {
            if (_gameSession.IsPlaying)
            {
                return;
            }

            if (_gameSession.ReadyToPlay)
            {
                _stateMachine.SetState(GameState.Play);
            }
            else
            {
                _positionsSnapshot.Restore();
                _stateMachine.SetState(GameState.Idle);
            }
        }

        private void OnLineTouched()
        {
            _stateMachine.SetState(GameState.Stop);
        }

        private void OnLinePassed()
        {
            _gameSession.IncreaseScore();
        }

        private void IdleState()
        {
            _gameSession.Idle();
            _pathController.Idle();
            _audioEffects.Play();

            if (_player != null)
            {
                _player.LinePassed -= OnLinePassed;
                _player.LineTouched -= OnLineTouched;
                Destroy(_player.gameObject);
            }

            _player = SpawnTriangle();

            NavigationManager.Instance.OpenScreen<StartPage>();
        }

        private void PlayState()
        {
            _gameSession.Play();
            _pathController.Play();

            _playerActivationTask = new WaitTask(this, _settings.ActivationTime, () => _player.Activate(true));
            _playerActivationTask.Start();

            NavigationManager.Instance.OpenScreen<PlayPage>();
        }

        private void StopState()
        {
            _gameSession.Stop();
            _pathController.Stop();
            _audioEffects.Stop();

            _playerActivationTask.Stop();
            _player.Activate(false);

            NavigationManager.Instance.OpenScreen<ReplayPage>();
        }

        private PlayerController SpawnTriangle()
        {
            var triangle = Instantiate(_playerPrefab, transform);
            triangle.transform.position = Vector3.zero;
            triangle.LinePassed += OnLinePassed;
            triangle.LineTouched += OnLineTouched;

            var cameraFollower = _camera.GetComponent<TargetFollower>();
            if (cameraFollower != null)
            {
                cameraFollower.SetTarget(triangle.transform);
            }

            return triangle;
        }

        private void OnDestroy()
        {
            _gameSession.GameData.Save();
            InputEventProvider.Instance.PointerDown -= OnPointerDown;

            if (_player != null)
            {
                _player.LinePassed -= OnLinePassed;
                _player.LineTouched -= OnLineTouched;
            }
        }
    }
}