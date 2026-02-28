using System.IO;
using System;
using NAudio.Wave;

class Program
{
    static void Main()
    {
        string currentUsername = Environment.GetEnvironmentVariable("USERNAME");

        string osuPath = $@"C:\Users\{currentUsername}\AppData\Local\osu!";

        string songsPath = osuPath + "\\Songs";
        string songPath;
        string[] songs = Directory.GetDirectories(songsPath);

        Random rnd = new Random();


        while (true)
        {
            Console.Clear();
            int num = rnd.Next(0, songs.Length);

            songPath = songs[num];
            
            string[] songInfos = Directory.GetFiles(songPath, "*.osu");

            int num2 = rnd.Next(0, songInfos.Length);

            string songInfo = songInfos[num2];


            string[] songInfoLines = File.ReadAllLines(songInfo);

            int index = songInfoLines[3].IndexOf("AudioFilename: ");
            string res = songInfoLines[3].Substring(index + "AudioFilename: ".Length);

            string songName = Path.GetFileName(songPath);

            Console.WriteLine($"Now Playing: {songName}");

            using (var audioFile = new AudioFileReader(Path.Combine(songPath, res)))
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



