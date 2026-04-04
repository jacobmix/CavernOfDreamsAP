# Libs Folder

This folder is used for referencing DLLs libraries from the game.

## Required files

You need DLLs from the game's `Data/Managed` folder.

Example (Windows):
C:\Program Files (x86)\Steam\steamapps\common\Cavern of Dreams\Cavern of Dreams_Data\Managed

## Quick Setup (Recommended)

You can automatically set up DLL symlinks using the provided scripts.

### Windows
Run:
LinkGameLibraries.bat

### Linux / macOS
Run:
./LinkGameLibraries.sh

Then select your game's `Managed` folder when prompted.

## What these scripts do

- Create symbolic links for all `.dll` files
- Place them directly inside this `libs/` folder
- Does NOT copy any files

## Alternative (Manual Setup)

You can manually copy `.dll` files into this folder if preferred.

## Notes

- `.dll` files are ignored by Git
- Symlinks are also ignored
- No game files are included in this repository
