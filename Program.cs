using System;
using System.Threading;
using NAudio.Wave;
using osu_music;
using SoundTouch.Net.NAudioSupport;

class Program
{
    static void Main()
    {
        string currentUsername = Environment.GetEnvironmentVariable("USERNAME");
        string osuPath = $@"C:\Users\{currentUsername}\AppData\Local\osu!";
        OsuSongLibrary lib = new OsuSongLibrary(osuPath);

        while (true)
        {
            OsuSong song = lib.randomSong();
            Console.WriteLine($"Now Playing: {song.Title}");

            using (var audioFile = new AudioFileReader(Path.Combine(songPath, res)))
            using (var outputDevice = new WaveOutEvent())
            {
                var soundTouch = new SoundTouchWaveProvider(audioFile);
                soundTouch.Tempo = 1.0f;
                outputDevice.Init(soundTouch);
                outputDevice.Play();

                while (outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true).Key;
                        if (key == ConsoleKey.N)
                            break;
                        else if (key == ConsoleKey.P)
                        {
                            outputDevice.Pause();
                            Console.WriteLine("Paused. Press R to resume.");
                            while (true)
                            {
                                var resumeKey = Console.ReadKey(true).Key;
                                if (resumeKey == ConsoleKey.R)
                                {
                                    outputDevice.Play();
                                    Console.WriteLine("Unpaused");
                                    break;
                                }
                            }
                        }
                        else if (key == ConsoleKey.D)
                        {
                            soundTouch.Tempo = 1.5f;
                        }
                    }
                    Thread.Sleep(1000 / 60);
                }
                Console.WriteLine("Stopped playing");
            }
        }
    }
}