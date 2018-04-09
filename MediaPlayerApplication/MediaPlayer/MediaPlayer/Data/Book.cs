using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace MediaPlayerApplication.Data
{
    public class Book
    {
        public string name { get; set; }
        public string fileType { get; set; }
    }//Book

    public class BookManager
    {
        //Loads text from Assets/Books folder and returns an ObservableCollection<Book>
        public async Task<ObservableCollection<Book>> loadBooks()
        {
            ObservableCollection<Book> books = new ObservableCollection<Book>();

            //Setting path to Assets/Books
            StorageFolder appInstalledFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            StorageFolder assets = await appInstalledFolder.GetFolderAsync(@"Assets\Books");
            var assetFiles = await assets.GetFilesAsync();

            //Loop over every file in Assets/Books and add books to books collection
            foreach (Windows.Storage.StorageFile file in assetFiles)
            {
                books.Add(new Book { name = Path.GetFileNameWithoutExtension(file.Path), fileType = file.FileType });
            }

            return books;
        }//loadBooks
    }//BookManager
}//Data