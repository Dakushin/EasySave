using System;
using System.Collections.Generic;
using System.IO;

public sealed class LanguageSystem
{
	public enum Language
    {
		English,
		French
    }

	private Language currentLanguage;
	private string translationPath;
	private Dictionary<string, string> translation;
	private LanguageSystem()
	{
		translationPath = Path.GetFullPath(@"..\..\..\translation.csv");
		currentLanguage = Language.English;
		translation = new Dictionary<string, string>();
		RecupFromFile();
	}

	private static LanguageSystem instance;

	public static LanguageSystem GetInstance()
    {
		if(instance == null)
        {
			instance = new LanguageSystem();
        }
		return instance;
    }

	private void RecupFromFile()
    {
		translation.Clear();
		if(File.Exists(translationPath))
        {
			var reader = new StreamReader(File.OpenRead(translationPath));
			while(!reader.EndOfStream)
            {
				string line = reader.ReadLine();
				string[] value = line.Split(';');

				if(currentLanguage == Language.French)
				{
					translation.Add(value[0], value[2]);
				}
				if(currentLanguage == Language.English)
                {
					translation.Add(value[0], value[1]);
                }
				
            }
        }
    }

	public void ChangeLanguage(Language language)
    {
		currentLanguage = language;
		RecupFromFile();
    }

	public string Get(string key)
    {
		if (translation.ContainsKey(key))
		{
			return translation[key];
		}
        else
        {
			return "Key is missing";
        }
    }

	public Language GetCurrentLanguage()
    {
		return currentLanguage;
    }
}
