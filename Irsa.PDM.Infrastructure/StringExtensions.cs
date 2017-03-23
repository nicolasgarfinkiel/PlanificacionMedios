using System;

namespace Irsa.PDM.Infrastructure
{
    public static class StringExtensions
    {
        public static string CapitalizeFirstLetter(this string s)
        {
            if (String.IsNullOrEmpty(s)) return s;
            if (s.Length == 1) return s.ToUpper();

            return s.Remove(1).ToUpper() + s.Substring(1);
        }

        public static string ReplicatePadLeft(this string s, char character, int count)
        {
            return Replicate(s, character, count);
        }

        public static string ReplicatePadLeft(this int s, char character, int count)
        {
            return Replicate(s.ToString(), character, count);
        }

        public static string ReplicatePadLeft(this long s, char character, int count)
        {
            return Replicate(s.ToString(), character, count);
        }

        public static string ReplicatePadLeft(this decimal s, char character, int count)
        {
            return Replicate(s.ToString().Replace(".", ","), character, count);
        }

        public static string ReplicatePadLeft(this DateTime s, char character, int count)
        {
            return Replicate(s.ToString("ddMMyyyy"), character, count);
        }

        public static string ReplicatePadLeft(this int? s, char character, int count)
        {
            return Replicate(s.HasValue ? s.Value.ToString() : string.Empty, character, count);
        }

        public static string ReplicatePadLeft(this long? s, char character, int count)
        {
            return Replicate(s.HasValue ? s.Value.ToString() : string.Empty, character, count);
        }

        public static string ReplicatePadLeft(this decimal? s, char character, int count)
        {
            return Replicate(s.HasValue ? s.Value.ToString().Replace(".", ",") : string.Empty, character, count);
        }

        public static string ReplicatePadLeft(this DateTime? s, char character, int count)
        {
            return Replicate(s.HasValue ? s.Value.ToString("ddMMyyyy") : string.Empty, character, count);
        }

        public static string Replicate(string value, char character, int count)
        {
            var result = value ?? string.Empty;
            return result.PadLeft(count, character);
        }
    }
}
