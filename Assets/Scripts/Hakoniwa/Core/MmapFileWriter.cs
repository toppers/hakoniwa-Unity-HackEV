using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using UnityEngine;

using Hakoniwa.Assets;

namespace Hakoniwa.Core
{
    public class MmapFileWriter: IIoWriter
    {
        private MemoryMappedFile mappedFile;
        private UnmanagedMemoryAccessor accessor;
        private int sim_time_off = 16;

        public void DoStart(string filepath)
        {
            if (!System.IO.File.Exists(filepath))
            {
                Debug.LogError("filepath is invalid:" + filepath);
                return;
            }
            else
            {
                Debug.Log("MmapFileWriter:filepath=" + filepath);
            }
            this.mappedFile = MemoryMappedFile.CreateFromFile(filepath, System.IO.FileMode.Open);
            this.accessor = mappedFile.CreateViewAccessor();
            int init_data = 0;
            for (int i = 0; i < 1024; i++)
            {
                accessor.Write<int>(i * 4, ref init_data);
            }
        }
        public void SetData(int off_byte, int data)
        {
            this.accessor.Write<int>(off_byte, ref data);
        }
        public void SetData64(int off, ulong data)
        {
            this.accessor.Write<ulong>(off + this.sim_time_off, ref data);
        }

        public void Send()
        {
            /* nothing to do */
        }

        public void SetData64(int off, double data)
        {
            this.accessor.Write<double>(off, ref data);
        }
    }

}

