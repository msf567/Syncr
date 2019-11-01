using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Syncr
{
   public class QuadScoreRunner
    {
        QuadScore CurrentScore;
        public int CurrentMillis { get; private set; } = 0;
        private int lastMillis = 0;
        private int frameIndex = 0;
        List<QuadFrame> _currentFrames;
        public delegate void NewFrame(QuadFrame f);
        public NewFrame OnNewFrame;

        public QuadScoreRunner(QuadScore _currentScore)
        {
            _currentFrames = new List<QuadFrame>();
            CurrentScore = _currentScore;

        }

        public void Update(float _deltatime)
        {
            CurrentMillis += (int)(_deltatime * 1000);

            if (lastMillis > CurrentMillis)
                RestartIndex();

            if (CurrentScore.frames.Count == 0)
                return;

            if (CurrentMillis >= CurrentScore.frames[frameIndex].millis)
                TriggerNextFrame();

            lastMillis = CurrentMillis;
        }

        public void SeekScore(int millis)
        {
            int newPosInMillis = millis;

            while (CurrentScore.frames[frameIndex].millis < CurrentMillis)
                SkipNextTrigger();
        }

        private void TriggerNextFrame()
        {
         //   Console.WriteLine("Triggering frame " + frameIndex);
            if (OnNewFrame != null)
                OnNewFrame(CurrentScore.frames[frameIndex]);
            frameIndex += frameIndex == CurrentScore.frames.Count - 1 ? -frameIndex : 1;
        }

        private void RestartIndex()
        {
            frameIndex = 0;
        }

        private void SkipNextTrigger()
        {
            frameIndex += frameIndex == CurrentScore.frames.Count - 1 ? -frameIndex : 1;
        }
    }
}
