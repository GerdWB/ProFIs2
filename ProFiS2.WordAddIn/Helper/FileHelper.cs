namespace ProFiS2.WordAddIn.Helper
{
    using System;
    using System.IO;

    public static class FileHelper
    {
        public static byte[] ReadBinaryFile(string filename)
        {
            try
            {
                return File.ReadAllBytes(filename);
            }
            catch (IOException)
            {
                var tempFileCopy = TempFileCollection.CreateFilename();
                File.Copy(filename, tempFileCopy);
                return File.ReadAllBytes(tempFileCopy);
            }
        }


        public static byte[] ReadFullyFromStream(Stream stream, int initialLength = 65536)
        {
            // If we've been passed an unhelpful initial length, just
            // use 32K.
            if (initialLength < 1)
            {
                initialLength = 65536;
            }

            var buffer = new byte[initialLength];
            var read = 0;

            int chunk;
            while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
            {
                read += chunk;

                // If we've reached the end of our buffer, check to see if there's
                // any more information
                if (read != buffer.Length)
                {
                    continue;
                }

                var nextByte = stream.ReadByte();

                // End of stream? If so, we're done
                if (nextByte == -1)
                {
                    return buffer;
                }

                // Nope. Resize the buffer, put in the byte we've just
                // read, and continue
                var newBuffer = new byte[buffer.Length + initialLength];
                Array.Copy(buffer, newBuffer, buffer.Length);
                newBuffer[read] = (byte)nextByte;
                buffer = newBuffer;
                read++;
            }

            // Buffer is now too big. Shrink it.
            var ret = new byte[read];
            Array.Copy(buffer, ret, read);
            return ret;
        }

        public static void WriteByteArrayToFile(byte[] buff, string fileName)
        {
            using var fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
            var bw = new BinaryWriter(fs);
            bw.Write(buff);
            bw.Close();
        }
    }
}