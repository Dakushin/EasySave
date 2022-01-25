
namespace EasySave
{
    abstract class FileFormat
    {
        public abstract void SaveInFormat(string path, object s);
    }
}
