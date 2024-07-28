using System.Collections;
using TMPro;
using UnityEngine;

namespace Script {
    public class TextFader : MonoBehaviour {
        public float fadeDuration = 2.0f;

        private TextMeshProUGUI _textMeshProUGUI;
        private Color _defaultColor;
        
        private void Start() {
            _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
            _defaultColor = _textMeshProUGUI.color;
            
            StartCoroutine(FadeOut());
        }

        private IEnumerator FadeOut() {
            Color endColor = new Color(_defaultColor.r, _defaultColor.g, _defaultColor.b, 0.0f);
            float time = 0.0f;
            while (time < fadeDuration) {
                time += Time.deltaTime;
                _textMeshProUGUI.color = Color.Lerp(_defaultColor, endColor, time / fadeDuration);
                yield return null;
            }
        }
    }
}
