using static System.Environment;

namespace WineManager.DataContext.Sqlite
{
    public class WineManagerContextLogger
    {
        /// <summary>
        /// Method for logging executed SQL commands
        /// Stored in a txt file on the desktop
        /// </summary>
        /// <param name="message">log message</param>
        public static void WriteLine(string message)
        {
            string path = Path.Combine(GetFolderPath(SpecialFolder.DesktopDirectory), "winemanagerlog.txt");

            StreamWriter textFile = File.AppendText(path);
            textFile.WriteLine(message);
            textFile.Close();
        }
    }
}
