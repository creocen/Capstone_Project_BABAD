using UnityEngine;
using UnityEngine.SceneManagement;

namespace Minigame.TowerBuilder
{
    public class GameManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TowerManager towerManager;
        [SerializeField] private GameObject gameOverPanel;

        [Header("Game Status")]
        [SerializeField] public BlockType CurrentStage;

        private static BlockType savedStage = BlockType.Brick;

        private void Awake()
        {
            CurrentStage = savedStage;
        }

        private void Start()
        {
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(false);
            }
        }

        public void OnGameOver()
        {
            if (towerManager != null)
            {
                BlockType nextStage = towerManager.CheckStageProgression();
                CurrentStage = nextStage;
                savedStage = nextStage;
            }

            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
            }
        }

        public void ReloadScene()
        {
            if (towerManager != null)
            {
                towerManager.ResetForNewGame();
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

