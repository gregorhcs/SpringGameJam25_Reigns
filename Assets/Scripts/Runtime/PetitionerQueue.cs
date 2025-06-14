using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Runtime {
    public class PetitionerQueue : MonoBehaviour {
        [SerializeField]
        PetitionerSlot slotPrefab = default;

        [SerializeField]
        int numberOfSlotsToGenerate = 10;

        [SerializeField]
        GameObject queueStart = default;

        [SerializeField]
        GameObject queueEnd = default;

        List<PetitionerSlot> slots = new();

        protected void Start() {
            for (int slotIndex = 0; slotIndex < numberOfSlotsToGenerate; slotIndex++) {
                var slot = Instantiate(slotPrefab);
                slot.transform.position = Vector3.Lerp(queueStart.transform.position, queueEnd.transform.position, slotIndex / (numberOfSlotsToGenerate-1f));
                slots.Add(slot);
            }
            slots.Last().isThroneSlot = true;
        }

        // @TODO ghs: maybe move the code below into separate class?

        [SerializeField]
        GameObject petitionerPrefab = default;

        GameObject SpawnPetitioner(GameObject spawnPoint) {
            var petitioner = Instantiate(petitionerPrefab);
            petitioner.transform.SetPositionAndRotation(spawnPoint.transform.position, spawnPoint.transform.rotation);
            return petitioner;
        }
    }
}
