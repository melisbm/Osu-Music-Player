using NAudio.Wave;
using SoundTouch.Net.NAudioSupport;
using System;

namespace osu_music
{
    class PlaybackController
    {
        private SoundTouchWaveProvider _soundTouch;
        private WaveOutEvent _outputDevice;
        public WaveOutEvent OutputDevice
        {
            get => _outputDevice;
            set
            {
                if (value == null) throw new NullReferenceException("Output Device (WaveOutEvent) must not be null.");
                _outputDevice = value;
            }
        }

        private float _volume = 0.5f;
        public float Volume
        {
            get => _volume;
            set
            {
                if (value < 0.0f)
                {
                    throw new ArgumentException("The volume cannot be negative.");
                }

                _volume = value;
            }
        }

        private const float VOLUME_ADDITION = 0.05f;

        private bool _isDT = false;
        private bool _isNC = false;
        public bool IsDT => _isDT;
        public bool IsNC => _isNC;

        private const float DEFAULT_TEMPO = 1.0f;
        private const float DEFAULT_PITCH = 1.0f;

        private const float DT_TEMPO = 1.5f;
        private const float DT_PITCH = 1.0f;

        private const float NC_TEMPO = 1.5f;
        private const float NC_PITCH = 1.5f;

        public PlaybackController(WaveOutEvent outputDevice)
        {
            if (outputDevice == null)
            {
                throw new NullReferenceException("Output Device (WaveOutEvent) must not be null.");
            }

            OutputDevice = outputDevice;
            OutputDevice.Volume = Volume;
        }

        public void PlaySong(AudioFileReader audioFile)
        {
            _soundTouch = new SoundTouchWaveProvider(audioFile);

            if (_isNC)
            {
                _soundTouch.Tempo = NC_TEMPO;
                _soundTouch.Pitch = NC_PITCH;
            }
            else if (_isDT)
            {
                _soundTouch.Pitch = DT_PITCH;
                _soundTouch.Tempo = DT_TEMPO;
            }
            else
            {
                _soundTouch.Tempo = DEFAULT_TEMPO;
                _soundTouch.Pitch = DEFAULT_PITCH;
            }

            if (OutputDevice.PlaybackState != PlaybackState.Stopped)
            {
                OutputDevice.Stop();
            }

            OutputDevice.Init(_soundTouch);
            OutputDevice.Play();
        }

        public void Play()
        {
            if (_soundTouch != null)
            {
                OutputDevice.Play();
            }
        }

        public void Stop()
        {
            if (_soundTouch != null)
            {
                OutputDevice.Stop();
            }
        }

        public void DT()
        {
            if (_soundTouch != null)
            {
                _soundTouch.Clear();

                _soundTouch.Tempo = DT_TEMPO;
                _soundTouch.Pitch = DT_PITCH;

                _isNC = false;
                _isDT = true;
            }
        }

        public void NC()
        {
            if (_soundTouch != null)
            {
                _soundTouch.Clear();

                _soundTouch.Tempo = NC_TEMPO;
                _soundTouch.Pitch = NC_PITCH;

                _isDT = false;
                _isNC = true;
            }
        }

        public void ResetPitch()
        {
            if (_soundTouch != null)
            {
                _soundTouch.Pitch = DEFAULT_PITCH;
            }

            _isDT = false;
            _isNC = false;
        }

        public void ResetTempo()
        {
            if (_soundTouch != null)
            {
                _soundTouch.Tempo = DEFAULT_TEMPO;
            }

            _isDT = false;
            _isNC = false;
        }

        public bool IsPlaying()
        {
            return OutputDevice.PlaybackState == PlaybackState.Playing;
        }

        public void Dispose()
        {
            OutputDevice.Stop();
            OutputDevice.Dispose();
        }

        public float LowerVolume()
        {
            if (this.Volume - VOLUME_ADDITION > 0.0f)
            {
                this.Volume -= VOLUME_ADDITION;
                OutputDevice.Volume = Volume;
            }

            return this.Volume;
        }

        public float HigherVolume()
        {
            if (this.Volume + VOLUME_ADDITION < 1.0f)
            {
                this.Volume += VOLUME_ADDITION;
                OutputDevice.Volume = Volume;
            }

            return this.Volume;
        }
    }
}
