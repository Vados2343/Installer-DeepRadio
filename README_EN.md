# Old RadioPlayer Installer

![Version](https://img.shields.io/badge/version-2.0-blue.svg)
![Status](https://img.shields.io/badge/status-production-green.svg)
![Language](https://img.shields.io/badge/language-C%23-blue.svg)
![Multi-Language](https://img.shields.io/badge/languages-EN%20%7C%20UA%20%7C%20IT-brightgreen.svg)

## 📋 Overview

A modern, professional installer for Old RadioPlayer, built in C# using Windows Forms. The installer features an elegant multilingual interface (Ukrainian, English, Italian) and all necessary functions for safe and convenient software installation.

> **Note**: The new version of RadioPlayer is currently under active development. This installer is for the legacy "Old RadioPlayer" version.

## ✨ Features

- 🌍 **Multilingual Support** - Ukrainian, English, and Italian interfaces
- 🎨 **Modern Design** - Gradient borders, animated buttons, rounded corners
- 🔐 **Security** - Administrator rights verification, Zip Slip attack protection
- 📊 **Detailed Progress** - Step-by-step installation indicator with percentages
- 🗂️ **OneDrive Support** - Smart shortcut handling for OneDrive Desktop
- 📝 **Detailed Logging** - All actions are recorded in install.log
- ↩️ **Installation Rollback** - Automatic rollback of changes in case of error
- 🗑️ **Uninstaller** - Full uninstallation support with registry cleanup

## 🔧 Technical Features

### Components

1. **LanguageManager.cs** - Multilingual system
   - 3 languages support
   - Dynamic language switching
   - Comprehensive translation coverage

2. **MainForm.cs** - Main installer form
   - Multi-panel interface
   - Step-by-step installation
   - Path validation
   - Disk space verification

3. **ShortcutManager.cs** - Shortcut manager
   - OneDrive Desktop support
   - Multiple Desktop path detection
   - Registry-based verification
   - Fallback mechanisms

4. **RegistryManager.cs** - Registry manager
   - Add to startup
   - File existence verification
   - Safe registry entry removal

5. **FileManager.cs** - File manager
   - Zip Slip protection
   - Extraction progress tracking
   - Path validation
   - Unique temporary file names

6. **UninstallManager.cs** - Uninstaller
   - Complete file removal
   - Shortcut cleanup
   - Registry cleanup
   - Start menu folder removal

7. **CustomButton.cs** - Custom buttons
   - Smooth animations
   - Gradients and shadows
   - Modern design
   - Hover effects

## 🚀 Usage

### System Requirements
- Windows 7 or higher
- .NET Framework 4.8 or higher
- Administrator rights
- Minimum 50 MB free space

### Installation Process

1. **Launch**
   - Double-click Setup.exe
   - Grant administrator rights

2. **Language Selection**
   - Choose your preferred language
   - Available: Ukrainian, English, Italian

3. **License Agreement**
   - Read and accept the agreement

4. **Path Selection**
   - Default: `C:\Program Files\RadioPlayer`
   - Option to choose custom path
   - Free space verification

5. **Installation Options**
   - ☑️ Launch after installation
   - ☐ Visit website
   - ☐ Create desktop shortcut
   - ☐ Create Start Menu shortcut
   - ☐ Run with Windows startup

6. **Installation**
   - Extracting files (0-70%)
   - Verifying file integrity (70-75%)
   - Creating shortcuts (75-85%)
   - Configuring startup (85-90%)
   - Creating uninstaller (90-98%)
   - Completion (98-100%)

7. **Finish**
   - Optional program launch
   - Optional website visit

## 📁 File Structure

```
Installer-DeepRadio/
│
├── MainForm.cs              # Main form
├── LanguageManager.cs       # Multilingual system
├── ShortcutManager.cs       # Shortcut manager
├── RegistryManager.cs       # Registry manager
├── FileManager.cs           # File manager
├── UninstallManager.cs      # Uninstaller
├── CustomButton.cs          # Custom buttons
├── BetaButton.cs            # Beta indicator
│
├── license_ua.txt           # Ukrainian license
├── license_en.txt           # English license
├── license_it.txt           # Italian license
│
├── README_EN.md             # English documentation
├── README_IT.md             # Italian documentation
│
└── Properties/
    └── ...                  # Resources and settings
```

## 🛡️ Security

### Implemented Protections:

1. **Administrator Rights** - Verification and request for rights
2. **Zip Slip Protection** - Path validation during extraction
3. **Path Verification** - Forbidden system directories
4. **Space Verification** - Sufficient disk space
5. **File Validation** - Verification of exe file existence
6. **Safe Rollback** - Removal only from Program Files

## 🌐 Support

- **Email:** support@deepradio.cloud
- **Website:** https://deepradio.cloud

## 📝 License

This installer is developed for Old RadioPlayer.
All rights reserved © 2025 DeepRadio

## 🎉 Acknowledgments

Thank you for using the Old RadioPlayer Installer!

---

**Version:** 2.0 (Modernized)
**Date:** November 2025
**Status:** ✅ Production Ready
