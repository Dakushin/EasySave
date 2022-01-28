

namespace EasySave
{
    internal class Console : View
    {
        private bool isRunning;
        public Console()
        {
            Afficher();
            isRunning = true;
        }
        public override void Afficher()
        {
            while (isRunning)
            {
                switch (Menu())
                {
                    case "1": System.Console.WriteLine("\nGive name to your work : ");
                        string name = System.Console.ReadLine();
                        System.Console.WriteLine("\nGive the source path directory : ");
                        string sourcePath = System.Console.ReadLine();
                        System.Console.WriteLine("\nGive the target path directory : ");
                        string targetPath = System.Console.ReadLine();
                        System.Console.WriteLine("\nGive the type [differential/complet]: ");
                        string stype = "";
                        Type type = Type.Complet;
                        while (stype != "differential" & stype != "complet")
                        {
                            stype = System.Console.ReadLine();
                            switch(stype)
                            {
                                case "differential": type = Type.Differential; break;
                                case "complet": type = Type.Complet; break;
                                case null:
                                default: System.Console.WriteLine("Please enter correct value [differential/complet] : "); break;
                            }
                        }
                        viewModel.CreateSaveWork(name, sourcePath, targetPath, type);
                        break;
                    case "2": break;
                    case "3": isRunning = false; break;
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
            System.Console.WriteLine("################################\n" +
                                     "##### 1 - Create Save Work #####\n" +
                                     "##### 2 - Delete Save Work #####\n" +
                                     "##### 2 - Rename Save Work #####\n" +
                                     "##### 2 - Change Langue    #####\n" +
                                     "##### 2 - Info             #####\n" +
                                     "##### 2 - Info             #####\n" +
                                     "################################");
            string valeurretour = System.Console.ReadLine();
            return valeurretour;

        }
    }
}
