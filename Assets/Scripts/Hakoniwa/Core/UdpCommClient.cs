using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

using Hakoniwa.Assets;

namespace Hakoniwa.Core
{
    public class UdpCommClient: IIoWriter
    {
        public string host = "192.168.0.30";
        public int port = 3333;
        public int comm_size = 256;
        private int[] buffer;
        public int buffer64_size = 1;
        private ulong[] buffer64;
        private UdpClient client;


        public void DoStart()
        {
            buffer = new int[comm_size];
            buffer64 = new ulong[buffer64_size];
            {
                //header
                byte[] byte_array = new byte[4];
                byte_array[0] = 0x45; /* E */
                byte_array[1] = 0x54; /* T */
                byte_array[2] = 0x52; /* R */
                byte_array[3] = 0x58; /* X */
                Buffer.BlockCopy(byte_array, 0, buffer, 0, byte_array.Length);
                int[] tmp_buf = new int[7];
                tmp_buf[0] = 1; //version
                tmp_buf[1] = 0; //reserve
                tmp_buf[2] = 0; //reserve
                tmp_buf[3] = 0; //unity time
                tmp_buf[4] = 0; //unity time
                tmp_buf[5] = 512; //ext_off
                tmp_buf[6] = 512; //ext_size
                Buffer.BlockCopy(tmp_buf, 0, buffer, 4, 28);
            }
            client = new UdpClient();
            client.Connect(host, port);
        }


        public void SetData(int off_byte, int data)
        {
            int off_word = off_byte / 4;
            if (off_word >= comm_size)
            {
                return;
            }
            //Debug.Log("off_word=" + off_word + " data=" + data);
            buffer[off_word] = data;
        }
        public void SetData64(int off, ulong data)
        {
            if (off >= this.buffer64_size)
            {
                return;
            }
            //Debug.Log("off_word=" + off_word + " data=" + data);
            buffer64[off] = data;
        }

        public void Send()
        {
            byte[] byte_array = new byte[comm_size * 4];
            Buffer.BlockCopy(buffer, 0, byte_array, 0, byte_array.Length);
            Buffer.BlockCopy(buffer64, 0, byte_array, 16, 8);

            client.Send(byte_array, comm_size * 4);
        }

    }
}

