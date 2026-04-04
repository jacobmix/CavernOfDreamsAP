# ap_generated Folder

This folder will contain generated data from the cavern_of_dreams_ap_logic generator. 

## Required files

You need py files `data.py`, `entrance_rando.py`, and `regions.py`.
From running: `python -m cavern_of_dreams_ap_logic.generate_ap_data`
In the root folder of the client repo. A folder up from `cavern_of_dreams_ap_logic`
Then files should show up in `cavern_of_dreams_ap_logic\ap_generated`

## Quick Setup (Recommended)

You can automatically set up py symlinks using the provided scripts.

### Windows
Run:
LinkPyFiles.bat

### Linux / macOS
Run:
./LinkPyFiles.sh

Then select your `ap_generated` folder when prompted.

## What these scripts do

- Create symbolic links for all `.py` files
- Place them directly inside this `ap_generated/` folder
- Does NOT copy any files

## Alternative (Manual Setup)

You can manually copy `.py` files into this folder if preferred.

## Notes

- `.py` files are ignored by Git
- Symlinks are also ignored
- No game files are included in this repository