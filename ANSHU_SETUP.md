# Anshu - MateEngine Custom Assistant

**Name:** Anshu  
**Based on:** MateEngine (forked from shinyflvre/Mate-Engine)

## Features to Implement

1. Rename display name to **Anshu**
2. Remove / tone down sexy animations and content
3. Telegram control with Magic Spell (hashed password)
4. Auto-start on Windows boot
5. Integrate local AI models
6. Google Search AI (Gemini) as main brain with structured output
7. Support text, voice, and images from Telegram
8. 20-minute session after Magic Spell

## Model Placement Guide

Place your models in the following locations (create folders if needed):

### Recommended Structure
```
C:\Anshu\models\
├── Qwen2.5-0.5B-Instruct-Q4_K_M.gguf
├── Qwen2.5-1.5B-Instruct-Q4_K_M.gguf
├── Qwen2.5-Coder-14B-Instruct-Q4_K_M.gguf
├── UI-TARS-1.5-7B-Q4_K_M.gguf
├── mmproj-model-f16.gguf          (for UI-TARS vision)
├── gemma-4-12b-it-Q4_K_M.gguf
└── mmproj-gemma.gguf               (if needed)
```

**When to load which model:**
- **Qwen 0.5B / 1.5B** → Fast replies, simple chat, status checks
- **Qwen2.5-Coder-14B** → Precise PC commands, Win+R paths, settings, coding tasks
- **UI-TARS or gemma-4-12b** → Screenshot / image understanding from Telegram
- **Google Gemini (browser or API)** → Main brain for analysis (Context / User Reply / AI Task)

## Magic Spell System

- Store the Magic Spell as a **SHA-256 hash** (never store plain text).
- On Telegram, user must send the Magic Spell to start a 20-minute session.
- After session expires, require Magic Spell again.

## Google AI Brain Instruction

Paste this into Google Search AI / Gemini at the start of every new session:

```
You are the full brain Analyzer AI supporting a Primary AI that controls a Windows PC and chats with the user.
The Primary AI forwards every user message + full chat history to you. You must analyze and reply in exactly three short sections. No extra text.

### Output Format (Always Use This):
**Context:**
[Fix spelling + clearly state the user’s exact intention in one line.]

**User Reply:**
[Direct short message the Primary AI should say to the user. Confirm what they want. Keep it natural and minimal.]

**AI Task:**
[Extremely precise instruction for the Primary AI on what to do on the PC. Include exact steps, Win+R commands, settings paths, app names, or search queries. Must be accurate and executable.]

### Strict Rules:
- Write only what is necessary. No unused information.
- In normal casual chat, still give all three sections, but keep AI Task very light or “No action needed.”
- **Movies:** If user wants to play a movie, always check availability. Prime Video only if included in normal Prime (not rent). Also check Netflix and Jio Hotstar.
- **Settings / Commands / Apps:** Give exact Windows 11 paths or Win+R commands.
- **YouTube / Songs:** Identify the exact song and give clear search query.
- Primary AI is female. Guide tone briefly inside User Reply when needed.
```

## Telegram Setup

1. Create bot with [@BotFather](https://t.me/BotFather)
2. Get Bot Token
3. Put token in Anshu config (to be added in code)

## Start on Boot

- Place shortcut of `MateEngineX.exe` in:
  `shell:startup`
- Or use Task Scheduler for more control

## Next Steps (Code Changes)

- Add `AnshuTelegramManager.cs` script
- Wire Magic Spell check
- Connect to local LLM via LLMUnity or external process (Ollama / llama.cpp)
- Browser automation for Google AI (or use Gemini API)
- Filter sexy animations in Animator controller

---

**Status:** Setup guide created. Code implementation in progress.
