

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
    }
}
