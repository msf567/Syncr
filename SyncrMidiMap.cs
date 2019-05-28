using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;
namespace Syncr
{
   public class SyncrMidiMap
    {
        public delegate void MidiAction(ChannelMessageEventArgs e);
        public Dictionary<int, MidiAction> ActionMappings = new Dictionary<int, MidiAction>();

        public SyncrControlForm control;

        public SyncrMidiMap(SyncrControlForm _control)
        {
            control = _control;
        }

        public virtual void LoadMappings() { }
        
        public void ExecuteAction(ChannelMessageEventArgs e)
        {
            int pitch = e.Message.Data1;
            if (ActionMappings.ContainsKey(pitch)) {
                ActionMappings[pitch](e);
            }
        }
    }
}
