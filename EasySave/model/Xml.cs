﻿using System.IO;
using System.Xml.Serialization;

namespace EasySave.model;

internal class Xml : FileFormat
{
    private const string Extension = ".xml";

    public override void SaveInFormat<T>(string path, T obj) //Serialize an object into a list of object in xml format
    {
        path = Checkpath(path, Extension);
        var list = new List<T>();
        if (File.Exists(path)) list = UnSerialize<T>(path);
        list.Add(obj);
        var x = new XmlSerializer(typeof(List<T>));
        var writer = new StreamWriter(path);
        x.Serialize(writer, list);
        writer.Close();
    }

    public override List<T> UnSerialize<T>(string path) //Deserialize an xml file into objects
    {
        path = Checkpath(path, Extension);
        if (File.Exists(path))
        {
            var serializer = new XmlSerializer(typeof(List<T>));

            // Declare an object variable of the type to be deserialized.
            List<T> i;

            using (Stream reader = new FileStream(path, FileMode.Open))
            {
                // Call the Deserialize method to restore the object's state.
                i = (List<T>) serializer.Deserialize(reader);
            }

            return i;
        }

        return null;
    }
}