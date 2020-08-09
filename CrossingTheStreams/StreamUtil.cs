using System.IO;

namespace CrossingTheStreams
{
    public static class StreamUtil
    {
        /// <summary>
        /// Reads a certain amount of bytes from the stream and returns them.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="amount">Amount of bytes to read.</param>
        /// <returns></returns>
        public static byte[] ReadExactly(this Stream stream, int amount)
        {
            var buffer = new byte[amount];
            stream.Read(buffer);
            return buffer;
        }
    }
}