using UnityEngine;
using Core.InputStates;

namespace Minigame.TowerBuilder
{
    [RequireComponent(typeof(Collider2D))]
    public class TowerCollapseChecker : MonoBehaviour
    {[Header("References")]
        [SerializeField] private GameManager gameManager;
        [SerializeField] private BoxCollider2D collider2D;
        private float collapseDelay = 0.5f;
        private bool gameOverTriggered = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (gameOverTriggered) return;

            if (!gameOverTriggered)
            {
                TriggerGameOver();
            }
        }

        private void TriggerGameOver()
        {
            if (gameOverTriggered) return;

            gameOverTriggered = true;

            Invoke(nameof(ShowGameOver), collapseDelay);
        }

        private void ShowGameOver()
        {
            if (gameManager != null)
            {
                gameManager.OnGameOver();
            }
        }

        public void ResetChecker()
        {
            gameOverTriggered = false;
        }
    }
}