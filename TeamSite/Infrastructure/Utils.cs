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

        public static string ByteArrToUtf8String(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        public static string ByteArrToWin1252String(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        public static byte[] Utf8ToWin1252(byte[] utf8Bytes)
        {
            Encoding wind1252 = Encoding.GetEncoding(1252);
            Encoding utf8 = Encoding.UTF8;
            byte[] win1252Bytes = Encoding.Convert(wind1252, utf8, utf8Bytes);

            return win1252Bytes;
        }

        public static string StringUtf8ToWin1252(string s)
        {
            byte[] utf8Bytes = Encoding.UTF8.GetBytes(s);
            byte[] win1252Bytes = Utils.Utf8ToWin1252(utf8Bytes);

            return Utils.ByteArrToWin1252String(win1252Bytes);
        }
    }
}
