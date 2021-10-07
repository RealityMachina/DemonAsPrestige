using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.Localization;
using Kingmaker.UI.UnitSettings;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using DemonAsPrestige.Config;
using DemonAsPrestige.Extensions;
using DemonAsPrestige.Utilities;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;
using Kingmaker.EntitySystem.Stats;

namespace DemonAsPrestige
{

    static class DemonPrestigePatcher
    {
        static bool Initialized;
        static void Postfix()
        {
            if (Initialized) return;
            Initialized = true;
            Main.LogHeader("Loading New Class");
            MakeDemonPrestige();
        }

        static void MakeDemonPrestige()
        {
            var DemonClass = Resources.GetBlueprint<BlueprintCharacterClass>("8e19495ea576a8641964102d177e34b7");
            var BabFull = Resources.GetBlueprint<BlueprintStatProgression>("b3057560ffff3514299e8b93e7648a9d");
            var SavePrestigeHigh = Resources.GetBlueprint<BlueprintStatProgression>("1f309006cd2855e4e91a6c3707f3f700");
            var DemonPrestigeClass = Helpers.CreateBlueprint<BlueprintCharacterClass>("RMDemonPrestigeClass");
            var DemonProgressionVisual = Resources.GetBlueprint<BlueprintClassAdditionalVisualSettingsProgression>("5cffc56c62114d12ba5ac319660dc2bf");
            var LegendClass = Resources.GetBlueprint<BlueprintCharacterClass>("3d420403f3e7340499931324640efe96");
            DemonPrestigeClass.m_Progression = DemonClass.m_Progression;
            DemonPrestigeClass.m_Spellbook = DemonClass.m_Spellbook;
            DemonPrestigeClass.HitDie = Kingmaker.RuleSystem.DiceType.D8;
            DemonPrestigeClass.m_SignatureAbilities = DemonClass.m_SignatureAbilities;
            DemonPrestigeClass.IsDivineCaster = true;
            DemonPrestigeClass.PrestigeClass = true;
            DemonPrestigeClass.SkillPoints = 2;
            DemonPrestigeClass.m_AdditionalVisualSettings = DemonProgressionVisual.ToReference<BlueprintClassAdditionalVisualSettingsProgression.Reference>();
            DemonPrestigeClass.m_BaseAttackBonus = BabFull.ToReference<BlueprintStatProgressionReference>();
            DemonPrestigeClass.m_FortitudeSave = SavePrestigeHigh.ToReference<BlueprintStatProgressionReference>();
            DemonPrestigeClass.m_ReflexSave = SavePrestigeHigh.ToReference<BlueprintStatProgressionReference>();
            DemonPrestigeClass.m_WillSave = SavePrestigeHigh.ToReference<BlueprintStatProgressionReference>();
            DemonPrestigeClass.ClassSkills = new StatType[] { StatType.SkillAthletics, StatType.SkillMobility, StatType.SkillPersuasion };
            if (!Main.settings.NoRequirement)
            {
                DemonPrestigeClass.AddComponent(Helpers.Create<PrerequisiteClassLevel>(c =>
                {
                    c.m_CharacterClass = LegendClass.ToReference<BlueprintCharacterClassReference>();
                    c.Level = 1; //has to be a Legend
                }));
                DemonPrestigeClass.AddComponent(Helpers.Create<PrerequisiteMainCharacter>(c =>
                {
                    //c.HideInUi = True;
                }));
            }
            DemonPrestigeClass.LocalizedName = Main.MakeLocalizedString("RMDemonHuName","Demonic Hunter");
            DemonPrestigeClass.LocalizedDescription = Main.MakeLocalizedString("RMDemonHuDesc", "You've regained control of your mortal fate, but your trials and tribulations in this crusade has left its abyssal mark on you nonetheless. You can harness the powers of the abyss on your own terms, and be a legendary demon hunter of your own making.");

            Helpers.RegisterClass(DemonPrestigeClass);
        }
       
    }

 
}
