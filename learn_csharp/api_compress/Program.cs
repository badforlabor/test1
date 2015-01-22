using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace api_compress
{
    class Program
    {
        static void Main(string[] args)
        {

            //string org = "1234567890";
            byte[] array_a = new byte[]{1, 2, 3, 4, 5, 6, 7};
            byte[] array_b = null;
            byte[] array_c = null;

            {
                array_c = Compress(array_a);
            }

            {
                array_b = Decompress(array_c);
            }

            if (array_a.Length != array_b.Length)
            {
                Console.WriteLine("xxxxxxxxxxxx 1.");
            }


        }

        public static byte[] Compress(byte[] data)
        {
            MemoryStream ms = new MemoryStream();
            GZipStream compress = new GZipStream(ms, CompressionMode.Compress, true);
            compress.Write(data, 0, data.Length);
            compress.Close();
            return ms.ToArray();
        }
        public static byte[] Decompress(byte[] data)
        {
            MemoryStream ms = new MemoryStream(data, false);
            GZipStream decompress = new GZipStream(ms, CompressionMode.Decompress);

            byte[] tmp = new byte[3];
            MemoryStream tmpbuf = new MemoryStream();
            int len = 0;
            do
            {
                len = decompress.Read(tmp, 0, tmp.Length);
                tmpbuf.Write(tmp, 0, len);
            }
            while (len == tmp.Length);
            decompress.Close();
            return tmpbuf.ToArray();      
        }
    }
}
