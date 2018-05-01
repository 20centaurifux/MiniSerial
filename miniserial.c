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
   @file miniserial.c
   @brief termios wrapper.
   @author Sebastian Fedrau <sebastian.fedrau@gmail.com>
 */
#include "miniserial.h"

#include <stdio.h>
#include <stdlib.h>
#include <fcntl.h>
#include <unistd.h>
#include <errno.h>
#include <string.h>
#include <errno.h>
#include <assert.h>

SerialPort *
serial_port_new(const char *path, int flags, uint32_t flush_delay)
{
	SerialPort *port = NULL;

	assert(path != NULL);

	if(path)
	{
		port = (SerialPort *)malloc(sizeof(SerialPort));

		if(port)
		{
			memset(port, 0, sizeof(SerialPort));

			port->path = (char *)malloc(strlen(path) + 1);

			if(port->path)
			{
				strcpy(port->path, path);

				port->fd = -1;
				port->flags = flags;
				port->flush_delay = flush_delay;
			}
			else
			{
				free(port);
				port = NULL;
			}
		}
	}

	return port;
}

void
serial_port_destroy(SerialPort *port)
{
	assert(port != NULL);

	if(port)
	{
		serial_port_close(port);

		free(port->path);
		free(port);
	}
}

bool
serial_port_open(SerialPort *port)
{
	bool success = false;

	assert(port != NULL);
	assert(port->path != NULL);

	if(port && port->fd == -1)
	{
		port->fd = open(port->path, port->flags);

		if(port->fd >= 0)
		{
			success = tcflush(port->fd, TCIOFLUSH) != -1 && tcgetattr(port->fd, &port->settings) != -1;
		}

		if(!success)
		{
			port->errcode = errno;
			close(port->fd);
			port->fd = -1;
		}
	}

	return success;
}

void
serial_port_close(SerialPort *port)
{
	assert(port != NULL);

	if(port && port->fd != -1)
	{
		close(port->fd);
	}
}

bool
serial_port_write_settings(SerialPort *port)
{
	bool success = false;

	assert(port != NULL);

	if(port && port->fd != -1)
	{
		if(tcflush(port->fd, TCIOFLUSH) == -1 || tcsetattr(port->fd, TCSANOW, &port->settings) == -1)
		{
			port->errcode = errno;
		}
		else
		{
			success = true;
		}
	}

	return success;
}

void
serial_port_set_speed(SerialPort *port, int ispeed, int ospeed)
{
	assert(port != NULL);

	if(port)
	{
		cfsetispeed(&port->settings, ispeed);
		cfsetospeed(&port->settings, ospeed);
	}
}

void
serial_port_get_modes(SerialPort *port, int *cflag, int *iflag, int *oflag, int *lflag)
{
	assert(port != NULL);
	assert(cflag != NULL);
	assert(iflag != NULL);
	assert(oflag != NULL);
	assert(lflag != NULL);

	if(port && cflag && iflag && oflag && lflag)
	{
		*cflag = port->settings.c_cflag;
		*iflag = port->settings.c_iflag;
		*cflag = port->settings.c_oflag;
		*cflag = port->settings.c_lflag;
	}
}

void
serial_port_set_modes(SerialPort *port, int cflag, int iflag, int oflag, int lflag)
{
	assert(port != NULL);

	if(port)
	{
		port->settings.c_cflag = cflag;
		port->settings.c_iflag = iflag;
		port->settings.c_oflag = oflag;
		port->settings.c_lflag = lflag;
	}
}

void
serial_port_set_special_char(SerialPort *port, int index, int value)
{
	assert(port != NULL);

	if(port)
	{
		port->settings.c_cc[index] = value;
	}
}

bool
serial_port_flush(SerialPort *port)
{
	bool success = false;

	assert(port != NULL);
	assert(port->fd != -1);

	if(port && port->fd != -1)
	{
		if(tcflush(port->fd, TCIOFLUSH) == -1)
		{
			port->errcode = errno;
		}
		else
		{
			success = true;
			usleep((useconds_t)port->flush_delay * 1000);
		}
	}

	return success;
}

ssize_t
serial_port_read(SerialPort *port, char *buffer, size_t count)
{
	return read(port->fd, buffer, count);
}

ssize_t
serial_port_write(SerialPort *port, const char *buffer, size_t count)
{
	ssize_t bytes = write(port->fd, buffer, count);

	if(bytes >= 0)
	{
		tcdrain(port->fd);
	}

	return bytes;
}

int
serial_port_last_error(SerialPort *port)
{
	assert(port != NULL);

	if(port)
	{
		return port->errcode;
	}

	return 0;
}

