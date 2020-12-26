# EAINDecompiler
Decompiler for Electronic Art's EAIN .dat files. This may only work on certain versions of games.

EA games (including Origin games) have programs in the \_\_Installer directory that are used to prepare the game for use, and are also used in game repair. These are named Touchup.exe and Cleanup.exe, and are used to write registry keys and such. The read data from their .dat files of the same prefixed name, and are encrypted.

This tool can reverse that encryption and decompile the LUA scripts contained within them.

To use: Pass the path of the .dat file to decrypt/decompile to this program, and then pipe the output to the file you want. For example:

`EAINDecompiler.exe "B:\Origin games\Mass Effect 2\__Installer\Touchup.dat" > me2touchup.txt`

This software is licenced under MIT, and uses LuaDec51.

## Technical info
These files contain a magic header (EAIN), and the remaining data is a single-byte cipher xor'd onto them. In the case of Mass Effect games, it is 0xB4 (this program will only apply 0xB4 cipher).
