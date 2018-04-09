using FFmpegInterop;
using MediaPlayerApplication.Model;
using System;
using System.Diagnostics;
using Windows.Media.Core;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MediaPlayerApplication.ViewModel
{
    public sealed partial class Video : Page
    {
        VideoHandler videoHandler = new VideoHandler();
        IRandomAccessStream stream;
        private FFmpegInteropMSS ffmpegMSS;
        public static Video Current;

        public Video()
        {
            this.InitializeComponent();
            Current = this;
            loadCortanaCommands();
        }

        //On navigation
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is string && !string.IsNullOrWhiteSpace((string)e.Parameter))
            {
                //Debug.WriteLine("Navigated to Video");
            }
            else
            {
                //Debug.WriteLine("Navigated to Video");
            }
            base.OnNavigatedTo(e);
        }//OnNavigatedTo

        //Page loaded event does not work, therefore loading cortana commands here
        private async void loadCortanaCommands()
        {
            var storageFile = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Commands/CortanaCommandsVideo.xml"));
            await Windows.ApplicationModel.VoiceCommands.VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(storageFile);
        }

        //Loads video and adds controls to commandbar
        private async void OpenMediaFile(object sender, RoutedEventArgs e)
        {
           //Get stream from VideoHandler
           stream = await  videoHandler.LoadMediaFile();

            try
            {
                //Try reading the stream using FFmpegInterop https://github.com/Microsoft/FFmpegInterop
                ffmpegMSS = FFmpegInteropMSS.CreateFFmpegInteropMSSFromStream(stream, true, true);
                MediaStreamSource mediaStreamSource = ffmpegMSS.GetMediaStreamSource();

                if (mediaStreamSource != null)
                {
                    //Add a bunch of command bar controls
                    mediaElement.AreTransportControlsEnabled = true;
                    mediaElement.TransportControls.IsFullWindowEnabled = true;
                    mediaElement.TransportControls.IsFullWindowButtonVisible = true;
                    mediaElement.TransportControls.IsStopEnabled = true;
                    mediaElement.TransportControls.IsStopButtonVisible = true;
                    mediaElement.TransportControls.IsVolumeEnabled = true;
                    mediaElement.TransportControls.IsVolumeButtonVisible = true;

                    mediaElement.SetMediaStreamSource(mediaStreamSource);
                    mediaElement.Play();
                }
                else
                {
                    var message = new MessageDialog("An error has occured while opening your selected video file.");
                    await message.ShowAsync();
                }
            }
            catch(Exception exception)
            {
                Debug.WriteLine(exception.Message);
                var message = new MessageDialog("An error has occured while opening your selected video file.");
                await message.ShowAsync();
            }
        }

        //Enable or disable full screen
        public void voiceFullWindow()
        {
            if (mediaElement.IsFullWindow == true)
            {
                mediaElement.IsFullWindow = false;
            }
            else
            {
                mediaElement.IsFullWindow = true;
            }
        }

        //Play/resume video
        public void voicePlayVideo()
        {
            mediaElement.Play();
        }

        //Stop playing video
        public void voiceStopVideo()
        {
            mediaElement.Stop();
        }

        //Pause video
        public void voicePauseVideo()
        {
            mediaElement.Pause();
        }

        //Exit application
        public void voiceExit()
        {
            Windows.UI.Xaml.Application.Current.Exit();
        }

        //Increase volume by 1
        public void voiceIncreaseVol()
        {
            mediaElement.Volume += .1;
        }

        //Decrease volume by 1
        public void voiceDecreaseVol()
        {
            mediaElement.Volume -= .1;
        }

        //Increase volume by half
        public void voiceHalfIncVol()
        {
            mediaElement.Volume = mediaElement.Volume * 2;
        }

        //Decrease volume by half
        public void voiceHalfDecVol()
        {
            mediaElement.Volume = mediaElement.Volume / 2;
        }
    }
}