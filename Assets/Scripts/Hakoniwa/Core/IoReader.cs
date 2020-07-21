using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hakoniwa.Core
{
    public class IoReader : MonoBehaviour
    {
#if VDEV_IO_MMAP
    public string filepath = null;
    private MmapFileReader io;
#else
        /*************************************
         * UDP SETTINGS
         *************************************/
        public int port = 54001;
        private UdpCommServer io;
        /*************************************/
#endif
        private IoBufferParameter bufferParams = null;

        public void Start()
        {
        }
        public void Initialize()
        {
            GameObject hakoniwa = GameObject.Find("Hakoniwa");
            bufferParams = hakoniwa.GetComponentInChildren<IoBufferParameter>();
#if VDEV_IO_MMAP
        io = new MmapFileReader();
        io.DoStart(this.filepath);
#else
            /*************************************
             * UDP SETTINGS
             *************************************/
            io = new UdpCommServer();
            io.port = port;
            /*************************************/
            io.DoStart();
#endif
        }
        public void SetCallback(IoReaderCallback func)
        {
            Debug.Log("io=" + this.io + ":func=" + func);
            this.io.SetCallback(func);
        }
        public void DoRun()
        {
            io.DoRun();
        }

        public bool RefData(string name, out int data)
        {
            int off = bufferParams.GetActuatorOffset(name);
            if (off > 0)
            {
                io.RefData(off, out data);
                return true;
            }
            data = 0;
            return false;
        }
        public void RefSimTime(int off, out ulong sim_time)
        {
            this.io.RefData64(off, out sim_time);
        }
    }

}
