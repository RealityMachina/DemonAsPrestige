﻿using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using System;
using DemonAsPrestige.Extensions;
using DemonAsPrestige;
using Kingmaker.Blueprints;

namespace DemonAsPrestige.Utilities {
    static class FeatTools {
        public static void AddAsFeat(params BlueprintFeature[] features) {
            foreach (var feature in features) {
                AddAsFeat(feature);
            }
        }
        public static void AddAsFeat(BlueprintFeature feature) {
            var BasicFeatSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("247a4068296e8be42890143f451b4b45");
            var ExtraFeatMythicFeat = Resources.GetBlueprint<BlueprintFeatureSelection>("e10c4f18a6c8b4342afe6954bde0587b");
            BasicFeatSelection.AddFeatures(feature);
            ExtraFeatMythicFeat.AddFeatures(feature);
        }
        public static void AddAsRogueTalent(BlueprintFeature feature) {
            var TalentSelections = new BlueprintFeatureSelection[] {
                Resources.GetBlueprint<BlueprintFeatureSelection>("290bbcc3c3bb92144b853fd8fb8ff452"), //SylvanTricksterTalentSelection
                Resources.GetBlueprint<BlueprintFeatureSelection>("913b9cf25c9536949b43a2651b7ffb66"), //SlayerTalentSelection10
                Resources.GetBlueprint<BlueprintFeatureSelection>("43d1b15873e926848be2abf0ea3ad9a8"), //SlayerTalentSelection6
                Resources.GetBlueprint<BlueprintFeatureSelection>("04430ad24988baa4daa0bcd4f1c7d118"), //SlayerTalentSelection2
                Resources.GetBlueprint<BlueprintFeatureSelection>("d2a8fde8985691045b90e1ec57e3cc57"), //SkaldTalentSelection
                Resources.GetBlueprint<BlueprintFeatureSelection>("c074a5d615200494b8f2a9c845799d93"), //RogueTalentSelection
                Resources.GetBlueprint<BlueprintFeatureSelection>("4b7018b1ed4b27140a5e7adfacaaf9c6"), //LoremasterRogueTalentSelection
                Resources.GetBlueprint<BlueprintFeatureSelection>("94e2cd84bf3a8e04f8609fe502892f4f"), //BardTalentSelection
                Resources.GetBlueprint<BlueprintFeatureSelection>("40f85fbe8cc35ef4fa96c66e06eeafe8")  //BardTalentSelection
            };
            TalentSelections.ForEach(selection => selection.AddFeatures(feature));
        }

        public static void AddAsRagePower(BlueprintFeature feature)
        {
            var TalentSelections = new BlueprintFeatureSelection[] {
                Resources.GetBlueprint<BlueprintFeatureSelection>("28710502f46848d48b3f0d6132817c4e"), //Rage selection
                Resources.GetBlueprint<BlueprintFeatureSelection>("0c7f01fbbe687bb4baff8195cb02fe6a"), //Extra Rage selection
                Resources.GetBlueprint<BlueprintFeatureSelection>("2476514e31791394fa140f1a07941c96"), // skald
                Resources.GetBlueprint<BlueprintFeatureSelection>("609f0e5336084442a0dafa3abd4d31c5"), // instinctual warrior
                Resources.GetBlueprint<BlueprintFeatureSelection>("29b480a26a88f9e47a10d8c9fab84ee6") //battle scion
            };
            TalentSelections.ForEach(selection => selection.AddFeatures(feature));

            //for lich
            var ParaFeature = Resources.GetBlueprint<BlueprintParametrizedFeature>("aaba9ebd2074e454aaed211698a34db0");  //lich
            
            AnyBlueprintReference newReference = feature.ToReference<AnyBlueprintReference>();

            ParaFeature.BlueprintParameterVariants.AppendToArray<AnyBlueprintReference>(newReference);

        }
        public static BlueprintFeature CreateSkillFeat(string name, StatType skill1, StatType skill2, Action<BlueprintFeature> init = null) {
            var SkillFeat = Helpers.CreateBlueprint<BlueprintFeature>(name, bp => {
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat };
                bp.AddComponent(Helpers.Create<AddContextStatBonus>(c => {
                    c.Stat = skill1;
                    c.Multiplier = 1;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.StatBonus,
                        Value = 2
                    };
                }));
                bp.AddComponent(Helpers.Create<AddContextStatBonus>(c => {
                    c.Stat = skill2;
                    c.Multiplier = 1;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.Default,
                        Value = 2
                    };
                }));
                bp.AddComponent(Helpers.Create<ContextRankConfig>(c => {
                    c.m_Type = AbilityRankType.StatBonus;
                    c.m_BaseValueType = ContextRankBaseValueType.BaseStat;
                    c.m_Stat = skill1;
                    c.m_Progression = ContextRankProgression.Custom;
                    c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[] {
                        new ContextRankConfig.CustomProgressionItem() {
                            BaseValue = 9,
                            ProgressionValue = 2
                        },
                        new ContextRankConfig.CustomProgressionItem() {
                            BaseValue = 100,
                            ProgressionValue = 4
                        }
                    };
                    c.m_StepLevel = 3;
                    c.m_Min = 10;
                    c.m_Max = 20;
                }));
                bp.AddComponent(Helpers.Create<ContextRankConfig>(c => {
                    c.m_Type = AbilityRankType.Default;
                    c.m_BaseValueType = ContextRankBaseValueType.BaseStat;
                    c.m_Stat = skill2;
                    c.m_Progression = ContextRankProgression.Custom;
                    c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[] {
                        new ContextRankConfig.CustomProgressionItem() {
                            BaseValue = 9,
                            ProgressionValue = 2
                        },
                        new ContextRankConfig.CustomProgressionItem() {
                            BaseValue = 100,
                            ProgressionValue = 4
                        }
                    };
                    c.m_StepLevel = 3;
                    c.m_Min = 10;
                    c.m_Max = 20;
                }));
                bp.AddComponent(Helpers.Create<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.Skills;
                }));
            });
            init?.Invoke(SkillFeat);
            return SkillFeat;
        }
    }
}
