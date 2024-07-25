using UnityEngine;

namespace Script {
    public class MeshFader : MonoBehaviour {
        public float fadeSpeed = 1.0f;
        public float fadeAmount = 0.5f;
        public bool shouldBeVisible = true;
        
        private float _defaultOpacity = 1.0f;
        private Renderer _renderer;
        private Material _material;
        
        private void Start() {
            _renderer = GetComponent<Renderer>();
            if (_renderer is null) return;
            
            _material = _renderer.material;
            if (_material is null) return;
            
            _defaultOpacity = _material.color.a;
        }

        private void Update() {
            if (_material is null) return;

            if (shouldBeVisible) {
                UnFade();
            } else {
                Fade();
            }
        }

        private void Fade() {
            if (Mathf.Approximately(_material.color.a, fadeAmount)) return;
            
            SetMaterialOpacity(Mathf.Lerp(_material.color.a, fadeAmount, fadeSpeed * Time.deltaTime));
        }

        private void UnFade() {
            if (Mathf.Approximately(_material.color.a, _defaultOpacity)) return;
            
            SetMaterialOpacity(Mathf.Lerp(_material.color.a, _defaultOpacity, fadeSpeed * Time.deltaTime));
        }

        private void SetMaterialOpacity(float opacity) {
            Color currentColor = _material.color;
            _material.color = new Color(currentColor.r, currentColor.g, currentColor.b, opacity);
        }
    }
}
