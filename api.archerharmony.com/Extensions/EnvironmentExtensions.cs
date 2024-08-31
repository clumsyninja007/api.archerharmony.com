using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace api.archerharmony.com.Extensions;

public static class EnvironmentExtensions
{
    private const string DockerSecretPath = "/run/secrets/";
    
    public static string? GetSecretOrEnvVar(this WebApplicationBuilder builder, string key)
    {
        if (!Directory.Exists(DockerSecretPath))
        {
            return builder.Configuration.GetValue<string>(key.Replace("__", ":"));
        }
        
        var envVariable = Environment.GetEnvironmentVariable(key);

        return string.IsNullOrEmpty(envVariable) 
            ? null 
            : File.ReadAllText(envVariable);
    }
}