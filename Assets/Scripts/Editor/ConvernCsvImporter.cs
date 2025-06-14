using System.Collections.Generic;
using System.IO;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace GameJam.Editor {

    [ScriptedImporter(1, "concerns")]
    sealed class ConvernCsvImporter : ScriptedImporter {
        static int ROWS_TO_SKIP = 2;
        static int NUM_COLUMNS = 10;

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

                if (columns.Length < NUM_COLUMNS) {
                    Debug.LogWarning($"[Concern CSV Import] Row {rowIndex + 1} has too few columns ({columns.Length}) in: {ctx.assetPath}\n→ {row}");
                    continue;
                }

                string faction = columns[0];

                var concern = ScriptableObject.CreateInstance<ConcernAsset>();
                concern.speech = columns[1];

                bool ok = true;
                ok &= TryParseFloat(columns[2], out concern.nobleSum, rowIndex, "NobleSum");
                ok &= TryParseFloat(columns[3], out concern.nobleMult, rowIndex, "NobleMult");
                ok &= TryParseFloat(columns[4], out concern.peasantSum, rowIndex, "PeasantSum");
                ok &= TryParseFloat(columns[5], out concern.peasantMult, rowIndex, "PeasantMult");
                ok &= TryParseFloat(columns[6], out concern.clericSum, rowIndex, "ClericSum");
                ok &= TryParseFloat(columns[7], out concern.clericMult, rowIndex, "ClericMult");
                ok &= TryParseFloat(columns[8], out concern.merchantSum, rowIndex, "MerchantSum");
                ok &= TryParseFloat(columns[9], out concern.merchantMult, rowIndex, "MerchantMult");

                if (!ok) {
                    DestroyImmediate(concern); // avoid dirty assets
                    continue;
                }

                if (!counter.ContainsKey(faction)) {
                    counter[faction] = 0;
                }

                concern.name = $"Concern_{faction}_{counter[faction]++}";
                ctx.AddObjectToAsset(concern.name, concern);
            }
        }

        bool TryParseFloat(string inputString, out float result, int rowIndex, string field) {
            if (float.TryParse(inputString, out result)) {
                return true;
            }
            else {
                Debug.LogWarning($"[Concern CSV Import] Row {rowIndex + 1}: Could not parse '{field}' from input '{inputString}'");
                result = 0f;
                return false;
            }
        }
    }
}
