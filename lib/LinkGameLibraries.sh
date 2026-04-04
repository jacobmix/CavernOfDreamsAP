#!/bin/bash

# ============================================
# DLL Symlink Setup Script (Linux/macOS)
# ============================================
# This script lets you select your game's
# Managed folder and creates symlinks for all
# .dll files into the current directory.
#
# Usage:
#   ./setup_symlinks.sh
#
# Notes:
# - Requires bash
# - Uses a GUI picker if available (zenity)
# - Falls back to manual path input
# ============================================

#!/bin/bash
set -e

# Folder where the script is located
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
echo "Symlinks will be created in: $SCRIPT_DIR"
echo

echo "Select your game's Managed folder..."

# GUI picker or fallback
if command -v zenity >/dev/null 2>&1; then
    SOURCE=$(zenity --file-selection --directory --title="Select Managed folder")
elif [[ "$OSTYPE" == "darwin"* ]]; then
    # macOS native folder picker
    SOURCE=$(osascript -e 'POSIX path of (choose folder with prompt "Select Managed folder")')
else
    read -p "Enter full path to Managed folder: " SOURCE
fi

# Validate
if [ -z "$SOURCE" ] || [ ! -d "$SOURCE" ]; then
    echo "Invalid folder. Exiting."
    exit 1
fi

echo "Selected folder: $SOURCE"
echo

# Remove DLL & symlinks
echo "Cleaning existing DLL & symlinks..."
rm -f ./*.dll

# Enable nullglob to handle empty directories
shopt -s nullglob
echo "Creating symlinks..."
for file in "$SOURCE"/*.dll; do
    name=$(basename "$file")
    if [ -e "$name" ]; then
        echo "Skipping $name, already exists"
        continue
    fi
    echo "Linking $name"
    ln -s "$file" "$name"
done
shopt -u nullglob

echo
echo "Done linking DLLs."