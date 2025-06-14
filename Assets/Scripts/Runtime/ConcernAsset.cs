using UnityEngine;

namespace GameJam {
    public sealed class ConcernAsset : ScriptableObject {
        [TextArea] public string speech;

        public float nobleSum, nobleMult;
        public float peasantSum, peasantMult;
        public float clericSum, clericMult;
        public float merchantSum, merchantMult;
    }
}
