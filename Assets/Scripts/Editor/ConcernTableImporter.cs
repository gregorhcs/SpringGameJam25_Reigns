﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Runtime;
using Slothsoft.UnityExtensions.Editor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Editor {

    [ScriptedImporter(VERSION, "concerns")]
    sealed class ConcernTableImporter : ScriptedImporter {
        const int VERSION = 2;
        const int ROWS_TO_SKIP = 2;
        const int NUM_COLUMNS = 10;

        [SerializeField]
        string[] factionPaths = Array.Empty<string>();

        FactionAsset[] factions;

        public override void OnImportAsset(AssetImportContext ctx) {

            factions = new FactionAsset[factionPaths.Length];
            for (int i = 0; i < factionPaths.Length; i++) {
                if (!ctx.TryDependOnArtifact(factionPaths[i], out factions[i])) {
                    Debug.LogError($"Missing FactionAsset at '{factionPaths[i]}'");
                    return;
                }
            }

            string[] rows = File.ReadAllLines(ctx.assetPath);

            if (rows.Length <= ROWS_TO_SKIP) {
                Debug.LogError($"[Concern CSV Import] Too few Rows in: {ctx.assetPath}. Need at least 2!");
                return;
            }

            var concernLibrary = ScriptableObject.CreateInstance<ConcernsLibraryAsset>();
            concernLibrary.name = "ConcernLibrary";
            ctx.AddObjectToAsset("Root", concernLibrary);
            ctx.SetMainObject(concernLibrary);

            Dictionary<string, int> counter = new();

            for (int rowIndex = ROWS_TO_SKIP; rowIndex < rows.Length; rowIndex++) {
                string row = rows[rowIndex];
                string[] columns = row.Split('\t');

                if (columns.Length != NUM_COLUMNS) {
                    Debug.LogError($"[Concern CSV Import] Row {rowIndex + 1} has the wrong number of columns ({columns.Length}, expected: {NUM_COLUMNS}) in: {ctx.assetPath}\n - {row}");
                    continue;
                }

                var concern = ScriptableObject.CreateInstance<ConcernAsset>();

                if (!TryParseFaction(columns[0], out concern.faction)) {
                    continue;
                }

                concern.speech = columns[1];

                if (!TryParseLoyalityModifierColumns(rowIndex, columns, concern)) {
                    DestroyImmediate(concern); // avoid dirty assets
                    continue;
                }

                if (!counter.ContainsKey(concern.faction.id)) {
                    counter[concern.faction.id] = 0;
                }
                concern.name = $"Concern_{concern.faction.id}_{counter[concern.faction.id]++}";

                concernLibrary.concerns.Add(concern);
                ctx.AddObjectToAsset(concern.name, concern);
            }
        }

        bool TryParseLoyalityModifierColumns(int rowIndex, string[] columns, ConcernAsset concern) {
            bool ok = true;
            var summands = new Dictionary<FactionAsset, float>();
            var multipliers = new Dictionary<FactionAsset, float>();

            for (int affectedFactionIndex = 0; affectedFactionIndex < factions.Length; affectedFactionIndex++) {
                var affectedfaction = factions[affectedFactionIndex];

                if (columns.Length <= 2 + (affectedFactionIndex * 2) + 1) {
                    break;
                }

                ok &= TryParseLoyaltyModifier(columns[2 + (affectedFactionIndex * 2) + 0], affectedfaction, rowIndex, summands);
                ok &= TryParseLoyaltyModifier(columns[2 + (affectedFactionIndex * 2) + 1], affectedfaction, rowIndex, multipliers);
            }

            concern.summands.SetItems(summands);
            concern.multipliers.SetItems(multipliers);
            return ok;
        }

        bool TryParseLoyaltyModifier(string inputString, FactionAsset affectedFaction, int rowIndex, Dictionary<FactionAsset, float> modifierDictionary) {
            if (float.TryParse(inputString, NumberStyles.Any, CultureInfo.InvariantCulture, out float parsedModifier)) {
                modifierDictionary.Add(affectedFaction, parsedModifier);
                return true;
            } else {
                Debug.LogError($"[Concern CSV Import] Row {rowIndex + 1}: Could not parse modifier from input '{inputString}'");
                parsedModifier = 0f;
                return false;
            }
        }

        bool TryParseFaction(string inputString, out FactionAsset outFaction) {
            foreach (var faction in factions) {
                if (faction.id ==  inputString) {
                    outFaction = faction;
                    return true;
                }
            }
            outFaction = null;
            return false;
        }
    }
}
