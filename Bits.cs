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
   @file Bits.cs
   @brief termios bitmasks.
   @author Sebastian Fedrau <sebastian.fedrau@gmail.com>
 */

namespace MiniSerial
{
	/**
	   @class Bits
	   @brief Many bitmasks copied from /usr/include/bits/termios.h
	 */
	public static class Bits
	{
		public const int VINTR = 0;
		public const int VQUIT = 1;
		public const int VERASE = 2;
		public const int VKILL = 3;
		public const int VEOF = 4;
		public const int VTIME = 5;
		public const int VMIN = 6;
		public const int VSWTC = 7;
		public const int VSTART = 8;
		public const int VSTOP = 9;
		public const int VSUSP = 10;
		public const int VEOL = 11;
		public const int VREPRINT = 12;
		public const int VDISCARD = 13;
		public const int VWERASE = 14;
		public const int VLNEXT = 15;
		public const int VEOL2 = 16;
		public const int IGNBRK = 1;
		public const int BRKINT = 2;
		public const int IGNPAR = 4;
		public const int PARMRK = 8;
		public const int INPCK = 16;
		public const int ISTRIP = 32;
		public const int INLCR = 64;
		public const int IGNCR = 128;
		public const int ICRNL = 256;
		public const int IUCLC = 512;
		public const int IXON = 1024;
		public const int IXANY = 2048;
		public const int IXOFF = 4096;
		public const int IMAXBEL = 8192;
		public const int IUTF8 = 16384;
		public const int OPOST = 1;
		public const int OLCUC = 2;
		public const int ONLCR = 4;
		public const int OCRNL = 8;
		public const int ONOCR = 16;
		public const int ONLRET = 32;
		public const int OFILL = 64;
		public const int OFDEL = 128;
		public const int NLDLY = 256;
		public const int NL0 = 0;
		public const int NL1 = 256;
		public const int CRDLY = 1536;
		public const int CR0 = 0;
		public const int CR1 = 512;
		public const int CR2 = 1024;
		public const int CR3 = 1536;
		public const int TABDLY = 6144;
		public const int TAB0 = 0;
		public const int TAB1 = 2048;
		public const int TAB2 = 4096;
		public const int TAB3 = 6144;
		public const int BSDLY = 8192;
		public const int BS0 = 0;
		public const int BS1 = 8192;
		public const int FFDLY = 32768;
		public const int FF0 = 0;
		public const int FF1 = 32768;
		public const int VTDLY = 16384;
		public const int VT0 = 0;
		public const int VT1 = 16384;
		public const int XTABS = 6144;
		public const int CBAUD = 4111;
		public const int B0 = 0;
		public const int B50 = 1;
		public const int B75 = 2;
		public const int B110 = 3;
		public const int B134 = 4;
		public const int B150 = 5;
		public const int B200 = 6;
		public const int B300 = 7;
		public const int B600 = 8;
		public const int B1200 = 9;
		public const int B1800 = 10;
		public const int B2400 = 11;
		public const int B4800 = 12;
		public const int B9600 = 13;
		public const int B19200 = 14;
		public const int B38400 = 15;
		public const int EXTA = B19200;
		public const int EXTB = B38400;
		public const int CSIZE = 48;
		public const int CS5 = 0;
		public const int CS6 = 16;
		public const int CS7 = 32;
		public const int CS8 = 48;
		public const int CSTOPB = 64;
		public const int CREAD = 128;
		public const int PARENB = 256;
		public const int PARODD = 512;
		public const int HUPCL = 1024;
		public const int CLOCAL = 2048;
		public const int B57600 = 4097;
		public const int B115200 = 4098;
		public const int B230400 = 4099;
		public const int B460800 = 4100;
		public const int B500000 = 4101;
		public const int B576000 = 4102;
		public const int B921600 = 4103;
		public const int B1000000 = 4104;
		public const int B1152000 = 4105;
		public const int B1500000 = 4106;
		public const int B2000000 = 4107;
		public const int B2500000 = 4108;
		public const int B3000000 = 4109;
		public const int B3500000 = 4110;
		public const int B4000000 = 4111;
		public const int ISIG = 1;
		public const int ICANON = 2;
		public const int XCASE = 4;
		public const int ECHO = 8;
		public const int ECHOE = 16;
		public const int ECHOK = 32;
		public const int ECHONL = 64;
		public const int NOFLSH = 128;
		public const int TOSTOP = 256;
		public const int ECHOCTL = 512;
		public const int ECHOPRT = 1024;
		public const int ECHOKE	 = 2048;
		public const int FLUSHO	 = 4096;
		public const int PENDIN	 = 16384;
		public const int IEXTEN = 32768;
		public const int EXTPROC = 65536;
	}
}
