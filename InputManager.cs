using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Text;

namespace osu_music
{
    class InputManager
    {

        public InputManager()
        {
        }
        public void Interact(PlaybackController playbackController, WaveOutEvent outputDevice)
        {

            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.Spacebar:
                    case ConsoleKey.Enter:
                        playbackController.Stop();
                        break;

                    case ConsoleKey.P:
                        outputDevice.Pause();
                        Console.WriteLine("Paused");
                        while (true)
                        {
                            var resumeKey = Console.ReadKey(true).Key;
                            if (resumeKey == ConsoleKey.P)
                            {
                                outputDevice.Play();
                                Console.WriteLine("Unpaused");
                                break;
                            }
                        }
                        break;

                    case ConsoleKey.D:
                        playbackController.DT();
                        break;

                    case ConsoleKey.N:
                        playbackController.NC();
                        break;

                    case ConsoleKey.UpArrow:
                        Console.WriteLine($"Volume: {playbackController.HigherVolume():P0}");
                        break;

                    case ConsoleKey.DownArrow:
                        Console.WriteLine($"Volume: {playbackController.LowerVolume():P0}");
                        break;

                    default:
                        break;
                }
            }

        }


    }
}
