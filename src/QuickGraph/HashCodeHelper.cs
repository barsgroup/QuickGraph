namespace QuickGraph
{
    internal static class HashCodeHelper
    {
        private const int Fnv1Prime32 = 16777619;

        private const int Fnv1Basis32 = unchecked((int)2166136261);

        private const long Fnv1Prime64 = 1099511628211;

        private const long Fnv1Basis64 = unchecked((int)14695981039346656037);

        /// <summary>Combines two hashcodes in a strong way.</summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static int Combine(int x, int y)
        {
            return Fold(Fold(Fnv1Basis32, x), y);
        }

        /// <summary>Combines three hashcodes in a strong way.</summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static int Combine(int x, int y, int z)
        {
            return Fold(Fold(Fold(Fnv1Basis32, x), y), z);
        }

        public static int GetHashCode(long x)
        {
            return Combine((int)x, (int)((ulong)x >> 32));
        }

        private static int Fold(int hash, byte value)
        {
            return (hash * Fnv1Prime32) ^ value;
        }

        private static int Fold(int hash, int value)
        {
            return Fold(
                Fold(
                    Fold(
                        Fold(
                            hash,
                            (byte)value),
                        (byte)((uint)value >> 8)),
                    (byte)((uint)value >> 16)),
                (byte)((uint)value >> 24));
        }
    }
}