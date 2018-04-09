using MediaPlayerApplication.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.Search;

namespace MediaPlayerApplication.Model
{
    public class MusicHandler
    {
        private SongManager songManager = new SongManager();
        private ObservableCollection<Song> songs = new ObservableCollection<Song>();
        MediaPlayer player = new MediaPlayer();
        private List<Song> playing = new List<Song>();
        Song currentlyPlaying;

        //Singleton
        private static MusicHandler instance = new MusicHandler();
        public static MusicHandler getInstance
        {
            get { return instance; }
        }
        private MusicHandler() {}

        //Load songs using songManager, called from Music.xaml.cs
        public async Task<ObservableCollection<Song>> loadSongs()
        {
            songs.Clear();
            foreach (var song in await songManager.loadSongs())
            {
                songs.Add(song);
                //Debug.WriteLine(song.name);
            }
            return songs;
        }

        //Retrieves the selected song from either MusicLibary or Assets/Music and plays it
        public async void playMedia(Song song)
        {
            //Load song from MusicLibrary
            if (song.folder == "MusicLibrary")
            {
                var folder = Windows.Storage.KnownFolders.MusicLibrary;
                var file = await folder.GetFileAsync(song.name + song.fileType);
                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                player.AutoPlay = false;
                player.Source = MediaSource.CreateFromStream(stream, file.ContentType);
                player.Play();
                playing.Add(song);
                currentlyPlaying = song;
            }
            else if (song.folder == "Assets/Music")
            {
                //Load song from Assets/Music folder
                var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                StorageFolder assets = await folder.GetFolderAsync(@"Assets\Music");
                var file = await assets.GetFileAsync(song.name + song.fileType);
                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                player.AutoPlay = false;
                player.Source = MediaSource.CreateFromStream(stream, file.ContentType);
                player.Play();
                playing.Add(song);
                currentlyPlaying = song;
            }
        }

        //Update current song text when the user clicks on back/next buttons
        public string ChangeSongText()
        {
            if (currentlyPlaying == null)
            {
                return "Currently Playing: Nothing";
            }
            else
            {
                return currentlyPlaying.name;
            }
        }

        //Plays the previous song by accessing List 'playing' which contains all the songs the user clicked on
        public void previousSong()
        {
            int currentSong = playing.IndexOf(currentlyPlaying);
            //Handles ArgumentOutOfRangeException if theres no previous song in the list
            try
            {
                Song previousSong = playing[currentSong - 1];
                playMedia(previousSong);
                currentlyPlaying = previousSong;
            }
            catch (System.ArgumentOutOfRangeException error)
            {
                Debug.WriteLine(error.Message);
            }
        }

        //Plays the next song by accessing List 'playing' which contains all the songs the user clicked on
        public void nextSong()
        {
            int currentSong = playing.IndexOf(currentlyPlaying);
            //Handles ArgumentOutOfRangeException if theres no next song in the list
            try
            {
                Song nextSong = playing[currentSong + 1];
                playMedia(nextSong);
                currentlyPlaying = nextSong;
            }
            catch (System.ArgumentOutOfRangeException error)
            {
                Debug.WriteLine(error.Message);
            }
        }

        //Allows user to select a folder and load all the music files in it, the files get copied to Assets/Music for future use
        public async void AddMediaFolder()
        {
            var folderPicker = new Windows.Storage.Pickers.FolderPicker();
            folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            Windows.Storage.StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                // Application now has read/write access to all contents in the picked folder
                // (including other sub-folder contents)
                Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", folder);
                Debug.WriteLine("Selected folder: " + folder.Name);

                //Filter types of files
                List<string> fileTypeFilter = new List<string>();
                fileTypeFilter.Add(".mp3");
                fileTypeFilter.Add(".mp3");
                fileTypeFilter.Add(".wma");
                fileTypeFilter.Add(".wav");
                fileTypeFilter.Add(".ogg");
                fileTypeFilter.Add(".flac");
                fileTypeFilter.Add(".aiff");
                fileTypeFilter.Add(".aac");

                //Get all the files from the folder
                QueryOptions queryOptions = new QueryOptions(CommonFileQuery.OrderByName, fileTypeFilter);
                StorageFileQueryResult results = folder.CreateFileQueryWithOptions(queryOptions);
                IReadOnlyList<StorageFile> sortedFiles = await results.GetFilesAsync();

                //Get a handle on Assets/Music
                StorageFolder assets = await Package.Current.InstalledLocation.GetFolderAsync(@"Assets\Music");

                //Loop over every file in sortedFiles and add to song collection as well as copy the files to Assets/Music for future use
                foreach (StorageFile file in sortedFiles)
                {
                    songs.Add(new Song { name = file.Name, dateCreated = file.DateCreated.ToString(), fileType = file.FileType, path = file.Path, folder = "Assets/Music" });
                    await file.CopyAsync(assets);
                }

                //Clear the list and reload the new list
                songs.Clear();
                await loadSongs();
            }
            else
            {
                Debug.WriteLine("Empty folder");
            }
        }

        //Voice command - pause music
        public void voicePauseSong()
        {
            player.Pause();
        }

        //Voice command - resume music
        public void voiceResumeMusic()
        {
            player.Play();
        }

        //Voice command - play next song
        public void voiceNextSong()
        {
            nextSong();
            
        }

        //Voice command - play previous song
        public void voicePreviousSong()
        {
            previousSong();
        }

        //Voice command - play specific song
        public void voicePlaySong(String songName)
        {
            //Loop over every song in songs and if the spoken song name matches a song name, play that song
            foreach (Song song in songs)
            {
                if (song.name == songName)
                {
                    playMedia(song);
                    currentlyPlaying = song;
                }
            }
        }

        static Random rnd = new Random();

        //Voice command - play a random song
        public void voicePlayRandom()
        {
            int rand = rnd.Next(songs.Count);
            Song randSong = songs[rand];
            playMedia(randSong);
            currentlyPlaying = randSong;
        }

        //Voice command - exit the application
        public void exit()
        {
            Windows.UI.Xaml.Application.Current.Exit();
        }
    }//MusicHandler
}//Model