using UnityEngine;
using UnityEngine.AI;

namespace Script {
    public class RSpotLight : MonoBehaviour {
        private bool _powered = true;
        private Light _light;
        private NavMeshObstacle _navMeshObstacle;

        private void Start() {
            _light = gameObject.GetComponent<Light>();
            _navMeshObstacle = gameObject.GetComponent<NavMeshObstacle>();
        }

        public void Toggle() {
            _powered = !_powered;

            _light.enabled = _powered;
            _navMeshObstacle.enabled = _powered;
        }
    }
}
