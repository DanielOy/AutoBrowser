namespace SharedLibrary
{
    public static class TextFormat
    {
        /// <summary>
        /// Return the size representation in base a the number of bytes
        /// (Ej. 2048 returns 2KB)
        /// </summary>
        /// <param name="bytesNumber">Number of bytes</param>
        /// <returns></returns>
        public static string NoBytesToSize(long bytesNumber)
        {
            bytesNumber = (bytesNumber / 1024);
            if (bytesNumber < 1024.0)
            {
                return $"{bytesNumber:0.##}KB";
            }

            bytesNumber = (bytesNumber / 1024);
            if (bytesNumber < 1024.0)
            {
                return $"{bytesNumber:0.##}MB";
            }

            bytesNumber = (bytesNumber / 1024);
            if (bytesNumber < 1024.0)
            {
                return $"{bytesNumber:0.##}GB";
            }
            return $"{bytesNumber:0.##}Bytes";
        }

        public static string URLEnconde(string text)
        {
            return System.Web.HttpUtility.UrlEncode(text);
        }
    }
}
