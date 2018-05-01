/***************************************************************************
    begin........: April 2018
    copyright....: Sebastian Fedrau
    email........: sebastian.fedrau@gmail.com
 ***************************************************************************/

/***************************************************************************
    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License v3 as published by
    the Free Software Foundation.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
    General Public License v3 for more details.
 ***************************************************************************/
/**
   @file MiniSerial.cs
   @brief .NET termios wrapper.
   @author Sebastian Fedrau <sebastian.fedrau@gmail.com>
 */
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace MiniSerial
{
	/**
	   @class Port
	   @brief Serial port access with termios.
	 */
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

		/*! Open device read-only. */
		public const int O_RDONLY = 0;
		/*! Open device write-only. */
		public const int O_WRONLY = 1;
		/*! Open device with read & write permissions. */
		public const int O_RDWR = 2;
		/*! Don't let terminal device become the process's controlling terminal. */
		public const int O_NOCTTY = 256;
		/*! Fulfill requirements of synchronized I/O file integrity completion  */
		public const int O_SYNC = 1052672;

		/**
		   @enum Flag
		   @brief Available flags.
		 */
		public enum Flag
		{
			/*! Input flag. */
			Input,
			/*! Output flag. */
			Output,
			/*! Control flag. */
			Control,
			/*! Local flag. */
			Local
		}

		/**
		   @param filename path to serial device
		   @param flags file open flags
		   @param flushDelay milliseconds to sleep after flush

		   Initializes a new Port instance.
		 */
		public Port(string filename, int flags, UInt32 flushDelay)
		{
			_port = serial_port_new(filename, flags, flushDelay);
		}

		/*! Opens the serial device connection. */
		public void Open()
		{
			if(!serial_port_open(_port))
			{
				throw new IOException();
			}
		}

		/*! Closes the serial device connection. */
		public void Close()
		{
			serial_port_close(_port);
		}

		/*! Sets input and output speed. */
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

		/**
		   @param flag flag to receive
		   @return a flag

		   Returns the desired flag.
		 */
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

		/**
		   @param flag flag to set
		   @param value the new value for the specified flag

		   Overwrites the specified flag.
		 */
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

		/**
		   @param index index of the character to set
		   @param value new value to set

		   Sets a special terminal character.
		 */
		public void SetSpecialChar(int index, int value)
		{
			serial_port_set_special_char(_port, index, value);
		}

		/**
		   @return read bytes

		   Reads from the serial device.
		 */
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

		/**
		   @param bytes bytes to write
		   @param count number of bytes to write
		   @return number of written bytes

		   Writes to the serial device.
		 */
		public long Write(byte[] bytes, int count)
		{
			int written = serial_port_write(_port, bytes, count);

			if(written < 0)
			{
				throw new IOException();
			}

			return written;
		}

		/**
		   @param text text to write to the serial device
		   @return number of written bytes

		   Converts an UTF-8 encoded string to a byte array and sends it
		   to the serial device.
		 */
		public long Write(string text)
		{
			var bytes = System.Text.Encoding.UTF8.GetBytes(text);

			return Write(bytes, bytes.Length);
		}

		/**
		   Flushes input and output buffers.
		 */
		public void Flush()
		{
			if(!serial_port_flush(_port))
			{
				throw new IOException();
			}
		}

		/**
		   Frees resources.
		 */
		public void Dispose()
		{
			serial_port_destroy(_port);
		}
	}

}
