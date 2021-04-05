using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using MahApps.Metro.Controls;
using NaverMovieFinderApp.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NaverMovieFinderApp
{
    /// <summary>
    /// TrailerWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TrailerWindow : MetroWindow
    {
        // 유투브 api 검색 결과
        List<YoutubeItem> youtubes;

        public TrailerWindow()
        {
            InitializeComponent();
        }

        public TrailerWindow(string movieName) : this()
        {
            LblMovieName.Content = $"{movieName} 예고편";
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            youtubes = new List<YoutubeItem>(); // 초기화

            //유투브 api로 검색
            //MessageBox.Show("유투브 검색");
            ProcSearchYoutubeApi();
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            BrwYoutubeWatch.Source = null;  // 해제
            BrwYoutubeWatch.Dispose();  // 리소스 즉시 해제
        }

        private async void ProcSearchYoutubeApi()
        {
            await LoadDataCollection();
            LsvYoutubeSearch.ItemsSource = youtubes;
        }

        private async Task LoadDataCollection()
        {
            var youtubeService = new YouTubeService(
                new BaseClientService.Initializer()
                {
                    ApiKey = "AIzaSyD5delLXpsmh7z0L58PGXXxpXLefpFb9kE",
                    ApplicationName = this.GetType().ToString()
                });

            var request = youtubeService.Search.List("snippet");    // snippet 조각
            request.Q = LblMovieName.Content.ToString();   // {영화이름} 예고편
            request.MaxResults = 10; // 사이즈가 크면 멈춰버림

            var response = await request.ExecuteAsync(); // 검색시작(youtube OpenAPI 호출)

            foreach (var item in response.Items)
            {
                if (item.Id.Kind.Equals("youtube#video"))
                {
                    YoutubeItem youtube = new YoutubeItem()
                    {
                        Title = item.Snippet.Title,
                        Author = item.Snippet.ChannelTitle,
                        URL = $"https://www.youtube.com/watch?v={item.Id.VideoId}"
                    };
                    // thumbnail image
                    youtube.Thumbnail = new BitmapImage(new Uri(item.Snippet.Thumbnails.Default__.Url, UriKind.RelativeOrAbsolute));
                   
                    youtubes.Add(youtube);
                }
            }
        }

        private void LsvYoutubeSearch_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LsvYoutubeSearch.SelectedItems.Count == 0)
            {
                Commons.ShowMessageAsync("유투브보기", "예고편을 볼 영화를 선택하세요.");
                return;
            }
            if (LsvYoutubeSearch.SelectedItems.Count > 1)
            {
                Commons.ShowMessageAsync("유투브보기", "예고편을 하나만 선택하세요.");
                return;
            }

            if (LsvYoutubeSearch.SelectedItem is YoutubeItem)
            {
                var video = LsvYoutubeSearch.SelectedItem as YoutubeItem;

                // 윈도우 브라우저에서 띄우기
                BrwYoutubeWatch.Source = new Uri(video.URL, UriKind.RelativeOrAbsolute);

                // 웹 브라우저에서 띄우기
                //Process.Start(video.URL);
            }
        }
    }
}
