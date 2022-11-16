using System;
using System.Text;

namespace GB2312ToUTF8
{
    internal class Program
    {
        // Design drawback:
        // It doesn't isolate the configuration of read stream and white stream
        public static string DirNameOutput;
        public static void Main(string[] args)
        {
            //Console.WriteLine("Hello, World!");
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding encoding936 = Encoding.GetEncoding(936);
            // bin-Debug-net6.0
            DirNameOutput =@"utf8-"+DateTime.Now.ToString(@"yyMMddHHmmssff");
            string nowDirectory = Directory.GetCurrentDirectory();
            string pathOutputDirectory = Path.Combine(nowDirectory, DirNameOutput);
            if (!(Directory.Exists(pathOutputDirectory)))
            {
                Directory.CreateDirectory(pathOutputDirectory);
            }
            else { }
            //string processFilePath = args[0];
            string absolutePathIn = @"";
            //string withoutPrefixName = @"";
            for (int i = 0; i < args.Length; i++)
            {
                int prefixNameLength = 0;
                args[i] = args[i].Trim();
                var judgeList = args[i].Split(Path.DirectorySeparatorChar);
                if (judgeList.Length > 1)
                {
                    if (judgeList[judgeList.Length - 1].Length == 0)
                    {
                        prefixNameLength = args[i].Length - judgeList[judgeList.Length - 2].Length - 1;
                    }
                    else
                    {
                        prefixNameLength = args[i].Length - judgeList[judgeList.Length - 1].Length;
                    }
                }
                //Input without slash
                else { }

                if (Path.IsPathRooted(args[i]))
                {
                    absolutePathIn = args[i];
                }
                else
                {
                    absolutePathIn = Path.Combine(nowDirectory, args[i]);
                }
                /*
                var forPureName=absolutePathIn.Split(Path.DirectorySeparatorChar);
                if (forPureName[forPureName.Length - 1].Length == 0) {
                    pureName = forPureName[forPureName.Length - 2];
                }
                else
                {
                    pureName = forPureName[forPureName.Length - 1];
                }
                */
                //Each input args uses the absolute path.
                transferArg(absolutePathIn, prefixNameLength);
            }
            /*
            string outfile=@"utf8"+processFilePath;
            using (StreamReader reader = new StreamReader(processFilePath, Encoding.GetEncoding(936)))
            {
                using (StreamWriter writer = new StreamWriter(outfile, false, Encoding.UTF8))
                {
                    string readToEnd=reader.ReadToEnd();
                    writer.WriteLine(readToEnd);
                }
            }
            */
        }

        public static void transferArg(string absolutePathIn, int prefixLength)
        {
            var attr = File.GetAttributes(absolutePathIn);
            if (!attr.HasFlag(FileAttributes.Directory))
            {
                transferFile(absolutePathIn, prefixLength);
            }
            else
            {
                transferDirectory(absolutePathIn, prefixLength);
            }
        }
        public static void transferDirectory(string absoluteInput, int prefixLength)
        {
            //string checkDirectoryPath = absoluteInput.Replace(Directory.GetCurrentDirectory(), Path.Combine(Directory.GetCurrentDirectory(), @"utf8"));
            string checkDirectoryPath = absoluteInput.Replace(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar, @"");
            checkDirectoryPath = checkDirectoryPath.Remove(0, prefixLength);
            checkDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), DirNameOutput, checkDirectoryPath);
            if (!(Directory.Exists(checkDirectoryPath)))
            {
                Directory.CreateDirectory(checkDirectoryPath);
            }
            else { }
            var filesInDire = Directory.GetFiles(absoluteInput);
            foreach (var file in filesInDire)
            {
                transferFile(file, prefixLength);
            }
            var subDire = Directory.GetDirectories(absoluteInput);
            foreach (var dire in subDire)
            {
                transferDirectory(dire, prefixLength);
            }
        }
        public static void transferFile(string filePath, int prefixLength)
        {
            //string outFilePath = filePath.Replace(Directory.GetCurrentDirectory(), Path.Combine(Directory.GetCurrentDirectory(), @"utf8"));
            string outFilePath = filePath.Replace(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar, @"");
            outFilePath = outFilePath.Remove(0, prefixLength);
            outFilePath = Path.Combine(Directory.GetCurrentDirectory(), DirNameOutput, outFilePath);
            using (StreamReader reader = new StreamReader(filePath, Encoding.GetEncoding(936)))
            {
                using (StreamWriter writer = new StreamWriter(outFilePath, false, Encoding.UTF8))
                {
                    string readToEnd = reader.ReadToEnd();
                    writer.WriteLine(readToEnd);
                }
            }
        }
    }
}