using StudyBuddyBot.Services;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudyBuddyBot.Commands
{
    internal class SummarizeCommand
    {
        public static async Task HandleAsync(SocketSlashCommand command, GroqService groq)
        {
            var text = command.Data.Options.First().Value.ToString();

            var prompt = $"Summarize the following text concisely: {text}";

            var result = await groq.AskAsync(prompt);
            await command.RespondAsync(result);
        }
    }
}
