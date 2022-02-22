using System.Collections.ObjectModel;
using System.Globalization;
using EasySave.translation;
using EasySave.view.wpf.core;
using EasySave.model;

namespace EasySave.viewmodel;

public class SettingsViewModel : ViewModelBase
{
    private readonly ObservableCollection<string> _highPriorityFileExtensions;

    public ObservableCollection<string> HighPriorityFileExtensions => _highPriorityFileExtensions;
    
    private readonly ObservableCollection<string> _encryptedFileExtensions;

    public ObservableCollection<string> EncryptedFileExtensions => _encryptedFileExtensions;

    public SettingsViewModel()
    {
        OnSelectEnglish = new CommandHandler(() => ChangeLanguage(Language.English));
        OnSelectFrench = new CommandHandler(() => ChangeLanguage(Language.French));
        OnSelectXML = new CommandHandler(() => ChangeLogFormat(new Xml()));
        OnSelectJSON = new CommandHandler(() => ChangeLogFormat(new Json()));

        _highPriorityFileExtensions = new ObservableCollection<string>(new[]
        {
            ".pdf",
            ".txt",
            ".docx",
            ".pdf",
            ".txt",
            ".docx"
        });
        
        _encryptedFileExtensions = new ObservableCollection<string>(new[]
        {
            ".pdf",
            ".txt",
            ".docx",
            ".mspaint"
        });
    }

    public CommandHandler OnSelectEnglish { get; set; }
    public CommandHandler OnSelectFrench { get; set; }
    public CommandHandler OnSelectXML { get; set; }
    public CommandHandler OnSelectJSON { get; set; }

    private static void ChangeLanguage(Language language) //Function to change language
    {
        Thread.CurrentThread.CurrentUICulture = language switch
        {
            Language.English => CultureInfo.GetCultureInfo("en"),
            Language.French => CultureInfo.GetCultureInfo("fr"),
            _ => CultureInfo.CurrentUICulture
        };
    }

    private static void ChangeLogFormat(FileFormat fileFormat)
    {
        Model.GetInstance().SetLogFileFormat(fileFormat);
    }
}