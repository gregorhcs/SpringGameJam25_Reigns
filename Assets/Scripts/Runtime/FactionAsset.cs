using Slothsoft.Aseprite;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace Runtime {
    [CreateAssetMenu]
    public sealed class FactionAsset : ScriptableObject {
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

        public void AddToLoyalty(float summand) {
            currentLoyalty = Mathf.Clamp(currentLoyalty + summand, 0f, 100f);
        }

        public void MultToLoyalty(float factor) {
            currentLoyalty = Mathf.Clamp(currentLoyalty * factor, 0f, 100f);
        }

        public float GetLoyalty() {
            return currentLoyalty;
        }
    }
}