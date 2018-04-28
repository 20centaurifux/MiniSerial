﻿/***************************************************************************
    begin........: April 2018
    copyright....: Sebastian Fedrau
    email........: sebastian.fedrau@gmail.com
 ***************************************************************************/

/***************************************************************************
    This library is free software; you can redistribute it and/or
    modify it under the terms of the GNU Lesser General Public
    License version 2.1 as published by the Free Software Foundation.
 
    This library is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
    Lesser General Public License for more details.
 ***************************************************************************/

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace MiniSerial
{
	public class Port : IDisposable
	{
		[DllImport("miniserial.so")]
		private static extern unsafe IntPtr serial_port_new([MarshalAs(UnmanagedType.LPStr)]string path, int flags, UInt32 flush_delay);

		[DllImport("miniserial.so")]
		private static extern unsafe void serial_port_destroy(IntPtr port);

		[DllImport("miniserial.so")]
		private static extern unsafe bool serial_port_open(IntPtr port);

		[DllImport("miniserial.so")]
		private static extern unsafe void serial_port_close(IntPtr port);

		[DllImport("miniserial.so")]
		private static extern unsafe void serial_port_set_speed(IntPtr port, int ispeed, int ospeed);

		[DllImport("miniserial.so")]
		private static extern unsafe void serial_port_get_modes(IntPtr port, out int cflag, out int iflag, out int oflag, out int lflag);

		[DllImport("miniserial.so")]
		private static extern unsafe void serial_port_set_modes(IntPtr port, int cflag, int iflag, int oflag, int lflag);

		[DllImport("miniserial.so")]
		private static extern unsafe void serial_port_set_special_char(IntPtr port, int id, int value);

		[DllImport("miniserial.so")]
		private static extern unsafe bool serial_port_flush(IntPtr port);

		[DllImport("miniserial.so")]
		private static extern unsafe int serial_port_read(IntPtr port, byte[] buffer, int count);

		[DllImport("miniserial.so")]
		private static extern unsafe int serial_port_write(IntPtr port, byte[] buffer, int count);

		private IntPtr _port;
		private int _speed = Bits.B9600;

		public const int O_RDONLY = 0;
		public const int O_WRONLY = 1;
		public const int O_RDWR = 2;
		public const int O_NOCTTY = 256;
		public const int O_SYNC = 1052672;

		public enum Flag
		{
			Input,
			Output,
			Control,
			Local
		}

		public Port(string filename, int flags, UInt32 flushDelay)
		{
			_port = serial_port_new(filename, flags, flushDelay);
		}

		public void Open()
		{
			if(!serial_port_open(_port))
			{
				throw new IOException();
			}
		}

		public void Close()
		{
			serial_port_close(_port);
		}

		public int Speed
		{
			set
			{
				_speed = value;
				serial_port_set_speed(_port, value, value);
			}

			get
			{
				return _speed;
			}
		}

		public int GetFlag(Flag flag)
		{
			int value = 0;

			serial_port_get_modes(_port, out int cflag, out int iflag, out int oflag, out int lflag);

			switch(flag)
			{
				case Flag.Control:
					value = cflag;
					break;

				case Flag.Input:
					value = iflag;
					break;

				case Flag.Output:
					value = oflag;
					break;

				case Flag.Local:
					value = lflag;
					break;
			}

			return value;
		}

		public void SetFlag(Flag flag, int value)
		{
			serial_port_get_modes(_port, out int cflag, out int iflag, out int oflag, out int lflag);

			switch(flag)
			{
				case Flag.Control:
					cflag = value;
					break;

				case Flag.Input:
					iflag = value;
					break;

				case Flag.Output:
					oflag = value;
					break;

				case Flag.Local:
					lflag = value;
					break;
			}

			serial_port_set_modes(_port, cflag, iflag, oflag, lflag);
		}

		public void SetSpecialChar(int index, int value)
		{
			serial_port_set_special_char(_port, index, value);
		}

		public byte[] Read()
		{
			byte[] bytes = new byte[512];

			int received = serial_port_read(_port, bytes, 512);

			if(received < 0)
			{
				throw new IOException();
			}

			Array.Resize(ref bytes, received);

			return bytes;
		}

		public long Write(byte[] bytes, int count)
		{
			int written = serial_port_write(_port, bytes, count);

			if(written < 0)
			{
				throw new IOException();
			}

			return written;
		}

		public long Write(string text)
		{
			var bytes = System.Text.Encoding.UTF8.GetBytes(text);

			return Write(bytes, bytes.Length);
		}

		public void Flush()
		{
			if(!serial_port_flush(_port))
			{
				throw new IOException();
			}
		}

		public void Dispose()
		{
			serial_port_destroy(_port);
		}
	}

}
