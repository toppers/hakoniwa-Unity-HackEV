using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using UnityEngine;

public class MmapFileReader
{
    private MemoryMappedFile mappedFile;
    private UnmanagedMemoryAccessor accessor;
    IoReaderCallback callback;
    private int sim_time_off = 16;

    public void DoStart(string filepath)
    {
        if (!System.IO.File.Exists(filepath))
        {
            Debug.LogError("filepath is invalid");
            return;
        }
        else
        {
            Debug.Log("MmapFileReader:filepath=" + filepath);
        }
        this.mappedFile = MemoryMappedFile.CreateFromFile(filepath, System.IO.FileMode.Open);
        this.accessor = mappedFile.CreateViewAccessor();
        int init_data = 0;
        for (int i = 0; i < 256; i++)
        {
            accessor.Write<int>(i * 4, ref init_data);
        }
    }

    public void RefData(int off_byte, out int data)
    {
        data = accessor.ReadInt32(off_byte);
    }

    public void RefData64(int off, out ulong data)
    {
        data = accessor.ReadUInt64(off + this.sim_time_off);
    }


    public void SetCallback(IoReaderCallback func)
    {
        this.callback = func;
    }
    public void DoRun()
    {
        this.callback();
    }

}
