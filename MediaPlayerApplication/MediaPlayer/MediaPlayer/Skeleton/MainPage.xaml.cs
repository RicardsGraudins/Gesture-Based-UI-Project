using MediaPlayerApplication.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MediaPlayerApplication
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            MyFrame.Navigate(typeof(Music));
            Music.IsSelected = true;
        }//MainPage

        //Open/close the splitview
        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }//Menu_Click

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            //Refresh data on the selected page
            if (Music.IsSelected)
            {
                MyFrame.Navigate(typeof(Music), "Music");
            }
            else if (Video.IsSelected)
            {
                MyFrame.Navigate(typeof(Video), "Video");
            }
            else if (Reader.IsSelected)
            {
                MyFrame.Navigate(typeof(Reader), "Reader");
            }
            else if (Dropbox.IsSelected)
            {
                MyFrame.Navigate(typeof(DropBox), "Dropbox");
            }
        }//RefreshButton_Click

        //Navigate to pages via splitview
        private void Menu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Music.IsSelected)
            {
                MyFrame.Navigate(typeof(Music));
                TitleTextBlock.Text = "Music";
                if (MySplitView.IsPaneOpen == true)
                {
                    MySplitView.IsPaneOpen = false;
                }
            }
            else if (Video.IsSelected)
            {
                MyFrame.Navigate(typeof(Video));
                TitleTextBlock.Text = "Video";
                if (MySplitView.IsPaneOpen == true)
                {
                    MySplitView.IsPaneOpen = false;
                }
            }
            else if (Reader.IsSelected)
            {
                MyFrame.Navigate(typeof(Reader));
                TitleTextBlock.Text = "Reader";
                if (MySplitView.IsPaneOpen == true)
                {
                    MySplitView.IsPaneOpen = false;
                }
            }
            else if (Dropbox.IsSelected)
            {
                MyFrame.Navigate(typeof(DropBox));
                TitleTextBlock.Text = "Download Free Books";
                if (MySplitView.IsPaneOpen == true)
                {
                    MySplitView.IsPaneOpen = false;
                }
            }
        }//Menu_SelectionChanged
    }//MainPage
}//MediaPlayer