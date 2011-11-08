using System.IO;

namespace Rackspace.CloudFiles.Utils
{
    public static class Writer
    {
        public static void WriteTo(this Stream source, Stream target)
        {
            WriteTo(source, target, 1024);
        }

        public static void WriteTo(this Stream source, Stream target, int bufferLength)
        {
            var buffer = new byte[bufferLength];
            int bytesRead;

            do
            {
                bytesRead = source.Read(buffer, 0, buffer.Length);
                target.Write(buffer, 0, bytesRead);
            } while (bytesRead > 0);
        }
    }
}