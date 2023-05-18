using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Dalamud.Interface;
using Dalamud.Interface.ImGuiFileDialog;
using FFXIVClientStructs.FFXIV.Client.Graphics.Scene;
using FFXIVClientStructs.FFXIV.Client.System.Resource.Handle;
using ImGuiNET;
using Lumina.Data.Parsing;
using Lumina.Excel.GeneratedSheets;
using OtterGui;
using OtterGui.Raii;
using OtterGui.Widgets;
using InfiniteRoleplay.Api.Enums;
using InfiniteRoleplay.String;
using InfiniteRoleplay.UI.Classes;

namespace InfiniteRoleplay.UI;

public partial class ConfigWindow
{
    // Draw text given by a ByteString.
    internal static unsafe void Text( ByteString s )
        => ImGuiNative.igTextUnformatted( s.Path, s.Path + s.Length );

    // Draw text given by a byte pointer.
    private static unsafe void Text( byte* s, int length )
        => ImGuiNative.igTextUnformatted( s, s + length );

    // Draw the name of a resource file.
  

    // Draw a ByteString as a selectable.
    internal static unsafe bool Selectable( ByteString s, bool selected )
    {
        var tmp = ( byte )( selected ? 1 : 0 );
        return ImGuiNative.igSelectable_Bool( s.Path, tmp, ImGuiSelectableFlags.None, Vector2.Zero ) != 0;
    }

    // Apply Changed Item Counters to the Name if necessary.
    private static string ChangedItemName( string name, object? data )
        => data is int counter ? $"{counter} Files Manipulating {name}s" : name;

    // Draw a changed item, invoking the Api-Events for clicks and tooltips.
    // Also draw the item Id in grey if requested
   

    // A selectable that copies its text to clipboard on selection and provides a on-hover tooltip about that,
    // using an ByteString.
    private static unsafe void CopyOnClickSelectable( ByteString text )
    {
        if( ImGuiNative.igSelectable_Bool( text.Path, 0, ImGuiSelectableFlags.None, Vector2.Zero ) != 0 )
        {
            ImGuiNative.igSetClipboardText( text.Path );
        }

        if( ImGui.IsItemHovered() )
        {
            ImGui.SetTooltip( "Click to copy to clipboard." );
        }
    }

   
    

    
    // Draw a collection selector of a certain width for a certain type.
   
    // Set up the file selector with the right flags and custom side bar items.
    public static FileDialogManager SetupFileManager()
    {
        var fileManager = new FileDialogManager
        {
            AddedWindowFlags = ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoDocking,
        };

        if( Functions.GetDownloadsFolder( out var downloadsFolder ) )
        {
            fileManager.CustomSideBarItems.Add( ( "Downloads", downloadsFolder, FontAwesomeIcon.Download, -1 ) );
        }

        if( Functions.GetQuickAccessFolders( out var folders ) )
        {
            foreach( var ((name, path), idx) in folders.WithIndex() )
            {
                fileManager.CustomSideBarItems.Add( ( $"{name}##{idx}", path, FontAwesomeIcon.Folder, -1 ) );
            }
        }

        // Add Penumbra Root. This is not updated if the root changes right now.
        fileManager.CustomSideBarItems.Add( ( "Root Directory", ".", FontAwesomeIcon.Gamepad, 0 ) );

        // Remove Videos and Music.
        fileManager.CustomSideBarItems.Add( ( "Videos", string.Empty, 0, -1 ) );
        fileManager.CustomSideBarItems.Add( ( "Music", string.Empty, 0, -1 ) );

        return fileManager;
    }

}
