using MediaPlayerApplication.Data;
using MediaPlayerApplication.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Media.SpeechSynthesis;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MediaPlayerApplication.ViewModel
{
    public sealed partial class Reader : Page
    {
        ReaderHandler reader = new ReaderHandler();
        private ObservableCollection<Book> books;
        ReaderHandler readerHandler = new ReaderHandler();
        public static Reader Current;

        public Reader()
        {
            books = new ObservableCollection<Book>();
            this.InitializeComponent();
            Current = this;
            loadCortanaCommands();
        }

        //On navigation load books
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is string && !string.IsNullOrWhiteSpace((string)e.Parameter))
            {
                //Debug.WriteLine("Navigated to Reader");
                books.Clear();
                foreach (var book in await readerHandler.loadBooks())
                {
                    books.Add(book);
                }
            }
            else
            {
                //Debug.WriteLine("Navigated to Reader");
                books.Clear();
                foreach (var book in await readerHandler.loadBooks())
                {
                    books.Add(book);
                    //Debug.WriteLine(book.name);
                }
            }
            base.OnNavigatedTo(e);
        }//OnNavigatedTo

        //Page loaded event does not work, therefore loading cortana commands here
        private async void loadCortanaCommands()
        {
            var storageFile = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Commands/CortanaCommandsReader.xml"));
            await Windows.ApplicationModel.VoiceCommands.VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(storageFile);

            await updateBookList();
        }

        //Dynamically updating phrase list for books using 'books' list
        private async Task updateBookList()
        {
            try
            {
                VoiceCommandDefinition commandDefinitions;
                List<String> bookNames = new List<String>();

                if (VoiceCommandDefinitionManager.InstalledCommandDefinitions.TryGetValue("MediaPlayerCommandSet_en-GB", out commandDefinitions))
                {
                    foreach (Book book in books)
                    {
                        bookNames.Add(book.name);
                    }

                    await commandDefinitions.SetPhraseListAsync("book", bookNames);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        //Loads files from a folder on button click
        private async void OpenBookFolder(object sender, RoutedEventArgs e)
        {
            readerHandler.AddBookFolder();
            //clear the list and load new books
            books.Clear();
            books = await readerHandler.loadBooks();
            var message = new MessageDialog("To view your new book list, please reload the page.");
            await message.ShowAsync();
        }

        //Read the text using speech synthesiser
        private async void playBook(Book book)
        {
            //Load book from Assets/Books folder into a string
            var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            StorageFolder assets = await folder.GetFolderAsync(@"Assets\Books");
            var file = await assets.GetFileAsync(book.name + book.fileType);
            string text = await Windows.Storage.FileIO.ReadTextAsync(file);

            //The object for controlling the speech synthesis engine (voice)
            var synth = new Windows.Media.SpeechSynthesis.SpeechSynthesizer();

            //Generate the audio stream from the passed in book object
            SpeechSynthesisStream speechStream = await synth.SynthesizeTextToStreamAsync(text);

            //Send the stream to the media element
            mediaElement.SetSource(speechStream, speechStream.ContentType);
            configureCommandBar();
            BookText.Visibility = Visibility.Visible;
            BookText.Text = text;
            BookTextScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            BookTextScrollViewer.Visibility = Visibility.Visible;
            //Clear the collection to hide the list item in order to make the textblock visible
            books.Clear();
            mediaElement.Play();
        }

        //Add a bunch of command bar controls
        private void configureCommandBar()
        {
            mediaElement.AreTransportControlsEnabled = true;
            mediaElement.TransportControls.IsFullWindowEnabled = true;
            mediaElement.TransportControls.IsFullWindowButtonVisible = true;
            mediaElement.TransportControls.IsStopEnabled = true;
            mediaElement.TransportControls.IsStopButtonVisible = true;
            mediaElement.TransportControls.IsVolumeEnabled = true;
            mediaElement.TransportControls.IsVolumeButtonVisible = true;
        }

        //On ListView item click, read the text clicked
        private void Books_BookClick(object sender, ItemClickEventArgs e)
        {
            var book = (Book)e.ClickedItem;
            playBook(book);
        }

        //Play/resume reading
        public void voiceRead()
        {
            mediaElement.Play();
        }

        //Stop reading
        public void voiceStopReading()
        {
            mediaElement.Stop();
        }

        //Pause reading
        public void voicePauseReading()
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

        //Read a specific book from the list
        public void voiceReadBook(string bookName)
        {
            //Loop over every book in books and if the spoken book name matches a book name, read that book
            foreach (Book book in books)
            {
                if (book.name == bookName)
                {
                    playBook(book);
                }
            }
        }
    }
}