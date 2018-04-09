using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace MediaPlayerApplication.Data
{
    public class Song
    {
        public string name { get; set; }
        public string dateCreated { get; set; }
        public string fileType { get; set; }
        public string path { get; set; }
        public string folder { get; set; }
    }//Song

    public class SongManager
    {
        //Loads music from MusicLibrary + Assets/Music folders and returns an ObservableCollection<Song>
        public async Task<ObservableCollection<Song>> loadSongs()
        {
            ObservableCollection<Song> songs = new ObservableCollection<Song>();
            //Setting path to MusicLibary
            var folder = Windows.Storage.KnownFolders.MusicLibrary;
            var query = folder.CreateFileQuery();
            var files = await query.GetFilesAsync();

            //Loop over every file in MusicLibrary and add songs to songs
            foreach (Windows.Storage.StorageFile file in files)
            {
                songs.Add(new Song { name = Path.GetFileNameWithoutExtension(file.Path), dateCreated = file.DateCreated.ToString(), fileType = file.FileType, path = file.Path, folder = "MusicLibrary" });
            }

            //Setting path to Assets/Music
            StorageFolder appInstalledFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            StorageFolder assets = await appInstalledFolder.GetFolderAsync(@"Assets\Music");
            var assetFiles = await assets.GetFilesAsync();

            //Loop over every file in Assets/Music and add songs to songs collection
            foreach (Windows.Storage.StorageFile file in assetFiles)
            {
                songs.Add(new Song { name = Path.GetFileNameWithoutExtension(file.Path), dateCreated = file.DateCreated.ToString(), fileType = file.FileType, path = file.Path, folder = "Assets/Music" });
            }

            return songs;
        }//loadSongs
    }//SongManager
}//Data