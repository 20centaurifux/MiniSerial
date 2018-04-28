using System;

namespace MiniSerial
{
	class Program
	{
		static void Main(string[] args)
		{
			using(var port = new Port("/dev/ttyACM0", Port.O_RDWR | Port.O_SYNC | Port.O_NOCTTY, 2500))
			{
				port.Open();

				port.Speed = Bits.B9600;

				var cflag = port.GetFlag(Port.Flag.Control);

				cflag |= (Bits.CLOCAL | Bits.CREAD);
				cflag &= ~Bits.CSIZE;
				cflag |= Bits.CS8;
				cflag &= ~Bits.PARENB;
				cflag &= ~Bits.CSTOPB;

				port.SetFlag(Port.Flag.Control, cflag);

				var iflag = port.GetFlag(Port.Flag.Input);

				iflag |= (Bits.IGNPAR | Bits.IGNCR);
				iflag &= ~(Bits.IXON | Bits.IXOFF | Bits.IXANY);
				iflag &= ~Bits.INPCK;
				iflag &= ~(Bits.ICRNL | Bits.IGNCR);
				iflag |= Bits.INPCK;

				port.SetFlag(Port.Flag.Input, iflag);

				var oflag = port.GetFlag(Port.Flag.Output);

				oflag &= ~Bits.OPOST;

				port.SetFlag(Port.Flag.Output, oflag);

				port.SetSpecialChar(Bits.VTIME, 0);
				port.SetSpecialChar(Bits.VMIN, 0);
				port.SetSpecialChar(Bits.VEOF, 4);

				port.Flush();

				port.Write("REDRUM\n");

				while(true)
				{
					var bytes = port.Read();

					if(bytes.Length > 0)
					{
						Console.Write(System.Text.Encoding.UTF8.GetString(bytes));
					}
				}
			}
		}
	}
}
