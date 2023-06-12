using Dalamud.Interface.Colors;
using Dalamud.Interface;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using FFXIVClientStructs.FFXIV.Common.Math;
using ImGuiNET;
using ImGuiScene;
using InfiniteRoleplay;
using OtterGui.Raii;
using OtterGui;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dalamud.Interface.GameFonts;
using Dalamud.Game.Gui.Dtr;
using Microsoft.VisualBasic;
using Networking;

namespace InfiniteRoleplay.Windows
{
    internal class AdminWindow : Window, IDisposable
    {
        private Plugin plugin;
        public static bool isAdmin;
        public Configuration configuration;
        private static bool moderateProfiles;
        public Dictionary<int, string> AdminProfilesData = new Dictionary<int, string>();
        public Dictionary<int, int> SheetVerificationStatuses = new Dictionary<int, int>();
        private string profileRace, profileAge, profileHeight, profileWeight, profileAbility1, profileAbility2, profileAbility3, profileAbility1Desc, profileAbility2Desc, profileAbility3Desc;
        private int profileHealth, profileHealthWidth, profileEminence, profileEminenceWidth, profileHardiness, profileHardinessWidth, profileIntelligence, profileIntelligenceWidth, profileNimbleness, profileNimblenessWith, profileSenses, profileSensesWidth, profileStrength, profileStrengthWidth;
        private GameFontHandle _nameFont;
        private GameFontHandle _infoFont;
        private float _modVersionWidth;
        private DalamudPluginInterface pg;
        private TextureWrap vitar;
        private string CharacterName = string.Empty;
        private string characterName;
        private int chrID;
        private string PlayerName = string.Empty;
        private string playerName;
        private string AvatarPath = string.Empty;
        private string avatarPath;
        private string Race = string.Empty;
        private string raceName;
        private string Age = string.Empty;
        private string age;
        private string Height = string.Empty;
        private string height;
        private string Weight = string.Empty;
        private string weight;
        private string Ability1Name = string.Empty;
        private string ability1Name;
        private string Ability2Name = string.Empty;
        private string ability2Name;
        private string Ability3Name = string.Empty;
        private string ability3Name;
        private string Ability1Description = string.Empty;
        private string ability1Description;
        private string Ability2Description = string.Empty;
        private string ability2Description;
        private string Ability3Description = string.Empty;
        private string ability3Description;
        private int verificationStatus;
        private bool showSheet;
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
        private TextureWrap HealthImage;
        public static int SelectedSheetID;
        private string message;
        private string eminenceTooltip, hardinessTooltip, intellectTooltip, nimblenessTooltip, sensesTooltip, strengthTootlip;

        public AdminWindow(Plugin plugin, DalamudPluginInterface Interface) : base(
       "ADMINISTRATION", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
        {
            this.SizeConstraints = new WindowSizeConstraints
            {
                MinimumSize = new Vector2(1200, 950),
                MaximumSize = new Vector2(1200, 950)
            };
            this.plugin = plugin;
            this.configuration = plugin.Configuration;
            this.characterName = CharacterName;
            this.pg = Interface;
            this._nameFont = pg.UiBuilder.GetGameFontHandle(new GameFontStyle(GameFontFamilyAndSize.Jupiter23));
            this._infoFont = pg.UiBuilder.GetGameFontHandle(new GameFontStyle(GameFontFamilyAndSize.Jupiter16));

            this.showSheet = false;
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

        }
        public override void Draw()
        {
            if (ImGui.BeginChild("ProfilesTitle", new Vector2(300, 500), true))
            {
                using var col = ImRaii.PushColor(ImGuiCol.Border, ImGuiColors.DalamudViolet);
                using var style = ImRaii.PushStyle(ImGuiStyleVar.FrameBorderSize, 2 * ImGuiHelpers.GlobalScale);
                var _nameFont = plugin.PluginInterfacePub.UiBuilder.GetGameFontHandle(new GameFontStyle(GameFontFamilyAndSize.Jupiter23));
                using var font = ImRaii.PushFont(_nameFont.ImFont, _nameFont.Available);
                ImGuiUtil.DrawTextButton("Profiles", Vector2.Zero, 0);



                using var defInfFontDen = ImRaii.DefaultFont();
                using var DefaultColor = ImRaii.DefaultColors();
                if (ImGui.Button("Character Profiles"))
                {
                    moderateProfiles = true;
                }

            }
            ImGui.EndChild();
            ImGui.SameLine();
            if (moderateProfiles == true)
            {
                if (ImGui.BeginChild("Profiles", new Vector2(120, 500), true))
                {

                    foreach (string character in AdminProfilesData.Values)
                    {

                        string[] characterSplit = character.Split(',');


                        int ID = int.Parse(characterSplit[0]);

                        //string character = sheetID + "," + player_name + "," + character_name + "," + race + "," + age + "," + height + "," + weight + "," + health + "," + senses + "," + strength + "," + hardiness + "," + intelligence + "," + nimbleness + "," + eminence + "," + ability_1 + "," + ability_2 + "," + ability_3 + "," + ability_1_description + "," + ability_2_description + "," + ability_3_description + "," + verification_status;
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
                        string chrVerStatus = characterSplit[20];

                        if (ImGui.Selectable(chrName))
                        {
                            this.chrID = ID;
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
                            this.verificationStatus = int.Parse(chrVerStatus);
                            SelectedSheetID = ID;

                            if (DataReceiver.adminCharacterAvatars.ContainsKey(ID))
                            {
                                byte[] avData = DataReceiver.adminCharacterAvatars[ID];
                                this.vitar = pg.UiBuilder.LoadImage(avData);
                            }
                            showSheet = true;
                        }
                    }
                }
                ImGui.EndChild();
                ImGui.SameLine();
                if (showSheet == true)
                {
                    if (ImGui.BeginChild("CHARACTERS SHEET TITLE", new Vector2(500, 850), true))
                    {
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

                        if (verificationStatus == -2)
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
                            ImGui.NewLine();
                            ImGui.SetCursorPosX(ImGui.GetWindowWidth() / 2.5f);
                            ImGui.TextColored(ImGuiColors.DPSRed, "Verification Denied");
                        }
                        if (verificationStatus == 0)
                        {
                            using var col = ImRaii.PushColor(ImGuiCol.Border, ImGuiColors.DalamudOrange);
                            using var style = ImRaii.PushStyle(ImGuiStyleVar.FrameBorderSize, 2 * ImGuiHelpers.GlobalScale);
                            using var font = ImRaii.PushFont(_nameFont.ImFont, _nameFont.Available);
                            ImGuiUtil.DrawTextButton(characterName, Vector2.Zero, 0);
                            using var defInfFontPen = ImRaii.DefaultFont();
                            ImGui.SameLine();
                            using var DefaultColor = ImRaii.DefaultColors();



                            if (offset > 0)
                            {
                                ImGui.SetCursorPosX(ImGui.GetWindowWidth() / 2 - NameWidth);
                            }
                            ImGui.NewLine();
                            ImGui.SetCursorPosX(ImGui.GetWindowWidth() / 2.5f);
                            ImGui.TextColored(ImGuiColors.DalamudOrange, "Pending Verification");

                        }
                        if (verificationStatus == 1)
                        {
                            using var col = ImRaii.PushColor(ImGuiCol.Border, ImGuiColors.HealerGreen);
                            using var style = ImRaii.PushStyle(ImGuiStyleVar.FrameBorderSize, 2 * ImGuiHelpers.GlobalScale);
                            using var font = ImRaii.PushFont(_nameFont.ImFont, _nameFont.Available);
                            ImGuiUtil.DrawTextButton(characterName, Vector2.Zero, 0);
                            using var defInfFontPen = ImRaii.DefaultFont();
                            ImGui.SameLine();
                            using var DefaultColor = ImRaii.DefaultColors();


                            if (offset > 0)
                            {
                                ImGui.SetCursorPosX(ImGui.GetWindowWidth() / 2);
                            }
                            ImGui.NewLine();
                            ImGui.SetCursorPosX(ImGui.GetWindowWidth() / 2.5f);
                            ImGui.TextColored(ImGuiColors.HealerGreen, "Verified");
                        }
                        if (verificationStatus == -1)
                        {
                            using var col = ImRaii.PushColor(ImGuiCol.Border, ImGuiColors.DalamudYellow);
                            using var style = ImRaii.PushStyle(ImGuiStyleVar.FrameBorderSize, 2 * ImGuiHelpers.GlobalScale);
                            using var font = ImRaii.PushFont(_nameFont.ImFont, _nameFont.Available);
                            ImGuiUtil.DrawTextButton(characterName, Vector2.Zero, 0);
                            using var defInfFontPen = ImRaii.DefaultFont();
                            ImGui.SameLine();
                            using var DefaultColor = ImRaii.DefaultColors();


                            if (offset > 0)
                            {
                                ImGui.SetCursorPosX(ImGui.GetWindowWidth() / 2);
                            }
                            ImGui.NewLine();
                            ImGui.SetCursorPosX(ImGui.GetWindowWidth() / 2.5f);
                            ImGui.TextColored(ImGuiColors.DalamudYellow, "Revision Requested");
                        }





                        //AVATAR
                        ImGui.SetCursorPosX(165f);
                        ImGui.Image(this.vitar.ImGuiHandle, new Vector2(200, 200));


                        if (ImGui.Button("Verify"))
                        {
                            MessageBox.TITLE = "Verification";
                            MessageBox.TITLE_COLOR = ImGuiColors.HealerGreen;
                            MessageBox.DESCRIPTION = "Are you sure you want to verify this character sheet? \n" +
                                                     "This will make the profile active for roleplay in the Shine system.";
                            MessageBox.DESC_COLOR = new Vector4(255, 255, 255, 255);
                            MessageBox.MESSAGE_TYPE = MessageBox.messageType.SHEET_VERIFY;
                            MessageBox.MESSAGE_CONTENT = this.chrID.ToString();
                            MessageBox.MESSAGE_TYPE = MessageBox.messageType.SHEET_VERIFY;
                            MessageBox.SELECTED_SHEET_ID = SelectedSheetID;
                            plugin.WindowSystem.GetWindow("Message").IsOpen = true;
                        }
                        ImGui.SameLine();
                        if (ImGui.Button("Decline"))
                        {
                            MessageBox.TITLE = "Decline Verification";
                            MessageBox.TITLE_COLOR = ImGuiColors.DalamudRed;
                            MessageBox.DESCRIPTION = "Are you sure you want to decline this character sheet? \n" +
                                                     "This will make the profile inactive for roleplay in the Shine system.\n" +
                                                     "The user will need to supply a new sheet to be accepted.";
                            MessageBox.DESC_COLOR = new Vector4(255, 255, 255, 255);
                            MessageBox.MESSAGE_TYPE = MessageBox.messageType.SHEET_DECLINE;
                            MessageBox.MESSAGE_CONTENT = this.chrID.ToString();
                            MessageBox.MESSAGE_TYPE = MessageBox.messageType.SHEET_DECLINE;
                            MessageBox.SELECTED_SHEET_ID = SelectedSheetID;
                            plugin.WindowSystem.GetWindow("Message").IsOpen = true;
                        }
                        ImGui.SameLine();
                        if (ImGui.Button("Request Revision"))
                        {
                            MessageBox.TITLE = "Send Revision Request";
                            MessageBox.TITLE_COLOR = ImGuiColors.DalamudYellow;
                            MessageBox.DESCRIPTION = "Are you sure you want to send a revision request for this character sheet? \n" +
                                                     "This will make the profile inactive for roleplay in the Shine system, until\n" +
                                                     "the user revises the sheet and it is verified.";
                            MessageBox.DESC_COLOR = new Vector4(255, 255, 255, 255);
                            MessageBox.MESSAGE_TYPE = MessageBox.messageType.SHEET_REVISION_REQUEST;
                            MessageBox.MESSAGE_CONTENT = this.chrID.ToString();
                            MessageBox.MESSAGE_TYPE = MessageBox.messageType.SHEET_REVISION_REQUEST;
                            MessageBox.SELECTED_SHEET_ID = SelectedSheetID;
                            plugin.WindowSystem.GetWindow("Message").IsOpen = true;
                        }
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

        }
        public void Dispose()
        {

        }
        public override void Update()
        {
            AdminProfilesData = DataReceiver.adminCharacters;
        }
    }

}
