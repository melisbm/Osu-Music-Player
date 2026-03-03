using System;
using System.Threading;
using NAudio.Wave;
using osu_music;
using SoundTouch.Net.NAudioSupport;

class Program
{
    static void Main()
    {
        Console.WriteLine("Loading...");
        string currentUsername = Environment.GetEnvironmentVariable("USERNAME");
        string osuPath = $@"C:\Users\{currentUsername}\AppData\Local\osu!";
        OsuSongLibrary lib = new OsuSongLibrary(osuPath);

        
        InputManager inputManager = new InputManager();
        WaveOutEvent? outputDevice = new WaveOutEvent();

        PlaybackController playbackController = new PlaybackController(outputDevice);

        while (true)
        {
            Console.Clear();
            OsuSong song = lib.RandomSong();
            Console.WriteLine($"Now Playing: {song.Title}");
            string randomAudio = song.GetRandomAudio();

            while(randomAudio == null)
            {
                song = lib.RandomSong();
                randomAudio = song.GetRandomAudio();
            }
            AudioFileReader audioFile = new AudioFileReader(randomAudio);

            playbackController.PlaySong(audioFile);

            while (playbackController.isPlaying())
            {
                inputManager.Interact(playbackController, outputDevice);
                Thread.Sleep(1000 / 60);
            }
            Console.WriteLine("Stopped playing");
        }
    }

}