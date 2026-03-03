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

        private Random _rnd = new Random();

        public OsuSong(string songPath)
        {
            Path = songPath;

            OsuFiles = GetOsuFiles();
            AudioFiles = GetAudioFiles();

            Title = SongTitle();
            TitleUnicode = SongUnicodeTitle();
        }

        private string[] GetOsuFiles()
        {
            return Directory.GetFiles(Path, "*.osu");
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

                if(audioFile == null)
                {
                    audioFilesArr[i] = null;
                }
                else
                {
                    audioFilesArr[i] = System.IO.Path.Combine(Path, audioFile);
                }
            }

            return audioFilesArr;
        }

        private string SongUnicodeTitle()
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

        private string SongTitle()
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

        //FIX: Add seen array to check if all audio files dont exist
        //ADD: .ogg support
        public string GetRandomAudio()
        {
            int randomIndex = _rnd.Next(AudioFiles.Length);
            int count = 0;

            while (count < 300)
            {
                if (File.Exists(AudioFiles[randomIndex]) && System.IO.Path.GetExtension(AudioFiles[randomIndex]) != ".ogg")
                {
                    return AudioFiles[randomIndex];
                }
                

                randomIndex = _rnd.Next(AudioFiles.Length);
                count++;
            }

            return null;
        }
    }
}
