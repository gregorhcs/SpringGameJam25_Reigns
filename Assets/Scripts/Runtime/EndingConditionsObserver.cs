using System;
using Assets.Scripts.Runtime;
using UnityEngine;

namespace Runtime {
    sealed class EndingConditionsObserver : MonoBehaviour {

        [SerializeField]
        HighscoresAsset highscoresAsset = default;

        [SerializeField]
        EndScreen endScreen = default;

        public void Start()
        {
            Petitioner.onPetitionerLeaves += HandlePetitionerLeaves;
            FactionAsset.onFactionLoyaltyReachesZero += HandleFactionLoyaltyReachesZero;
            HighscoresAsset.onConcernHandled += HandleConcernHandled;

            highscoresAsset.currentRun = new HighscoreEntry {
                startTime = DateTime.Now
            };

            GameState.ChangeHasEnded(false);
        }

        void HandleConcernHandled() {
            if (highscoresAsset.currentRun.year == 40) {
                highscoresAsset.currentRun.endTime = DateTime.Now;
                highscoresAsset.currentRun.died = true;
                highscoresAsset.highscores.Add(highscoresAsset.currentRun);
                Unsubscribe();
                GameState.ChangeHasEnded(true);
                endScreen.Open();
                Debug.Log("Died of old age!");
            }
        }

        void Unsubscribe() {
            Petitioner.onPetitionerLeaves -= HandlePetitionerLeaves;
            FactionAsset.onFactionLoyaltyReachesZero -= HandleFactionLoyaltyReachesZero;
            HighscoresAsset.onConcernHandled -= HandleConcernHandled;
        }

        void HandlePetitionerLeaves(Petitioner leavingPetitioner) {
            highscoresAsset.currentRun.handledConcerns++;
            HandleConcernHandled();
        }

        void HandleFactionLoyaltyReachesZero(FactionAsset faction) {
            highscoresAsset.currentRun.endTime = DateTime.Now;
            highscoresAsset.currentRun.died = false;
            highscoresAsset.highscores.Add(highscoresAsset.currentRun);
            Unsubscribe();
            GameState.ChangeHasEnded(true);
            endScreen.Open();
            Debug.Log("Cast down by rebels!");
        }
    }
}
