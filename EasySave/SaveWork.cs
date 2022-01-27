

namespace EasySave
{
    enum Type{
        Complet,
        Differential
    }
    internal class SaveWork
    {
        private string name;
        private string sourcePath;
        private string targetPath;
        Type type;

        public SaveWork(string name, string sourcePath, string targetPath, Type type)
        {
            this.name = name;
            this.sourcePath = sourcePath;
            this.targetPath = targetPath;
            this.type = type;
        }
<<<<<<< HEAD

        public string GetName()
        {
            return name;
        }

        public void SetName(string n)
        {
            name = n;
        }

        public string GetSourcePath()
        {
            return sourcePath;
        }

        public string GetTargetPath()
        {
            return targetPath;
        }

        public Type Gettype()
        {
            return type;
        }
=======
>>>>>>> origin/main
    }
}
