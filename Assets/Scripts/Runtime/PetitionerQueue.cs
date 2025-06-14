using System.Collections.Generic;
using System.Linq;
using Runtime;
using UnityEngine;

namespace Assets.Scripts.Runtime {
    public class PetitionerQueue : MonoBehaviour {
        [SerializeField]
        PetitionerSlot slotPrefab = default;

        [SerializeField]
        int numberOfSlotsToGenerate = 10;

        [SerializeField]
        GameObject petitionerSpawnPoint = default;

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

            SpawnPetitioner();
        }

        // @TODO ghs: maybe move the code below into separate class?

        [SerializeField]
        Petitioner petitionerPrefab = default;

        [SerializeField]
        ConcernsLibraryAsset concernsLibrary = default;

        Petitioner SpawnPetitioner() {
            var petitioner = Instantiate(petitionerPrefab);

            var concern = concernsLibrary.concerns[Random.Range(0, concernsLibrary.concerns.Count)];
            petitioner.SetUp(petitionerSpawnPoint.transform, concern);

            return petitioner;
        }
    }
}
