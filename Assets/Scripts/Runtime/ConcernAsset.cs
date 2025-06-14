using Slothsoft.UnityExtensions;
using UnityEngine;

namespace Runtime {
    public sealed class ConcernAsset : ScriptableObject {
        [TextArea] public string speech;
        public SerializableKeyValuePairs<FactionAsset, float> summands = new();
        public SerializableKeyValuePairs<FactionAsset, float> multipliers = new();
    }
}
