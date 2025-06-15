using Slothsoft.UnityExtensions;
using UnityEngine;

namespace Runtime {
    public sealed class ConcernAsset : ScriptableObject {
        public FactionAsset faction;
        [TextArea] public string speech;
        public SerializableKeyValuePairs<FactionAsset, float> summands = new();
        public SerializableKeyValuePairs<FactionAsset, float> multipliers = new();

        public void ExecuteOnConcern(float royalSupport) {

            if (BalancingLogs.enabled) {
                Debug.Log($"Royal Support: {royalSupport}");
            }

            float royalSupportNormalized = (royalSupport / 100f * 2f) - 1f;
            float royalSupportMultiplier = Mathf.Sign(royalSupportNormalized) * Mathf.Sqrt(Mathf.Abs(royalSupportNormalized));

            if (BalancingLogs.enabled) {
                Debug.Log($"Royal Support Normalized: {royalSupportNormalized}");
                Debug.Log($"Royal Support Multiplier: {royalSupportMultiplier}");
            }

            foreach (var kvp in summands) {
                var affectedFaction = kvp.Key;
                float summand = kvp.Value;
                float multiplier = multipliers[affectedFaction];

                float summandFinal = summand * royalSupportMultiplier;
                float multiplierFinal = ((multiplier - 1f) * royalSupportMultiplier) + 1f;

                if (BalancingLogs.enabled) {
                    Debug.Log($"Affected Faction: {affectedFaction.id}");
                    Debug.Log($" + {summand} * {royalSupportMultiplier} == {summandFinal}");
                    Debug.Log($" * (({multiplier} - 1f) * {royalSupportMultiplier}) + 1f == {multiplierFinal}");
                    Debug.Log($" --> {affectedFaction.GetLoyalty()} -> {affectedFaction.GetLoyalty() + summandFinal}");
                    float preCalcNewLoyalty = affectedFaction.GetLoyalty() + summandFinal;
                    Debug.Log($" --> {preCalcNewLoyalty} -> {preCalcNewLoyalty * multiplierFinal}");
                }

                affectedFaction.ModifyLoyalty(summandFinal, multiplierFinal);
            }
        }
    }
}
