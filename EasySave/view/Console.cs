using System.Text.RegularExpressions;
using EasySave.model;
using EasySave.translation;
using EasySave.viewmodel;
using static System.Console;

namespace EasySave.view;

internal class Console : View
{
    private bool _isRunning;

    public Console()
    {
        _isRunning = true;
        ViewModel.OnProgresseUpdate += DisplayProgressBar;
        Display();
    }

    private void Display()
    {
        while (_isRunning)
            switch (GetUserChoice())
            {
                case UserChoice.ShowAllSaveWork:
                    ViewModel.AfficherAllSaveWork();
                    break;
                case UserChoice.CreateSaveWork:
                {
                    var (name, sourcePath, targetPath, backupType) = AskForCreatingSaveWork();
                    ViewModel.CreateSaveWork(name, sourcePath, targetPath, backupType);
                    break;
                }
                case UserChoice.DeleteSaveWork:
                {
                    var name = AskForDeletingSaveWork();
                    ViewModel.DeleteSaveWork(name);
                    break;
                }
                case UserChoice.RenameSaveWork:
                {
                    var (name, newName) = AskForRenamingSaveWork();
                    ViewModel.RenameSaveWork(name, newName);
                    break;
                }
                case UserChoice.ExecuteAllSaveWork:
                {
                    ViewModel.ExecAllSaveWork();
                    break;
                }
                case UserChoice.ChangeLanguage:
                {
                    var language = AskForChangingLanguage();
                    ViewModel.ChangeLanguage(language);
                    break;
                }
                case UserChoice.ExecuteSaveWork:
                {
                    var name = AskForNametoExecute();
                    ViewModel.ExecSaveWork(name);
                    break;
                }
                case UserChoice.Exit:
                    _isRunning = false;
                    break;
            }
    }

    private static string AskForNametoExecute()
    {
        Write(strings.Ask_Backup_Name_ToExecute);
        var name = ReadLineNoEmpty();

        return name;
    }
    private static Language AskForChangingLanguage()
    {
        string language;
        do
        {
            Write(strings.Ask_Language);
            language = ReadLineNoEmpty();
        } while (!Regex.IsMatch(language, "^(fr|en)$"));

        return language.Equals("en") ? Language.English : Language.French;
    }

    private static (string, string) AskForRenamingSaveWork()
    {
        Write(strings.Ask_Backup_Name_Rename);
        var name = ReadLineNoEmpty();
        Write(strings.Ask_Backup_New_Name_Rename);
        var newName = ReadLineNoEmpty();

        return (name, newName);
    }

    private static string AskForDeletingSaveWork()
    {
        Write(strings.Ask_Backup_Name_Delete);
        return ReadLineNoEmpty();
    }

    private static (string, string, string, SaveType) AskForCreatingSaveWork()
    {
        Write(strings.Ask_Backup_Name);
        var name = ReadLineNoEmpty();
        Write(strings.Ask_Backup_Source_Path);
        var sourcePath = ReadLineNoEmpty();
        Write(strings.Ask_Backup_Target_Path);
        var targetPath = ReadLineNoEmpty();

        string backupType;
        do
        {
            Write(strings.Ask_Backup_Type);
            backupType = ReadLineNoEmpty();
        } while (!Regex.IsMatch(backupType, "^(c|d)"));

        return (name, sourcePath, targetPath,
            backupType.StartsWith('c') ? SaveType.Complete : SaveType.Differential);
    }

    private static string ReadLineNoEmpty()
    {
        while (true)
        {
            var input = ReadLine();
            if (input?.Length > 0) return input;
            WriteLine(strings.Ask_Valid_Input);
        }
    }

    public override void DisplayText(string text)
    {
        WriteLine(text);
    }
    
    public override void DisplayError(string text)
    {
        ForegroundColor = ConsoleColor.DarkRed;
        WriteLine(text);
        ResetColor();
    }
    
    public override void DisplaySuccess(string text)
    {
        ForegroundColor = ConsoleColor.Green;
        WriteLine(text);
        ResetColor();
    }

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
            SetCursorPosition(CursorLeft, CursorTop + 3);
            WriteLine(strings.Success);
        }
    }

    private static UserChoice GetUserChoice()
    {
        while (true)
        {
            WriteLine(
                "##########################################\n"
                + $"1 - {strings.Show_Backups}\n"
                + $"2 - {strings.Execute_Backup}\n"
                + $"3 - {strings.Create_Backup}\n"
                + $"4 - {strings.Delete_Backup}\n"
                + $"5 - {strings.Rename_Backup}\n"
                + $"6 - {strings.Execute_All_Backups}\n"
                + $"7 - {strings.Change_Language}\n"
                + $"8 - {strings.Exit_App}\n"
                + "##########################################"
            );

            if (int.TryParse(ReadLine(), out var choice))
            {
                choice--; // Enumeration's index starts from 0

                if (Enum.IsDefined(typeof(UserChoice), choice)) return (UserChoice) choice;
            }

            WriteLine(strings.Console_Ask_Valid_Choice);
        }
    }

    private enum UserChoice
    {
        ShowAllSaveWork,
        ExecuteSaveWork,
        CreateSaveWork,
        RenameSaveWork,
        DeleteSaveWork,
        ExecuteAllSaveWork,
        ChangeLanguage,
        Exit
    }
}