using Console = EasySave.view.Console;

namespace EasySave;

internal class Program
{
    private static void Main()
    {
        if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasySave"))
        {
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                      "\\EasySave");
        }
        
        var view = new Console();
    }
}