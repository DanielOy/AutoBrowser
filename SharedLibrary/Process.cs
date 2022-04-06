namespace SharedLibrary
{
    public class Process
    {
        public static void OpenFolder(string folder)
        {
            string fullPath = new System.IO.DirectoryInfo(folder).FullName;

            if (!System.IO.Directory.Exists(fullPath))
            {
                throw new System.Exception($"The folder {folder} doesn't exists");
            }

            var info = new System.Diagnostics.ProcessStartInfo("explorer.exe", fullPath);

            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = @"cmd.exe";
            p.StartInfo.Arguments = $"/C \"explorer.exe {fullPath}";
            p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            p.Start();
        }
    }
}
