namespace APICatalogo.Logging;

public class CustomerLogger : ILogger
{
    readonly string _loggerName;
    readonly CustomLoggerProviderConfiguration _loggerConfiguration; 
    
    public CustomerLogger(string loggerName, CustomLoggerProviderConfiguration loggerConfiguration)
    {
        _loggerName = loggerName;
        _loggerConfiguration = loggerConfiguration;
    }
    
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel == _loggerConfiguration.LogLevel;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        string message = $"{logLevel.ToString()}: {eventId.Id} - {formatter(state, exception)}";

        WriteLogOnFile(message);
    }

    private void WriteLogOnFile(string message)
    {
        const string pathFile = @"E:\api_log.txt";

        using var writer = new StreamWriter(pathFile, true);
        try
        {
            writer.WriteLine(message);
            writer.Close();
        }
        catch (Exception)
        {
            throw;
        }
    }
}