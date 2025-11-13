using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

namespace Minigame.TowerBuilder
{
    public class TowerManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private DropBlock dropController;
        [SerializeField] private Transform spawnPoint;

        [Header("Config Instantiate")]
        [SerializeField] private GameObject blockPrefab;
        [SerializeField] private float spawnHeight = 10f;
        [SerializeField] private float spawnDelay = 1f;
        [SerializeField] private float alignmentTolerance = 0.01f;

        [Header("Stage Progression Settings")]
        [SerializeField] private int brickAlignedThreshold = 8;
        [SerializeField] private int brickStackThreshold = 20;
        [SerializeField] private int woodAlignedThreshold = 6;
        [SerializeField] private int woodStackThreshold = 25;

        [Header("Tower Status")]
        [SerializeField] private int totalBlocksPlaced = 0;
        [SerializeField] private int currentAlignedStreak = 0;
        [SerializeField] private int maxAlignedStreak = 0;
        [SerializeField] private bool hasMetBrickRequirement = false;
        [SerializeField] private bool hasMetWoodRequirement = false;
        [SerializeField] private List<Block> placedBlocks = new List<Block>();
        [SerializeField] private BlockType currentStage = BlockType.Brick;
        [SerializeField] private Block lastPlacedBlock;
        [SerializeField] private bool isWaitingForNextBlock = false;

        private void Start()
        {
            SpawnNextBlock();
        }

        private void SpawnNextBlock()
        {
            if (isWaitingForNextBlock) return;

            Vector3 spawnPosition = spawnPoint != null ? spawnPoint.position : new Vector3(0, spawnHeight, 0);
            GameObject iBlock = Instantiate(blockPrefab, spawnPosition, Quaternion.identity);
            Block block = iBlock.GetComponent<Block>();

            if (block != null)
            {
                BlockMass massType = block.blockMass;
                block.Initialize(currentStage, massType);
                dropController.StartMovingBlock(block);
            }
        }

        public void OnBlockDropped(Block block)
        {
            totalBlocksPlaced++;

            bool isAligned = CheckAlignment(block);

            if (isAligned)
            {
                currentAlignedStreak++;
                maxAlignedStreak = Mathf.Max(maxAlignedStreak, currentAlignedStreak);
                block.SetPerfectAlignment(true);
            } else
            {
                currentAlignedStreak = 0;
            }

            if (lastPlacedBlock != null)
            {
                block.blockBelow = lastPlacedBlock;
                lastPlacedBlock.blockAbove = block;
            }

            placedBlocks.Add(block);
            lastPlacedBlock = block;

            CheckStageProgression();

            isWaitingForNextBlock = true;
            Invoke(nameof(ReadyNextBlock), spawnDelay);
        }

        private bool CheckAlignment(Block block)
        {
            if (lastPlacedBlock == null) return false;

            float xPos = block.GetPosition().x;
            float lastXPos = lastPlacedBlock.GetPosition().x;
            float distance = Mathf.Abs(xPos - lastXPos);

            return distance <= alignmentTolerance;
        }

        private void CheckStageProgression()
        {
            switch (currentStage)
            {
                case BlockType.Brick:
                    if (maxAlignedStreak >= brickAlignedThreshold && totalBlocksPlaced >= brickStackThreshold)
                    {
                        hasMetBrickRequirement = true;
                    }
                    else if (totalBlocksPlaced >= brickStackThreshold && maxAlignedStreak < brickStackThreshold)
                    {
                        TransitionToStage(BlockType.Wood);
                    }
                    break;
                case BlockType.Wood:
                    if (maxAlignedStreak >= woodAlignedThreshold && totalBlocksPlaced >= woodStackThreshold)
                    {
                        hasMetWoodRequirement = true;
                    } 
                    else
                    {
                        TransitionToStage(BlockType.Straw);
                    }
                    break;
                case BlockType.Straw:
                    // B A C K L O G : transition to next scene or dialogue sequence
                    break;
            }
        }

        private void TransitionToStage(BlockType nextStage)
        {
            currentStage = nextStage;
        }

        private void ReadyNextBlock()
        {
            isWaitingForNextBlock = false;
            SpawnNextBlock();
        }
    }
}