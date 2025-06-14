using UnityEngine;

namespace Runtime {
    sealed class FactionInitializer : MonoBehaviour {
        [SerializeField]
        FactionAsset[] factions = default;

        void Awake()
        {
            foreach (var faction in factions) {
                faction.SetUp();
            }
        }
    }
}
