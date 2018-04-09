using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

namespace MediaPlayerApplication.Model
{
    public class VideoHandler
    {
        //Select a video file to play using FileOpenPicker
        public async Task<IRandomAccessStream> LoadMediaFile()
        {
            //Create instance of FileOpenPicker
            var filePicker = new FileOpenPicker();
            filePicker.ViewMode = PickerViewMode.Thumbnail;
            filePicker.SuggestedStartLocation = PickerLocationId.VideosLibrary;

            //Filter which kind of files can be picked
            filePicker.FileTypeFilter.Add(".mp3");
            filePicker.FileTypeFilter.Add(".mp4");
            filePicker.FileTypeFilter.Add(".avi");
            filePicker.FileTypeFilter.Add(".flv");
            filePicker.FileTypeFilter.Add(".fla");
            filePicker.FileTypeFilter.Add(".mov");
            filePicker.FileTypeFilter.Add(".mkv");
            filePicker.FileTypeFilter.Add(".mpeg");
            filePicker.FileTypeFilter.Add(".mpeg2");
            filePicker.FileTypeFilter.Add(".mpg");
            filePicker.FileTypeFilter.Add(".rm");
            filePicker.FileTypeFilter.Add(".rmvb");
            filePicker.FileTypeFilter.Add(".vob");
            filePicker.FileTypeFilter.Add(".wmv");
            filePicker.FileTypeFilter.Add(".webm");

            //Read the file and return stream
            StorageFile file = await filePicker.PickSingleFileAsync();
            IRandomAccessStream readStream = await file.OpenAsync(FileAccessMode.Read);

            if (file != null)
            {
                return readStream;
            }
            else
            {
                return null;
            }
        }
    }
}