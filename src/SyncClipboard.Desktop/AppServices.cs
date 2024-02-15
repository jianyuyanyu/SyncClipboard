﻿using System;
using Microsoft.Extensions.DependencyInjection;
using SharpHook;
using SyncClipboard.Abstract;
using SyncClipboard.Abstract.Notification;
using SyncClipboard.Core;
using SyncClipboard.Core.Clipboard;
using SyncClipboard.Core.Interfaces;
using SyncClipboard.Desktop.ClipboardAva;
using SyncClipboard.Desktop.Utilities;
using SyncClipboard.Desktop.Views;

namespace SyncClipboard.Desktop;

public class AppServices
{
    public static void ConfigDesktopCommonService(IServiceCollection services)
    {
        ProgramWorkflow.ConfigCommonService(services);
        ProgramWorkflow.ConfigurateViewModels(services);
        ProgramWorkflow.ConfigurateUserService(services);

        services.AddTransient<IAppConfig, AppConfig>();

        services.AddSingleton<IContextMenu, TrayIconContextMenu>();
        services.AddSingleton<ITrayIcon, TrayIconImpl>();

        services.AddSingleton<ClipboardFactory>();
        services.AddSingleton<IClipboardFactory>(sp => sp.GetRequiredService<ClipboardFactory>());
        services.AddSingleton<IProfileDtoHelper>(sp => sp.GetRequiredService<ClipboardFactory>());
        services.AddSingleton<IClipboardChangingListener, ClipboardListener>();
        services.AddTransient<IClipboardSetter<TextProfile>, TextClipboardSetter>();
        services.AddTransient<IClipboardSetter<FileProfile>, FileClipboardSetter>();
        services.AddTransient<IClipboardSetter<ImageProfile>, ImageClipboardSetter>();

        services.AddSingleton<INativeHotkeyRegistry, SharpHookHotkeyRegistry>();
        services.AddSingleton<IGlobalHook>((sp) => new SimpleGlobalHook(true));

        services.AddTransient<IFontManager, FontManager>();

        if (!OperatingSystem.IsMacOS() && !OperatingSystem.IsLinux())
        {
            services.AddSingleton<INotification, Notification>();
        }

        if (!OperatingSystem.IsMacOS())
        {
            services.AddSingleton<IMainWindow, MainWindow>();
        }
    }

    public static ServiceCollection ConfigureServices()
    {
        var services = new ServiceCollection();

        ConfigDesktopCommonService(services);

        return services;
    }
}
