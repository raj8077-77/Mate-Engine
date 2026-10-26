# Anshu - Personal Desktop Assistant

**Anshu** is a customized MateEngine turned into a personal Windows assistant controlled via Telegram.

## One Command Install (from Terminal)

Open PowerShell and paste this:

```powershell
irm https://raw.githubusercontent.com/raj8077-77/Mate-Engine/main/install-anshu.ps1 | iex
```

Or download the script and run:
```powershell
powershell -ExecutionPolicy Bypass -File install-anshu.ps1
```

This will:
- Create `Desktop\Anshu` folder
- Download the latest ready-to-go release (when available)
- Put a shortcut on your Desktop

## Main Features

- Telegram control (text, voice, images)
- Magic Spell (password) protected sessions (20 minutes)
- Google AI (Gemini) as main brain
- Local AI models support
- Settings menu to set Bot Token and Magic Spell
- Auto-start support
- Custom avatar

## Quick Start (after install)

1. Run the EXE from Desktop\Anshu or the shortcut.
2. Press `M` or right-click avatar → Settings.
3. Enter Telegram Bot Token and set Magic Spell.
4. On Telegram, send the Magic Spell to unlock the session.

## Models

Put models here:
```
C:\Anshu\models\
```

Full details in `ANSHU_SETUP.md`.

## Build Yourself (if no release yet)

1. Install Unity Hub + Editor (2022.3 LTS recommended)
2. Open this project in Unity
3. Open scene: `Mate Engine Main`
4. File → Build Settings → Build
5. Upload the built ZIP as a GitHub Release so the installer can download it

## Original Project

Based on MateEngine by shinyflvre. Customized for personal assistant use.
