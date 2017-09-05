using System.Text;

namespace TeamSite.Infrastructure
{
    public class Utils
    {
        public static byte[] Win1252ToUtf8(byte[] wind1252Bytes)
        {
            Encoding wind1252 = Encoding.GetEncoding(1252);
            Encoding utf8 = Encoding.UTF8;
            byte[] utf8Bytes = Encoding.Convert(wind1252, utf8, wind1252Bytes);
            return utf8Bytes;
        }

        public static string byteArrToString(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
