using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using CSCore;
using CSCore.Codecs;
using CSCore.Codecs.WAV;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;

namespace Syncr
{

    public class Score
    {
        public Score() { }

        public string TrackName = "NO_NAME";
        public int musicResource = 0;
        public List<Trigger> Triggers;
        public bool looping = false;
        public IWaveSource MusicSource;

        public Score(UnmanagedMemoryStream _audioRes, string _textRes, string _name)
        {
            Triggers = new List<Trigger>();
            MusicSource = GetSoundSource(_audioRes);
            TrackName = _name;

            if (LoadTriggers(_textRes))
                Triggers.Sort();
        }

        public Score(string _audioFile, string _textRes, string _name)
        {
            Triggers = new List<Trigger>();
            if (_audioFile == "" && _textRes == null)
                return;
            MusicSource = LoadFromFile(_audioFile);
            TrackName = _name;

            if (LoadTriggers(_textRes))
                Triggers.Sort();
        }

        public bool IsNullOrWhiteSpace(string value)
        {
            if (value == null) return true;
            return string.IsNullOrEmpty(value.Trim());
        }

        private bool LoadTriggers(string textFile)
        {
            string[] lines = textFile.Split('\n');


            foreach (string line in lines)
            {
                if (IsNullOrWhiteSpace(line))
                    continue;

                //Console.WriteLine("LINE: " + line);

                string[] parts = line.Split('\t');


                float time = float.Parse(parts[0]);
                string name = parts[2];

                ParseTriggerString(name, time);
            }

            return true;
        }

        bool commentsSection = false;
        private void ParseTriggerString(string fullTrigger, float time)
        {
            fullTrigger = fullTrigger.Trim();
            string[] args = fullTrigger.Split(' ');
            string name = args[0];

            if (name == "COMMENTS_END")
            {
                commentsSection = false;
                return;
            }

            if (commentsSection)
                return;

            if (name == "COMMENTS")
            {
                commentsSection = true;
                return;
            }


            string param = "";
            if (args.Length > 1)
                for (int x = 1; x < args.Length; x++)
                    param += args[x] + " ";

            param.Trim();

            int bTimeMillis = (int)Math.Floor((time * 1000.0f));
            if (param != "")
                Console.WriteLine("Loading new trigger: " + name + " and " + param);
            else
                Console.WriteLine("Loading new trigger: " + name);
            Trigger newTrig = new Trigger(name, bTimeMillis, param);
            Triggers.Add(newTrig);
        }

        private IWaveSource GetSoundSource(Stream stream)
        {
            return new WaveFileReader(stream);
        }

        private IWaveSource LoadFromFile(string filepath)
        {
            Console.WriteLine("Looking for " + filepath);
            return CodecFactory.Instance.GetCodec(filepath);
        }
    }

    public struct Trigger : IComparable
    {
        public Trigger(string _name, int _time, string _param)
        {
            name = _name;
            time = _time;

            param = _param;
            triggered = false;
        }

        public string name;
        public int time;
        public string param;
        private bool triggered;

        public int CompareTo(object obj)
        {
            Trigger t = (Trigger)obj;
            return Math.Sign(time - t.time);
        }
    };

}
