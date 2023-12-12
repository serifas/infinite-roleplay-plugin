using Dalamud.Interface.Internal;
using InfiniteRoleplay.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniteRoleplay.Defines
{
    internal class UIDefines
    {
        public static Plugin plugin;


        public static IDalamudTextureWrap AvatarHolder,
                                            //Icons
                                            lawfulGood,
                                            neutralGood,
                                            chaoticGood,
                                            lawfulNeutral,
                                            trueNeutral,
                                            chaoticNeutral,
                                            lawfulEvil,
                                            neutralEvil,
                                            chaoticEvil,

                                            //bars
                                            lawfulGoodBar,
                                            neutralGoodBar,
                                            chaoticGoodBar,
                                            lawfulNeutralBar,
                                            trueNeutralBar,
                                            chaoticNeutralBar,
                                            lawfulEvilBar,
                                            neutralEvilBar,
                                            chaoticEvilBar,
                                            pictureTabWrap;
        public static string pictureTab;
        public static string blank_holder;
        public static byte[] imgBytes;

        public static byte[] picTabBytes;
        public static System.Drawing.Image picTab;
        public static byte[] emptyByteImage;

        public static byte[] blankTabBytes;
        public static System.Drawing.Image blankTab;

        public static List<IDalamudTextureWrap> textureList = new List<IDalamudTextureWrap>();

        public static void LoadTextures()
        {
            AvatarHolder = plugin.PluginInterfacePub.UiBuilder.LoadImage(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/common/avatar_holder.png"));
            textureList.Add(AvatarHolder);
            //Icons
            lawfulGood = plugin.PluginInterfacePub.UiBuilder.LoadImage(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/alignments/lawful_good.png"));
            textureList.Add(lawfulGood);
            neutralGood = plugin.PluginInterfacePub.UiBuilder.LoadImage(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/alignments/neutral_good.png"));
            textureList.Add(neutralGood);
            chaoticGood = plugin.PluginInterfacePub.UiBuilder.LoadImage(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/alignments/chaotic_good.png"));
            textureList.Add(chaoticGood);
            lawfulNeutral = plugin.PluginInterfacePub.UiBuilder.LoadImage(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/alignments/lawful_neutral.png"));
            textureList.Add(lawfulNeutral);
            trueNeutral = plugin.PluginInterfacePub.UiBuilder.LoadImage(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/alignments/true_neutral.png"));
            textureList.Add(trueNeutral);
            chaoticNeutral = plugin.PluginInterfacePub.UiBuilder.LoadImage(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/alignments/chaotic_neutral.png"));
            textureList.Add(chaoticNeutral);
            lawfulEvil = plugin.PluginInterfacePub.UiBuilder.LoadImage(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/alignments/lawful_evil.png"));
            textureList.Add(lawfulEvil);
            neutralEvil = plugin.PluginInterfacePub.UiBuilder.LoadImage(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/alignments/neutral_evil.png"));
            textureList.Add(neutralEvil);
            chaoticEvil = plugin.PluginInterfacePub.UiBuilder.LoadImage(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/alignments/chaotic_evil.png"));
            

            
            lawfulGoodBar = plugin.PluginInterfacePub.UiBuilder.LoadImage(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/alignments/lawful_good_bar.png"));
            textureList.Add(lawfulGoodBar);
            neutralGoodBar = plugin.PluginInterfacePub.UiBuilder.LoadImage(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/alignments/neutral_good_bar.png"));
            textureList.Add(neutralGoodBar);
            chaoticGoodBar = plugin.PluginInterfacePub.UiBuilder.LoadImage(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/alignments/chaotic_good_bar.png"));
            textureList.Add(chaoticGoodBar);
            lawfulNeutralBar = plugin.PluginInterfacePub.UiBuilder.LoadImage(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/alignments/lawful_neutral_bar.png"));
            textureList.Add(lawfulNeutralBar);
            trueNeutralBar = plugin.PluginInterfacePub.UiBuilder.LoadImage(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/alignments/true_neutral_bar.png"));
            textureList.Add(trueNeutralBar);    
            chaoticNeutralBar = plugin.PluginInterfacePub.UiBuilder.LoadImage(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/alignments/chaotic_neutral_bar.png"));
            textureList.Add(chaoticNeutralBar);
            lawfulEvilBar = plugin.PluginInterfacePub.UiBuilder.LoadImage(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/alignments/lawful_evil_bar.png"));
            textureList.Add(lawfulEvilBar);
            neutralEvilBar = plugin.PluginInterfacePub.UiBuilder.LoadImage(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/alignments/neutral_evil_bar.png"));
            textureList.Add(neutralEvilBar);
            chaoticEvilBar = plugin.PluginInterfacePub.UiBuilder.LoadImage(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/alignments/chaotic_evil_bar.png"));
            textureList.Add(chaoticEvilBar);
            pictureTab = Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/common/picturetab.png");
            
            blank_holder = Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/common/blank.png");
            
            imgBytes = File.ReadAllBytes(pictureTab);



            picTabBytes = Imaging.ScaleImageBytes(imgBytes, 300, 300);

            pictureTabWrap = plugin.PluginInterfacePub.UiBuilder.LoadImage(picTabBytes);
            textureList.Add(pictureTabWrap);
            picTab = Imaging.byteArrayToImage(picTabBytes);

            emptyByteImage = File.ReadAllBytes(blank_holder);

            blankTabBytes = Imaging.ScaleImageBytes(emptyByteImage, 300, 300);

            blankTab = Imaging.byteArrayToImage(blankTabBytes);
        }

        public static void DisposeTextures()
        {
            foreach (IDalamudTextureWrap tex in textureList)
            {
                tex.Dispose();
            }
        }
    }
}
