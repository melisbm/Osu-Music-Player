using NAudio.Wave;
using SoundTouch.Net.NAudioSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace osu_music
{
    class PlaybackController
    {
        private SoundTouchWaveProvider _soundTouch;
        private WaveOutEvent OutputDevice { get; set; }
        public float PlaybackSpeed = 1.0f;
        public float Volume { get; set; } = 0.5f;
        private bool _isDT = false;
        private bool _isNC = false;
        public bool IsDT => _isDT;
        public bool IsNC => _isNC;

        public PlaybackController(AudioFileReader audioFile, WaveOutEvent outputDevice)
        {
            _soundTouch = new SoundTouchWaveProvider(audioFile);
            OutputDevice = outputDevice;

            OutputDevice.Volume = Volume;

            OutputDevice.Init(_soundTouch);
            OutputDevice.Play();
        }

        public PlaybackController(WaveOutEvent outputDevice)
        {
            OutputDevice = outputDevice;

            OutputDevice.Volume = Volume;
        }

        public void PlaySong(AudioFileReader audioFile)
        {
            _soundTouch = new SoundTouchWaveProvider(audioFile);

            if (_isNC)
            {
                _soundTouch.Tempo = 1.5f;
                _soundTouch.Pitch = 1.5f;
            }
            else if (_isDT)
            {
                _soundTouch.Pitch = 1.0f;
                _soundTouch.Tempo = 1.5f;
            }
            else
            {
                _soundTouch.Tempo = 1.0f;
                _soundTouch.Pitch = 1.0f;
            }

            if (OutputDevice != null && OutputDevice.PlaybackState != PlaybackState.Stopped)
            {
                OutputDevice.Stop();
            }

            if (OutputDevice != null)
            {
                OutputDevice.Init(_soundTouch);
                OutputDevice.Play();
            }
        }

        public void Stop()
        {
            if (OutputDevice != null)
            {
                OutputDevice.Stop();
            }

            if (_soundTouch != null)
            {
                _soundTouch.Clear();
            }
        }

        public void playbackSpeed(float speed)
        {
            _soundTouch.Tempo = speed;
        }

        public void DT()
        {
            _soundTouch.Clear();

            _isNC = false;
            _soundTouch.Pitch = 1.0f;
            if (!_isDT)
            {
                _soundTouch.Tempo = 1.5f;
                _isDT = true;
            }
            else
            {
                _soundTouch.Tempo = 1.0f;
                _isDT = false;
            }
        }

        public void NC()
        {
            OutputDevice.Stop();
            _soundTouch.Clear();

            _isDT = false;
            if (!_isNC)
            {
                _soundTouch.Tempo = 1.5f;
                _soundTouch.Pitch = 1.5f;
                _isNC = true;
            }
            else
            {
                _soundTouch.Tempo = 1.0f;
                _soundTouch.Pitch = 1.0f;
                _isNC = false;
            }

            OutputDevice.Play();

        }

        public bool isPlaying()
        {
            return OutputDevice != null && OutputDevice.PlaybackState == PlaybackState.Playing;
        }

        public void Dispose()
        {
            if (OutputDevice != null)
            {
                OutputDevice.Stop();
                OutputDevice.Dispose();
                OutputDevice = null;
            }
        }

        public void LowerVolume(float volume)
        {
            OutputDevice.Volume = volume;
        }

        public float LowerVolume()
        {
            if (this.Volume - 0.05f > 0.0f)
            {
                this.Volume -= 0.05f;
                OutputDevice.Volume = Volume;
            }

            return this.Volume;
        }

        public float HigherVolume()
        {
            if (this.Volume + 0.05f < 1.0f)
            {
                this.Volume += 0.05f;
                OutputDevice.Volume = Volume;
            }

            return this.Volume;
        }
    }
}
