﻿using CommunityToolkit.Mvvm.ComponentModel;
using SyncClipboard.Core.Commons;
using SyncClipboard.Core.Models.UserConfigs;

namespace SyncClipboard.Core.ViewModels;

public partial class SyncSettingViewModel : ObservableObject
{
    #region server
    [ObservableProperty]
    private bool serverEnable;
    partial void OnServerEnableChanged(bool value) => ServerConfig = ServerConfig with { SwitchOn = value };

    [ObservableProperty]
    private ServerConfig serverConfig;
    partial void OnServerConfigChanged(ServerConfig value)
    {
        _userConfig.SetConfig(ConfigKey.Server, value);
    }

    #endregion

    #region client
    [ObservableProperty]
    private bool syncEnable;
    partial void OnSyncEnableChanged(bool value) => ClientConfig = ClientConfig with { SyncSwitchOn = value };

    [ObservableProperty]
    private bool useLocalServer;
    partial void OnUseLocalServerChanged(bool value) => ClientConfig = ClientConfig with { UseLocalServer = value };

    [ObservableProperty]
    private uint intervalTime;
    partial void OnIntervalTimeChanged(uint value) => ClientConfig = ClientConfig with { IntervalTime = value };

    [ObservableProperty]
    private uint timeOut;
    partial void OnTimeOutChanged(uint value) => ClientConfig = ClientConfig with { TimeOut = value };

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IntervalTime))]
    [NotifyPropertyChangedFor(nameof(SyncEnable))]
    [NotifyPropertyChangedFor(nameof(UseLocalServer))]
    [NotifyPropertyChangedFor(nameof(TimeOut))]
    private SyncConfig clientConfig;
    partial void OnClientConfigChanged(SyncConfig value)
    {
        IntervalTime = value.IntervalTime;
        SyncEnable = value.SyncSwitchOn;
        UseLocalServer = value.UseLocalServer;
        TimeOut = value.TimeOut;
        _userConfig.SetConfig(ConfigKey.Sync, value);
    }
    #endregion

    private readonly UserConfig2 _userConfig;

    public SyncSettingViewModel(UserConfig2 userConfig)
    {
        _userConfig = userConfig;
        _userConfig.ListenConfig<ServerConfig>(ConfigKey.Server, LoadSeverConfig);
        serverConfig = _userConfig.GetConfig<ServerConfig>(ConfigKey.Server) ?? new();
        serverEnable = serverConfig.SwitchOn;

        _userConfig.ListenConfig<SyncConfig>(ConfigKey.Sync, LoadClientConfig);
        ClientConfig = _userConfig.GetConfig<SyncConfig>(ConfigKey.Sync) ?? new();
    }

    private void LoadSeverConfig(object? config)
    {
        ServerConfig = config as ServerConfig ?? new();
        ServerEnable = ServerConfig.SwitchOn;
    }

    private void LoadClientConfig(object? config)
    {
        ClientConfig = config as SyncConfig ?? new();
    }

    public string? SetServerConfig(string portString, string username, string password)
    {
        if (!ushort.TryParse(portString, out var port))
        {
            return "端口号的范围是1~65535";
        }
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            return "用户名或密码不能为空";
        }

        ServerConfig = ServerConfig with { Password = password, Port = port, UserName = username };

        return null;
    }

    public string? SetClientConfig(string url, string username, string password)
    {
        ClientConfig = ClientConfig with { RemoteURL = url, UserName = username, Password = password };
        return null;
    }
}
