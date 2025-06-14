using Runtime;
using UnityEngine;

namespace Assets.Scripts.Runtime {
    public class Petitioner : MonoBehaviour {
        public bool isAtThrone = false;
        public float royalSupport = 0f;
        public ConcernAsset concern = default;

        public FactionAsset faction => concern.faction;

        [SerializeField]
        Animator animator = default;

        public void SetUp(Transform spawnPoint, ConcernAsset inConcern) {
            transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
            animator.runtimeAnimatorController = inConcern.faction.animatorController;
            concern = inConcern;
        }
    }
}
