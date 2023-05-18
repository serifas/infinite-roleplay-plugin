using Dalamud.Game.ClientState.Objects.Types;
using Lumina.Data;
using System;
using System.Collections.Generic;
using Penumbra.Api.Enums;

namespace Penumbra.Api;

public interface IPenumbraApi : IPenumbraApiBase
{
    #region Plugin State

    /// <returns>The current penumbra root directory.</returns>
    public string GetModDirectory();

    /// <returns>The entire current penumbra configuration as a json encoded string.</returns>
    public string GetConfiguration();

    /// <summary>
    /// Fired whenever a mod directory change is finished.
    /// </summary>
    /// <returns>The full path of the mod directory and whether Penumbra treats it as valid.</returns>
    public event Action< string, bool >? ModDirectoryChanged;

    /// <returns>True if Penumbra is enabled, false otherwise.</returns>
    public bool GetEnabledState();

    /// <summary>
    /// Fired whenever the enabled state of Penumbra changes.
    /// </summary>
    /// <returns>True if the new state is enabled, false if the new state is disabled</returns>
    public event Action< bool >? EnabledChange;

    #endregion

    #region UI

    /// <summary>
    /// Triggered when the user hovers over a listed changed object in a mod tab.<para />
    /// Can be used to append tooltips.
    /// </summary>
    /// <returns><inheritdoc cref="ChangedItemHover"/></returns>
    public event ChangedItemHover? ChangedItemTooltip;

    /// <summary>
    /// Triggered before the content of a mod settings panel is drawn.
    /// </summary>
    /// <returns>The directory name of the currently selected mod.</returns>
    public event Action< string >? PreSettingsPanelDraw;

    /// <summary>
    /// Triggered after the content of a mod settings panel is drawn, but still in the child window.
    /// </summary>
    /// <returns>The directory name of the currently selected mod.</returns>
    public event Action< string >? PostSettingsPanelDraw;

    /// <summary>
    /// Triggered when the user clicks a listed changed object in a mod tab.
    /// </summary>
    /// <returns><inheritdoc cref="ChangedItemClick"/></returns>
    public event ChangedItemClick? ChangedItemClicked;

    #endregion

    #region Redrawing

    /// <summary>
    /// Queue redrawing of all actors of the given <paramref name="name"/> with the given RedrawType <paramref name="setting"/>.
    /// </summary>
    /// <param name="name" />
    /// <param name="setting" />
    public void RedrawObject( string name, RedrawType setting );

    /// <summary>
    /// Queue redrawing of the specific actor <paramref name="gameObject"/> with the given RedrawType <paramref name="setting"/>. Should only be used when the actor is sure to be valid.
    /// </summary>
    /// <param name="gameObject" />
    /// <param name="setting" />
    public void RedrawObject( GameObject gameObject, RedrawType setting );

    /// <summary>
    /// Queue redrawing of the actor with the given object <paramref name="tableIndex" />, if it exists, with the given RedrawType <paramref name="setting"/>.
    /// </summary>
    /// <param name="tableIndex" />
    /// <param name="setting" />
    public void RedrawObject( int tableIndex, RedrawType setting );

    /// <summary>
    /// Queue redrawing of all currently available actors with the given RedrawType <paramref name="setting"/>.
    /// </summary>
    /// <param name="setting" />
    public void RedrawAll( RedrawType setting );

    /// <summary>
    /// Triggered whenever a game object is redrawn via Penumbra.
    /// </summary>
    /// /<returns><inheritdoc cref="GameObjectRedrawnDelegate"/></returns>
    public event GameObjectRedrawnDelegate? GameObjectRedrawn;

    #endregion

    #region Game State

    /// <param name="drawObject"></param>
    /// <returns>The game object associated with the given <paramref name="drawObject">draw object</paramref> and the name of the collection associated with this game object.</returns>
    public (IntPtr, string) GetDrawObjectInfo( IntPtr drawObject );

    /// <summary>
    /// Obtain the parent game object index for an unnamed cutscene actor by its <paramref name="actorIdx">index</paramref>.
    /// </summary>
    /// <param name="actorIdx"></param>
    /// <returns>The parent game object index.</returns>
    public int GetCutsceneParentIndex( int actorIdx );

    /// <summary>
    /// Triggered when a character base is created and a corresponding gameObject could be found,
    /// before the Draw Object is actually created, so customize and equipdata can be manipulated beforehand.
    /// </summary>
    /// <returns><inheritdoc cref="CreatingCharacterBaseDelegate"/></returns>
    public event CreatingCharacterBaseDelegate? CreatingCharacterBase;

    /// <summary>
    /// Triggered after a character base was created if a corresponding gameObject could be found,
    /// so you can apply flag changes after finishing.
    /// </summary>
    /// <returns><inheritdoc cref="CreatedCharacterBaseDelegate"/></returns>
    public event CreatedCharacterBaseDelegate? CreatedCharacterBase;

    /// <summary>
    /// Triggered whenever a resource is redirected by Penumbra for a specific, identified game object.
    /// Does not trigger if the resource is not requested for a known game object.
    /// </summary>
    /// <returns><inheritdoc cref="GameObjectResourceResolvedDelegate"/></returns>
    public event GameObjectResourceResolvedDelegate? GameObjectResourceResolved;

    #endregion

    #region Resolving

    /// <summary>
    /// Resolve a given <paramref name="gamePath" /> via Penumbra using the Base collection.
    /// </summary>
    /// <returns>The resolved path, or the given path if Penumbra would not manipulate it.</returns>
    public string ResolveDefaultPath( string gamePath );

    /// <summary>
    /// Resolve a given <paramref name="gamePath" /> via Penumbra using the Interface collection.
    /// </summary>
    /// <returns>The resolved path, or the given path if Penumbra would not manipulate it.</returns>
    public string ResolveInterfacePath( string gamePath );

    /// <summary>
    /// Resolve a given <paramref name="gamePath" /> via Penumbra using the character collection
    /// for <paramref name="characterName" /> or the Base collection if none exists.
    /// </summary>
    /// <returns>The resolved path, or the given path if Penumbra would not manipulate it.</returns>
    public string ResolvePath( string gamePath, string characterName );

    /// <summary>
    /// Resolve a given <paramref name="gamePath" /> via Penumbra using collection applying to the <paramref name="gameObjectIdx"/> 
    /// given by its index in the game object table.
    /// </summary>
    /// <remarks>If the object does not exist in the table, the default collection is used.</remarks>
    /// <returns>The resolved path, or the given path if Penumbra would not manipulate it.</returns>
    public string ResolveGameObjectPath( string gamePath, int gameObjectIdx );

    /// <summary>
    /// Resolve a given <paramref name="gamePath" /> via Penumbra using the collection currently applying to the player character.
    /// </summary>
    /// <returns>The resolved path, or the given path if Penumbra would not manipulate it.</returns>
    public string ResolvePlayerPath( string gamePath );

    /// <summary>
    /// Reverse resolves a given local <paramref name="moddedPath" /> into its replacement in form of all applicable game paths
    /// for the character collection for <paramref name="characterName" />.
    /// </summary>
    /// <returns>A list of game paths resolving to the modded path.</returns>
    public string[] ReverseResolvePath( string moddedPath, string characterName );

    /// <summary>
    /// Reverse resolves a given local <paramref name="moddedPath" /> into its replacement in form of all applicable game paths
    /// for the collection applying to the <paramref name="gameObjectIdx"/>th game object in the game object table.
    /// </summary>
    /// <remarks>If the object does not exist in the table, the default collection is used.</remarks>
    /// <returns>A list of game paths resolving to the modded path.</returns>
    public string[] ReverseResolveGameObjectPath( string moddedPath, int gameObjectIdx );

    /// <summary>
    /// Reverse resolves a given local <paramref name="moddedPath" /> into its replacement in form of all applicable game paths
    /// for the collection currently applying to the player character.
    /// </summary>
    /// <returns>A list of game paths resolving to the modded path.</returns>
    public string[] ReverseResolvePlayerPath( string moddedPath );

    /// <summary>
    /// Try to load a given <paramref name="gamePath" /> with the resolved path from Penumbras Base collection.
    /// </summary>
    /// <returns>The file of type T if successful, null otherwise.</returns>
    public T? GetFile< T >( string gamePath ) where T : FileResource;

    /// <summary>
    /// Try to load a given <paramref name="gamePath" /> with the resolved path from Penumbra
    /// using the character collection for <paramref name="characterName" />.
    /// </summary>
    /// <returns>The file of type T if successful, null otherwise.</returns>
    public T? GetFile< T >( string gamePath, string characterName ) where T : FileResource;

    #endregion

    #region Collections

    /// <returns>A list of the names of all currently installed collections.</returns>
    public IList< string > GetCollections();

    /// <returns>The name of the currently selected collection.</returns>
    public string GetCurrentCollection();

    /// <returns>The name of the default collection.</returns>
    public string GetDefaultCollection();

    /// <returns>The name of the interface collection.</returns>
    public string GetInterfaceCollection();

    /// <returns>The name of the collection associated with <paramref name="characterName"/> and whether it exists as character collection.</returns>
    public (string, bool) GetCharacterCollection( string characterName );

    /// <returns>A dictionary of affected items in <paramref name="collectionName"/> via name and known objects or null.</returns>
    public IReadOnlyDictionary< string, object? > GetChangedItemsForCollection( string collectionName );

    #endregion

    #region Meta

    /// <returns>A base64 encoded, zipped json-string with a prepended version-byte of the current manipulations
    /// in the collection currently associated with the player.</returns>
    public string GetPlayerMetaManipulations();

    /// <returns>A base64 encoded, zipped json-string with a prepended version-byte of the current manipulations
    /// in the given collection associated with the character name, or the default collection.</returns>
    public string GetMetaManipulations( string characterName );

    /// <returns>A base64 encoded, zipped json-string with a prepended version-byte of the current manipulations
    /// in the given collection applying to the given game object or the default collection if it does not exist.</returns>
    public string GetGameObjectMetaManipulations( int gameObjectIdx );

    #endregion

    #region Mods

    /// <returns>A list of all installed mods. The first string is their directory name, the second string is their mod name.</returns>
    public IList< (string, string) > GetModList();

    /// <summary> Try to reload an existing mod given by its <paramref name="modDirectory" /> name or <paramref name="modName" />.</summary>
    /// <remarks>Reload is the same as if triggered by button press and might delete the mod if it is not valid anymore.</remarks>
    /// <returns>ModMissing if the mod can not be found or Success</returns>
    public PenumbraApiEc ReloadMod( string modDirectory, string modName );

    /// <summary> Try to add a new mod inside the mod root directory.</summary>
    /// <remarks>Note that success does only imply a successful call, not a successful mod load.</remarks>
    /// <param name="modDirectory">The name (not full name) of the mod directory.</param>
    /// <returns>FileMissing if <paramref name="modDirectory" /> does not exist, Success otherwise.</returns>
    public PenumbraApiEc AddMod( string modDirectory );

    /// <summary>Try to delete a mod  given by its <paramref name="modDirectory" /> name or <paramref name="modName" />.</summary>
    /// <remarks>Note that success does only imply a successful call, not successful deletion.</remarks>
    /// <returns>NothingDone if the mod can not be found, Success otherwise.</returns>
    public PenumbraApiEc DeleteMod( string modDirectory, string modName );

    /// <summary> Triggers whenever a mod is deleted. </summary>
    /// <returns>The base directory name of the deleted mod.</returns>
    public event Action< string >? ModDeleted;

    /// <summary> Triggers whenever a mod is deleted. </summary>
    /// <returns>The base directory name of the new mod.</returns>
    public event Action<string>? ModAdded;

    /// <summary> Triggers whenever a mods base name is changed from inside Penumbra. </summary>
    /// <returns>The previous base directory name of the mod and the new base directory name of the mod.</returns>
    public event Action<string, string>? ModMoved;

    /// <summary>
    /// Get the internal full filesystem path including search order for the specified mod
    /// given by its <paramref name="modDirectory" /> name or <paramref name="modName" />.
    /// </summary>
    /// <returns>On Success, the full path and a bool indicating whether this is default (false) or manually set (true).
    /// Otherwise returns ModMissing if the mod can not be found.</returns>
    public (PenumbraApiEc, string, bool) GetModPath( string modDirectory, string modName );

    /// <summary>
    /// Set the internal search order and filesystem path of the specified mod
    /// given by its <paramref name="modDirectory" /> name or <paramref name="modName" />
    /// to the <paramref name="newPath" />.
    /// </summary>
    /// <returns>InvalidArgument if <paramref name="newPath" /> is empty, ModMissing if the mod can not be found,
    /// PathRenameFailed if <paramref name="newPath"/> could not be set or Success.</returns>
    public PenumbraApiEc SetModPath( string modDirectory, string modName, string newPath );

    #endregion

    #region Mod Settings

    /// <summary>
    /// Obtain the potential settings of a mod given by its <paramref name="modDirectory" /> name or <paramref name="modName" />.
    /// </summary>
    /// <returns>A dictionary of group names to lists of option names and the group type. Null if the mod could not be found.</returns>
    public IDictionary< string, (IList< string >, GroupType) >? GetAvailableModSettings( string modDirectory, string modName );

    /// <summary>
    /// Obtain the enabled state, the priority, the settings of a mod given by its <paramref name="modDirectory" /> name or <paramref name="modName" /> in the specified collection.
    /// </summary>
    /// <param name="collectionName">Specify the collection.</param>
    /// <param name="modDirectory">Specify the mod via its directory name.</param>
    /// <param name="modName">Specify the mod via its (non-unique) display name.</param>
    /// <param name="allowInheritance">Whether the settings need to be from the given collection or can be inherited from any other by it.</param>
    /// <returns>ModMissing, CollectionMissing or Success. <para />
    /// On Success, a tuple of Enabled State, Priority, a dictionary of option group names and lists of enabled option names and a bool whether the settings are inherited or not.</returns>
    public (PenumbraApiEc, (bool, int, IDictionary< string, IList< string > >, bool)?) GetCurrentModSettings( string collectionName,
        string modDirectory, string modName, bool allowInheritance );

    /// <summary> Try to set the inheritance state of a mod in a collection. </summary>
    /// <returns>ModMissing, CollectionMissing, NothingChanged or Success.</returns>
    public PenumbraApiEc TryInheritMod( string collectionName, string modDirectory, string modName, bool inherit );

    /// <summary> Try to set the enabled state of a mod in a collection. </summary>
    /// <returns>ModMissing, CollectionMissing, NothingChanged or Success.</returns>
    public PenumbraApiEc TrySetMod( string collectionName, string modDirectory, string modName, bool enabled );

    /// <summary> Try to set the priority of a mod in a collection. </summary>
    /// <returns>ModMissing, CollectionMissing, NothingChanged or Success.</returns>
    public PenumbraApiEc TrySetModPriority( string collectionName, string modDirectory, string modName, int priority );

    /// <summary> Try to set a specific option group of a mod in the given collection to a specific value. </summary>
    /// <remarks>Removes inheritance. Single Selection groups should provide a single option, Multi Selection can provide multiple.
    /// If any setting can not be found, it will not change anything.</remarks>
    /// <returns>ModMissing, CollectionMissing, OptionGroupMissing, SettingMissing, NothingChanged or Success.</returns>
    public PenumbraApiEc TrySetModSetting( string collectionName, string modDirectory, string modName, string optionGroupName, string option );

    /// <inheritdoc cref="TrySetModSetting"/>
    public PenumbraApiEc TrySetModSettings( string collectionName, string modDirectory, string modName, string optionGroupName,
        IReadOnlyList< string > options );

    /// <summary> This event gets fired when any setting in any collection changes. </summary>
    /// <returns><inheritdoc cref="ModSettingChangedDelegate" /></returns>
    public event ModSettingChangedDelegate? ModSettingChanged;

    /// <summary>
    /// Copy all current settings for a mod to another mod.
    /// </summary>
    /// <param name="collectionName">Specify the collection to work in, leave empty or null to do it in all collections.</param>
    /// <param name="modDirectoryFrom">Specify the mod to take the settings from via its directory name.</param>
    /// <param name="modDirectoryTo">Specify the mod to put the settings on via its directory name. If the mod does not exist, it will be added as unused settings.</param>
    /// <returns>CollectionMissing if collectionName is not empty but does not exist or Success.</returns>
    /// <remarks>If the target mod exists, the settings will be fixed before being applied. If the source mod does not exist, it will use unused settings if available and remove existing settings otherwise.</remarks>
    public PenumbraApiEc CopyModSettings( string? collectionName, string modDirectoryFrom, string modDirectoryTo );

    #endregion

    #region Temporary

    /// <summary>
    /// Create a temporary collection without actual settings but with a cache and assign it to a specific character by name only.
    /// </summary>
    /// <remarks>This function is outdated, prefer to use <see cref="CreateNamedTemporaryCollection"/> and <see cref="AssignTemporaryCollection"/>.</remarks>
    /// <param name="tag">A custom tag for your collections.</param>
    /// <param name="character">The full, case-sensitive character name this collection should apply to.</param>
    /// <param name="forceOverwriteCharacter">Whether to overwrite an existing character collection.</param>
    /// <returns>Success, CharacterCollectionExists or NothingChanged and the name of the new temporary collection on success.</returns>
    public (PenumbraApiEc, string) CreateTemporaryCollection( string tag, string character, bool forceOverwriteCharacter );

    /// <summary>
    /// Create a temporary collection of the given <paramref name="name"/>.
    /// </summary>
    /// <param name="name">The intended name. It may not be empty or contain symbols invalid in a path used by XIV.</param>
    /// <returns>Success, InvalidArgument if name is not valid for a collection, or TemporaryCollectionExists.</returns>
    public PenumbraApiEc CreateNamedTemporaryCollection( string name );

    /// <summary>
    /// Assign an existing temporary collection to an actor that currently occupies a specific slot.
    /// </summary>
    /// <param name="collectionName">The chosen collection assigned to the actor.</param>
    /// <param name="actorIndex">The current object table index of the actor.</param>
    /// <param name="forceAssignment">Whether to assign even if the actor is already assigned either a temporary or a permanent collection.</param>
    /// <returns>Success, InvalidArgument if the actor can not be identified, CollectionMissing if the collection does not exist, CharacterCollectionExists if <paramref name="forceAssignment"/> is false and the actor is already assigned a collection. </returns>
    public PenumbraApiEc AssignTemporaryCollection( string collectionName, int actorIndex, bool forceAssignment );

    /// <summary>
    /// Remove the temporary collection assigned to characterName if it exists.
    /// </summary>
    /// <remarks>This function is outdated, prefer to use <see cref="RemoveTemporaryCollectionByName" />.</remarks>
    /// <returns>NothingChanged or Success.</returns>
    public PenumbraApiEc RemoveTemporaryCollection( string characterName );

    /// <summary>
    /// Remove the temporary collection of the given name.
    /// </summary>
    /// <param name="collectionName">The chosen temporary collection to remove.</param>
    /// <returns>NothingChanged or Success.</returns>
    public PenumbraApiEc RemoveTemporaryCollectionByName( string collectionName );

    /// <summary>
    /// Set a temporary mod with the given paths, manipulations and priority and the name tag to all regular and temporary collections.
    /// </summary>
    /// <param name="tag">Custom name for the temporary mod.</param>
    /// <param name="paths">List of redirections (can be swaps or redirections).</param>
    /// <param name="manipString">Zipped Base64 string of meta manipulations.</param>
    /// <param name="priority">Desired priority.</param>
    /// <returns>InvalidGamePath, InvalidManipulation or Success.</returns>
    public PenumbraApiEc AddTemporaryModAll( string tag, Dictionary< string, string > paths, string manipString, int priority );

    /// <summary>
    /// Set a temporary mod with the given paths, manipulations and priority and the name tag to a specific collection.
    /// </summary>
    /// <param name="tag">Custom name for the temporary mod.</param>
    /// <param name="collectionName">Name of the collection the mod should apply to. Can be a temporary collection name.</param>
    /// <param name="paths">List of redirections (can be swaps or redirections).</param>
    /// <param name="manipString">Zipped Base64 string of meta manipulations.</param>
    /// <param name="priority">Desired priority.</param>
    /// <returns>CollectionMissing, InvalidGamePath, InvalidManipulation or Success.</returns>
    public PenumbraApiEc AddTemporaryMod( string tag, string collectionName, Dictionary< string, string > paths, string manipString,
        int priority );

    /// <summary>
    /// Remove the temporary mod with the given tag and priority from the temporary mods applying to all collections, if it exists.
    /// </summary>
    /// <param name="tag">The tag to look for.</param>
    /// <param name="priority">The initially provided priority.</param>
    /// <returns>NothingDone or Success.</returns>
    public PenumbraApiEc RemoveTemporaryModAll( string tag, int priority );

    /// <summary>
    /// Remove the temporary mod with the given tag and priority from the temporary mods applying to a specific collection, if it exists.
    /// </summary>
    /// <param name="tag">The tag to look for.</param>
    /// <param name="collectionName">Name of the collection the mod should apply to. Can be a temporary collection name.</param>
    /// <param name="priority">The initially provided priority.</param>
    /// <returns>CollectionMissing, NothingDone or Success.</returns>
    public PenumbraApiEc RemoveTemporaryMod( string tag, string collectionName, int priority );

    #endregion
}