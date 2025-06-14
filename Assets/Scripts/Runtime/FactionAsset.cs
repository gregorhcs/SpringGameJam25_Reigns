using Slothsoft.Aseprite;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace Runtime {
    [CreateAssetMenu]
    public sealed class FactionAsset : ScriptableObject {
        [SerializeField]
        internal string id = default;

        [SerializeField, Range(0, 100)]
        internal float startingLoyalty = 50;

        [SerializeField, Range(0, 100)]
        internal float currentLoyalty = 50;

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
    }
}