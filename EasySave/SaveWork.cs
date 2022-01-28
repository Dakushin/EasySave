

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
    }
}
