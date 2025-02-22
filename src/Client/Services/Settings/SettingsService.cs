﻿namespace Baustellen.App.Client.Services.Settings;

internal class SettingsService : ISettingsService
{
    private const string IdUseMocks = "use_mocks";
    private readonly bool UseMocksDefault = true;

    public bool UseMocks
    {
        get => Preferences.Get(IdUseMocks, UseMocksDefault);
        set => Preferences.Set(IdUseMocks, value);
    }
}
