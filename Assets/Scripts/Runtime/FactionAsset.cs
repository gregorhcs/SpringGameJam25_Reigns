using Slothsoft.Aseprite;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace Runtime {
    [CreateAssetMenu]
    public sealed class FactionAsset : ScriptableObject {
        [SerializeField, Range(0, 100)]
        internal float startingLoyalty = 50;
        [SerializeField, Range(0, 100)]
        internal float currentLoyalty = 50;
        [SerializeField, Expandable]
        ColorAsset colorAsset;

        internal Color color => colorAsset;

        internal void SetUp() {
            currentLoyalty = startingLoyalty;
        }
    }
}