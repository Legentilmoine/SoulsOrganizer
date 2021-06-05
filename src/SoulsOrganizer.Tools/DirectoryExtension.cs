namespace SoulsOrganizer.Tools
{
    public class DirectoryExtension
    {
        public static void Copy(string sourcePath, string destinationPath)
        {
            if (!System.IO.Directory.Exists(destinationPath))
                System.IO.Directory.CreateDirectory(destinationPath);

            foreach (string directory in System.IO.Directory.GetDirectories(sourcePath, "*", System.IO.SearchOption.AllDirectories))
                System.IO.Directory.CreateDirectory(directory.Replace(sourcePath, destinationPath));

            foreach (string file in System.IO.Directory.GetFiles(sourcePath, "*.*", System.IO.SearchOption.AllDirectories))
                System.IO.File.Copy(file, file.Replace(sourcePath, destinationPath));
        }
    }
   
}
