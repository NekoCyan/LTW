namespace LTW.Utils
{
    public static class Utilities
    {
        public static bool IsValidUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return false;
            }

            try
            {
                new Uri(url);
                return true;
            }
            catch (UriFormatException)
            {
                return false;
            }
        }
    }
}
