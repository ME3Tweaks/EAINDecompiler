using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CliWrap;
using CliWrap.Buffered;

namespace EAINDecompiler
{
    class Program
    {
        public static void Main(string[] args)
        {
            app(args);
        }

        private static async void app(string[] args)
        {
            if (args.Length == 1 && File.Exists(args[0]))
            {
                MemoryStream ms = new MemoryStream(File.ReadAllBytes(args[0]));
                var magic = ms.ReadInt32();
                if (magic != 0x4E494145) //EAIN 
                {
                    Console.WriteLine(@"Not an EAIN file!");
                    Environment.Exit(1);
                }

                //var buildNum = ms.ReadByte(); // 
                //Console.WriteLine($"EAIN format version (?): {buildNum}");
                byte[] encryptedBuff = new byte[ms.Length - ms.Position];
                Console.WriteLine($"Encrypted data length: {encryptedBuff.Length} bytes");
                ms.Read(encryptedBuff, 0, encryptedBuff.Length);

                var decrypted = new byte[encryptedBuff.Length];
                for (int i = 0; i < encryptedBuff.Length; i++)
                {
                    decrypted[i] = (byte)(encryptedBuff[i] ^ 0xB4);
                }

                var temp = Path.GetTempFileName();
                File.WriteAllBytes(temp, decrypted);



                var result = await Cli.Wrap("LuaDec51.exe")
                    .WithValidation(CommandResultValidation.None)
                    .WithArguments(temp)
                    .ExecuteBufferedAsync();

                Console.WriteLine(result.StandardOutput);
                Console.WriteLine(result.StandardError);
                Console.ReadKey();
                File.Delete(temp);

            }
        }
    }

    static class Extensions
    {
        public static int ReadInt32(this Stream stream)
        {
            var buffer = new byte[sizeof(int)];
            if (stream.Read(buffer, 0, sizeof(int)) != sizeof(int))
                throw new Exception();
            return BitConverter.ToInt32(buffer, 0);
        }
    }
}
