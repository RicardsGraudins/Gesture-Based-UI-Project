using MediaPlayerApplication.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Search;

namespace MediaPlayerApplication.Model
{
    public class ReaderHandler
    {
        private BookManager bookManager = new BookManager();
        private ObservableCollection<Book> books = new ObservableCollection<Book>();

        //Load songs using bookManager, called from Reader.xaml.cs
        public async Task<ObservableCollection<Book>> loadBooks()
        {
            books.Clear();
            foreach (var book in await bookManager.loadBooks())
            {
                books.Add(book);
            }
            return books;
        }

        //Allows user to select a folder and load all the text files in it, the files get copied to Assets/Books for future use
        public async void AddBookFolder()
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
                fileTypeFilter.Add(".txt");

                //Get all the files from the folder
                QueryOptions queryOptions = new QueryOptions(CommonFileQuery.OrderByName, fileTypeFilter);
                StorageFileQueryResult results = folder.CreateFileQueryWithOptions(queryOptions);
                IReadOnlyList<StorageFile> sortedFiles = await results.GetFilesAsync();

                //Get a handle on Assets/Books
                StorageFolder assets = await Package.Current.InstalledLocation.GetFolderAsync(@"Assets\Books");

                //Loop over every file in sortedFiles and add to book collection as well as copy the files to Assets/Books for future use
                foreach (StorageFile file in sortedFiles)
                {
                    books.Add(new Book { name = file.Name, fileType = file.FileType });
                    await file.CopyAsync(assets);
                }

                //Clear the list and reload the new list
                books.Clear();
                await loadBooks();
            }
            else
            {
                Debug.WriteLine("Empty folder");
            }
            
        }
    }
}