using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace api.archerharmony.com.Extensions;

public static class EnvironmentExtensions
{
    private const string DockerSecretPath = "/run/secrets/";
    
    public static string GetSecretOrEnvVar(this WebApplicationBuilder builder, string key)
    {
        if (Directory.Exists(DockerSecretPath))
        {
            return File.ReadAllText(Environment.GetEnvironmentVariable(key));
        }

        return builder.Configuration.GetValue<string>(key.Replace("__", ":"));
    }
}