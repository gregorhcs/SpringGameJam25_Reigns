using UnityEngine;

namespace Assets.Scripts.Runtime {
    public class PetitionerSlot : MonoBehaviour {
        public bool isThroneSlot = false;
        public Petitioner petitioner = null;

        public bool IsFree() => petitioner == null;
    }
}
