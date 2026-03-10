using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace osu_music
{
    class OsuSong
    {
        public string Title { get; }
        public string TitleUnicode { get; }
        public string SongPath { get; }
        public string[] AudioFiles { get; }
        public string[] OsuFiles { get; }

        private Random _rnd = new Random();

        public OsuSong(string songPath)
        {
            SongPath = songPath;

            OsuFiles = GetOsuFiles();
            AudioFiles = GetAudioFiles();

            Title = SongTitle();
            TitleUnicode = SongUnicodeTitle();
        }

        private string[] GetOsuFiles()
        {
            return Directory.GetFiles(SongPath, "*.osu");
        }

        public string[] GetOsuFiles(string songPath)
        {
            return Directory.GetFiles(songPath, "*.osu");
        }

        private string[] GetAudioFiles()
        {
            string[] audioFilesArr = new string[OsuFiles.Length];

            for (int i = 0; i < OsuFiles.Length; i++)
            {
                string[] osuFileLines = File.ReadAllLines(OsuFiles[i]);

                string keyWord = "AudioFilename: ";

                string? audioFile = null;

                foreach (string line in osuFileLines)
                {
                    if (line.IndexOf(keyWord) != -1)
                    {
                        int startIndex = line.IndexOf(keyWord);
                        audioFile = line.Substring(startIndex + keyWord.Length);

                        break;
                    }
                }

                if(audioFile != null)
                {
                    audioFilesArr[i] = Path.Combine(SongPath, audioFile);
                }
            }

            return audioFilesArr;
        }

        private string SongUnicodeTitle()
        {
            string titileUnicode = "Unknown Song";

            if(OsuFiles.Length == 0)
            {
                return titileUnicode;
            }

            string[] osuFileLines = File.ReadAllLines(OsuFiles[0]);

            string keyWord = "TitleUnicode:";

            foreach (string line in osuFileLines)
            {
                int startIndex = line.IndexOf(keyWord);

                if (startIndex != -1)
                {
                    titileUnicode = line.Substring(startIndex + keyWord.Length);
                    break;
                }
            }

            return titileUnicode;
        }

        private string SongTitle()
        {
            string titileUnicode = "Unknown Song";

            if (OsuFiles.Length == 0)
            {
                return titileUnicode;
            }

            string[] osuFileLines = File.ReadAllLines(OsuFiles[0]);

            string keyWord = "Title:";

            foreach (string line in osuFileLines)
            {
                int startIndex = line.IndexOf(keyWord);

                if (startIndex != -1)
                {
                    titileUnicode = line.Substring(startIndex + keyWord.Length);
                    break;
                }
            }

            return titileUnicode;
        }
        
        //Todo:
        //ADD: .ogg support
        public string GetRandomAudio()
        {
            int randomIndex = _rnd.Next(AudioFiles.Length);

            string randomAudio = AudioFiles[randomIndex];

            if (File.Exists(randomAudio) && Path.GetExtension(randomAudio) != ".ogg")
            {
                return AudioFiles[randomIndex];
            }

            return null;
        }
    }
}
