using Slothsoft.UnityExtensions;
using UnityEngine;

namespace Runtime {
    public sealed class ConcernAsset : ScriptableObject {
        public FactionAsset faction;
        [TextArea] public string speech;
        public SerializableKeyValuePairs<FactionAsset, float> summands = new();
        public SerializableKeyValuePairs<FactionAsset, float> multipliers = new();

        public void ExecuteOnConcern(float royalSupportMultiplier) {
            foreach (var kvp in summands) {
                var affectedFaction = kvp.Key;
                float summand = kvp.Value;
                float multiplier = multipliers[affectedFaction];

                affectedFaction.currentLoyalty += summand * royalSupportMultiplier;
                affectedFaction.currentLoyalty *= multiplier * royalSupportMultiplier;
            }
        }
    }
}
