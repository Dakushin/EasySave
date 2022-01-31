﻿using System.Collections.Generic;

namespace EasySave
{
    public abstract class FileFormat
    {
        public abstract void SaveInFormat<T>(string path, T obj);
        public abstract List<T> UnSerialize<T>(string path);
    }
}
