using System;
using Slothsoft.Aseprite;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace Runtime {
    [CreateAssetMenu]
    public sealed class FactionAsset : ScriptableObject {
        public static Action<FactionAsset> onFactionLoyaltyReachesZero;
        public Action<float> onChangeLoyalty;

        [SerializeField]
        internal string id = default;

        [SerializeField, Range(0, 100)]
        float startingLoyalty = 50;

        [SerializeField, Range(0, 100)]
        float currentLoyalty = 50;

        [SerializeField, Expandable]
        ColorAsset colorAsset;

        internal Color color => colorAsset;

        [SerializeField, Expandable]
        internal Sprite banner;

        [SerializeField]
        internal RuntimeAnimatorController animatorController = default;

        public void SetUp() {
            currentLoyalty = startingLoyalty;
        }

        public void ModifyLoyalty(float summand, float factor) {
            float previous = currentLoyalty;

            currentLoyalty = Mathf.Clamp(currentLoyalty + summand, 0f, 100f);
            currentLoyalty = Mathf.Clamp(currentLoyalty * factor, 0f, 100f);

            if (!Mathf.Approximately(currentLoyalty, previous)) {
                onChangeLoyalty?.Invoke(currentLoyalty - previous);
            }

            if (Mathf.Approximately(currentLoyalty, 0f)) {
                onFactionLoyaltyReachesZero?.Invoke(this);
            }
        }

        public void ModifyLoyalty(float amount) {
            ModifyLoyalty(amount, 1);
        }

        public float GetLoyalty() {
            return currentLoyalty;
        }
    }
}