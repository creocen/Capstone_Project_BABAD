using UnityEngine;
namespace Minigame.TowerBuilder
{
    public class CameraController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TowerManager towerManager;

        [Header("Camera Config")]
        [SerializeField] private float heightOffset = 5f; 
        [SerializeField] private float smoothSpeed = 5f; 
        [SerializeField] private float minHeight = 0f; 
        [SerializeField] private bool onlyMoveUp = true; 
        [SerializeField] private int blocksBeforeCameraStarts = 1; 
        [SerializeField] private float movementThreshold = 0.01f; 

        private float targetYPosition;
        private float highestYPosition;
        private bool hasStartedMoving = false;

        private void Start()
        {
            highestYPosition = transform.position.y;
            targetYPosition = transform.position.y;
        }

        private void LateUpdate()
        {
            if (towerManager == null) return;

            int blocksPlaced = towerManager.GetTotalBlocksPlaced();
            if (!hasStartedMoving && blocksPlaced < blocksBeforeCameraStarts)
            {
                return;
            }

            hasStartedMoving = true;

            float towerHeight = towerManager.GetTowerHeight();
            float desiredYPosition = Mathf.Max(minHeight, towerHeight + heightOffset);

            if (onlyMoveUp)
            {
                if (desiredYPosition > highestYPosition)
                {
                    highestYPosition = desiredYPosition;
                }
                targetYPosition = highestYPosition;
            }
            else
            {
                targetYPosition = desiredYPosition;
            }

            Vector3 currentPosition = transform.position;

            if (Mathf.Abs(currentPosition.y - targetYPosition) > movementThreshold)
            {
                float newY = Mathf.Lerp(currentPosition.y, targetYPosition, smoothSpeed * Time.deltaTime);
                transform.position = new Vector3(currentPosition.x, newY, currentPosition.z);
            }
        }

        public void ResetCamera()
        {
            highestYPosition = minHeight;
            targetYPosition = minHeight;
            hasStartedMoving = false;
            transform.position = new Vector3(transform.position.x, minHeight, transform.position.z);
        }
    }
}