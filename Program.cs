using System.IO;
using System;
using NAudio.Wave;

string currentUsername = Environment.GetEnvironmentVariable("USERNAME");
Console.WriteLine(currentUsername);

string osuPath = $@"C:\Users\{currentUsername}\AppData\Local\osu!";

string songsPath = osuPath + "\\Songs";
string songPath;
string[] songs = Directory.GetDirectories(songsPath);

Random rnd = new Random();


while (true)
{
    int num = rnd.Next(1, songs.Length + 1);

    songPath = songs[num];
    Console.WriteLine(songPath);
    string[] songInfos = Directory.GetFiles(songPath, "*.osu");
 
    int num2 = rnd.Next(0, songInfos.Length);

    string songInfo = songInfos[num2];


    string[] songInfoLines = File.ReadAllLines(songInfo);

    int index = songInfoLines[3].IndexOf("AudioFilename: ");
    string res = songInfoLines[3].Substring(index + "AudioFilename: ".Length);

    Console.WriteLine(res);
    using (var audioFile = new AudioFileReader(songPath + "\\" + res))
    using (var outputDevice = new WaveOutEvent())
    {
        outputDevice.Init(audioFile);
        outputDevice.Play();
        Console.WriteLine("Song playing");
        while (outputDevice.PlaybackState == PlaybackState.Playing)
        {
            Thread.Sleep(1000);
        }
        Console.WriteLine("Stopped playing");
    }

}



