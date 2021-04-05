using MahApps.Metro.Controls;
using NaverMovieFinderApp.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace NaverMovieFinderApp
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 초기화를 위한 메소드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //Debug.WriteLine(Commons.IsFavorite);
        }
        private void TxtMovieName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) BtnSearch_Click(sender, e);
        }

        /// <summary>
        /// 영화 검색 버튼 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            StsResult.Content = "";
            ImgPoster.Source = (new BitmapImage(new Uri("NoPicture.jpg", UriKind.RelativeOrAbsolute)));

            if (string.IsNullOrEmpty(TxtMovieName.Text))
            {
                StsResult.Content = "검색할 영화명을 입력 후, 검색버튼을 눌러주세요.";
                Commons.ShowMessageAsync("검색", "검색할 영화명을 입력 후, 검색버튼을 눌러주세요.");
                return;
            }

            try
            {
                ProcSearchNaverApi(TxtMovieName.Text);
                Commons.ShowMessageAsync("검색", "영화 검색 완료");
            }
            catch (Exception ex)
            {
                Commons.ShowMessageAsync("예외", $"예외발생 : {ex}");
                Commons.LOGGER.Error($"예외발생 BtnSearch_Click : {ex}");
            }

            Commons.IsFavorite = false; // 즐겨찾기 아님
        }

        /// <summary>
        /// 영화선택 하면 이미지 표시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrdData_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (GrdData.SelectedItem is MovieItem)
            {
                var movie = GrdData.SelectedItem as MovieItem;

                if (string.IsNullOrEmpty(movie.Image))
                    ImgPoster.Source = (new BitmapImage(new Uri("NoPicture.jpg", UriKind.RelativeOrAbsolute)));
                else
                    ImgPoster.Source = (new BitmapImage(new Uri(movie.Image, UriKind.RelativeOrAbsolute)));
            }

            if (GrdData.SelectedItem is NaverFavoriteMovies)
            {
                var movie = GrdData.SelectedItem as NaverFavoriteMovies;

                if (string.IsNullOrEmpty(movie.Image))
                    ImgPoster.Source = (new BitmapImage(new Uri("NoPicture.jpg", UriKind.RelativeOrAbsolute)));
                else
                    ImgPoster.Source = (new BitmapImage(new Uri(movie.Image, UriKind.RelativeOrAbsolute)));
            }
        }

        /// <summary>
        /// 즐겨찾기 추가 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddWatchList_Click(object sender, RoutedEventArgs e)
        {
            if (GrdData.SelectedItems.Count == 0)
            {
                Commons.ShowMessageAsync("오류", "즐겨찾기에 추가할 영화를 선택하세요(복수선택 가능)");
                return;
            }

            if (Commons.IsFavorite) // 이미 즐겨찾기한 내용을 막아주기 위해
            {
                Commons.ShowMessageAsync("즐겨찾기", "즐겨찾기 조회내용을 다시 즐겨찾기할 수 없습니다.");
                return;
            }

            List<NaverFavoriteMovies> list = new List<NaverFavoriteMovies>();

            foreach (MovieItem item in GrdData.SelectedItems)
            {
                NaverFavoriteMovies temp = new NaverFavoriteMovies()
                {
                    Title = item.Title,
                    Link = item.Link,
                    Image = item.Image,
                    Subtitle = item.Subtitle,
                    PubDate = item.PubDate,
                    Director = item.Director,
                    Actor = item.Actor,
                    UserRating = item.UserRating,
                    RegDate = DateTime.Now
                };

                list.Add(temp);
            }

            try
            {
                using (var ctx = new OpenApiLabEntities())
                {
                    ctx.Set<NaverFavoriteMovies>().AddRange(list);
                    // ctx.NaverFavoriteMovies.AddRange(list); //위와 동일

                    ctx.SaveChanges();
                }

                Commons.ShowMessageAsync("저장", "즐겨찾기 추가 성공");
            }
            catch (Exception ex)
            {
                Commons.ShowMessageAsync("예외", $"예외발생 : {ex}");
                Commons.LOGGER.Error($"예외발생 BtnAddWatchList_Click : {ex}");
            }
        }

        /// <summary>
        /// 즐겨찾기 보기 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnViewWatchList_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext = null;
            TxtMovieName.Text = "";

            List<MovieItem> listData = new List<MovieItem>();
            List<NaverFavoriteMovies> list = new List<NaverFavoriteMovies>();

            try
            {
                using (var ctx = new OpenApiLabEntities())
                {
                    list = ctx.NaverFavoriteMovies.ToList();
                }
                this.DataContext = list;

                StsResult.Content = $"즐겨찾기 {list.Count}개 조회";

                if (Commons.IsDelete)   // 즐겨찾기 삭제 일때
                    Commons.ShowMessageAsync("즐겨찾기", "즐겨찾기 삭제 완료");
                else
                    Commons.ShowMessageAsync("즐겨찾기", "즐겨찾기 보기 조회 완료");

                Commons.IsFavorite = true; // 즐겨찾기 맞음
            }
            catch (Exception ex)
            {
                Commons.ShowMessageAsync("예외", $"예외발생 : {ex}");
                Commons.LOGGER.Error($"예외발생 BtnViewWatchList_Click : {ex}");

                Commons.IsFavorite = false; // 즐겨찾기 아님
            }

            Commons.IsDelete = false;   // 즐겨찾기 삭제 상태 아님
        }

        /// <summary>
        /// 즐겨찾기 삭제버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDeleteWatchList_Click(object sender, RoutedEventArgs e)
        {
            if (Commons.IsFavorite == false)
            {
                Commons.ShowMessageAsync("즐겨찾기", "즐겨찾기 내용이 아니면 삭제할 수 없습니다.");
                return;
            }

            if (GrdData.SelectedItems.Count == 0)
            {
                Commons.ShowMessageAsync("즐겨찾기", "삭제할 즐겨찾기 영화를 선택하세요.");
                return;
            }

            foreach (NaverFavoriteMovies item in GrdData.SelectedItems)
            {
                using (var ctx = new OpenApiLabEntities())
                {
                    /*  // 삭제 된당 - attach가 뭘까
                    ctx.NaverFavoriteMovies.Attach(item);
                    ctx.NaverFavoriteMovies.Remove(item);
                    ctx.SaveChanges();*/

                    var itemDelete = ctx.NaverFavoriteMovies.Find(item.Idx);    // 삭제할 영화 객체 검색 후 생성
                    ctx.Entry(itemDelete).State = EntityState.Deleted;          // 검색객체 상태를 삭제로 변경
                    ctx.SaveChanges();  // commit
                }
            }
            // 조회 쿼리 다시 실행
            Commons.IsDelete = true;
            BtnViewWatchList_Click(sender, e);
        }

        /// <summary>
        /// 유투브에서 예고편 가져오기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnWatchTrailer_Click(object sender, RoutedEventArgs e)
        {
            if (GrdData.SelectedItems.Count == 0)
            {
                Commons.ShowMessageAsync("유투브영화", "영화를 선택하세요.");
                return;
            }

            if (GrdData.SelectedItems.Count > 1)
            {
                Commons.ShowMessageAsync("유투브영화", "영화를 하나만 선택하세요.");
                return;
            }

            string movieName = "";

            if (Commons.IsFavorite) // 즐겨찾기
            {
                var item = GrdData.SelectedItem as NaverFavoriteMovies;
                //MessageBox.Show(item.Link);
                movieName = item.Title;
            }
            else // Naver API
            {
                var item = GrdData.SelectedItem as MovieItem;
                //MessageBox.Show(item.Link);
                movieName = item.Title;
            }

            var trailerWindow = new TrailerWindow(movieName);
            trailerWindow.Owner = this;
            trailerWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            trailerWindow.ShowDialog();
        }

        /// <summary>
        /// 네이버에서 영화정보 브라우저에서 띄우기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNaverMovieLink_Click(object sender, RoutedEventArgs e)
        {
            if (GrdData.SelectedItems.Count == 0)
            {
                Commons.ShowMessageAsync("네이버영화", "영화를 선택하세요.");
                return;
            }

            if (GrdData.SelectedItems.Count > 1)
            {
                Commons.ShowMessageAsync("네이버영화", "영화를 하나만 선택하세요.");
                return;
            }

            // 선택된 영화 네이버영화 URL 가져오기   
            string linkUrl = "";
            
            if (Commons.IsFavorite) // 즐겨찾기
            {
                var item = GrdData.SelectedItem as NaverFavoriteMovies;
                //MessageBox.Show(item.Link);
                linkUrl = item.Link;
            }
            else // Naver API
            {
                var item = GrdData.SelectedItem as MovieItem;
                //MessageBox.Show(item.Link);
                linkUrl = item.Link;
            }

            Process.Start(linkUrl);

/*            if (GrdData.SelectedItem is MovieItem)
            {
                var movie = GrdData.SelectedItem as MovieItem;

                Process.Start(new ProcessStartInfo(movie.Link));
                e.Handled = true;
            }
            if (GrdData.SelectedItem is NaverFavoriteMovies)
            {
                var movie = GrdData.SelectedItem as NaverFavoriteMovies;

                Process.Start(new ProcessStartInfo(movie.Link));
                e.Handled = true;
            }*/
        }

        /// <summary>
        /// Naver API Open
        /// </summary>
        /// <param name="movieName"></param>
        private void ProcSearchNaverApi(string movieName)
        {
            string clientID = "j2bYdxDrbReYpqnfGCTm";
            string clientSecret = "3LOYLcsGSc";

            string openApiUrl = $"https://openapi.naver.com/v1/search/movie?start=1&display=30&query={movieName}";

            // request 요청
            string resJson = Commons.GetOpenApiResult(openApiUrl, clientID, clientSecret);
            var parsedJson = JObject.Parse(resJson);

            int total = Convert.ToInt32(parsedJson["total"]);
            int display = Convert.ToInt32(parsedJson["display"]);

            StsResult.Content = $"{total} 중 {display} 호출 성공";

            JToken items = parsedJson["items"];
            JArray json_array = (JArray)items;

            List<MovieItem> movieItems = new List<MovieItem>();

            foreach (var item in json_array)
            {
                MovieItem movie = new MovieItem(
                    Commons.StripHTMLTag(item["title"].ToString()),
                    item["link"].ToString(),
                    item["image"].ToString(),
                    item["subtitle"].ToString(),
                    item["pubDate"].ToString(),
                    Commons.StripPipe(item["director"].ToString()),
                    Commons.StripPipe(item["actor"].ToString()),
                    item["userRating"].ToString()
                    );
                movieItems.Add(movie);
            }

            this.DataContext = movieItems;
        }
    }
}
