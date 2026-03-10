using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace osu_music
{
    class OsuSongLibrary
    {
        public string OsuPath { get; set; }
        public string SongsPath { get; set; }
        public OsuSong[] OsuSongs { get; }
        private string[] _songPaths;

        private Random _rnd = new Random();

        public OsuSongLibrary(string osuPath)
        {
            OsuPath = osuPath;
            SongsPath = Path.Combine(osuPath, "Songs");

            string[] songsPathList = SongsPathList();
            _songPaths = Directory.GetDirectories(SongsPath);

            //OsuSongs = new OsuSong[songsPathList.Length];

            //for (int i = 0; i < OsuSongs.Length; i++)
            //{
            //    OsuSong song = new OsuSong(songsPathList[i]);
            //    OsuSongs[i] = song;
            //    Debug.WriteLine($"Importing song #{i + 1}: {song.Title}");
            //}

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
            int randomIndex = _rnd.Next(0, _songPaths.Length);

            return new OsuSong(_songPaths[randomIndex]);
        }
    }
}
