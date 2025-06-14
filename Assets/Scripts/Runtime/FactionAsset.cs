using UnityEngine;

namespace Runtime {
    [CreateAssetMenu]
    public sealed class FactionAsset : ScriptableObject {
        [SerializeField, Range(0, 100)]
        internal float startingLoyalty = 50;
        [SerializeField, Range(0, 100)]
        internal float currentLoyalty = 50;
        [SerializeField]
        internal Color color = Color.white;

        internal void SetUp() {
            currentLoyalty = startingLoyalty;
        }
    }
}