using StudyBuddyBot.Services;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudyBuddyBot.Commands
{
    public  class StudyTipCommand
    {
        public static async Task HandleAsync(SocketSlashCommand command, GroqService groq)
        {

            var prompt = "Give me one specific, actionable study tip. Keep it under 3 sentences. Be friendly and encouraging.";

            var result = await groq.AskAsync(prompt);
            await command.RespondAsync(result);
        }
    }
}
