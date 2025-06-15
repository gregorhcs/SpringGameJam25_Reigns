using System;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime {
    sealed class RigidbodyEvents : MonoBehaviour {
        [SerializeField]
        float killY = 0;

        [SerializeField]
        UnityEvent<GameObject> onKillEvent = new();

        public event Action onKill;

        void RaiseKill() {
            onKillEvent.Invoke(gameObject);
            onKill?.Invoke();
        }

        void OnTriggerEnter(Collider collider) {
            RaiseKill();
        }

        void FixedUpdate() {
            if (transform.position.y < killY) {
                RaiseKill();
            }
        }

        public void InstantiateHere(GameObject prefab) {
            Instantiate(prefab, transform.position, Quaternion.identity);
        }
    }
}