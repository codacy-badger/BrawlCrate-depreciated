﻿using System.IO.Ports;

namespace System.IO
{
    public unsafe class GeckoStream
    {
        public SerialPort _port;
        public Stream _stream => _port.BaseStream;

        public GeckoStream(SerialPort port)
        {
            (_port = port).Open();
        }
    }
}