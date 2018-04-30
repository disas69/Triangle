using Game.Path.Lines.Base;
using Game.Path.Lines.Configuration;

namespace Game.Path.Lines
{
    public class SlowPathPathLine : PathLine<SlowPathLineSettings>
    {
        private bool _isApplied;

        public override LineType LineType
        {
            get { return LineType.Slow; }
        }

        protected override void OnLineTriggered()
        {
            base.OnLineTriggered();

            if (!_isApplied)
            {
                Settings.TriangleRotationSpeed.Value /= Settings.SpeedDecreaseMultiplier;
                Settings.PathSpeedMultiplier.Value /= Settings.SpeedDecreaseMultiplier;

                _isApplied = true;
            }
        }

        protected override void OnLinePassed()
        {
            base.OnLinePassed();
            StopEffect();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            StopEffect();
        }

        private void StopEffect()
        {
            if (_isApplied)
            {
                Settings.TriangleRotationSpeed.Value *= Settings.SpeedDecreaseMultiplier;
                Settings.PathSpeedMultiplier.Value *= Settings.SpeedDecreaseMultiplier;

                _isApplied = false;
            }
        }
    }
}