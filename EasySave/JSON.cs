using System.IO;
using Newtonsoft.Json;


namespace EasySave
{
    internal class JSON : FileFormat
    {
        public override void SaveInFormat(string path, object obj)
        {
            string s = JsonConvert.SerializeObject(obj);
            File.WriteAllText(path, s);
        }
    }
}
