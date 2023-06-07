﻿using PhotinoNET;
using PhotinoNET.Server;
using Sentry;

namespace SpinShareClient;

using MessageParser;

internal static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        SentrySdk.Init(options =>
        {
            options.Dsn = "https://8ee3ec205d27494ebaff5ce378db752c@o1420803.ingest.sentry.io/4505318580879360";
            options.Debug = true;
            options.AutoSessionTracking = true;
            options.IsGlobalModeEnabled = true;
            options.EnableTracing = true;
        });
        
        PhotinoServer
            .CreateStaticFileServer(args, out string baseUrl)
            .RunAsync();

        string windowTitle = "SpinShare";
        
        Console.WriteLine("[MAIN] Creating Window");

        MessageHandler messageHandler = new MessageHandler();

        var window = new PhotinoWindow()
            .SetLogVerbosity(2)
            .SetTitle(windowTitle)
            .SetSize(1100, 750)
            .SetUseOsDefaultSize(false)
            .Center()
            .SetResizable(true)
            /* FIXME: https://github.com/tryphotino/photino.NET/issues/83#issuecomment-1554395461
            .RegisterSizeChangedHandler((sender, size) =>
            {
                var window = (PhotinoWindow?)sender;
                
                if (size.Width < 600)
                {
                    Console.WriteLine("Window Width Too Small");
                    window?.SetWidth(600);
                }
                if (size.Height < 400)
                {
                    Console.WriteLine("Window Height Too Small");
                    window?.SetHeight(400);
                }
            }) */
            .RegisterWebMessageReceivedHandler(messageHandler.RegisterWebMessageReceivedHandler);

        var initPage = "#/";

        SettingsManager settingsManager = SettingsManager.GetInstance();
        
        // Setup if there are no settings
        if (!SettingsManager.SettingsFileExists())
        {
            initPage = "#/setup/step-0";
        }
        else
        {
            if (!settingsManager.Exists("library.path") || !settingsManager.Exists("app.silentQueue") ||
                !settingsManager.Exists("app.language") || !settingsManager.Exists("app.setup.done"))
            {
                initPage = "#/setup/step-0";
            }
        
            // Checking for Update
            /*
            if (settingsManager.Exists("app.setup.done"))
            {
                string version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? string.Empty;
                // TODO: API Call for Latest Version Check
                initPage = "#/update";
            } */
        }

#if DEBUG
        window.SetDevToolsEnabled(true);
        window.Load(new Uri("http://localhost:5173/" + initPage));
#else
        window.Load($"{baseUrl}/index.html" + initPage);
#endif

        window.WaitForClose();
    }
}
