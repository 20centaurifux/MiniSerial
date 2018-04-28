CC?=gcc
CFLAGS?=-Wall -std=gnu99 -fPIC -O2 -nostartfiles -shared

all:
	$(CC) ./miniserial.c -o ./miniserial.so $(CFLAGS)

clean:
	rm ./miniserial.so
