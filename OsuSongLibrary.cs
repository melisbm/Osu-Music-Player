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

        public OsuSongLibrary(string osuPath)
        {
            OsuPath = osuPath;
            SongsPath = System.IO.Path.Combine(osuPath, "Songs");

            string[] songsPathList = getSongsPathList();

            OsuSongs = new OsuSong[songsPathList.Length];

            for(int i = 0; i < OsuSongs.Length; i++)
            {
                OsuSongs[i] = new OsuSong(songsPathList[i]);
            }

        }

        private string[] getSongsPathList()
        {
            return Directory.GetDirectories(SongsPath);
        }

        public string[] getSongsPathList(string path)
        {
            return Directory.GetDirectories(path);
        }

        public OsuSong randomSong()
        {
            Random rnd = new Random();
            int randomSongIndex = rnd.Next(0, OsuSongs.Length);

            return OsuSongs[randomSongIndex];
        }
    }
}
