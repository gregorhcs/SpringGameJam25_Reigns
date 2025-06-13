using MyBox;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace Runtime {
    sealed class FactionBar : MonoBehaviour {
        [SerializeField, Expandable]
        FactionAsset asset;
        [SerializeField]
        SpriteRenderer bar;

        void Start() {
            UpdateHeight(1000);
        }

        void LateUpdate() {
            UpdateHeight(Time.deltaTime);
        }

        [SerializeField]
        float smoothTime = 1;

        [Header("Runtime")]
        [SerializeField, ReadOnly]
        float height;
        [SerializeField, ReadOnly]
        float speed;

        void UpdateHeight(float deltaTime) {
            if (bar && asset) {
                height = Mathf.SmoothDamp(height, asset.currentLoyalty, ref speed, smoothTime, float.PositiveInfinity, deltaTime);

                bar.size = new(1, height / 100);
                bar.color = asset.color;
            }
        }
    }
}