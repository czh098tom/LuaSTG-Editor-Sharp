using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using CSCore;
using CSCore.Codecs;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;

namespace LuaSTGEditorSharp.Windows.Input.MediaPlayer
{
    /// <summary>
    /// MediaPlayer.xaml 的交互逻辑
    /// </summary>
    public partial class MediaPlayer : UserControl
    {
        static MediaPlayer()
        {
            CodecFactory
                .Instance
                .Register("ogg-vorbis", new CodecFactoryEntry((s) => new OggSource(s).ToWaveSource(), ".ogg"));
        }

        private static ISoundOut _soundOut;
        private static IWaveSource _waveSource;

        private static MMDevice _firstActive;

        public event EventHandler<PlaybackStoppedEventArgs> PlaybackStopped;

        private Uri source = null;

        public Uri Source
        {
            get
            {
                return source;
            }
            set
            {
                source = value;
                if (source != null) Load(source);
            }
        }

        public void Load(Uri source)
        {
            CleanupPlayback();

            //MessageBox.Show(string.Join(",", CodecFactory.Instance.GetSupportedFileExtensions()));
            var a = CodecFactory.Instance.GetCodec(source);
            _waveSource = a
                    .ToSampleSource()
                    .ToMono()
                    .ToWaveSource();
            _soundOut = new WasapiOut() { Latency = 100, Device = _firstActive };
            _soundOut.Initialize(_waveSource);
            if (PlaybackStopped != null) _soundOut.Stopped += PlaybackStopped;
        }

        public void Play()
        {
            if (_soundOut != null)
                _soundOut.Play();
        }

        private void CleanupPlayback()
        {
            if (_soundOut != null)
            {
                _soundOut.Dispose();
                _soundOut = null;
            }
            if (_waveSource != null)
            {
                _waveSource.Dispose();
                _waveSource = null;
            }
        }

        public void Pause()
        {
            if (_soundOut != null)
                _soundOut.Pause();
        }

        public void Stop()
        {
            if (_soundOut != null)
                _soundOut.Stop();
        }

        public MediaPlayer()
        {
            InitializeComponent();

            Unloaded += (o, e) => CleanupPlayback();
            List<MMDevice> devices = new List<MMDevice>();
            _firstActive = new MMDeviceEnumerator().GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);
        }
    }
}
