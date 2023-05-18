using System;
using Penumbra.Communication;

namespace Penumbra.Services;

public class CommunicatorService : IDisposable
{
    /// <inheritdoc cref="Communication.CollectionChange"/>

    /// <inheritdoc cref="Communication.CreatingCharacterBase"/>
    public readonly CreatingCharacterBase CreatingCharacterBase = new();

    /// <inheritdoc cref="Communication.CreatedCharacterBase"/>
    public readonly CreatedCharacterBase CreatedCharacterBase = new();

    /// <inheritdoc cref="Communication.ModDataChanged"/>

    /// <inheritdoc cref="Communication.ModDiscoveryStarted"/>
    public readonly ModDiscoveryStarted ModDiscoveryStarted = new();

    /// <inheritdoc cref="Communication.ModDiscoveryFinished"/>
    public readonly ModDiscoveryFinished ModDiscoveryFinished = new();

    /// <inheritdoc cref="Communication.ModDirectoryChanged"/>
    public readonly ModDirectoryChanged ModDirectoryChanged = new();

    /// <inheritdoc cref="Communication.ModPathChanged"/>

    /// <inheritdoc cref="Communication.CollectionInheritanceChanged"/>

    /// <inheritdoc cref="Communication.EnabledChanged"/>
    public readonly EnabledChanged EnabledChanged = new();

    /// <inheritdoc cref="Communication.PreSettingsPanelDraw"/>
    public readonly PreSettingsPanelDraw PreSettingsPanelDraw = new();

    /// <inheritdoc cref="Communication.PostSettingsPanelDraw"/>
    public readonly PostSettingsPanelDraw PostSettingsPanelDraw = new();

    /// <inheritdoc cref="Communication.ChangedItemHover"/>
    public readonly ChangedItemHover ChangedItemHover = new();

    /// <inheritdoc cref="Communication.ChangedItemClick"/>
    public readonly ChangedItemClick ChangedItemClick = new();

    public void Dispose()
    {
        CreatingCharacterBase.Dispose();
        CreatedCharacterBase.Dispose();
        ModDiscoveryStarted.Dispose();
        ModDiscoveryFinished.Dispose();
        ModDirectoryChanged.Dispose();
        EnabledChanged.Dispose();
        PreSettingsPanelDraw.Dispose();
        PostSettingsPanelDraw.Dispose();
        ChangedItemHover.Dispose();
        ChangedItemClick.Dispose();
    }
}
