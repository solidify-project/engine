using System.IO;

namespace SolidifyProject.Engine.Utils.FileSystem
{
    public static class FileSystemUtils
    {
        public static void CleanFolder(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            if (di.Exists)
            {
                foreach (var file in di.GetFiles())
                {
                    file.Delete();
                }

                foreach (var directory in di.GetDirectories())
                {
                    directory.Delete(true);
                }
            }
        }
    }
}