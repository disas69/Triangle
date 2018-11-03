using System.Collections.Generic;
using UnityEngine;

namespace Framework.Utils.Math
{
    public abstract class Averager<T>
    {
        protected struct Sample
        {
            public readonly T Value;
            public readonly float TimeStamp;

            public Sample(T value, float timeStamp)
            {
                Value = value;
                TimeStamp = timeStamp;
            }
        }

        private float _samplingWindow;

        protected readonly Queue<Sample> Samples;

        public T Value { get; protected set; }

        protected Averager(float samplingWindow)
        {
            _samplingWindow = samplingWindow;
            Samples = new Queue<Sample>();
        }

        public void SetSamplingWindow(float samplingWindow)
        {
            _samplingWindow = samplingWindow;
        }

        public void AddSample(T value)
        {
            Samples.Enqueue(new Sample(value, Time.time));
            RemoveExpiredSamples();
            Recalculate();
        }

        public void Clear()
        {
            Samples.Clear();
            Value = default(T);
        }

        protected abstract void Recalculate();

        private void RemoveExpiredSamples()
        {
            for (var sample = Samples.Peek();
                Samples.Count > 0 && Time.time - sample.TimeStamp > _samplingWindow;
                sample = Samples.Peek())
            {
                Samples.Dequeue();
            }
        }
    }
}