namespace api.archerharmony.com.Configuration;

public record BotConfiguration
{
    public required string BotToken { get; init; }
    public string? Socks5Host { get; init; }
    public int Socks5Port { get; init; }
}