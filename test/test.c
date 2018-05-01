#include "miniserial.h"

#include <stdio.h>
#include <fcntl.h>
#include <unistd.h>

#define DEVICE "/dev/ttyACM0"

int
main(int argc, char *argv[])
{
	SerialPort *port = serial_port_new("/dev/ttyACM0", O_RDWR|O_NOCTTY|O_SYNC, 2500);

	if(!serial_port_open(port))
	{
		fprintf(stderr, "Couldn't open device.\n");
		abort();
	}

	serial_port_set_speed(port, B9600, B9600);

	int cflag, iflag, oflag, lflag;

	serial_port_get_modes(port, &cflag, &iflag, &oflag, &lflag);

	cflag |= (CLOCAL | CREAD);
	iflag |= (IGNPAR | IGNCR);
	iflag &= ~(IXON | IXOFF | IXANY);
	oflag &= ~OPOST;
	cflag &= ~CSIZE;
	cflag |= CS8;
	cflag &= ~PARENB;
	iflag &= ~INPCK;
	iflag &= ~(ICRNL|IGNCR);
	cflag &= ~CSTOPB;
	iflag |= INPCK;

	serial_port_set_modes(port, cflag, iflag, oflag, lflag);

	serial_port_set_special_char(port, VTIME, 0);
	serial_port_set_special_char(port, VMIN, 0);
	serial_port_set_special_char(port, VEOF, 4);

	char buffer[7];

	if(!serial_port_write_settings(port))
	{
		fprintf(stderr, "Couldn't write settings.\n");
		abort();
	}

	if(!serial_port_flush(port))
	{
		fprintf(stderr, "Flush failed.\n");
		abort();
	}

	serial_port_write(port, "REDRUM\n", 7);
	serial_port_write(port, "too much work makes jack a dull boy\n", 36);

	ssize_t bytes = serial_port_read(port, buffer, 512);

	while(bytes >= 0)
	{
		if(bytes)
		{
			write(0, buffer, bytes);
		}

		bytes = serial_port_read(port, buffer, 512);
	}

	serial_port_destroy(port);

	return 0;
}

