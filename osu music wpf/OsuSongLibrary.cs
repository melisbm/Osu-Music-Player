using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace osu_music
{
    class OsuSongLibrary
    {
        public string OsuPath { get; set; }
        public string SongsPath { get; set; }
        public OsuSong[] OsuSongs { get; }

        private Random _rnd = new Random();

        public OsuSongLibrary(string osuPath)
        {
            OsuPath = osuPath;
            SongsPath = System.IO.Path.Combine(osuPath, "Songs");

            string[] songsPathList = SongsPathList();

            OsuSongs = new OsuSong[songsPathList.Length];

            for (int i = 0; i < OsuSongs.Length; i++) {
                OsuSong song = new OsuSong(songsPathList[i]);
                OsuSongs[i] = song;
                Console.WriteLine($"Importing song #{i + 1}: {song.Title}");
            }

        }

        private string[] SongsPathList()
        {
            return Directory.GetDirectories(SongsPath);
        }

        public string[] SongsPathList(string path)
        {
            return Directory.GetDirectories(path);
        }

        public OsuSong RandomSong()
        {
            int randomSongIndex = _rnd.Next(0, OsuSongs.Length);

            return OsuSongs[randomSongIndex];
        }
    }
}
