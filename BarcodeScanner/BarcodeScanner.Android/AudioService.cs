﻿using Android.Media;
using BarcodeScanner.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(AudioService))]
namespace BarcodeScanner.Droid
{
    public class AudioService : IAudio
    {
        public AudioService()
        { }
        public void PlayAudioFile(string fileName)
        {
            var player = new MediaPlayer();
            var fd = global::Android.App.Application.Context.Assets.OpenFd(fileName);
            player.Prepared += (s, e) =>
            {
                player.Start();
            };
            player.SetDataSource(fd.FileDescriptor, fd.StartOffset, fd.Length);
            player.Prepare();
        }
    }
}