

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
                    case "1": System.Console.WriteLine("Give name to your work :");
                        string name = System.Console.ReadLine();
                        System.Console.WriteLine("Give name to your work :");
                        string sourcePath = System.Console.ReadLine();
                        System.Console.WriteLine("Give name to your work :");
                        string name = System.Console.ReadLine();
                        System.Console.WriteLine("Give name to your work :");
                        string name = System.Console.ReadLine();
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
                                     "##### 2 - Info             #####\n" +
                                     "##### 2 - Info             #####\n" +
                                     "##### 2 - Info             #####\n" +
                                     "##### 2 - Info             #####\n" +
                                     "##### 2 - Info             #####\n" +
                                     "#####################~##########");
            string valeurretour = System.Console.ReadLine();
            return valeurretour;

        }
    }
}
