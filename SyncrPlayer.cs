using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;
namespace Syncr
{
    public enum SyncrParamType
    {
        VOID, BOOL, STRING, INT, FLOAT, ENUM, INTERNAL
    }

    public struct SyncrFunctionType
    {
        public SyncrFunctionType(SyncrParamType t, string[] e = null, string g = "")
        {
            type = t;
            enumChoices = e;
            group = g;
        }

        public SyncrParamType type;
        public string[] enumChoices;
        public string group;
    }

    public class SyncrPlayer
    {
        public Score CurrentScore;
        public IWaveSource Music { get { return currentMusic; } }
        private IWaveSource currentMusic;

        public ISoundOut SoundOut;

        public bool paused = false;
        public delegate void TriggerFunction(string param);

        private int currentMillis = 0;
        private int lastMillis = 0;
        private int triggerIndex = 0;

        public Dictionary<string, TriggerFunction> BoundFunctions;
        List<Trigger> _currentTriggers;
        public bool IsPlaying;
        public List<Trigger> GetCurrentTriggers()
        {
            List<Trigger> temp = new List<Trigger>(_currentTriggers);

            _currentTriggers.Clear();
            return temp;
        }

        public SyncrPlayer()
        {
            BoundFunctions = new Dictionary<string, TriggerFunction>();
            _currentTriggers = new List<Trigger>();
        }

        private MMDevice GetFirstDevice()
        {
            using (var mmdeviceEnumerator = new MMDeviceEnumerator())
            {
                using (
                    var mmdeviceCollection = mmdeviceEnumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active))
                {
                    return mmdeviceCollection[0];
                }
            }
        }

        public void LoadScore(Score _currentScore)
        {
            CurrentScore = _currentScore;

            currentMusic = CurrentScore.MusicSource;

            SoundOut = new DirectSoundOut() { Latency = 100 };
            SoundOut.Initialize(currentMusic);
        }
        public void StartCurrentScore()
        {
            SoundOut.Resume();
            IsPlaying = true;
            Console.WriteLine("Starting score - " + CurrentScore.TrackName);
            RestartIndex();
            currentMusic.Position = 0;
            SoundOut.Play();
        }

        public void SeekScore(float position)
        {
            if (IsPlaying && currentMusic != null)
            {
                int newPosInMillis = (int)(currentMusic.GetLength().TotalMilliseconds * position);
                Console.WriteLine("Seeking to " + newPosInMillis);

                currentMusic.SetPosition(TimeSpan.FromMilliseconds(newPosInMillis));
                UpdateMillis();

                while (CurrentScore.Triggers[triggerIndex].time < currentMillis)
                    SkipNextTrigger();
            }
        }

        public void PauseScore()
        {
            IsPlaying = false;
            if (SoundOut != null)
                SoundOut.Pause();
        }
        public void StopScore()
        {
            IsPlaying = false;
            if (SoundOut != null)
            {
                SoundOut.Stop();
                SoundOut.Dispose();
            }

        }
        public void ResumeScore()
        {
            IsPlaying = true;
            if (SoundOut != null)
                SoundOut.Play();
        }
        public void Update(float _deltatime)
        {
            UpdateMillis();

            if (lastMillis > currentMillis)
                RestartIndex();

            //_currentTriggers.Clear();

            if (CurrentScore.Triggers.Count == 0)
                return;

            if (currentMillis >= CurrentScore.Triggers[triggerIndex].time)
                ActivateNextTrigger();
        }

        private void UpdateMillis()
        {
            lastMillis = currentMillis;
            currentMillis = (int)currentMusic.GetPosition().TotalMilliseconds;
            // Console.WriteLine(currentMillis);

        }

        private void ActivateNextTrigger()
        {
            ActivateTrigger(CurrentScore.Triggers[triggerIndex]);

            _currentTriggers.Add(CurrentScore.Triggers[triggerIndex]);
            triggerIndex += triggerIndex == CurrentScore.Triggers.Count - 1 ? -triggerIndex : 1;
        }

        private void SkipNextTrigger()
        {
            Console.WriteLine("Skipping " + CurrentScore.Triggers[triggerIndex].name);
            triggerIndex += triggerIndex == CurrentScore.Triggers.Count - 1 ? -triggerIndex : 1;
        }

        private void RestartIndex()
        {
            triggerIndex = 0;
        }
        public void BindFunctionToTrigger(string _name, TriggerFunction _function)
        {
            Console.WriteLine("bound " + _name);
            BoundFunctions[_name] = _function;
        }

        private void ActivateTrigger(Trigger t)
        {
            //Console.WriteLine("Looking for " + _name);

            if (BoundFunctions.ContainsKey(t.name))
                BoundFunctions[t.name](t.param);
            else
                Console.WriteLine("No registered function for trigger -> " + t.name.ToUpper());
        }
    }
}
