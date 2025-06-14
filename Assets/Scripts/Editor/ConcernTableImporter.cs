using System.Collections.Generic;
using System.IO;
using Runtime;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Editor {

    [ScriptedImporter(1, "concerns")]
    sealed class ConcernTableImporter : ScriptedImporter {
        static int ROWS_TO_SKIP = 2;
        static int NUM_COLUMNS = 10;

        [SerializeField]
        FactionAsset[] factions;

        public override void OnImportAsset(AssetImportContext ctx) {
            string[] rows = File.ReadAllLines(ctx.assetPath);

            if (rows.Length <= ROWS_TO_SKIP) {
                Debug.LogWarning($"[Concern CSV Import] Too few Rows in: {ctx.assetPath}. Need at least 2!");
                return;
            }

            var concernTable = new GameObject {
                name = "ConcernTable"
            };
            ctx.AddObjectToAsset("Root", concernTable);
            ctx.SetMainObject(concernTable);

            Dictionary<string, int> counter = new();

            for (int rowIndex = ROWS_TO_SKIP; rowIndex < rows.Length; rowIndex++) {
                string row = rows[rowIndex];
                string[] columns = row.Split('\t');

                if (columns.Length != NUM_COLUMNS) {
                    Debug.LogWarning($"[Concern CSV Import] Row {rowIndex + 1} has the wrong number of columns ({columns.Length}, expected: {NUM_COLUMNS}) in: {ctx.assetPath}\n - {row}");
                    continue;
                }

                string petitioningFactionString = columns[0];

                var concern = ScriptableObject.CreateInstance<ConcernAsset>();
                concern.speech = columns[1];

                if (!TryParseLoyalityModifierColumns(rowIndex, columns, concern)) {
                    DestroyImmediate(concern); // avoid dirty assets
                    continue;
                }

                if (!counter.ContainsKey(petitioningFactionString)) {
                    counter[petitioningFactionString] = 0;
                }

                concern.name = $"Concern_{petitioningFactionString}_{counter[petitioningFactionString]++}";
                ctx.AddObjectToAsset(concern.name, concern);
            }
        }

        bool TryParseLoyalityModifierColumns(int rowIndex, string[] columns, ConcernAsset concern) {
            bool ok = true;
            var summands = new Dictionary<FactionAsset, float>();
            var multipliers = new Dictionary<FactionAsset, float>();

            for (int affectedFactionIndex = 0; affectedFactionIndex < factions.Length; affectedFactionIndex++) {
                var affectedfaction = factions[affectedFactionIndex];
                ok &= TryParseLoyaltyModifier(columns[2 + (affectedFactionIndex * 2) + 0], affectedfaction, rowIndex, summands);
                ok &= TryParseLoyaltyModifier(columns[2 + (affectedFactionIndex * 2) + 1], affectedfaction, rowIndex, multipliers);
            }

            concern.summands.SetItems(summands);
            concern.multipliers.SetItems(multipliers);
            return ok;
        }

        bool TryParseLoyaltyModifier(string inputString, FactionAsset affectedFaction, int rowIndex, Dictionary<FactionAsset, float> modifierDictionary) {
            if (float.TryParse(inputString, out float parsedModifier)) {
                modifierDictionary.Add(affectedFaction, parsedModifier);
                return true;
            } else {
                Debug.LogWarning($"[Concern CSV Import] Row {rowIndex + 1}: Could not parse modifier from input '{inputString}'");
                parsedModifier = 0f;
                return false;
            }
        }
    }
}
