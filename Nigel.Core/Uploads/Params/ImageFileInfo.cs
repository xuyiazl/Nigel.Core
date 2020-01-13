﻿using System;
using System.Collections.Generic;
using System.Text;
using FileInfo = Nigel.Files.FileInfo;

namespace Nigel.Core.Uploads.Params
{
    public class ImageFileInfo : FileInfo
    {
        public ImageFileInfo(string path, long? size, string fileName = null, string id = null)
            : base(path, size, fileName, id)
        {

        }

        public Dictionary<string, string> Thumbs { get; set; } = new Dictionary<string, string>();
    }
}
