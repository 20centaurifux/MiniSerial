CC?=gcc
CFLAGS?=-Wall -std=gnu99 -fPIC -O2 -nostartfiles -shared

all:
	dotnet build -c Release
	$(CC) $(CFLAGS) ./miniserial.c -o ./bin/Release/netcoreapp2.0/miniserial.so

clean:
	dotnet clean -c Release
	rm -f ./bin/Release/netcoreapp2.0/miniserial.so
