using System.Globalization;
using EasySave.translation;
using EasySave.view.wpf.core;

namespace EasySave.viewmodel;

public class SettingsViewModel : ViewModelBase
{
    public SettingsViewModel()
    {
        OnSelectEnglish = new CommandHandler(() => ChangeLanguage(Language.English));
        OnSelectFrench = new CommandHandler(() => ChangeLanguage(Language.French));
    }

    public CommandHandler OnSelectEnglish { get; set; }
    public CommandHandler OnSelectFrench { get; set; }

    private static void ChangeLanguage(Language language) //Function to change language
    {
        Thread.CurrentThread.CurrentUICulture = language switch
        {
            Language.English => CultureInfo.GetCultureInfo("en"),
            Language.French => CultureInfo.GetCultureInfo("fr"),
            _ => CultureInfo.CurrentUICulture
        };
    }
}