using System.Text.RegularExpressions;
using EasySave.model;
using EasySave.properties;
using EasySave.translation;
using static System.Console;

namespace EasySave.view;

internal class Console : View
{
    //PRIVATE VARIABLE
    private bool _isRunning;

    //CONSTRUCTOR
    public Console()
    {
        _isRunning = true;
        BackupsViewModel.OnProgressUpdate += DisplayProgressBar;
        Display();
    }


    private void Display()//FIRST FUNCTION LAUNCH, Function that get user input option
    {
        while (_isRunning)
            switch (GetUserChoice())
            {
                case UserChoice.ShowAllSaveWork:
                    BackupsViewModel.DisplayAllSaveWork();
                    break;
                case UserChoice.CreateSaveWork:
                {
                    var (name, sourcePath, targetPath, backupType) = AskForCreatingSaveWork();
                    BackupsViewModel.CreateSaveWork(name, sourcePath, targetPath, backupType);
                    break;
                }
                case UserChoice.DeleteSaveWork:
                {
                    var name = AskForDeletingSaveWork();
                    BackupsViewModel.DeleteSaveWork(name);
                    break;
                }
                case UserChoice.RenameSaveWork:
                {
                    var (name, newName) = AskForRenamingSaveWork();
                    BackupsViewModel.RenameSaveWork(name, newName);
                    break;
                }
                case UserChoice.ExecuteAllSaveWork:
                {
                    BackupsViewModel.ExecAllSaveWork();
                    break;
                }
                case UserChoice.ChangeLanguage:
                {
                    var language = AskForChangingLanguage();
                    BackupsViewModel.ChangeLanguage(language);
                    break;
                }
                case UserChoice.ExecuteSaveWork:
                {
                    var name = AskForNametoExecute();
                    BackupsViewModel.ExecSaveWork(name);
                    break;
                }
                case UserChoice.Exit:
                    _isRunning = false;
                    break;
            }
    }

    //function that ask the name of the save work to execute
    private static string AskForNametoExecute()
    {
        Write(Resources.Ask_Backup_Name_ToExecute);
        var name = ReadLineNoEmpty();

        return name;
    }

    //function that ask the name of the language you want
    private static Language AskForChangingLanguage()
    {
        string language;
        do
        {
            Write(Resources.Ask_Language);
            language = ReadLineNoEmpty();
        } while (!Regex.IsMatch(language, "^(fr|en)$"));

        return language.Equals("en") ? Language.English : Language.French;
    }

    //ask the name of the save work you want to rename and the rename
    private static (string, string) AskForRenamingSaveWork()
    {
        Write(Resources.Ask_Backup_Name_Rename);
        var name = ReadLineNoEmpty();
        Write(Resources.Ask_Backup_New_Name_Rename);
        var newName = ReadLineNoEmpty();

        return (name, newName);
    }

    //Ask the name of the deleting savework
    private static string AskForDeletingSaveWork()
    {
        Write(Resources.Ask_Backup_Name_Delete);
        return ReadLineNoEmpty();
    }


    //ask info for the creation of a savework
    private static (string, string, string, SaveType) AskForCreatingSaveWork()
    {
        Write(Resources.Ask_Backup_Name);
        var name = ReadLineNoEmpty();
        Write(Resources.Ask_Backup_Source_Path);
        var sourcePath = ReadLineNoEmpty();
        Write(Resources.Ask_Backup_Target_Path);
        var targetPath = ReadLineNoEmpty();

        string backupType;
        do
        {
            Write(Resources.Ask_Backup_Type);
            backupType = ReadLineNoEmpty();
        } while (!Regex.IsMatch(backupType, "^(c|d)"));

        return (name, sourcePath, targetPath,
            backupType.StartsWith('c') ? SaveType.Complete : SaveType.Differential);
    }

    //function that bully the user for a valid input
    private static string ReadLineNoEmpty()
    {
        while (true)
        {
            var input = ReadLine();
            if (input?.Length > 0) return input;
            WriteLine(Resources.Ask_Valid_Input);
        }
    }

    //function that display text, usualy use in ViewModel
    public override void NotifyInfo(string text)
    {
        WriteLine(text);
    }
    
    //Display an Error
    public override void NotifyError(string text)
    {
        ForegroundColor = ConsoleColor.DarkRed;
        WriteLine(text);
        ResetColor();
    }
    
    //Display Success Message
    public override void NotifySuccess(string text)
    {
        ForegroundColor = ConsoleColor.Green;
        WriteLine(text);
        ResetColor();
    }

    //Display the progress bar 
    public void DisplayProgressBar(object sender, string s)
    {
        if (s != null)
        {
            WriteLine(" ========================================");
            WriteLine(s);
            WriteLine(" ========================================");
            SetCursorPosition(CursorLeft, CursorTop - 3);
            CursorVisible = false;
        } 
        else
        {

            for (int i = 0; i < 3; i++)
                WriteLine("");
        }
    }

    private static UserChoice GetUserChoice()
    {
        while (true)
        {
            WriteLine(
                "##########################################\n"
                + $"1 - {Resources.Show_Backups}\n"
                + $"2 - {Resources.Execute_Backup}\n"
                + $"3 - {Resources.Create_Backup}\n"
                + $"4 - {Resources.Delete_Backup}\n"
                + $"5 - {Resources.Rename_Backup}\n"
                + $"6 - {Resources.Execute_All_Backups}\n"
                + $"7 - {Resources.Change_Language}\n"
                + $"8 - {Resources.Exit_App}\n"
                + "##########################################"
            );

            if (int.TryParse(ReadLine(), out var choice))
            {
                choice--; // Enumeration's index starts from 0

                if (Enum.IsDefined(typeof(UserChoice), choice)) return (UserChoice) choice;
            }

            WriteLine(Resources.Console_Ask_Valid_Choice);
        }
    }

    private enum UserChoice
    {
        ShowAllSaveWork,
        ExecuteSaveWork,
        CreateSaveWork,
        DeleteSaveWork,
        RenameSaveWork,
        ExecuteAllSaveWork,
        ChangeLanguage,
        Exit
    }
}