using Slothsoft.UnityExtensions;
using UnityEngine;

namespace Runtime {
    sealed class FactionBar : MonoBehaviour {
        [SerializeField, Expandable]
        FactionAsset asset;
        [SerializeField]
        SpriteRenderer bar;

        Vector3 initialPosition = default;

        void Start() {
            bar.sprite = asset.banner;
            initialPosition = bar.transform.position;
            UpdateHeight(1000);
        }

        void LateUpdate() {
            UpdateHeight(Time.deltaTime);
        }

        [SerializeField]
        float smoothTime = 1;
        [SerializeField]
        float minHeight = -10;
        [SerializeField]
        float maxHeight = 6;

        [Header("Runtime")]
        [SerializeField]
        float height;
        [SerializeField]
        float speed;

        void UpdateHeight(float deltaTime) {
            if (bar && asset) {
                height = Mathf.SmoothDamp(height, asset.GetLoyalty(), ref speed, smoothTime, float.PositiveInfinity, deltaTime);
                bar.transform.SetPositionAndRotation(initialPosition + (Mathf.Lerp(minHeight, maxHeight, height / 100) * Vector3.up), Quaternion.identity);
            }
        }
    }
}