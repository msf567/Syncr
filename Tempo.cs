using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Syncr
{
    public class Tempo
    {
        public delegate void TempoTick(int beatNum);
        public TempoTick OnTempoTick;

        public Tempo()
        {
            _newTimes = new List<float>();
        }

        public int BeatsPerMeasure = 8;

        public void Update(double _elapsed)
        {
            if (count >= tempo)
            {
                count = count - tempo;
                Tick();
            }

            count += (float)_elapsed;
            tapT += (float)_elapsed;

            if (tapT > tapTimeout && tapping)
            {
                StartNewTapTempo(); tapping = false;
            }
        }
        private void Tick()
        {
            if (OnTempoTick != null)
                OnTempoTick(beatNumber);
            beatNumber++;
            if (beatNumber > BeatsPerMeasure) beatNumber = 1;
        }

        public void ResetCount()
        {
            beatNumber = 1;
            count = 0;
            Tick();
        }
        public void DoubleTempo()
        {
            tempo /= 2;
        }
        public void HalfTempo()
        {
            tempo *= 2;
        }

        private List<float> _newTimes;
        private int _tapi = 1;
        private bool armedToApplyTapTempo = false;
        float tapTimeout = 3;
        bool tapping = false;

        public void Tap()
        {
            if (!tapping) StartNewTapTempo();
            tapping = true;

            _newTimes.Add(tapT);

            if (armedToApplyTapTempo)
            {
                ApplyTapTempo(Average(_newTimes));
                return;
            }
            lastTap = tapT;
            tapT = 0;
            _tapi++;
            Console.WriteLine("Tap!");
            if (_tapi > BeatsPerMeasure) armedToApplyTapTempo = true;
        }

        private void ApplyTapTempo(float _f)
        {
            armedToApplyTapTempo = false;
            tapping = false;
            tempo = _f;
            ResetCount();
            _newTimes = new List<float>();
        }

        private void StartNewTapTempo()
        {
            _tapi = 1;
            tapT = 0;
        }

        private float Average(List<float> f)
        {
            float result = 0;

            for (int x = 1; x < f.Count; x++)
                result += f[x];
            result /= f.Count - 1;
            return result;
        }

        private int beatNumber = 0;
        private float count = 0;
        private float tapT = 0;
        private float lastTap = 0;
        private float tempo = 120;
    }
}
