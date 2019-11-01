using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.IO;

namespace Syncr
{
    public class QuadScore
    {
        public List<QuadFrame> frames;

        public QuadScore() { }

        public QuadScore(string filePath)
        {
            string fileContents = File.ReadAllText(filePath);
            var obj = JsonConvert.DeserializeObject<FramesWrapper>(fileContents);
            if (obj == null)
                Console.WriteLine("FAILED TO DESERIALIZE!");
            Console.WriteLine("Loaded " + obj.Items.Length + " quad frames!");
            frames = new List<QuadFrame>(obj.Items);
        }
    }

    [System.Serializable]
    public class FramesWrapper
    {
        public QuadFrame[] Items;
    }

    [System.Serializable]
    public struct QuadFrame
    {
        public uint millis;
        public Quad[] quads;
    }

    [System.Serializable]
    public struct Quad
    {
        public string ID;
        public string tag;
        public float x;
        public float y;
        public float w;
        public float h;
    }
}
