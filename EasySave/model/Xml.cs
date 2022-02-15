using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace EasySave.model
{
    internal class Xml : FileFormat
    {
        private string extention = ".xml";
        public override void SaveInFormat<T>(string path, T obj) //Serialize an object into a list of object in xml format
        {
            path = Checkpath(path, extention);
            var list = new List<T>();
            if (File.Exists(path)) list = UnSerialize<T>(path);
            list.Add(obj);
            XmlSerializer x = new XmlSerializer(typeof(List<T>));
            StreamWriter writer = new StreamWriter(path);
            x.Serialize(writer, list);
            writer.Close();

        }
        public override List<T> UnSerialize<T>(string path) //Deserialize an xml file into objects
        {
            path = Checkpath(path, extention);
            if (File.Exists(path))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<T>));

                // Declare an object variable of the type to be deserialized.
                List<T> i;

                using (Stream reader = new FileStream(path, FileMode.Open))
                {
                    // Call the Deserialize method to restore the object's state.
                    i = (List<T>)serializer.Deserialize(reader);
                }
                return i;
            }
            return null;
        }
    }
}
