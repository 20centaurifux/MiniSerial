/***************************************************************************
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
/**
   @file miniserial.h
   @brief termios wrapper.
   @author Sebastian Fedrau <sebastian.fedrau@gmail.com>
 */
#ifndef MINISERIAL_H
#define MINISERIAL_H

#include <stdint.h>
#include <stdbool.h>
#include <termios.h>
#include <stdlib.h>
#include <unistd.h>

/**
   @struct SerialPort
   @brief File descriptor & options.
 */
typedef struct
{
	/*! Path to the TTY device. */
	char *path;
	/*! File open flags (see open(2)). */
	int flags;
	/*! Descriptor of the opened TTY file. */
	int fd;
	/*! Milliseconds to sleep after flushing input & output buffers. */
	uint32_t flush_delay;
	/*! Last set termios settings. */
	struct termios settings;
	/*! Error code of the last failed operation. */
	int errcode;
} SerialPort;

/**
 * @param path to the TTY device.
 * @param flags file open flags (see open(2)).
 * @param flush_delay time to sleep after flushing input & output buffers.
 * @return a new SerialPort instance or NULL on failure
 *
 * Initializes a new SerialPort instance.
 */
SerialPort *serial_port_new(const char *path, int flags, uint32_t flush_delay);

/**
 * @param port SerialPort instance to free
 *
 * Frees a SerialPort instance. The assigned file descriptor is closed automatically.
 */
void serial_port_destroy(SerialPort *port);

/**
 * @param port SerialPort instance
 * @return TRUE on success
 *
 * Opens the TTY device.
 */
bool serial_port_open(SerialPort *port);

/**
 * @param port SerialPort instance
 *
 * Closes the TTY device.
 */
void serial_port_close(SerialPort *port);

/**
 * @param port SerialPort instance
 * @return TRUE on success
 *
 * Sends the current settings to the TTY device.
 */
bool serial_port_write_settings(SerialPort *port);

/**
 * @param port SerialPort instance
 * @param ispeed input speed
 * @param ospeed output speed
 *
 * Sets speed options.
 */
void serial_port_set_speed(SerialPort *port, int ispeed, int ospeed);

/**
 * @param port SerialPort instance
 * @param cflag control modes
 * @param iflag input modes
 * @param oflag output modes
 * @param lflag local modes
 *
 * Receives current modes.
 */
void serial_port_get_modes(SerialPort *port, int *cflag, int *iflag, int *oflag, int *lflag);

/**
 * @param port SerialPort instance
 * @param cflag control modes
 * @param iflag input modes
 * @param oflag output modes
 * @param lflag local modes
 *
 * Sets modes.
 */
void serial_port_set_modes(SerialPort *port, int cflag, int iflag, int oflag, int lflag);

/**
 * @param port SerialPort instance
 * @param index index of the character
 * @param value value to set
 *
 * Defines a special character.
 */
void serial_port_set_special_char(SerialPort *port, int index, int value);

/**
 * @param port SerialPort instance
 * @return true on success
 *
 * Flushes input & output buffers.
 */
bool serial_port_flush(SerialPort *port);

/**
 * @param port SerialPort instance
 * @param buffer location to store read bytes to
 * @param count number of bytes to read
 * @return number of read bytes or -1 on failure
 *
 * Reads from the TTY device.
 */
ssize_t serial_port_read(SerialPort *port, char *buffer, size_t count);

/**
 * @param port SerialPort instance
 * @param buffer bytes to write to the TTY device.
 * @param count number of bytes to write
 * @return number of written bytes or -1 on failure
 *
 * Writes to the TTY device.
 */
ssize_t serial_port_write(SerialPort *port, const char *buffer, size_t count);

#endif

