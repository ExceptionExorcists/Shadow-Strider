using UnityEngine;

namespace Script {
    public class MeshFader : MonoBehaviour {
        public float fadeSpeed = 10.0f;
        public float fadeAmount = 0.5f;
        public bool shouldBeVisible = true;
        private float _defaultOpacity;
        private Material _material;
        
        private void Start() {
            _material = GetComponent<Renderer>().material;
            _defaultOpacity = _material.color.a;
        }

        private void Update() {
            if (shouldBeVisible) {
                UnFade();
            } else {
                Fade();
            }
        }

        private void Fade() {
            Color currentColor = _material.color;
            Color smoothColor = new Color(
                currentColor.r,
                currentColor.g,
                currentColor.b, 
                Mathf.Lerp(currentColor.a, fadeAmount, fadeSpeed * Time.deltaTime)
            );

            _material.color = smoothColor;
        }

        private void UnFade() {
            Color currentColor = _material.color;
            Color smoothColor = new Color(
                currentColor.r,
                currentColor.g,
                currentColor.b, 
                Mathf.Lerp(currentColor.a, _defaultOpacity, fadeSpeed * Time.deltaTime)
            );

            _material.color = smoothColor;
        }
    }
}
