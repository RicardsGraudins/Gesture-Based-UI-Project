using MediaPlayerApplication.Model;
using MediaPlayerApplication.ViewModel;
using System;
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MediaPlayerApplication
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        MusicHandler musicHandler = MusicHandler.getInstance;
        #pragma warning disable CS0414
        bool _isInBackgroundMode = false;
        #pragma warning restore CS0414
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            this.EnteredBackground += App_EnteredBackground;
            this.LeavingBackground += App_LeavingBackground;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        //Entering background
        private void App_EnteredBackground(object sender, EnteredBackgroundEventArgs e)
        {
            _isInBackgroundMode = true;
        }

        //Leaving background
        private void App_LeavingBackground(object sender, LeavingBackgroundEventArgs e)
        {
            _isInBackgroundMode = false;
        }

        protected override void OnActivated(IActivatedEventArgs e)
        {
            // Check if the app was activated by voice command
            if (e.Kind != Windows.ApplicationModel.Activation.ActivationKind.VoiceCommand)
            {
                return;
            }

            //Get vocal input
            var commandArgs = e as Windows.ApplicationModel.Activation.VoiceCommandActivatedEventArgs;
            var speechRecognitionResult = commandArgs.Result;
            string voiceCommandName = speechRecognitionResult.RulePath[0];
            string textSpoken = speechRecognitionResult.Text;

            Debug.WriteLine(voiceCommandName);

            //Execute methods depending on command spoken
            switch (voiceCommandName)
            {
                case "playMusic":
                    musicHandler.voiceResumeMusic();
                    break;
                case "resumeMusic":
                    musicHandler.voiceResumeMusic();
                    break;
                case "pauseSong":
                    musicHandler.voicePauseSong();
                    break;
                case "pauseMusic":
                    musicHandler.voicePauseSong();
                    break;
                case "nextSong":
                    musicHandler.voiceNextSong();
                    Music.Current.nowPlayingSwitch();
                    break;
                case "goBack":
                    musicHandler.voicePreviousSong();
                    Music.Current.nowPlayingSwitch();
                    break;
                case "previousSong":
                    musicHandler.voicePreviousSong();
                    Music.Current.nowPlayingSwitch();
                    break;
                case "playSpecificSong":
                    string songName = speechRecognitionResult.SemanticInterpretation.Properties["song"][0];
                    Debug.WriteLine("Song spoken: " + songName);
                    musicHandler.voicePlaySong(songName);
                    Music.Current.nowPlayingSwitch();
                    break;
                case "playRandomSong":
                    musicHandler.voicePlayRandom();
                    Music.Current.nowPlayingSwitch();
                    break;
                case "skipSong":
                    musicHandler.voicePlayRandom();
                    Music.Current.nowPlayingSwitch();
                    break;
                case "exitApp":
                    musicHandler.exit();
                    break;
                case "closeApp":
                    musicHandler.exit();
                    break;
                case "quitApp":
                    musicHandler.exit();
                    break;
                case "resumeVideo":
                    Video.Current.voicePlayVideo();
                    break;
                case "playVideo":
                    Video.Current.voicePlayVideo();
                    break;
                case "pauseVideo":
                    Video.Current.voicePauseVideo();
                    break;
                case "stopVideo":
                    Video.Current.voiceStopVideo();
                    break;
                case "fullScreen":
                    Video.Current.voiceFullWindow();
                    break;
                case "incVol":
                    Video.Current.voiceIncreaseVol();
                    break;
                case "decVol":
                    Video.Current.voiceDecreaseVol();
                    break;
                case "incHalfVol":
                    Video.Current.voiceHalfIncVol();
                    break;
                case "decHalfVol":
                    Video.Current.voiceHalfDecVol();
                    break;
                case "exitVideo":
                    Video.Current.voiceExit();
                    break;
                case "resumeReading":
                    Reader.Current.voiceRead();
                    break;
                case "continueReading":
                    Reader.Current.voiceRead();
                    break;
                case "pauseReading":
                    Reader.Current.voicePauseReading();
                    break;
                case "stopReading":
                    Reader.Current.voiceStopReading();
                    break;
                case "incVolume":
                    Reader.Current.voiceIncreaseVol();
                    break;
                case "decVolume":
                    Reader.Current.voiceDecreaseVol();
                    break;
                case "incHalfVolume":
                    Reader.Current.voiceHalfIncVol();
                    break;
                case "decHalfVolume":
                    Reader.Current.voiceHalfDecVol();
                    break;
                case "exitReader":
                    Reader.Current.voiceExit();
                    break;
                case "stopReader":
                    Reader.Current.voiceExit();
                    break;
                case "readSpecificBook":
                    string bookName = speechRecognitionResult.SemanticInterpretation.Properties["book"][0];
                    Debug.WriteLine("Book spoken: " + bookName);
                    Reader.Current.voiceReadBook(bookName);
                    break;
                default:
                    //Command not found
                    break;
            }
        }
    }
}
