using System;
using System.Collections.Generic;
using Assets.Scripts.Game;
using Framework.Tools.Gameplay;
using Game.Path.Configuration;
using Game.Path.Lines.Base;
using UnityEngine;

namespace Game.Path
{
    public class PathController : MonoBehaviour
    {
        private Dictionary<LineType, Pool<Line>> _linesPools;
        private List<Line> _activeLines;
        private PathPattern _pathPattern;
        private bool _isPlaying;
        private bool _isIdle;
        private float _speed;

        [SerializeField] private PathLinesMapping _pathLinesMapping;
        [SerializeField] private PathPatternsStorage _pathPatterns;
        [SerializeField] private PathSettings _pathSettings;

        public void Idle()
        {
            _isIdle = true;

            ClearPath();
            for (int i = 0; i < _pathSettings.InitialPathSize; i++)
            {
                AddLine();
            }
        }

        public void Play()
        {
            _isIdle = false;
            _isPlaying = true;
        }

        public void Stop()
        {
            _isIdle = false;
            _isPlaying = false;
        }

        private void Awake()
        {
            _linesPools = CreateLinesPools();
            _activeLines = new List<Line>(_pathSettings.InitialPathSize);
            _pathPattern = _pathPatterns.GetNextPathPattern();
        }

        private Dictionary<LineType, Pool<Line>> CreateLinesPools()
        {
            var linesPools = new Dictionary<LineType, Pool<Line>>();
            var lineTypes = (LineType[]) Enum.GetValues(typeof(LineType));

            for (int i = 0; i < lineTypes.Length; i++)
            {
                var lineType = lineTypes[i];
                var pool = new Pool<Line>(_pathLinesMapping.GetLinePrefab(lineType), transform, _pathSettings.PoolSize);
                linesPools.Add(lineType, pool);
            }

            return linesPools;
        }

        private void Update()
        {
            if (_isIdle || _isPlaying)
            {
                while (_activeLines.Count < _pathSettings.InitialPathSize)
                {
                    AddLine();
                }

                AdjustSpeed();
                MoveLines();
            }
        }

        private void AdjustSpeed()
        {
            var newSpeed = _pathSettings.InitialMoveSpeed + 1f * _pathSettings.SpeedMultiplier.Value;
            _speed = Mathf.Clamp(newSpeed, _pathSettings.InitialMoveSpeed, _pathSettings.MaxMoveSpeed);
        }

        private void MoveLines()
        {
            for (int i = 0; i < _activeLines.Count; i++)
            {
                var pathLine = _activeLines[i];
                pathLine.transform.position += Vector3.back * _speed * Time.deltaTime;
            }
        }

        private void AddLine()
        {
            var settings = GetNextLineSettings();
            var line = _linesPools[settings.LineType].GetNext();

            if (_activeLines.Count > 0)
            {
                line.Setup(settings);

                var previousPathLine = _activeLines[_activeLines.Count - 1];
                line.SetPosition(previousPathLine.EndPointPosition);
            }
            else
            {
                line.transform.position = Vector3.back * 100f;
            }

            line.OnBecameVisible += OnLineBecomeVisible;
            line.OnBecameInvisible += OnLineBecomeInvisible;

            _activeLines.Add(line);
        }

        private void OnLineBecomeVisible(Line pathLine)
        {
            if (_activeLines.Count >= _pathSettings.MaxPathSize)
            {
                return;
            }

            var index = _activeLines.FindIndex(l => l == pathLine);
            if (index == _activeLines.Count - 1)
            {
                AddLine();
            }
        }

        private void OnLineBecomeInvisible(Line pathLine)
        {
            if (pathLine.IsPassed)
            {
                pathLine.OnBecameVisible -= OnLineBecomeVisible;
                pathLine.OnBecameInvisible -= OnLineBecomeInvisible;

                _activeLines.Remove(pathLine);
                _linesPools[pathLine.LineType].Return(pathLine);
            }
        }

        private Settings GetNextLineSettings()
        {
            if (_isIdle)
            {
                return _pathPatterns.DefaultSettings;
            }

            var settings = _pathPattern.GetNextValue();
            while (settings == null)
            {
                _pathPattern = _pathPatterns.GetNextPathPattern();
                settings = _pathPattern.GetNextValue();
            }

            return settings;
        }

        private void ClearPath()
        {
            for (int i = 0; i < _activeLines.Count; i++)
            {
                var line = _activeLines[i];

                line.OnBecameVisible -= OnLineBecomeVisible;
                line.OnBecameInvisible -= OnLineBecomeInvisible;

                _linesPools[line.LineType].Return(line);
            }

            _activeLines.Clear();
        }

        public Line GetLineInPosition(Vector3 position)
        {
            Line closestLine = null;
            var distance = float.MaxValue;

            for (int i = 0; i < _activeLines.Count; i++)
            {
                var line = _activeLines[i];
                var lineDistance = MathUtils.GetDistanceToSegment(new Vector2(position.x, position.z),
                    new Vector2(line.StartPointPosition.x, line.StartPointPosition.z),
                    new Vector2(line.EndPointPosition.x, line.EndPointPosition.z));

                if (lineDistance >= 0 && lineDistance < distance)
                {
                    distance = lineDistance;
                    closestLine = line;
                }

                /*
                var x1 = line.StartPointPosition.x;
                var x2 = line.EndPointPosition.x;

                if (x1 > x2)
                {
                    var temp = x1;
                    x1 = x2;
                    x2 = temp;
                }

                var z1 = line.StartPointPosition.z;
                var z2 = line.EndPointPosition.z;

                if (z1 > z2)
                {
                    var temp = z1;
                    z1 = z2;
                    z2 = temp;
                }

                if (position.x >= x1 && position.x <= x2 || position.z >= z1 && position.z <= z2)
                {
                    return line;
                }

                
                //Debug.DrawLine(position, line.StartPointPosition);
                var startPointDistance = Vector3.Distance(position, line.StartPointPosition);

                if (startPointDistance < distance)
                {
                    distance = startPointDistance;
                    closestLine = line;
                }

                //Debug.DrawLine(position, line.EndPointPosition);
                var endPointDistance = Vector3.Distance(position, line.EndPointPosition);
                if (endPointDistance < distance)
                {
                    distance = endPointDistance;
                    closestLine = line;
                }
                */
            }

            return closestLine;
        }
    }
}