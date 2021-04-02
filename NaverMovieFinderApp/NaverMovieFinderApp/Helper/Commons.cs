using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using NLog;
using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace NaverMovieFinderApp
{
    class Commons
    {
        // NLog 정적 인스턴스 생성
        public static readonly Logger LOGGER = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Metro MessageBox 공통메서드
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        public static async Task<MessageDialogResult> ShowMessageAsync(
            string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative)
        {
            return await ((MetroWindow)Application.Current.MainWindow)
                .ShowMessageAsync(title, message, style, null);
        }

        /// <summary>
        /// OpenAPI Request & Response
        /// </summary>
        /// <param name="openApiUrl"></param>
        /// <param name="clientID"></param>
        /// <param name="clientSecret"></param>
        /// <returns></returns>
        public static string GetOpenApiResult(string openApiUrl, string clientID, string clientSecret)
        {
            var result = "";

            try
            {
                WebRequest request = WebRequest.Create(openApiUrl);
                request.Headers.Add("X-Naver-Client-Id", clientID);
                request.Headers.Add("X-Naver-Client-Secret", clientSecret);

                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);

                result = reader.ReadToEnd();

                reader.Close();
                stream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"예외발생: {ex}");
            }
            return result;
        }

        /// <summary>
        /// HTML 태그 삭제하는 정규표현식
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string StripHTMLTag(string text)
        {
            return Regex.Replace(text, @"<(.|\n)*?>", "");
        }

        /// <summary>
        /// |를 ,로 바꾸고 마지막 , 삭제
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string StripPipe(string text)
        {
            /* text = text.Replace("|", ",");

               if (text.EndsWith(","))
               {
                   text = text.Substring(0, text.Length - 1);
               }
               return text; */

            if (string.IsNullOrEmpty(text)) return "";  // 배우나 감독이 없는 경우
            
                return text.Substring(0, text.LastIndexOf("|")).Replace("|", ", ");

        }
    }
}
