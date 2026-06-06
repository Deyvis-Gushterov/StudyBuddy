using StudyBuddyBot.Commands;
using StudyBuddyBot.Services;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;


var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddUserSecrets<Program>()
    .Build();

var client = new DiscordSocketClient(new DiscordSocketConfig
{
    LogLevel = LogSeverity.Info,
    GatewayIntents = GatewayIntents.None
});

// Logging
client.Log += log =>
{
    Console.WriteLine($"[{log.Severity}] {log.Message}");
    return Task.CompletedTask;
};

// Ready — runs once bot connects
client.Ready += async () =>
{
    Console.WriteLine("StudyBuddy Bot is online!");

    ulong guildId = 1454186415641460798;

    var guild = client.GetGuild(guildId);

    var studyTip = new SlashCommandBuilder()
        .WithName("studytip")
        .WithDescription("Get an AI-powered study tip");

    var quiz = new SlashCommandBuilder()
        .WithName("quiz")
        .WithDescription("Generate a quiz on any topic")
        .AddOption("topic", ApplicationCommandOptionType.String,
                   "The topic to quiz you on", isRequired: true);

    var summarize = new SlashCommandBuilder()
        .WithName("summarize")
        .WithDescription("Summarize any text using AI")
        .AddOption("text", ApplicationCommandOptionType.String,
                   "The text to summarize", isRequired: true);

    await guild.CreateApplicationCommandAsync(studyTip.Build());
    await guild.CreateApplicationCommandAsync(quiz.Build());
    await guild.CreateApplicationCommandAsync(summarize.Build());
};

// Handle slash commands
client.SlashCommandExecuted += async command =>
{
    Console.WriteLine($"Command received: {command.CommandName}");
    try
    {
        var groq = new GroqService(config);

        switch (command.CommandName)
        {
            case "studytip":
                await StudyTipCommand.HandleAsync(command, groq);
                break;
            case "quiz":
                await QuizCommand.HandleAsync(command, groq);
                break;
            case "summarize":
                await SummarizeCommand.HandleAsync(command, groq);
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
        await command.RespondAsync("Something went wrong!");
    }
};


await client.LoginAsync(TokenType.Bot, config["Discord:Token"]);
await client.StartAsync();

// Keep the bot running
await Task.Delay(-1);