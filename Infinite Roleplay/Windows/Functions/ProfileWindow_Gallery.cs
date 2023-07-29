using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Interface.ImGuiFileDialog;
using FFXIVClientStructs.FFXIV.Common.Math;
using ImGuiNET;
using ImGuiScene;
using Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniteRoleplay.Windows.Functions
{
    internal class ProfileWindow_Gallery
    {
        public static SortedList<int, TextureWrap> galleryImages = new SortedList<int, TextureWrap>();
        public static SortedList<int, TextureWrap> galleryThumbs = new SortedList<int, TextureWrap>();
        public static bool addImageToGallery = false;
        public static int imageIndexVal = 0;
        public static void LoadGallery(Plugin plugin, FileDialogManager _fileDialogManager, int imageIndex, bool load, PlayerCharacter playerCharacter)
        {
            if(load == true)
            {

                for (int i = 0; i < imageIndex; i++)
                {
                    if (i % 3 == 0)
                    {

                    }
                    else
                    {
                        ImGui.SameLine();
                    }
                    if (ImGui.BeginChild("##GalleryImage" + i, new Vector2(160, 200), false))
                    {
                        galleryImages.Values[i] = plugin.PluginInterfacePub.UiBuilder.LoadImage(ProfileWindow.galleryImageBytes[i]);
                        galleryThumbs.Values[i] = plugin.PluginInterfacePub.UiBuilder.LoadImage(ProfileWindow.galleryThumbBytes[i]);
                        ImGui.Image(galleryThumbs[i].ImGuiHandle, new Vector2(galleryThumbs[i].Width, galleryThumbs[i].Height));
                        if (ImGui.IsItemHovered()) { ImGui.SetTooltip("Click to enlarge"); }
                        if (ImGui.IsItemClicked())
                        {
                            ImagePreview.width = galleryImages[i].Width;
                            ImagePreview.height = galleryImages[i].Height;
                            ImagePreview.PreviewImage = galleryImages[i];
                            plugin.loadPreview = true;
                        }
                        if (galleryImages[i].Width > 20)
                        {
                            if (ImGui.BeginChild("#GalleryButtons" + i, new Vector2(160, 45), false))
                            {
                                if (ImGui.Button("Upload##" + "gallery_add" + i))
                                {
                                    addImageToGallery = true;
                                    imageIndexVal = i;
                                }
                                ImGui.SameLine();
                                if (ImGui.Button("Remove##" + "gallery_remove" + i))
                                {
                                    DataSender.RemoveGalleryImage(playerCharacter.Name.ToString(), playerCharacter.HomeWorld.GameData.Name.ToString(), i);
                                }

                            }

                        }
                        ImGui.EndChild();

                    }
                    ImGui.EndChild();
                    // TextureWrap galleryImages = plugin.PluginInterfacePub.UiBuilder.LoadImage(GalleryImageURL);
                    //ImGui.Image(galleryImages.ImGuiHandle, new Vector2(300, 100));                        
                }
            if (ImGui.Button("Submit Gallery"))
            {
                DataSender.SendGalleryImage(playerCharacter.Name.ToString(), playerCharacter.HomeWorld.GameData.Name.ToString(), imageIndex, ProfileWindow.galleryImageBytes);

                plugin.DisconnectFromServer();
                plugin.RefreshConnection();
                plugin.loggedIn = false;
                LoginWindow.loginRequest = true;
            }

            }
            if(addImageToGallery == true)
            {
             //   Uploader.AddImage(plugin, _fileDialogManager, plugin.Configuration, false, null, imageIndexVal, ProfileWindow.galleryImageBytes, ProfileWindow.galleryThumbBytes);
            }
        }
        
}
}
        
    

