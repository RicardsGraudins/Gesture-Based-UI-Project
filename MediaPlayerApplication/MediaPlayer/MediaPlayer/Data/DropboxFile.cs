using Dropbox.Api;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;

namespace MediaPlayerApplication.Data
{
    public class DropboxFile
    {
        public string name { get; set; }
        public string folder { get; set; }
    }

    public class DropboxManager
    {
        //Creating a new dropbox client connection using a key generated for this application
        DropboxClient dbx = new DropboxClient("6k8hio5GZwAAAAAAAAAAC0xyk9D1HxZ5w1whGYbOeStekqlHNmGFbOtZxoPSTu_A");

        //Loading files from dropbox into an ObservableCollection
        public async Task<ObservableCollection<DropboxFile>> loadFiles()
        {
            ObservableCollection<DropboxFile> files = new ObservableCollection<DropboxFile>();

            try
            {
                var list = await dbx.Files.ListFolderAsync(string.Empty);

                /*
                //Show folders then files
                foreach (var item in list.Entries.Where(i => i.IsFolder))
                {
                    Debug.WriteLine("D  {0}/", item.Name);
                }

                foreach (var item in list.Entries.Where(i => i.IsFile))
                {
                    Debug.WriteLine("F{0,8} {1}", item.AsFile.Size, item.Name);
                }
                */

                //Add files as DropboxFile
                foreach (var item in list.Entries.Where(i => i.IsFile))
                {
                    files.Add(new DropboxFile { name = item.Name, folder = "" });
                }

                return files;
            } catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                var message = new MessageDialog("Something went wrong, check your internet connection and try again.");
                await message.ShowAsync();
                return null;
            }
        }

        //Download file from dropbox
        public async Task Download(string folder, string file)
        {
            try
            {
                using (var response = await dbx.Files.DownloadAsync(folder + "/" + file))
                {
                    string text = (await response.GetContentAsStringAsync());
                    //Debug.WriteLine(text);

                    //Files in assets folder are read only therefore we can't directly create a file in that folder
                    //alternatively create the file in MusicLibrary and move it to Assets/Books
                    var musicFolder = Windows.Storage.KnownFolders.MusicLibrary;
                    Windows.Storage.StorageFile textFile = await musicFolder.CreateFileAsync(file, Windows.Storage.CreationCollisionOption.ReplaceExisting);
                    Windows.Storage.StorageFile getFile = await musicFolder.GetFileAsync(file);
                    await Windows.Storage.FileIO.WriteTextAsync(getFile, text);

                    //Move file to Assets/Books
                    getFile = await musicFolder.GetFileAsync(file);
                    var directory = Windows.ApplicationModel.Package.Current.InstalledLocation;
                    StorageFolder assets = await directory.GetFolderAsync(@"Assets\Books");
                    await getFile.MoveAsync(assets, file, NameCollisionOption.ReplaceExisting);
                    var message = new MessageDialog("Download Successful.");
                    await message.ShowAsync();
                }
            } catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                var message = new MessageDialog("Something went wrong, check your internet connection and try again.");
                await message.ShowAsync();
            }
        }
    }
}