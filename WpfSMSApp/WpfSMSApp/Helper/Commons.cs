using NLog;
using System.Security.Cryptography;
using System.Text;
using WpfSMSApp.Model;

namespace WpfSMSApp
{
    public class Commons
    {
        // NLog 정적 인스턴스 생성
        public static readonly Logger LOGGER = LogManager.GetCurrentClassLogger();

        // 로그인한 유저 정보 인스턴스(객체)
        public static User LOGINED_USER;

        /// <summary>
        ///  비밀번호 암호화 메소드
        /// </summary>
        /// <param name="md5Hash"></param>
        /// <param name="plainStr"></param>
        /// <returns></returns>
        public static string GetMd5Hash(MD5 md5Hash, string plainStr)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(plainStr));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                builder.Append(data[i].ToString("x2")); // 16진수로 바꿈
            }

            return builder.ToString();
        }
    }
}
