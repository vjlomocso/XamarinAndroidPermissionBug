using System;
using Android.Media;

namespace MicrophonePermission
{
    public class Recorder : IDisposable
    {
        public const int DefaultBufferSize = 4000;
        public const ChannelIn Channel = ChannelIn.Mono;
        public const Encoding AudioEncoding = Encoding.Pcm16bit;

        private static readonly int[] SAMPLE_RATES = { 44100, 22050, 11025, 16000, 8000 };
        private readonly AudioRecord _recorder;
        private readonly int _sampleRate;
        private readonly int _bufferSize;
        public bool IsRecording { get; private set; }

        public Recorder()
        {
            _sampleRate = Init();

            _bufferSize = AudioRecord.GetMinBufferSize(_sampleRate, Channel, AudioEncoding) * 3;

            _recorder = new AudioRecord(AudioSource.Mic,
                                        _sampleRate, Channel,
                                        AudioEncoding, _bufferSize);
        }

        public static int Init()
        {
            foreach (var rate in SAMPLE_RATES)
            {  // add the rates you wish to check against
                var bufferSize = AudioRecord.GetMinBufferSize(rate, Channel, AudioEncoding);
                if (bufferSize > 0)
                {
                    return rate;
                }
            }

            throw new NotSupportedException("Sample rate is not supported.");
        }

        public void Start()
        {
            _recorder.StartRecording();
            IsRecording = true;
        }

        public void Stop()
        {
            _recorder.Stop();
            IsRecording = false;
        }

        public void Dispose()
        {
            _recorder.Dispose();
        }
    }
}
