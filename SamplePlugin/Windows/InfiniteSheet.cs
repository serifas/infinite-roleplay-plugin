using Dalamud.Interface.ImGuiFileDialog;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using ImGuiScene;
using System;
using System.IO;
using System.Numerics;
using ImGuiFileDialog;
using System.Collections.Generic;
using Dalamud.Game.ClientState.Objects.Types;
using Windows.UI.Xaml.Controls;
using Dalamud.Interface;
using Windows.ApplicationModel.UserDataTasks;
using Windows.Gaming.Input;
using Dalamud.Plugin;
using Windows.UI;
using OtterGui.Raii;
using OtterGui;
using Dalamud.Interface.GameFonts;
using Dalamud.Interface.Style;
using OtterGui.Widgets;
using Windows.Media.Devices;
using InfiniteRoleplay.UI.Classes;
using Dalamud.Interface.Colors;
using System.Linq;
using Dalamud;
using Lumina.Excel.GeneratedSheets;
using static Uno.UI.FeatureConfiguration;
using Dalamud.IoC;
using Windows.UI.Xaml.Shapes;
using Path = System.IO.Path;
using static System.Net.Mime.MediaTypeNames;
using Lumina.Data.Files;
using FileDialog = ImGuiFileDialog.FileDialog;
using System.Runtime.CompilerServices;
using FFXIVClientStructs.FFXIV.Client.Graphics.Render;
using FileDialogManager = Dalamud.Interface.ImGuiFileDialog.FileDialogManager;
using InfiniteRoleplay;
using System.Threading.Tasks;
using System.Diagnostics;
using OtterGui.Filesystem;
using Windows.UI.Xaml;
using UpdateTest;

namespace InfiniteRP.Windows;

public class InfiniteSheet : Dalamud.Interface.Windowing.Window, IDisposable
{
    private TextureWrap HealthImage;
    private TextureWrap HealthPlus;
    private TextureWrap HealthMinus;
    private TextureWrap EminencePlus;
    private TextureWrap EminenceMinus;
    private TextureWrap HardinessPlus;
    private TextureWrap HardinessMinus;
    private float HealthWidth;
    private float IntelligenceWidth;
    private float NimblenessWidth;
    private float SensesWidth;
    private float StrengthWidth;
    private float HardinessWidth;
    private float EminenceWidth;
    private float NgWidth;
    private TextureWrap IntelligencePlus;
    private TextureWrap IntelligenceMinus;
    private TextureWrap NimblenessPlus;
    private TextureWrap NimblenessMinus;
    private TextureWrap SensesPlus;
    private TextureWrap SensesMinus;
    private TextureWrap StrengthPlus;
    private TextureWrap StrengthMinus;
    private TextureWrap vitar;
    public string message;
    private bool eminenceUpdated, hardinessUpdated, intelligenceUpdated, nimblenessUpdated, sensesUpdated, strengthUpdated;
    private TextureWrap EminenceImage;
    private TextureWrap HardinessImage;
    private TextureWrap IntelligenceImage;
    private TextureWrap NimblenessImage;
    private TextureWrap SensesImage;
    private TextureWrap StrengthImage;
    private TextureWrap HealthBar;
    private TextureWrap EminenceBar;
    private TextureWrap HardinessBar;
    private TextureWrap IntelligenceBar;
    private TextureWrap NimblenessBar;
    private TextureWrap SensesBar;
    private TextureWrap StrengthBar;
    private TextureWrap avatar;
    private TextureWrap ngImage;
    private TextureWrap avatarImg;
    private TextureWrap penIcon;
    private Plugin Plugin;
    private DalamudPluginInterface pg;
    private int health = 5;
    private int eminence;
    private int hardiness;
    private int intelligence;
    private int nimbleness;
    private int senses;
    private int strength;

    private string profileRace, profileAge, profileHeight, profileWeight, profileAbility1, profileAbility2, profileAbility3, profileAbility1Desc, profileAbility2Desc, profileAbility3Desc;
    private int profileHealth, profileHealthWidth, profileEminence, profileEminenceWidth, profileHardiness, profileHardinessWidth, profileIntelligence, profileIntelligenceWidth, profileNimbleness, profileNimblenessWith, profileSenses, profileSensesWidth, profileStrength, profileStrengthWidth;
    private int startHP = 5;
    private bool connectionToggled;
    private string CharacterName = string.Empty;
    private string PlayerName = string.Empty;
    private string AvatarPath = string.Empty;
    private string Race = string.Empty;
    private string Age = string.Empty;
    private string Height = string.Empty;
    private string Weight = string.Empty;
    private string Ability1Name = string.Empty;
    private string Ability2Name = string.Empty;
    private string Ability3Name = string.Empty;
    private string Ability1Description = string.Empty;
    private string Ability2Description = string.Empty;
    private string Ability3Description = string.Empty;
    private int MaxStats = 12;
    private int MaxReductionPerStat = -2;
    private bool creation = true;
    private int key = 0;
    private int MaxReduction = -2;
    private int MaxAllocation = 4;
    private int reducedCount;
    private int abilityCount;
    private int VerificationStatus;
    private int statCount;
    private int availabelPoints;
    private byte[] avatarBytes;
    private string characterName;
    private int characterHealth;
    private int characterEminence;
    private int characterHardiness;
    private float _modAuthorWidth;
    private float _modVersionWidth;
    private float _modWebsiteButtonWidth;
    private float _secondRowWidth;
    private GameFontHandle _nameFont;
    private GameFontHandle _infoFont;
    private string eminenceTooltip, hardinessTooltip, intellectTooltip, nimblenessTooltip, sensesTooltip, strengthTootlip;
    private int sheetID;
    private byte[] avatarBytesData;
    public Dictionary<int, string> AccountCharacters = new Dictionary<int, string>();
    public Dictionary<int, int> SheetVerificationStatuses = new Dictionary<int, int>();
    private int availableReductions;
    public InfiniteSheet(Plugin plugin, TextureWrap emnPlus, TextureWrap emnMinus,
                                        TextureWrap hrdPlus, TextureWrap hrdMinus,
                                        TextureWrap intPlus, TextureWrap intMinus,
                                        TextureWrap nimPlus, TextureWrap nimMinus,
                                        TextureWrap senPlus, TextureWrap senMinus,
                                        TextureWrap strPlus, TextureWrap strMinus,
                                        TextureWrap hpImage, TextureWrap emnImage, TextureWrap hrdImage, TextureWrap intImage, TextureWrap nimImage, TextureWrap senImage, TextureWrap strImage,
                                        TextureWrap hpBar, TextureWrap emnBar, TextureWrap hrdBar, TextureWrap intBar, TextureWrap nimBar, TextureWrap senBar, TextureWrap strBar,
                                        TextureWrap ngBar, TextureWrap avatarHolder) : base(
        "CHARACTER SHEET", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(750, 950),
            MaximumSize = new Vector2(750, 950)
        };

        this.pg = plugin.PluginInterfacePub;
        this._nameFont = pg.UiBuilder.GetGameFontHandle(new GameFontStyle(GameFontFamilyAndSize.Jupiter23));
        this._infoFont = pg.UiBuilder.GetGameFontHandle(new GameFontStyle(GameFontFamilyAndSize.Jupiter16));



        this.HealthImage = hpImage; this.HealthBar = hpBar;
        this.EminenceImage = emnImage; this.EminenceBar = emnBar; this.EminencePlus = emnPlus; this.EminenceMinus = emnMinus;
        this.HardinessImage = hrdImage; this.HardinessBar = hrdBar; this.HardinessPlus = hrdPlus; this.HardinessMinus = hrdMinus;
        this.IntelligenceImage = intImage; this.IntelligenceBar = intBar; this.IntelligencePlus = intPlus; this.IntelligenceMinus = intMinus;
        this.NimblenessImage = nimImage; this.NimblenessBar = nimBar; this.NimblenessPlus = nimPlus; this.NimblenessMinus = nimMinus;
        this.SensesImage = senImage; this.SensesBar = senBar; this.SensesPlus = senPlus; this.SensesMinus = senMinus;
        this.StrengthImage = strImage; this.StrengthBar = strBar; this.StrengthPlus = strPlus; this.StrengthMinus = strMinus;
        this.avatarImg = avatarHolder;
        this.ngImage = ngBar;
        this.Plugin = plugin;
        this.eminenceTooltip = "Eminence:\n" +
                                        "Measures a character’s force of personality, persuasiveness, personal magnetism, leadership capabilities, and attractiveness. \n" +
                                        "A check might arise when you try to influence or entertain others, when you try\n" +
                                        "to make an impression or tell a convincing lie, or when you are navigating a tricky social situation.";
        this.hardinessTooltip = "Hardiness: \n" +
                                    "The strength of your body. Increases your Health points.\n" +
                                    "If you can take a hit or not. Magical or physical. If you can drink straight up poison and be fine.\n" +
                                    "Also counts for alcohol. Determines your overall wellness. \n" +
                                    "It is a 1:1 bonus (a +1 in the stat gives you a +1 to HEALTH). For the record, all members start at 5 HP.";

        this.intellectTooltip = "Intelligence:\n" +
                                    "Indicates your skill, prowess, and knowledge with occult/magic/aetherial related business. \n" +
                                    "This can also be equated to how inclined someone is to “book smarts”. \n" +
                                    "This is also a DAMAGE STAT, which covers magic-based combat styles where spells are cast or aether is manipulated in some capacity.";

        this.nimblenessTooltip = "Nimbleness:\n" +
                        "How fast you run, how light you are on your feet. If you have the capacity to actually be stealthy.\n" +
                        "If you are relatively balanced and acrobatic. This is also a DAMAGE STAT, which covers some physical\n" +
                        "based combat styles (e.g. rapier, daggers) and ranged non-magical based combat styles (e.g. guns, bows).";

        this.sensesTooltip = "Senses:\n" +
                        "Covers the senses. How aware you are overall and of your surroundings. \n" +
                        "Determines your perception, your ability to not be taken by surprise.\n" +
                        "Includes improvised intelligence / street smarts. Also gives bonuses to defending rolls.";

        this.strengthTootlip = "Strength:\n" +
                        "A good indicator of your straight up brute strength!\n" +
                        "Raw exertion of physical force, your bodily power, and your athletic capabilities.\n" +
                        "This is also a DAMAGE STAT, which covers most physical based combat styles \n" +
                        "such as but not limited to: close quarters combat, wielding of martial weapons that require\n" +
                        "physical exertion of force through slashing, stabbing, or blunt force.";

        var penImage = Path.Combine(pg.AssemblyLocation.Directory?.FullName!, "editbtn.png");
        this.penIcon = pg.UiBuilder.LoadImage(penImage);

    }









    public void Dispose()
    {
        this.HealthImage.Dispose();
        this.EminenceImage.Dispose();
        this.HardinessImage.Dispose();
        this.IntelligenceImage.Dispose();
        this.NimblenessImage.Dispose();
        this.SensesImage.Dispose();
        this.StrengthImage.Dispose();
        this.HealthBar.Dispose();
        this.EminenceBar.Dispose();
        this.HardinessBar.Dispose();
        this.IntelligenceBar.Dispose();
        this.NimblenessBar.Dispose();
        this.SensesBar.Dispose();
        this.StrengthBar.Dispose();
        this.EminencePlus.Dispose();
        this.EminenceMinus.Dispose();
        this.HardinessPlus.Dispose();
        this.HardinessMinus.Dispose();
        this.IntelligencePlus.Dispose();
        this.IntelligenceMinus.Dispose();
        this.NimblenessPlus.Dispose();
        this.NimblenessMinus.Dispose();
        this.SensesPlus.Dispose();
        this.SensesMinus.Dispose();
        this.StrengthPlus.Dispose();
        this.StrengthMinus.Dispose();
    }



    public override void Draw()
    {
        Vector4 color = new Vector4(0, 0, 0, 0);
        string connectionLabel = "Disconnected";
        
        if (ImGui.BeginChild("CONNECTION", new Vector2(100, 45), false))
        {
           
            ImGui.TextColored(color, connectionLabel);

            if (ImGui.Button("Show Settings"))
            {
                this.Plugin.DrawConfigUI();
            }

        }
        ImGui.EndChild();

        if (ImGui.BeginChild("CHARACTER SHEET LIST", new Vector2(200, 850), true))
        {
            if (ImGui.Button("Add Character", new Vector2(100, 20)))
            {
                creation = true;
            }
            //CHARACTER SELECTION
            if (AccountCharacters.Count > 0)
            {
                foreach (string character in AccountCharacters.Values)
                {
                    string[] characterSplit = character.Split(',');

                    this.avatarBytesData = new byte[] { };
                    int ID = int.Parse(characterSplit[0]);
                    this.sheetID = ID;
                    //senses + "," + strength + "," + hardiness + "," + intelligence + "," + nimbleness + "," + eminence + "," + ability_1 + "," + ability_2 + "," + ability_3 + "," + ability_1_description + "," + ability_2_description + "," + ability_3_description;
                    string chrName = characterSplit[2];
                    string chrRace = characterSplit[3];
                    string chrAge = characterSplit[4];
                    string chrHeight = characterSplit[5];
                    string chrWeight = characterSplit[6];
                    int chrHealth = int.Parse(characterSplit[7]);
                    int chrSenses = int.Parse(characterSplit[8]);
                    int chrStrength = int.Parse(characterSplit[9]);
                    int chrHardiness = int.Parse(characterSplit[10]);
                    int chrIntelligence = int.Parse(characterSplit[11]);
                    int chrNimbleness = int.Parse(characterSplit[12]);
                    int chrEminence = int.Parse(characterSplit[13]);
                    string chrAbil1Title = characterSplit[14];
                    string chrAbil2Title = characterSplit[15];
                    string chrAbil3Title = characterSplit[16];
                    string chrAbil1Desc = characterSplit[17];
                    string chrAbil2Desc = characterSplit[18];
                    string chrAbil3Desc = characterSplit[19];
                    if (ImGui.Selectable(chrName))
                    {
                        creation = false;
                        this.characterName = chrName;
                        this.profileRace = chrRace;
                        this.profileAge = chrAge;
                        this.profileHeight = chrHeight;
                        this.profileWeight = chrWeight;
                        this.profileHealth = chrHealth;
                        this.profileSenses = chrSenses;
                        this.profileStrength = chrStrength;
                        this.profileHardiness = chrHardiness;
                        this.profileIntelligence = chrIntelligence;
                        this.profileNimbleness = chrNimbleness;
                        this.profileEminence = chrEminence;
                        this.profileAbility1 = chrAbil1Title;
                        this.profileAbility2 = chrAbil2Title;
                        this.profileAbility3 = chrAbil3Title;
                        this.profileAbility1Desc = chrAbil1Desc;
                        this.profileAbility2Desc = chrAbil2Desc;
                        this.profileAbility3Desc = chrAbil3Desc;
                        this.VerificationStatus = SheetVerificationStatuses[ID];

                        if (DataReceiver.characterAvatars.ContainsKey(ID))
                        {
                            byte[] avData = DataReceiver.characterAvatars[ID];
                            this.vitar = pg.UiBuilder.LoadImage(avData);
                        }

                    }
                }
            }
        }
        ImGui.EndChild();

        ImGui.SameLine();
        if (creation == true)
        {
            if (ImGui.BeginChild("CHARACTER SHEET CREATION", new Vector2(500, 850), true))
            {
                ImGui.SameLine();

                ImGui.Spacing();
                ImGui.InputTextWithHint("##playername", $"Players Name (Who we know you as)", ref PlayerName, 100);
                ImGui.InputTextWithHint("##charactername", $"Your Characters Name", ref CharacterName, 100);

                //ImGui.Image(this.avatar.ImGuiHandle, new Vector2(100, 100));

                
                
                ImGui.Image(this.avatarImg.ImGuiHandle, new Vector2(65, 65));
                if (ImGui.Button("Add Avatar"))
                {
                    var avatarImage = Path.GetFullPath(AvatarPath);
                    
                    this.avatarImg = this.Plugin.PluginInterfacePub.UiBuilder.LoadImage(avatarImage);
                    this.avatar = avatarImg;
                    this.avatarBytes = File.ReadAllBytes(AvatarPath);

                   
                }

                ImGui.SameLine();
                ImGui.InputTextWithHint("##avatar", $"Your Avatar Path", ref AvatarPath, 100);

                ImGui.InputTextWithHint("##race", $"Race", ref Race, 100);
                ImGui.InputTextWithHint("##age", $"Age", ref Age, 100);
                ImGui.InputTextWithHint("##height", $"Height", ref Height, 100);
                ImGui.InputTextWithHint("##weight", $"Weight", ref Weight, 100);


                this.availabelPoints = MaxStats - statCount - abilityCount + reducedCount;
                this.availableReductions = MaxReduction - reducedCount;

                ImGui.TextColored(new Vector4(1, 1, 0, 1), "##STATS:");
                ImGui.SameLine();
                ImGui.Text(this.availabelPoints + " Stat Points Available:");

                //HEALTH
                ImGui.Image(this.HealthImage.ImGuiHandle, new Vector2(20, 20));
                if (ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip("Health:\n" +
                                    "Just your overall hit points. (Modified by Hardiness)");
                }
                ImGui.SameLine();
                ImGui.Image(this.HealthBar.ImGuiHandle, new Vector2(HealthWidth * 30, 20));

                ImGui.SameLine();
                ImGui.Text(health.ToString());

                //EMINENCE
                ImGui.Image(this.EminenceImage.ImGuiHandle, new Vector2(20, 20));
                if (ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip(eminenceTooltip);
                }
                ImGui.SameLine();

                if (ImGui.ImageButton(this.EminencePlus.ImGuiHandle, new Vector2(20, 20)))
                {

                    ModStat("eminence", true);
                }

                ImGui.SameLine();
                if (ImGui.ImageButton(this.EminenceMinus.ImGuiHandle, new Vector2(20, 20)))
                {
                    ModStat("eminence", false);
                }
                ImGui.SameLine();


                if (eminence > 0) { ImGui.Image(this.EminenceBar.ImGuiHandle, new Vector2(EminenceWidth * 30, 20)); }
                if (eminence < 0) { ImGui.Image(this.ngImage.ImGuiHandle, new Vector2(Math.Abs(EminenceWidth) * 30, 20)); }
                ImGui.SameLine();
                ImGui.Text(eminence.ToString());

                //HARDINESS

                ImGui.Image(this.HardinessImage.ImGuiHandle, new Vector2(20, 20));
                if (ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip(hardinessTooltip);
                }
                ImGui.SameLine();
                if (ImGui.ImageButton(this.HardinessPlus.ImGuiHandle, new Vector2(20, 20)))
                {
                    ModStat("hardiness", true);
                }
                ImGui.SameLine();
                if (ImGui.ImageButton(this.HardinessMinus.ImGuiHandle, new Vector2(20, 20)))
                {
                    ModStat("hardiness", false);
                }
                ImGui.SameLine();

                if (hardiness > 0) { ImGui.Image(this.HardinessBar.ImGuiHandle, new Vector2(HardinessWidth * 30, 20)); }
                if (hardiness < 0) { ImGui.Image(this.ngImage.ImGuiHandle, new Vector2(Math.Abs(HardinessWidth) * 30, 20)); }

                ImGui.SameLine();
                ImGui.Text(hardiness.ToString());
                //INTELLIGENCE

                ImGui.Image(this.IntelligenceImage.ImGuiHandle, new Vector2(20, 20));
                if (ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip(intellectTooltip);
                }
                ImGui.SameLine();
                if (ImGui.ImageButton(this.IntelligencePlus.ImGuiHandle, new Vector2(20, 20)))
                {
                    ModStat("intelligence", true);
                }
                ImGui.SameLine();
                if (ImGui.ImageButton(this.IntelligenceMinus.ImGuiHandle, new Vector2(20, 20)))
                {
                    ModStat("intelligence", false);
                }
                ImGui.SameLine();
                if (intelligence > 0) { ImGui.Image(this.IntelligenceBar.ImGuiHandle, new Vector2(IntelligenceWidth * 30, 20)); }
                if (intelligence < 0) { ImGui.Image(this.ngImage.ImGuiHandle, new Vector2(Math.Abs(IntelligenceWidth) * 30, 20)); }

                ImGui.SameLine();
                ImGui.Text(intelligence.ToString());
                //NIMBLENESS

                ImGui.Image(this.NimblenessImage.ImGuiHandle, new Vector2(20, 20));
                if (ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip(nimblenessTooltip);
                }
                ImGui.SameLine();
                if (ImGui.ImageButton(this.NimblenessPlus.ImGuiHandle, new Vector2(20, 20)))
                {
                    ModStat("nimbleness", true);
                }
                ImGui.SameLine();
                if (ImGui.ImageButton(this.NimblenessMinus.ImGuiHandle, new Vector2(20, 20)))
                {
                    ModStat("nimbleness", false);
                }
                ImGui.SameLine();

                if (nimbleness > 0) { ImGui.Image(this.NimblenessBar.ImGuiHandle, new Vector2(NimblenessWidth * 30, 20)); }
                if (nimbleness < 0) { ImGui.Image(this.ngImage.ImGuiHandle, new Vector2(Math.Abs(NimblenessWidth) * 30, 20)); }

                ImGui.SameLine();
                ImGui.Text(nimbleness.ToString());
                //SENSES

                ImGui.Image(this.SensesImage.ImGuiHandle, new Vector2(20, 20));
                if (ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip(sensesTooltip);
                }
                ImGui.SameLine();

                if (ImGui.ImageButton(this.SensesPlus.ImGuiHandle, new Vector2(20, 20)))
                {
                    ModStat("senses", true);
                }
                ImGui.SameLine();

                if (ImGui.ImageButton(this.SensesMinus.ImGuiHandle, new Vector2(20, 20)))
                {
                    ModStat("senses", false);
                }
                ImGui.SameLine();

                if (senses > 0) { ImGui.Image(this.SensesBar.ImGuiHandle, new Vector2(SensesWidth * 30, 20)); }
                if (senses < 0) { ImGui.Image(this.ngImage.ImGuiHandle, new Vector2(Math.Abs(SensesWidth) * 30, 20)); }

                ImGui.SameLine();
                ImGui.Text(senses.ToString());
                //STRENGTH

                ImGui.Image(this.StrengthImage.ImGuiHandle, new Vector2(20, 20));
                if (ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip(strengthTootlip);
                }
                ImGui.SameLine();

                if (ImGui.ImageButton(this.StrengthPlus.ImGuiHandle, new Vector2(20, 20)))
                {
                    ModStat("strength", true);
                }
                ImGui.SameLine();

                if (ImGui.ImageButton(this.StrengthMinus.ImGuiHandle, new Vector2(20, 20)))
                {
                    ModStat("strength", false);
                }
                ImGui.SameLine();
                if (strength > 0) { ImGui.Image(this.StrengthBar.ImGuiHandle, new Vector2(StrengthWidth * 30, 20)); }
                if (strength < 0) { ImGui.Image(this.ngImage.ImGuiHandle, new Vector2(Math.Abs(StrengthWidth) * 30, 20)); }

                ImGui.SameLine();
                ImGui.Text(strength.ToString());


                ImGui.Text("Abilities:" + abilityCount.ToString());
                ImGui.SameLine();


                if (ImGui.Button("+", new Vector2(20, 20)))
                {
                    if (availabelPoints >= 1)
                    {
                        abilityCount++;

                        if (abilityCount > 3)
                        {
                            abilityCount = 3;
                        }
                    }
                }
                ImGui.SameLine();
                if (ImGui.Button("-", new Vector2(20, 20)))
                {
                    abilityCount--;
                    if (abilityCount < 0)
                    {
                        abilityCount = 0;
                    }
                }

                if (abilityCount == 1)
                {
                    ImGui.InputTextWithHint("##ability1Name", $"Ability Name", ref Ability1Name, 100);
                    ImGui.InputTextMultiline("##ability1Description", ref Ability1Description, 3000, new Vector2(350, 50));
                }
                if (abilityCount == 2)
                {
                    ImGui.InputTextWithHint("##ability1Name", $"Ability Name", ref Ability1Name, 100);
                    ImGui.InputTextMultiline("##ability1Description", ref Ability1Description, 3000, new Vector2(350, 50));
                    ImGui.InputTextWithHint("##ability2Name", $"Ability Name", ref Ability2Name, 100);
                    ImGui.InputTextMultiline("##ability2Description", ref Ability2Description, 3000, new Vector2(350, 50));
                }
                if (abilityCount == 3)
                {
                    ImGui.InputTextWithHint("##ability1Name", $"Ability Name", ref Ability1Name, 100);
                    ImGui.InputTextMultiline("##ability1Description", ref Ability1Description, 3000, new Vector2(350, 50));
                    ImGui.InputTextWithHint("##ability2Name", $"Ability Name", ref Ability2Name, 100);
                    ImGui.InputTextMultiline("##ability2Description", ref Ability2Description, 3000, new Vector2(350, 50));
                    ImGui.InputTextWithHint("##ability3Name", $"Ability Name", ref Ability3Name, 100);
                    ImGui.InputTextMultiline("##ability3Description", ref Ability3Description, 3000, new Vector2(350, 50));
                }

                if (ImGui.Button("Create Character"))
                {
                    if (abilityCount == 0)
                    {
                        DataSender.CreateSheetProfile(Plugin.Configuration.username, PlayerName, CharacterName, this.avatarBytes, this.avatarBytes.Length, Race, Age, Height, Weight, health, strength, senses, hardiness, intelligence, nimbleness, eminence, "","","","","","");
                    }
                    if (abilityCount == 1)
                    {
                        DataSender.CreateSheetProfile(Plugin.Configuration.username, PlayerName, CharacterName, this.avatarBytes, this.avatarBytes.Length, Race, Age, Height, Weight, health, strength, senses, hardiness, intelligence, nimbleness, eminence, Ability1Name, "", "", Ability1Description, "", "");

                    }
                    if (abilityCount == 2)
                    {
                        DataSender.CreateSheetProfile(Plugin.Configuration.username, PlayerName, CharacterName, this.avatarBytes, this.avatarBytes.Length, Race, Age, Height, Weight, health, strength, senses, hardiness, intelligence, nimbleness, eminence, Ability1Name, Ability2Name, "", Ability1Description, Ability2Description, "");
                    }
                    if (abilityCount == 3)
                    {
                        DataSender.CreateSheetProfile(Plugin.Configuration.username, PlayerName, CharacterName, this.avatarBytes, this.avatarBytes.Length, Race, Age, Height, Weight, health, strength, senses, hardiness, intelligence, nimbleness, eminence, Ability1Name, Ability2Name, Ability3Name, Ability1Description, Ability2Description, Ability3Description);
                    }
                }

                ImGui.Unindent(55);

            }
            ImGui.EndChild();
        }
        else
        {
            if (ImGui.BeginChild("CHARACTER SHEET TITLE", new Vector2(500, 850), true)) {
                //NAME TAG
                int NameWidth = characterName.Length * 10;
                var decidingWidth = Math.Max(500, ImGui.GetWindowWidth());
                var offsetWidth = (decidingWidth - NameWidth) / 2;
                var offsetVersion = characterName.Length > 0
                    ? _modVersionWidth + ImGui.GetStyle().ItemSpacing.X + ImGui.GetStyle().WindowPadding.X
                    : 0;
                var offset = Math.Max(offsetWidth, offsetVersion);
                if (offset > 0)
                {
                    ImGui.SetCursorPosX(offset);
                }
                if (VerificationStatus == int.Parse("-2"))
                {
                    using var col = ImRaii.PushColor(ImGuiCol.Border, ImGuiColors.DPSRed);
                    using var style = ImRaii.PushStyle(ImGuiStyleVar.FrameBorderSize, 2 * ImGuiHelpers.GlobalScale);
                    using var font = ImRaii.PushFont(_nameFont.ImFont, _nameFont.Available);
                    ImGuiUtil.DrawTextButton(characterName, Vector2.Zero, 0);
                    using var defInfFontDen = ImRaii.DefaultFont();
                    if (offset > 0)
                    {
                        ImGui.SetCursorPosX(ImGui.GetWindowWidth() / 2 - NameWidth);
                    }

                    ImGui.SetCursorPosX(ImGui.GetWindowWidth() / 2.5f);
                    ImGui.TextColored(ImGuiColors.DPSRed, "Verification Denied");
                }
                if (VerificationStatus == int.Parse("0"))
                {
                    using var col = ImRaii.PushColor(ImGuiCol.Border, ImGuiColors.DalamudOrange);
                    using var style = ImRaii.PushStyle(ImGuiStyleVar.FrameBorderSize, 2 * ImGuiHelpers.GlobalScale);
                    using var font = ImRaii.PushFont(_nameFont.ImFont, _nameFont.Available);
                    ImGuiUtil.DrawTextButton(characterName, Vector2.Zero, 0);
                    using var defInfFontPen = ImRaii.DefaultFont();
                    ImGui.SameLine();
                    using var DefaultColor = ImRaii.DefaultColors();

                    if (ImGui.ImageButton(this.penIcon.ImGuiHandle, new Vector2(20, 20)))
                    {

                    }

                    if (offset > 0)
                    {
                        ImGui.SetCursorPosX(ImGui.GetWindowWidth() / 2 - NameWidth);
                    }

                    ImGui.SetCursorPosX(ImGui.GetWindowWidth() / 2.5f);
                    ImGui.TextColored(ImGuiColors.DalamudOrange, "Pending Verification");

                }
                if (VerificationStatus == int.Parse("1"))
                {
                    using var col = ImRaii.PushColor(ImGuiCol.Border, ImGuiColors.HealerGreen);
                    using var style = ImRaii.PushStyle(ImGuiStyleVar.FrameBorderSize, 2 * ImGuiHelpers.GlobalScale);
                    using var font = ImRaii.PushFont(_nameFont.ImFont, _nameFont.Available);
                    ImGuiUtil.DrawTextButton(characterName, Vector2.Zero, 0);
                    using var defInfFontPen = ImRaii.DefaultFont();
                    ImGui.SameLine();
                    using var DefaultColor = ImRaii.DefaultColors();

                    if (ImGui.ImageButton(this.penIcon.ImGuiHandle, new Vector2(20, 20)))
                    {

                    }
                    if (offset > 0)
                    {
                        ImGui.SetCursorPosX(ImGui.GetWindowWidth() / 2);
                    }

                    ImGui.SetCursorPosX(ImGui.GetWindowWidth() / 2.5f);
                    ImGui.TextColored(ImGuiColors.HealerGreen, "Verified");
                }
                if (VerificationStatus == int.Parse("-1"))
                {
                    using var col = ImRaii.PushColor(ImGuiCol.Border, ImGuiColors.DalamudYellow);
                    using var style = ImRaii.PushStyle(ImGuiStyleVar.FrameBorderSize, 2 * ImGuiHelpers.GlobalScale);
                    using var font = ImRaii.PushFont(_nameFont.ImFont, _nameFont.Available);
                    ImGuiUtil.DrawTextButton(characterName, Vector2.Zero, 0);
                    using var defInfFontPen = ImRaii.DefaultFont();
                    ImGui.SameLine();
                    using var DefaultColor = ImRaii.DefaultColors();

                    if (ImGui.ImageButton(this.penIcon.ImGuiHandle, new Vector2(20, 20)))
                    {

                    }
                    if (offset > 0)
                    {
                        ImGui.SetCursorPosX(ImGui.GetWindowWidth() / 2);
                    }

                    ImGui.SetCursorPosX(ImGui.GetWindowWidth() / 2.5f);
                    ImGui.TextColored(ImGuiColors.DalamudYellow, "Revision Requested");
                }





                //AVATAR
                ImGui.SetCursorPosX(165f);
                ImGui.Image(this.vitar.ImGuiHandle, new Vector2(200, 200));

                ImGui.Separator();

                //BASIC INFO
                using var basCol = ImRaii.PushColor(ImGuiCol.Border, new Vector4(0, 0, 0, 0));
                using var basFont = ImRaii.PushFont(_infoFont.ImFont, _nameFont.Available);
                ImGuiUtil.DrawTextButton("Basic Info:", Vector2.Zero, 0);


                using var defInfFont = ImRaii.DefaultFont();
                ImGui.Text("Race: " + profileRace);
                ImGui.Text("Age: " + profileAge);
                ImGui.Text("Height: " + profileHeight);
                ImGui.Text("Weight: " + profileWeight);



                ImGui.Separator();
                using var whiteCol = ImRaii.PushColor(ImGuiCol.Border, new Vector4(0, 0, 0, 0));
                using var infoFont = ImRaii.PushFont(_infoFont.ImFont, _nameFont.Available);

                ImGuiUtil.DrawTextButton("Stats:", Vector2.Zero, 0);





                using var defFont = ImRaii.DefaultFont();

                ImGui.Image(this.EminenceImage.ImGuiHandle, new Vector2(20, 20));
                if (ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip(eminenceTooltip);
                }
                ImGui.SameLine();
                if (profileEminence > 0) { ImGui.Image(this.EminenceBar.ImGuiHandle, new Vector2(profileEminence * 30, 20)); }
                if (profileEminence < 0) { ImGui.Image(this.ngImage.ImGuiHandle, new Vector2(Math.Abs(profileEminence) * 30, 20)); }
                ImGui.SameLine();
                ImGui.Text(profileEminence.ToString());



                ImGui.Image(this.HardinessImage.ImGuiHandle, new Vector2(20, 20));
                if (ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip(hardinessTooltip);
                }
                ImGui.SameLine();
                if (profileHardiness > 0) { ImGui.Image(this.HardinessBar.ImGuiHandle, new Vector2(profileHardiness * 30, 20)); }
                if (profileHardiness < 0) { ImGui.Image(this.ngImage.ImGuiHandle, new Vector2(Math.Abs(profileHardiness) * 30, 20)); }
                ImGui.SameLine();
                ImGui.Text(profileHardiness.ToString());



                ImGui.Image(this.IntelligenceImage.ImGuiHandle, new Vector2(20, 20));
                if (ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip(intellectTooltip);
                }
                ImGui.SameLine();
                if (profileIntelligence > 0) { ImGui.Image(this.IntelligenceBar.ImGuiHandle, new Vector2(profileIntelligence * 30, 20)); }
                if (profileIntelligence < 0) { ImGui.Image(this.ngImage.ImGuiHandle, new Vector2(Math.Abs(profileIntelligence) * 30, 20)); }
                ImGui.SameLine();
                ImGui.Text(profileIntelligence.ToString());


                ImGui.Image(this.NimblenessImage.ImGuiHandle, new Vector2(20, 20));

                if (ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip(nimblenessTooltip);
                }
                ImGui.SameLine();
                if (profileNimbleness > 0) { ImGui.Image(this.NimblenessBar.ImGuiHandle, new Vector2(profileNimbleness * 30, 20)); }
                if (profileNimbleness < 0) { ImGui.Image(this.ngImage.ImGuiHandle, new Vector2(Math.Abs(profileNimbleness) * 30, 20)); }
                ImGui.SameLine();
                ImGui.Text(profileNimbleness.ToString());


                ImGui.Image(this.SensesImage.ImGuiHandle, new Vector2(20, 20));

                if (ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip(sensesTooltip);
                }
                ImGui.SameLine();
                if (profileSenses > 0) { ImGui.Image(this.SensesBar.ImGuiHandle, new Vector2(profileSenses * 30, 20)); }
                if (profileSenses < 0) { ImGui.Image(this.ngImage.ImGuiHandle, new Vector2(Math.Abs(profileSenses) * 30, 20)); }
                ImGui.SameLine();
                ImGui.Text(profileSenses.ToString());


                ImGui.Image(this.StrengthImage.ImGuiHandle, new Vector2(20, 20));

                if (ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip(strengthTootlip);
                }
                ImGui.SameLine();
                if (profileStrength > 0) { ImGui.Image(this.StrengthBar.ImGuiHandle, new Vector2(profileStrength * 30, 20)); }
                if (profileStrength < 0) { ImGui.Image(this.ngImage.ImGuiHandle, new Vector2(Math.Abs(profileStrength) * 30, 20)); }
                ImGui.SameLine();
                ImGui.Text(profileStrength.ToString());
                ImGui.Separator();

                using var abilWhiteCol = ImRaii.PushColor(ImGuiCol.Border, new Vector4(0, 0, 0, 0));
                using var abilInfoFont = ImRaii.PushFont(_infoFont.ImFont, _nameFont.Available);
                ImGuiUtil.DrawTextButton("Abilities:", Vector2.Zero, 0);
                using var abilDefFont = ImRaii.DefaultFont();
                ImGui.Text(profileAbility1);
                ImGui.Text(profileAbility1Desc);
                ImGui.Spacing();
                ImGui.Text(profileAbility2);
                ImGui.Text(profileAbility2Desc);
                ImGui.Spacing();
                ImGui.Text(profileAbility3);
                ImGui.Text(profileAbility3Desc);

            }

            ImGui.EndChild();

        }


    }
    private string? _tmpPath;
    private static void DrawOpenDirectoryButton(int id, DirectoryInfo directory, bool condition)
    {
        using var _ = ImRaii.PushId(id);
        var ret = ImGui.Button("Open Directory");
        ImGuiUtil.HoverTooltip("Open this directory in your configured file explorer.");
        if (ret && condition && Directory.Exists(directory.FullName))
        {
            Process.Start(new ProcessStartInfo(directory.FullName)
            {
                UseShellExecute = true,
            });
        }
    }
    public void ModStat(string statType, bool up)
    {
        if (up == true)
        {
            if (statType == "eminence") { if (eminence < MaxAllocation && this.availabelPoints > 0) { eminence += 1; statCount += 1; }  }
            if (statType == "hardiness") { if (hardiness < MaxAllocation && this.availabelPoints > 0) { hardiness += 1; statCount += 1; health += 1; } }
            if (statType == "intelligence") { if (intelligence < MaxAllocation && this.availabelPoints > 0) { intelligence += 1; statCount += 1; } }
            if (statType == "nimbleness") { if (nimbleness < MaxAllocation && this.availabelPoints > 0) { nimbleness += 1; statCount += 1; } }
            if (statType == "senses") { if (senses < MaxAllocation && this.availabelPoints > 0) { senses += 1; statCount += 1; } }
            if (statType == "strength") { if (strength < MaxAllocation && this.availabelPoints > 0) { strength += 1; statCount += 1; } }
        }
        if (up == false)
        {
            if (statType == "eminence") { if (eminence < 0 && eminence > MaxReductionPerStat) { eminence -= 1; reducedCount += 1; } if (eminence == 0) { /* do nothing*/ } if (eminence >= 0) { eminence -= 1; statCount -= 1; } }
            if (statType == "hardiness") { health -= 1; if (health <= 3) { health = 3; } if (hardiness < 0 && hardiness > MaxReductionPerStat) { hardiness -= 1; reducedCount += 1; } if (hardiness == 0) { /* do nothing*/ } if (hardiness >= 0) { hardiness -= 1; statCount -= 1; } }
            if (statType == "intelligence") { if (intelligence < 0 && intelligence > MaxReductionPerStat) { intelligence -= 1; reducedCount += 1; } if (intelligence == 0) { /* do nothing*/ } if (intelligence >= 0) { intelligence -= 1; statCount -= 1; } }
            if (statType == "nimbleness") { if (nimbleness < 0 && nimbleness > MaxReductionPerStat) { nimbleness -= 1; reducedCount += 1; } if (nimbleness == 0) { /* do nothing*/ } if (nimbleness >= 0) { nimbleness -= 1; statCount -= 1; } }
            if (statType == "senses") { if (senses < 0 && senses > MaxReductionPerStat) { senses -= 1; reducedCount += 1; } if (senses == 0) { /* do nothing*/ } if (senses >= 0) { senses -= 1; statCount -= 1; } }
            if (statType == "strength") { if (strength < 0 && strength > MaxReductionPerStat) { strength -= 1; reducedCount += 1; } if (strength == 0) { /* do nothing*/ } if (strength >= 0) { strength -= 1; statCount -= 1; } }

        }
    }
    public bool CheckStatUp(int stat)
    {
        if (stat >= MaxAllocation)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    public override void Update()
    {
        SheetVerificationStatuses = DataReceiver.characterVerificationStatuses;
        AccountCharacters = DataReceiver.characters;
        if (HealthWidth < health) { HealthWidth += 0.1f; }
        if (HealthWidth > health) {  HealthWidth -= 0.1f; }
        if (HardinessWidth < hardiness) { HardinessWidth += 0.1f; }
        if (HardinessWidth > hardiness) { HardinessWidth -= 0.1f; }
        if (IntelligenceWidth < intelligence) { IntelligenceWidth += 0.1f; }
        if (IntelligenceWidth > intelligence) { IntelligenceWidth -= 0.1f; }
        if (NimblenessWidth < nimbleness) { NimblenessWidth += 0.1f; }
        if (NimblenessWidth > nimbleness) { NimblenessWidth -= 0.1f; }
        if (EminenceWidth < eminence) { EminenceWidth += 0.1f; }
        if (EminenceWidth > eminence) { EminenceWidth -= 0.1f; }
        if (SensesWidth < senses) { SensesWidth += 0.1f; }
        if (SensesWidth > senses) { SensesWidth -= 0.1f; }
        if (StrengthWidth < strength) { StrengthWidth += 0.1f; }
        if (StrengthWidth > strength) { StrengthWidth -= 0.1f; }
       
      



    }
    public byte[] ImageToByteArray(System.Drawing.Image img)
    {
        MemoryStream ms = new MemoryStream();
        img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        return ms.ToArray();
    }
}

