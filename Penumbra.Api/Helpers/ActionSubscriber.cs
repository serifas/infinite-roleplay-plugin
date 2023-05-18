using System;
using Dalamud.Logging;
using Dalamud.Plugin;
using Dalamud.Plugin.Ipc;

namespace Penumbra.Api.Helpers;

/// <summary>
/// Specialized subscriber only allowing to invoke actions.
/// </summary>
public readonly struct ActionSubscriber< T1 >
{
    private readonly ICallGateSubscriber< T1, object? >? _subscriber;

    /// <summary> Whether the subscriber could successfully be created. </summary>
    public bool Valid
        => _subscriber != null;

    public ActionSubscriber( DalamudPluginInterface pi, string label )
    {
        try
        {
            _subscriber = pi.GetIpcSubscriber< T1, object? >( label );
        }
        catch( Exception e )
        {
            PluginLog.Error( $"Error registering IPC Subscriber for {label}\n{e}" );
            _subscriber = null;
        }
    }

    /// <summary> Invoke the action. See the source of the subscriber for details.</summary>
    public void Invoke( T1 a )
        => _subscriber?.InvokeAction( a );
}

/// <inheritdoc cref="ActionSubscriber{T1}"/> 
public readonly struct ActionSubscriber< T1, T2 >
{
    private readonly ICallGateSubscriber< T1, T2, object? >? _subscriber;

    /// <inheritdoc cref="ActionSubscriber{T1}.Valid"/> 
    public bool Valid
        => _subscriber != null;

    public ActionSubscriber( DalamudPluginInterface pi, string label )
    {
        try
        {
            _subscriber = pi.GetIpcSubscriber< T1, T2, object? >( label );
        }
        catch( Exception e )
        {
            PluginLog.Error( $"Error registering IPC Subscriber for {label}\n{e}" );
            _subscriber = null;
        }
    }

    /// <inheritdoc cref="ActionSubscriber{T1}.Invoke"/> 
    public void Invoke( T1 a, T2 b )
        => _subscriber?.InvokeAction( a, b );
}