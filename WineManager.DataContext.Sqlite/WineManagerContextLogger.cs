using static System.Environment;

namespace WineManager.DataContext.Sqlite
{
    public class WineManagerContextLogger
    {
        public static void WriteLine(string message)
        {
            string path = Path.Combine(GetFolderPath(SpecialFolder.DesktopDirectory), "winemanagerlog.txt");

            StreamWriter textFile = File.AppendText(path);
            textFile.WriteLine(message);
            textFile.Close();
        }
    }
}
