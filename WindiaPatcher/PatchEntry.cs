namespace WindiaPatcher
{
    using System;
    using System.Runtime.CompilerServices;

    internal class PatchEntry
    {
        public PatchEntry(string filename, long size, string url)
        {
            this.FileName = filename;
            this.SizeInBytes = size;
            this.URL = url;
        }

        public override string ToString() => 
            $"{this.FileName} (Size: {this.SizeInBytes}) URL: {this.URL}";

        public string FileName { get; set; }

        public long SizeInBytes { get; set; }

        public string URL { get; set; }
    }
}

