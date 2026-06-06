# 📚 StudyBuddy

> An AI-powered educational social platform for students — built to centralise learning, spark collaboration, and make studying less lonely.

![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-512BD4?style=flat&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=flat&logo=c-sharp&logoColor=white)
![MySQL](https://img.shields.io/badge/MySQL-4479A1?style=flat&logo=mysql&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=flat&logo=docker&logoColor=white)
![SignalR](https://img.shields.io/badge/SignalR-FF4500?style=flat)
![Discord.Net](https://img.shields.io/badge/Discord.Net-5865F2?style=flat&logo=discord&logoColor=white)
![Groq API](https://img.shields.io/badge/Groq_API-000000?style=flat)

---

## 📖 Table of Contents

- [About](#-about)
- [Features](#-features)
- [Discord Bot](#-discord-bot)
- [Tech Stack](#-tech-stack)
- [Project Structure](#-project-structure)
- [Getting Started](#-getting-started)
- [Configuration](#-configuration)
- [Author](#-author)

---

## 🧠 About

StudyBuddy is a full-stack social learning platform that combines a community feed, collaborative study groups, AI-assisted note management, and real-time notifications — all in one place. It also ships with a companion **Discord bot** that brings AI study tools directly into your server.

---

## ✨ Features

### 👥 Social
- Follow users and build your academic network
- Like and comment on posts and blogs
- Save content for later
- Full CRUD on posts and blogs

### 📖 Study Groups
- Create subject-specific groups
- Share notes and blogs within a group
- Collaborate with peers around shared topics

### 🤖 AI — powered by Groq API
- **Note summarisation** — get the key points of any note instantly
- **Q&A on note content** — ask questions directly about your notes
- **Blog tag & title recommendations** — let AI suggest labels for your content
- **Content enhancement** — improve the quality of your writing
- **AI Chat Assistant** — built-in bubble chat for quick help anywhere on the platform

### 🔔 Real-time
- Live notification system built with SignalR

### 🧠 Smart Recommendations
- Custom-built algorithm that suggests users and study groups based on interests — no third-party library, fully self-implemented

### 🔐 Auth
- JWT-based authentication & authorisation
- Role management system (Admin panel included)

---

## 🤖 Discord Bot

StudyBuddy includes a standalone Discord bot (`ConsoleApp1/`) that brings AI study tools into any Discord server.

### Commands

| Command | Description |
|---|---|
| `/studytip` | Get an AI-generated, actionable study tip |
| `/quiz <topic>` | Generate a 5-question multiple choice quiz on any topic |
| `/summarize <text>` | Summarise any text using AI |

### How it works

The bot is built with **Discord.Net** and uses the same **Groq API** (with `llama-3.3-70b-versatile`) as the main web app. Slash commands are registered per-guild on startup. Each command handler sends a prompt to `GroqService` and responds directly in the Discord channel.

### Running the bot

```bash
cd ConsoleApp1/ConsoleApp1
dotnet run
```

Make sure `appsettings.json` contains your `Discord:Token` and `Groq:ApiKey` (see [Configuration](#-configuration)).

> **Note:** The bot uses `GatewayIntents.None` — it only needs slash command access, so no privileged intents are required.

---

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Backend | C#, ASP.NET Core (Razor Pages + MVC) |
| ORM | Entity Framework Core |
| Database | MySQL (Docker container) |
| Real-time | SignalR |
| AI | Groq API (`llama-3.3-70b-versatile`) |
| Auth | ASP.NET Core Identity + JWT |
| Discord Bot | Discord.Net 3.19.1 |

---

## 📁 Project Structure

```
StudyBuddy/
├── StudyBuddy/                  # Main web application
│   ├── Controllers/             # API controllers (AI chat, comments, notifications…)
│   ├── Pages/                   # Razor Pages (Feed, Notes, Blogs, Groups, Profile…)
│   ├── Services/                # Business logic (AI, notes, blogs, users, groups…)
│   ├── Models/                  # EF Core models
│   ├── Hubs/                    # SignalR hubs
│   ├── Migrations/              # EF Core migrations
│   └── Program.cs               # App entry point & DI setup
│
└── ConsoleApp1/ConsoleApp1/     # Discord bot
    ├── Commands/                # Slash command handlers
    │   ├── QuizCommand.cs
    │   ├── StudyTipCommand.cs
    │   └── SummarizeCommand.cs
    ├── Services/
    │   └── GroqService.cs       # Groq API client
    ├── Program.cs               # Bot entry point
    └── appsettings.json         # Config (tokens & API keys)
```

---

## 🚀 Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) (for the MySQL container)
- A [Groq API key](https://console.groq.com/)
- A Discord bot token (for the Discord bot)

### Web App

```bash
git clone https://github.com/Deyvis-Gushterov/StudyBuddy.git
cd StudyBuddy/StudyBuddy
dotnet run
```

### Discord Bot

```bash
cd StudyBuddy/ConsoleApp1/ConsoleApp1
dotnet run
```

---

## ⚙️ Configuration

### Web App — `StudyBuddy/appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;database=studybuddy;user=root;password=yourpassword"
  },
  "Groq": {
    "ApiKey": "your-groq-api-key"
  }
}
```

### Discord Bot — `ConsoleApp1/ConsoleApp1/appsettings.json`

```json
{
  "Discord": {
    "Token": "your-discord-bot-token"
  },
  "Groq": {
    "ApiKey": "your-groq-api-key",
    "Model": "llama-3.3-70b-versatile"
  }
}
```

> ⚠️ Never commit real tokens or API keys to version control. Use [.NET User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) or environment variables in production.

---

## 🚧 In Progress

- Comments on notes

---

## 📸 Screenshots

*Coming soon*

---
<img width="2552" height="1288" alt="Screenshot 2026-06-05 234453" src="https://github.com/user-attachments/assets/e2bbb302-e589-4ab8-aae1-167f512be1db" />
<img width="2553" height="1210" alt="Screenshot 2026-06-05 234526" src="https://github.com/user-attachments/assets/5c8b3f93-73df-4f55-8e88-76d31e7c1be8" />
<img width="2539" height="1305" alt="Screenshot 2026-06-05 234541" src="https://github.com/user-attachments/assets/bf8ac84c-b4b8-4c8a-9aff-ab70a6259987" />



## 👤 Author

**Deyvis Gushterov**  
- GitHub: [@Deyvis-Gushterov](https://github.com/Deyvis-Gushterov)
