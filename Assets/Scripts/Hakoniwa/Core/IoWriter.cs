using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Hakoniwa.Core;

namespace Hakoniwa.Core
{
    public class IoWriter : MonoBehaviour
    {
        /*************************************
         * MMAP SETTINGS
         *************************************/
        public string filepath = null;
        private MmapFileWriter io_mmap;

        /*************************************
         * UDP SETTINGS
         *************************************/
        public string host = null;
        public int port = 0;
        private UdpCommClient io_udp;
        /*************************************/

        private IIoWriter io;
        private IoBufferParameter bufferParams = null;

        public void Initialize()
        {
            GameObject hakoniwa = GameObject.Find("Hakoniwa");

            bufferParams = hakoniwa.GetComponentInChildren<IoBufferParameter>();

            /*************************************
             * UDP SETTINGS
             *************************************/
            io_udp = new UdpCommClient();
            io_udp.host = host;
            io_udp.port = port;
            /*************************************/
            io_udp.DoStart();
            this.io = io_udp;
            Debug.Log("params=" + bufferParams);
            return;
        }
        public void InitializeMmap()
        {
            GameObject hakoniwa = GameObject.Find("Hakoniwa");
            bufferParams = hakoniwa.GetComponentInChildren<IoBufferParameter>();
            io_mmap = new MmapFileWriter();
            io_mmap.DoStart(this.filepath);
            this.io = io_mmap;
            Debug.Log("params=" + bufferParams);
            return;
        }


        public void Set(string name, int value1)
        {
            int off = bufferParams.GetSensorOffset(name);
            if (off > 0)
            {
                //Debug.Log("off=" + off + ": value=" + value1);
                io.SetData(off, value1);
            }
        }
        public void SetSimTime(ulong time)
        {
            io.SetData64(0, time);
        }


        public void Publish()
        {
            io.Send();
        }
    }

}

