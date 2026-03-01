using System.IO;
using System;
using NAudio.Wave;
using osu_music;
class Program
{
    static void Main()
    {

        string currentUsername = Environment.GetEnvironmentVariable("USERNAME");

        string osuPath = $@"C:\Users\{currentUsername}\AppData\Local\osu!";

        string songsPath = osuPath + "\\Songs";

        OsuSongLibrary lib = new OsuSongLibrary(osuPath);

        

        while (true)
        {

            OsuSong song = lib.randomSong();

            Console.WriteLine($"Now Playing: {song.Title}");

            using (var audioFile = new AudioFileReader(song.AudioFiles[0]))
            using (var outputDevice = new WaveOutEvent())
            {
                outputDevice.Init(audioFile);
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
                    }
                    Thread.Sleep(1000 / 60);
                }

                Console.WriteLine("Stopped playing");
            }

        }
    }

}



