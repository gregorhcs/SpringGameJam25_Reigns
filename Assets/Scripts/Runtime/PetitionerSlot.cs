using UnityEngine;

namespace Assets.Scripts.Runtime {
    public class PetitionerSlot : MonoBehaviour {
        [SerializeField]
        public int spriteOrderIndex = 0;
        [SerializeField]
        public bool setSpriteOrderIndex = false;
        [SerializeField]
        public bool isThroneSlot = false;

        [SerializeField]
        Petitioner m_petitioner;
        public Petitioner petitioner {
            get => m_petitioner;
            set {
                m_petitioner = value;

                if (setSpriteOrderIndex && value && value.TryGetComponent<SpriteRenderer>(out var renderer)) {
                    renderer.sortingOrder = spriteOrderIndex;
                }
            }
        }

        public bool IsFree() => !petitioner;
    }
}
