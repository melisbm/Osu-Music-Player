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

        public PlaybackController(AudioFileReader audioFile, WaveOutEvent outputDevice)
        {
            _soundTouch = new SoundTouchWaveProvider(audioFile);
            OutputDevice = outputDevice;

            OutputDevice.Volume = Volume;
            _soundTouch.Tempo = 1.0f;

            OutputDevice.Init(_soundTouch);
            OutputDevice.Play();
        }

        public void playbackSpeed(float speed)
        {
            _soundTouch.Tempo = speed;
        }

        public bool isPlaying()
        {
            return OutputDevice.PlaybackState == PlaybackState.Playing;
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
