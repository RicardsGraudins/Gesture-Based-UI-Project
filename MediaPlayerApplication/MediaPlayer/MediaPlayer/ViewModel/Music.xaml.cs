using MediaPlayerApplication.Data;
using MediaPlayerApplication.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.VoiceCommands;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace MediaPlayerApplication.ViewModel
{
    public sealed partial class Music : Page
    {
        private ObservableCollection<Song> songs;
        MusicHandler musicHandler = MusicHandler.getInstance;
        public Song currentlyPlaying;
        public static Music Current;

        public Music()
        {
            this.InitializeComponent();
            songs = new ObservableCollection<Song>();
            loadCortanaCommands();
            Current = this;
        }

        //Page loaded event does not work, therefore loading cortana commands here
        private async void loadCortanaCommands()
        {
            var storageFile = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Commands/CortanaCommandsMusic.xml"));
            await Windows.ApplicationModel.VoiceCommands.VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(storageFile);

            await updateSongList();
        }

        //Dynamically updating phrase list for songs using 'songs' list which contains a list of all songs loaded from MusicLibary, Assets/Music and
        //any other folder accessed by the user via AddMediaFolder @ MusicHandler
        private async Task updateSongList()
        {
            try
            {
                VoiceCommandDefinition commandDefinitions;
                List<String> songNames = new List<String>();

                if (VoiceCommandDefinitionManager.InstalledCommandDefinitions.TryGetValue("MediaPlayerCommandSet_en-GB", out commandDefinitions))
                {
                    foreach (Song song in songs)
                    {
                        songNames.Add(song.name);
                    }

                    await commandDefinitions.SetPhraseListAsync("song", songNames);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        //On navigation load all the songs and display them out to the user using an observable collection
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is string && !string.IsNullOrWhiteSpace((string)e.Parameter))
            {
                //Debug.WriteLine("Navigated to Music");
                songs.Clear();
                foreach (var song in await musicHandler.loadSongs())
                {
                    songs.Add(song);
                }
            }
            else
            {
                //Debug.WriteLine("Navigated to Music");
                songs.Clear();
                foreach (var song in await musicHandler.loadSongs())
                {
                    songs.Add(song);
                }
            }
            base.OnNavigatedTo(e);
        }//OnNavigatedTo

        //On ListView item click, play the song clicked
        private void Songs_SongClick(object sender, ItemClickEventArgs e)
        {
            var song = (Song)e.ClickedItem;
            currentlyPlaying = song;
            musicHandler.playMedia(song);
            nowPlaying();
        }

        //Resumes playing current song on button click
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            musicHandler.voiceResumeMusic();
        }

        //Pauses current song on button click
        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            musicHandler.voicePauseSong();
        }

        //Plays previous song on button click
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            musicHandler.previousSong();
            nowPlayingSwitch();
        }

        //Plays next song on button click
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            musicHandler.voiceNextSong();
            nowPlayingSwitch();
        }
        
        //Loads files from a folder on button click
        private async void AddMedia_Click(object sender, RoutedEventArgs e)
        {
            musicHandler.AddMediaFolder();
            //clear the list and load new songs
            songs.Clear();
            songs = await musicHandler.loadSongs();
            var message = new MessageDialog("To view your new song list, please reload the page.");
            await message.ShowAsync();
        }

        //Switches NowPlaying.text to the song name currently playing
        public void nowPlaying()
        {
            NowPlaying.Text = currentlyPlaying.name;
        }

        //Switches NowPlaying.text to the song name currently playing, triggered on NextButton_Click & BackButton_Click
        public void nowPlayingSwitch()
        {
            NowPlaying.Text = musicHandler.ChangeSongText();
        }
    }
}