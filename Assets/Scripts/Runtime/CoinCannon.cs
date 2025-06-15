using System;
using MyBox;
using UnityEngine;
using URandom = UnityEngine.Random;

namespace Runtime {
    sealed class CoinCannon : MonoBehaviour {
        [SerializeField]
        Rigidbody coinPrefab;

        [SerializeField]
        float angle = 45;
        [SerializeField]
        float angleRange = 10;
        [SerializeField]
        float speed = 10;
        [SerializeField]
        float speedRange = 5;

        void OnDrawGizmos() {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, transform.position + (Quaternion.Euler(0, 0, angle - angleRange) * Vector3.up));
            Gizmos.DrawLine(transform.position, transform.position + (Quaternion.Euler(0, 0, angle + angleRange) * Vector3.up));
        }

        [ContextMenu(nameof(TestShoot))]
        void TestShoot() {
            Shoot(() => Debug.Log("Hit!"));
        }

        public void Shoot(Action onHit) {
            var instance = Instantiate(coinPrefab, transform);

            float instanceAngle = Mathf.Lerp(angle - angleRange, angle + angleRange, URandom.value);
            float instanceSpeed = Mathf.Lerp(speed - speedRange, speed + speedRange, URandom.value);

            var direction = Quaternion.Euler(0, 0, instanceAngle) * Vector3.up;
            var force = direction * instanceSpeed;

            instance.AddForce(force, ForceMode.VelocityChange);
            var events = instance.GetOrAddComponent<RigidbodyEvents>();

            events.onKill += () => Destroy(instance.gameObject);
            events.onKill += onHit;
        }
    }
}