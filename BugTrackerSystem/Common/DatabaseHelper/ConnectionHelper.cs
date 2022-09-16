using Npgsql;

namespace BugTrackerAPI.Common.DatabaseHelper;

public static class ConnectionHelper
{
    public static string GetConnectionString(IConfiguration configuration)
    {
        var configurationUrl = configuration.GetConnectionString("DefaultConnection");
        var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
        
        return string.IsNullOrWhiteSpace(databaseUrl) ? configurationUrl : BuildConnectionString(databaseUrl);
    }
    
    private static string BuildConnectionString(string databaseUrl)
    {
        var databaseUri = new Uri(databaseUrl);
        var userInfo = databaseUri.UserInfo.Split(':');
        
        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = databaseUri.Host,
            Port = databaseUri.Port,
            Username = userInfo[0],
            Password = userInfo[1],
            Database = databaseUri.LocalPath.TrimStart('/'),
            SslMode = SslMode.Require,
            TrustServerCertificate = true
        };
        
        return builder.ToString();
    }
}