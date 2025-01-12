using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace api.archerharmony.com.Extensions;

public static class EnvironmentExtensions
{
    public static string? GetSecretOrEnvVar(this WebApplicationBuilder builder, string key)
    {
        var envVariable = Environment.GetEnvironmentVariable(key);

        if (string.IsNullOrEmpty(envVariable))
        {
            return builder.Configuration.GetValue<string>(key.Replace("__", ":"));
        }
        
        if (File.Exists(envVariable))
        {
            return File.ReadAllText(envVariable);
        }
        
        return envVariable;
    }
}