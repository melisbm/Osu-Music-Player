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

        public PlaybackController(AudioFileReader audioFile, WaveOutEvent outputDevice)
        {
            _soundTouch = new SoundTouchWaveProvider(audioFile);
            OutputDevice = outputDevice;

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
    }
}
