using System;
using System.Collections.Generic;
using System.Text;

namespace osu_music
{
    class OsuSong
    {
        public string Title { get; }
        public string TitleUnicode { get; }
        public string Path { get; }
        public string[] AudioFiles { get; }
        public string[] OsuFiles { get; }

        public OsuSong(string songPath)
        {
            Path = songPath;

            OsuFiles = getOsuFiles();
            AudioFiles = getAudioFiles();

            Title = getSongTitle();
            TitleUnicode = getSongUnicodeTitle();
        }

        private string[] getOsuFiles()
        {
            return Directory.GetFiles(Path, "*.osu");
        }

        public string[] getOsuFiles(string songPath)
        {
            return Directory.GetFiles(songPath, "*.osu");
        }

        private string[] getAudioFiles()
        {
            string[] audioFilesArr = new string[OsuFiles.Length];

            for (int i = 0; i < OsuFiles.Length; i++)
            {
                string[] osuFileLines = File.ReadAllLines(OsuFiles[i]);

                string keyWord = "AudioFilename: ";

                int startIndex = osuFileLines[3].IndexOf(keyWord);
                string audioFile = osuFileLines[3].Substring(startIndex + keyWord.Length);

                audioFilesArr[i] = System.IO.Path.Combine(Path, audioFile);
            }

            return audioFilesArr;
        }

        private string getSongUnicodeTitle()
        {
            string titileUnicode = "Unknown Song";

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

        private string getSongTitle()
        {
            string titileUnicode = "Unknown Song";

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
    }
}
