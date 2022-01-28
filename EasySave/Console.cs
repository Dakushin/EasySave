

namespace EasySave
{
    internal class Console : View
    {
        private LanguageSystem lsInstance;
        private bool isRunning;
        public Console()
        {
            Afficher();
            isRunning = true;
            lsInstance = LanguageSystem.GetInstance();
        }
        public override void Afficher()
        {
            while (isRunning)
            {
                switch (Menu())
                {
                    case "1":
                        {
                            System.Console.WriteLine(lsInstance.Get("give-name-to-work"));
                            string name = System.Console.ReadLine();
                            System.Console.WriteLine("Give the source path directory : ");
                            string sourcePath = System.Console.ReadLine();
                            System.Console.WriteLine("Give the target path directory : ");
                            string targetPath = System.Console.ReadLine();
                            System.Console.WriteLine("Give the type [differential/complet]: ");
                            string stype = "";
                            Type type = Type.Complet;
                            while (stype != "differential" & stype != "complet")
                            {
                                stype = System.Console.ReadLine();
                                switch (stype)
                                {
                                    case "differential": type = Type.Differential; break;
                                    case "complet": type = Type.Complet; break;
                                    case null:
                                    default: System.Console.WriteLine("Please enter correct value [differential/complet] : "); break;
                                }
                            }
                            viewModel.CreateSaveWork(name, sourcePath, targetPath, type);
                            break;
                        }
                    case "2":
                        {
                            System.Console.WriteLine("Give the name of the work you want to delete : ");
                            string name = System.Console.ReadLine();
                            viewModel.DeleteSaveWork(name);
                            break;
                        }
                    case "3":
                        {
                            System.Console.WriteLine("Give the name of the work you want to rename : ");
                            string name = System.Console.ReadLine();
                            System.Console.WriteLine("Give the rename of it : ");
                            string rename = System.Console.ReadLine();
                            viewModel.RenameSaveWork(name, rename);
                            break;
                        }
                    case "4":
                        {
                            System.Console.WriteLine(lsInstance.Get("change-language"));
                            string info = System.Console.ReadLine();
                            if (info == lsInstance.Get("yes"))
                            {
                                if (lsInstance.GetCurrentLanguage() == LanguageSystem.Language.French)
                                {
                                    lsInstance.ChangeLanguage(LanguageSystem.Language.English);
                                }
                                else
                                {
                                    lsInstance.ChangeLanguage(LanguageSystem.Language.French);
                                }
                            }

                            break;
                        }
                    case "123465789": isRunning = false; break;
                    default:
                        System.Console.WriteLine("Veuillez rentrer une info valide");
                        break;
                }
            }
        }

        public override void AfficherText(string s)
        {
            System.Console.WriteLine(s);
        }


        private string Menu()
        {
            System.Console.WriteLine("#################################\n" +
                                     "##### 1 - Create Save Work  #####\n" +
                                     "##### 2 - Delete Save Work  #####\n" +
                                     "##### 2 - Rename Save Work  #####\n" +
                                     "##### 2 - Show All SaveWork #####\n" +
                                     "##### 2 - Info              #####\n" +
                                     "#################################");
            string valeurretour = System.Console.ReadLine();
            return valeurretour;

        }
    }
}
