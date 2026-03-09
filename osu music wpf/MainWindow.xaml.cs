using NAudio.Wave;
using osu_music;
using System.Windows;
using System.Windows.Threading;

namespace osu_music_wpf
{
    public partial class MainWindow : Window
    {
        private PlaybackController _playbackController;
        private OsuSong _song;
        private OsuSongLibrary _lib;
        AudioFileReader _audioFile;
        WaveOutEvent _outputDevice;
        private DispatcherTimer _timer;

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
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void btna_Click(object sender, RoutedEventArgs e)
        {
            _playbackController.Stop();
            _audioFile.Dispose();
            
            _song = _lib.RandomSong();
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
            if (!_playbackController.IsPlaying())
            {
                _playbackController.Stop();
                _song = _lib.RandomSong();
                _audioFile = new AudioFileReader(_song.GetRandomAudio());
                _playbackController.PlaySong(_audioFile);
                SongName.Text = $"Now Playing: {_song.Title}";
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_audioFile != null)
            {
                var current = _audioFile.CurrentTime;
                var total = _audioFile.TotalTime;
                TimeLine.Text = $"{current:mm\\:ss} / {total:mm\\:ss}";
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            _playbackController.ResetTempo();
            _playbackController.ResetPitch();
        }
    }
}