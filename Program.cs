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

            var audioFile = new AudioFileReader(song.AudioFiles[0]);
            var outputDevice = new WaveOutEvent();

            PlaybackController pc = new PlaybackController(audioFile, outputDevice);

            while (pc.isPlaying())
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.Spacebar || key == ConsoleKey.Enter)
                    {
                        pc.Dispose();
                        break;
                    }
                        
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
                        pc.DT();
                    }
                    else if (key == ConsoleKey.N)
                    {
                        pc.NC();
                    }
                    else if (key == ConsoleKey.UpArrow)
                    {
                        Console.WriteLine($"Volume: {pc.HigherVolume():P0}");
                    }
                    else if (key == ConsoleKey.DownArrow)
                    {
                        Console.WriteLine($"Volume: {pc.LowerVolume():P0}");
                    }
                }
                Thread.Sleep(1000 / 60);
            }
            Console.WriteLine("Stopped playing");
        }
    }

}