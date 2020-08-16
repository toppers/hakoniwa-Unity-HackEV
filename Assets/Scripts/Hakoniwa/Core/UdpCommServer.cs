using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

using Hakoniwa.Assets;

namespace Hakoniwa.Core
{
    public class UdpCommServer: IIoReader
    {
        public int port = 3331;
        private int[] buffer;
        private ulong[] buffer64;
        private int buffer64_size = 2;
        private int packet_size = 1024;
        private int comm_size;
        private string packet_header = "ETTX";
        private int packet_version = 0x1;
        private int packet_ext_off = 512;
        private int packet_ext_size = 512;
        private Thread thread;
        IoReaderCallback callback;

        // Start is called before the first frame update
        public void DoStart()
        {
            this.comm_size = this.packet_size / 4;
            buffer = new int[comm_size];
            buffer64 = new ulong[buffer64_size];
            thread = new Thread(new ThreadStart(ThreadMethod));
            thread.Start();
        }

        public void RefData(int off_byte, out int data)
        {
            int off_word = off_byte / 4;
            if (off_word >= comm_size)
            {
                data = 0;
                return;
            }
            data = buffer[off_word];
        }

        public void RefData64(int off, out ulong data)
        {
            if (off >= buffer64_size)
            {
                data = 0;
                return;
            }
            data = buffer64[off];
            return;
        }


        public void SetCallback(IoReaderCallback func)
        {
            this.callback = func;
        }
        public void DoRun()
        {
            /* nothing to do */
        }
        private bool IsValidUdpPacket(byte[] data)
        {
            if (data.Length != packet_size)
            {
                Debug.LogError("udp msg invalid length:" + data.Length);
                return false;
            }
            byte[] byte_array = new byte[4];
            Buffer.BlockCopy(data, 0, byte_array, 0, byte_array.Length);
            string recv_header = System.Text.Encoding.ASCII.GetString(byte_array);
            if (!recv_header.Equals(this.packet_header))
            {
                Debug.LogError("udp msg invalid header:" + recv_header);
                return false;
            }
            int[] tmp_buf = new int[7];
            Buffer.BlockCopy(data, 4, tmp_buf, 0, 28);
            if (tmp_buf[0] != this.packet_version)
            {
                Debug.LogError("udp msg invalid version:" + packet_version);
                return false; //version
            }
            if (tmp_buf[5] != this.packet_ext_off)
            {
                Debug.LogError("udp msg invalid ext_off:" + packet_ext_off);
                return false; //ext_off
            }
            if (tmp_buf[6] != this.packet_ext_size)
            {
                Debug.LogError("udp msg invalid ext_size:" + packet_ext_size);
                return false; //ext_size
            }
            return true;
        }
        private void ThreadMethod()
        {
            UdpClient client;
            client = new UdpClient(port);
            Debug.Log("io server up");
            while (true)
            {
                IPEndPoint remoteEP = null;
                byte[] data = client.Receive(ref remoteEP);
                if (IsValidUdpPacket(data))
                {
                    Buffer.BlockCopy(data, 0, buffer, 0, data.Length);
                    Buffer.BlockCopy(data, 8, buffer64, 0, 16);
                    this.callback();
                }
                //Debug.Log("io msg in");
            }
        }
    }
}
