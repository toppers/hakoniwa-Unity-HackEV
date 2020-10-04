using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hakoniwa.Core
{
    public class IoReader : MonoBehaviour
    {
        private IIoReader io;
        /*************************************
         * MMAP SETTINGS
         *************************************/
        public string filepath = null;
        private MmapFileReader io_mmap;

        /*************************************
         * UDP SETTINGS
         *************************************/
        public int port = 0;
        private UdpCommServer io_udp;
        /*************************************/

        private IoBufferParameter bufferParams = null;

        public void Initialize()
        {
            GameObject hakoniwa = GameObject.Find("Hakoniwa");
            bufferParams = hakoniwa.GetComponentInChildren<IoBufferParameter>();
            /*************************************
             * UDP SETTINGS
             *************************************/
            io_udp = new UdpCommServer();
            io_udp.port = port;
            /*************************************/
            io_udp.DoStart();
            this.io = io_udp;
            return;
        }
        public void InitializeMmap()
        {
            GameObject hakoniwa = GameObject.Find("Hakoniwa");
            bufferParams = hakoniwa.GetComponentInChildren<IoBufferParameter>();
            io_mmap = new MmapFileReader();
            io_mmap.DoStart(this.filepath);
            this.io = io_mmap;
            return;
        }
        public void SetCallback(IoReaderCallback func)
        {
            //Debug.Log("io=" + this.io + ":func=" + func);
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
