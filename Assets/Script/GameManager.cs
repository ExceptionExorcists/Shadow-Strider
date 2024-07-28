using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script {
    public class GameManager : MonoBehaviour {
        public static GameManager Instance { get; set; }
        public static PlayerController PlayerController { get; set; }
        public static WallhuggerController WallhuggerController { get; set; }

        private GameManager() {
            Instance = this;
        }
        
        public void Reset() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
