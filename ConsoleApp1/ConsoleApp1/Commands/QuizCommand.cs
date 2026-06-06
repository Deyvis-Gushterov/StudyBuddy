using StudyBuddyBot.Services;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudyBuddyBot.Commands
{
    internal class QuizCommand
    {
        public static async Task HandleAsync(SocketSlashCommand command, GroqService groq)
        {
            var topic = command.Data.Options.First().Value.ToString();

            var prompt = $"Generate a 5 question multiple choice quiz about {topic} Number each question.";

            var result = await groq.AskAsync(prompt);
            await command.RespondAsync(result);
        }
    }

}
