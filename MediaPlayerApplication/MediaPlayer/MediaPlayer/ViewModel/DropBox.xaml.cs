using Dropbox.Api;
using MediaPlayerApplication.Data;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MediaPlayerApplication.ViewModel
{
    public sealed partial class DropBox : Page
    {
        DropboxClient dbx = new DropboxClient("6k8hio5GZwAAAAAAAAAAC0xyk9D1HxZ5w1whGYbOeStekqlHNmGFbOtZxoPSTu_A");
        private ObservableCollection<DropboxFile> files;
        private DropboxManager dbm;

        public DropBox()
        {
            this.InitializeComponent();
            files = new ObservableCollection<DropboxFile>();
            dbm = new DropboxManager();
        }

        //On navigation load dropbox files
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is string && !string.IsNullOrWhiteSpace((string)e.Parameter))
            {
                //Debug.WriteLine("Navigated to Dropbox");
                files.Clear();
                foreach (var item in await dbm.loadFiles())
                {
                    files.Add(item);
                }
                files = await dbm.loadFiles();
            }
            else
            {
                //Debug.WriteLine("Navigated to Dropbox");
                files.Clear();
                foreach (var item in await dbm.loadFiles())
                {
                    files.Add(item);
                }
            }
            base.OnNavigatedTo(e);
        }//OnNavigatedTo

        //On Listview item click, download the file to Assets/Books
        private async void DropboxItem_Click(object sender, ItemClickEventArgs e)
        {
            var file = (DropboxFile)e.ClickedItem;
            await dbm.Download(file.folder, file.name);
        }
    }
}