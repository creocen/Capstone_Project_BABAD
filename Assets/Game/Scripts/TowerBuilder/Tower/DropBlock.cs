using UnityEngine;
using Core.PlayerInput;
using Core.InputStates;

namespace Minigame.TowerBuilder
{
    public class DropBlock : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private InputReader inputReader;
        [SerializeField] private TowerManager towerManager;
        [SerializeField] private GameStateManager stateManager;

        [Header("Movement Settings")]
        [SerializeField] private float movementSpeed = 3f;
        [SerializeField] private float movementRange = 5f; // How far left/right it travels
        [SerializeField] private bool startMovingRight = true;

        private Block currentBlock;
        private float direction = 1f;
        private bool isMoving = false;
        private bool canDrop = true;


        #region Enable-Disable Input
        void OnEnable()
        {
            if (inputReader != null)
            {
                inputReader.DropBlockPressed += HandleBlockDrop;
            }
        }

        void OnDisable()
        {
            if (inputReader != null)
            {
                inputReader.DropBlockPressed -= HandleBlockDrop;
            }
        }
        #endregion

        void Start()
        {
            stateManager.StartTowerBuilding(); // [TEMP] for demo purposes
            direction = startMovingRight ? 1f : -1f;
        }

        private void Update()
        {
            if (isMoving && currentBlock != null)
            {
                MoveBlock();
            }
        }

        private void MoveBlock()
        {
            Vector3 position = currentBlock.transform.position;
            position.x += direction * movementSpeed * Time.deltaTime;

            if (position.x > movementRange)
            {
                position.x = movementRange;
                direction = -1f;
            }
            else if (position.x <= -movementRange)
            {
                position.x = -movementRange;
                direction = 1f;
            }

            currentBlock.transform.position = position;
        }

        public void StartMovingBlock(Block block)
        {
            currentBlock = block;
            currentBlock.MakeKinematic();
            isMoving = true;
            canDrop = true;
        }

        private void HandleBlockDrop()
        {
            if (!canDrop || currentBlock == null || !isMoving) return;

            canDrop = false;
            isMoving = false;

            currentBlock.MakeDynamic();

            if (towerManager != null)
            {
                towerManager.OnBlockDropped(currentBlock);
            }

            currentBlock = null;
        }

        public void StopMovement()
        {
            isMoving = false;
            currentBlock = null;
        }

    }
}