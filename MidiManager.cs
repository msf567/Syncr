using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using Sanford.Multimedia;
using Sanford.Multimedia.Midi;
namespace Syncr
{
    public class MidiManager
    {
        InputDevice inputDevice = null;
        SyncrControlForm control;
        bool initialized;
        SyncrMidiMap map = null;
        public MidiManager(SyncrControlForm _control)
        {
            control = _control;
            initialized = InitMidiDevice();
            if (initialized)
                Console.WriteLine("Midi initialized on " + inputDevice.DeviceID.ToString());
            else
                Console.WriteLine("Midi failed to initialize!");
        }

        public void Play()
        {
            if (initialized)
            {
                inputDevice.StartRecording();
                Console.WriteLine("Started Midi Recording!");
            }
            else
            {
                initialized = InitMidiDevice();
                if (initialized)
                    Console.WriteLine("Midi initialized on " + inputDevice.DeviceID.ToString());
                else
                    Console.WriteLine("Midi failed to initialize!");
            }
        }

        public void Stop()
        {
            if (initialized)
                inputDevice.StopRecording();
        }

        public void SetMidiMap(SyncrMidiMap _map)
        {
            map = _map;
            map.LoadMappings();
            Console.WriteLine("LOADED MIDI MAP");
        }

        private bool InitMidiDevice()
        {
            if (InputDevice.DeviceCount == 0)
            {
                Console.WriteLine("No MIDI Devices found!");
                return false;
            }
            else
                Console.WriteLine("Found " + InputDevice.DeviceCount + " MIDI  Devices");
            inputDevice = new InputDevice(InputDevice.DeviceCount > 1 ? 1:0);
            inputDevice.ChannelMessageReceived += HandleChannelMessageReceived;
            inputDevice.Error += new EventHandler<ErrorEventArgs>(inDevice_Error);

            return true;
        }

        private void inDevice_Error(object sender, ErrorEventArgs e)
        {
            MessageBox.Show(e.Error.Message, "Error!",
                   MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        private void HandleChannelMessageReceived(object sender, ChannelMessageEventArgs e)
        {
            //Console.WriteLine("new msg");
            if (map != null)
                map.ExecuteAction(e);

            
          //  Console.WriteLine(e.Message.Command.ToString() + '\t' + '\t' +
           // e.Message.MidiChannel.ToString() + '\t' +
           // e.Message.Data1.ToString() + '\t' +
           // e.Message.Data2.ToString() + '\t' +
           // e.Message.Status.ToString());
            
        }
    }
}
