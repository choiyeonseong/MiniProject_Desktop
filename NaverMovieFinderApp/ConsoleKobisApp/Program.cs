﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleKobisApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string key = "46d27592727103f11f0f6ea44dd1ddae";
            string movieNm = "비밀의 정원";
            string movieCd = "";

            // 영화 진흥 위원회 API
            // 포스터 없음ㅠㅠ
            string openApiUrl = $@"http://www.kobis.or.kr/kobisopenapi/webservice/rest/movie/searchMovieList.json?key={key}&movieNm={movieNm}";

            WebRequest request = WebRequest.Create(openApiUrl);
            request.Method = "GET";
            request.ContentType = "application/json";

            using (WebResponse response = request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                string data = reader.ReadToEnd();

                var obj = JObject.Parse(data);
                
                Console.WriteLine("============ List ============");
                Console.WriteLine(obj);

                Console.WriteLine("============ Item ============");
                var list = obj["movieListResult"];
                
                foreach (var item in list["movieList"])
                {
                    Console.WriteLine(item["movieNm"]);
                    Console.WriteLine(item["movieCd"]);
                    Console.WriteLine(item["imgsrc"]);
                    movieCd = item["movieCd"].ToString();
                }
            }
        }
    }
}
