using Slothsoft.UnityExtensions;
using UnityEngine;

namespace Runtime {
    public sealed class ConcernAsset : ScriptableObject {
        [TextArea] public string speech;
        public SerializableKeyValuePairs<FactionAsset, float> summands = new();
        public SerializableKeyValuePairs<FactionAsset, float> multipliers = new();

        public void ExecuteOnConcern(float royalMultiplier) {
            foreach (var kvp in summands) {
                var affectedFaction = kvp.Key;
                float summand = kvp.Value;
                float multiplier = multipliers[affectedFaction];

                affectedFaction.currentLoyalty += summand * royalMultiplier;
                affectedFaction.currentLoyalty *= multiplier * royalMultiplier;
            }
        }
    }
}
