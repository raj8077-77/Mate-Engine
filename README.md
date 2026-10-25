# Anshu - Personal Desktop Assistant

**Anshu** is a customized MateEngine turned into a personal Windows assistant controlled via Telegram.

## Main Features

- Telegram control (text, voice, images)
- Magic Spell (password) protected sessions (20 minutes)
- Google AI (Gemini) as main brain
- Local AI models support
- Settings menu to set Bot Token and Magic Spell
- Reset option protected by current password
- Auto-start support
- Custom avatar

## Quick Start

1. Build the project in Unity or wait for a release.
2. Run the exe.
3. Press `M` or right-click avatar → Settings.
4. Enter Telegram Bot Token and set Magic Spell.
5. On Telegram, send the Magic Spell to unlock the session.

## Models

Put models here:
```
C:\Anshu\models\
```

Full details in `ANSHU_SETUP.md`.

## Settings

From the in-app settings you can:
- Set / change Bot Token
- Set / change Magic Spell (requires current password to reset)
- View session status

## Build Instructions

1. Open this project in Unity Hub.
2. Open the main scene.
3. File → Build Settings → Build for Windows.

## Original Project

Based on MateEngine by shinyflvre. Customized for personal assistant use.
