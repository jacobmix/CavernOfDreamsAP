# Cavern of Dreams AP Client
This client makes a large amount of changes to the base game. Here is a non-exhaustive list:

* A lot of basic movement has been split into collectible items:
    * High Jump
    * Sprinting (gaining speed by holding the roll button)
    * Rolling
    * Carrying items
    * Tail is split into Grounded Tail and Aerial Tail
    * Climbing vines and ladders
    * Swimming
    * ...and some others
* Flight has been replaced with a Double Jump
* Pause menu allows you to toggle collected abilities
* Cannot pick up Jester Boots while carrying an item, or vice-versa (to lower logic complexity)
* Checkpoints are removed
* Keehees are automatically reignited upon room re-entry
* Almost all cutscenes are now actionable for convenience
* Many dialog trees have been cut down for convenience
* Return to Start button in pause menu
* Sun Cavern no longer has a Jester Boots removal trigger. Instead, Jester Boots are lost upon entering a portal
* Gallery of Nightmare's Water Lobby (the sewers) spawns in some safety bars if you do not yet have access to Swim

---

# How to Build (Windows)

This guide explains how to build **CavernOfDreamsAP** on Windows, including prebuild setup, required dependencies, and generating the plugin DLL.

---

## 1. Pre-build Setup

### Install Git

Download and install Git for Windows:
[https://git-scm.com/install/windows](https://git-scm.com/install/windows)

### Install Python 3.12.x

Download and install Python 3.12.x:
[https://www.python.org/downloads/release/python-31210/](https://www.python.org/downloads/release/python-31210/)

* Make sure to **add Python to your PATH** during installation.
* Check the right Python version is being used with `python --version`.
* Ensure `pip` is installed.
* Install any required Python packages. (Sorry. Got no `requirements.txt` for you currently)
> Note: You can fix path issues with [Rapid Environment Editor](<https://www.rapidee.com/en/download>)

### Install Microsoft Visual Studio

Install **Visual Studio** (Current year "Community" version if unsure) with:

* .NET desktop development
* Optional: C++ support (if prompted)
* No Unity support is required for this build.

---

## 2. Download the Repositories

> ⚠️ You need **Administrator privileges** to create symlinks.

Open **Command Prompt as Administrator**, then:

```bat
:: Clone the AP world repo
git clone -b apworld_test --recurse-submodules https://github.com/jacobmix/CavernOfDreamsAP.git CavernOfDreamsAP_Apworld

:: Create a symlink to the world folder for ease of access (relative to your current directory)
mklink /D "cavern_of_dreams" "%CD%\CavernOfDreamsAP_Apworld\worlds\cavern_of_dreams"

:: Clone the client repo
git clone -b client_test --recurse-submodules https://github.com/jacobmix/CavernOfDreamsAP.git CavernOfDreamsAP_Client
```

---

## 3. Prebuild Steps

1. Continue in **Command Prompt** (No admin required) and navigate to the client folder:

```bat
cd CavernOfDreamsAP_Client
```

2. **Set your Cavern of Dreams installation path**:

```bat
:: Change this to your Steam install path for Cavern of Dreams
set "CoDSTEAM=C:\Path\To\SteamLibrary\steamapps\common\Cavern of Dreams\"
```

3. **Copy DLLs** from the game to the client lib folder:

```bat
mkdir lib 2>nul
copy "%CoDSTEAM%Cavern of Dreams_Data\Managed\*.dll" lib\
```

4. **Run Python prebuild scripts**:

```bat
cd cavern_of_dreams_ap_logic
python prebuild.py
cd ..
python prebuild.py
```

5. **Generate AP data**:

```bat
mkdir "cavern_of_dreams_ap_logic\ap_generated" 2>nul
python -m cavern_of_dreams_ap_logic.generate_ap_data
```

6. **Copy generated data into the symlinked world folder**:

```bat
xcopy /s /i "cavern_of_dreams_ap_logic\ap_generated" "..\cavern_of_dreams\ap_generated"
```

---

## 4. Build CoDArchipelago Plugin (DLL)

1. Open Visual Studio and load the solution:

```text
CavernOfDreamsAP_Client\CoDArchipelago.sln
```

2. Set the build configuration:

* **Configuration:** Release (or Debug)
* **Platform:** Any CPU

3. Build the solution:

```text
Build > Build Solution
```

4. After building, the plugin DLL will be located in:

```text
CavernOfDreamsAP_Client\bin\<Configuration>\CoDArchipelago.dll
```

---

## 5. Creating the Apworld Package

To package the AP world:

1. Simply zip the `cavern_of_dreams` worlds folder.
2. Rename the `.zip` extension to `.apworld`.

> Example:
> `cavern_of_dreams.zip` → `cavern_of_dreams.apworld`

---

## 6. Notes & Troubleshooting

* **Admin for symlink:** If `mklink` fails, run Command Prompt as Administrator.
* **Python errors:** Ensure Python 3.12.x is installed, and required packages are present.
* **DLLs not copying:** Verify your `CoDSTEAM` path points to the correct Steam installation folder.
* **Rebuild Visual Studio solution:** If errors occur, try `Rebuild Solution`.

---

If all that is too much at once. Then here's a [bat](<https://gist.github.com/jacobmix/9e9ad2134fb9ec198dfc2586cd0a637f>) file.
