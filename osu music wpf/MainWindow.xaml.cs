using NAudio.Wave;
using osu_music;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace osu_music_wpf
{
    public partial class MainWindow : Window
    {
        private PlaybackController _playbackController;
        private OsuSong _song;
        private OsuSongLibrary _lib;
        AudioFileReader _audioFile;
        WaveOutEvent _outputDevice;
        public MainWindow()
        {
            InitializeComponent();
            a.Text = "Loading...";
            string currentUsername = Environment.GetEnvironmentVariable("USERNAME");

            if (currentUsername == null)
            {
                string messageBoxText = "Enviroment username couldn't be found.";
                string caption = "Enviroment username not found";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Error;

                MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
                Close();
            }

            string osuPath = $@"C:\Users\{currentUsername}\AppData\Local\osu!";
            _lib = new OsuSongLibrary(osuPath);
            a.Text = "Osu! Music Player";

            _outputDevice = new WaveOutEvent();
            _outputDevice.PlaybackStopped += OnPlaybackStopped;

            _playbackController = new PlaybackController(_outputDevice);

            _song = _lib.RandomSong();
            SongName.Text = $"Now Playing: {_song.Title}";

            string randomAudio = _song.GetRandomAudio();

            while (randomAudio == null)
            {
                _song = _lib.RandomSong();
                randomAudio = _song.GetRandomAudio();
            }
            _audioFile = new AudioFileReader(randomAudio);

            _playbackController.PlaySong(_audioFile);

            

        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            _playbackController.Stop();
            _audioFile.Dispose();
            
            _song = _lib.RandomSong();
            String songAudio = _song.GetRandomAudio();
            while(songAudio == null)
            {
                _song = _lib.RandomSong();
                songAudio = _song.GetRandomAudio();
            }
            _audioFile = new AudioFileReader(songAudio);
            _playbackController.PlaySong(_audioFile);
            SongName.Text = $"Now Playing: {_song.Title}";

        }

        private void btnDT_Click(object sender, RoutedEventArgs e)
        {
            _playbackController.DT();
        }

        private void btnNC_Click(object sender, RoutedEventArgs e)
        {
            _playbackController.NC();
        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (!_playbackController.isPlaying())
            {
                _playbackController.Stop();
                _song = _lib.RandomSong();
                _audioFile = new AudioFileReader(_song.GetRandomAudio());
                _playbackController.PlaySong(_audioFile);
                SongName.Text = $"Now Playing: {_song.Title}";
            }
        }
    }
}