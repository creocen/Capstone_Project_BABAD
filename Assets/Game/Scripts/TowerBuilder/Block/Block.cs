using UnityEngine;

namespace Minigame.TowerBuilder
{
    public class Block : MonoBehaviour
    {
        [Header("Block Info")]
        public BlockType blockType = BlockType.Brick;
        public BlockMass blockMass = BlockMass.Base;
        public Block blockBelow;
        public Block blockAbove;
        public bool isPerfectlyAligned = false;

        [Header("BlockWeight Config")]
        [SerializeField] private float alignedMass = 12f;
        [SerializeField] private float brickMass = 8f;
        [SerializeField] private float woodMass = 5.8f;
        [SerializeField] private float strawMass = 3f;

        // [ B A C K L O G ] 
        /*[Header("BlockSize Config")]
        [SerializeField] private float normal = 4f;
        [SerializeField] private float wide = 7.5f;
        [SerializeField] private float narrow = 2f;*/

        [Header("Block Sprites")]
        [SerializeField] private Sprite brickSprite;
        [SerializeField] private Sprite woodSprite;
        [SerializeField] private Sprite strawSprite;

        [Header("References")]
        [SerializeField] private Rigidbody2D body;
        [SerializeField] private BoxCollider2D boxCollider;
        [SerializeField] private SpriteRenderer spriteRenderer;



        public void Initialize(BlockType type, BlockMass mass)
        {
            blockType = type;
            blockMass = mass;
            UpdateMass(blockMass);
            UpdateSprite(blockType);
        }

        public void SetPerfectAlignment(bool aligned)
        {
            isPerfectlyAligned = aligned;

            if(aligned)
            {
                blockMass = BlockMass.Aligned;
                UpdateMass(blockMass);
            }
        }

        private void UpdateMass(BlockMass mass)
        {
            body.mass = mass switch
            {
                BlockMass.Aligned => alignedMass,
                BlockMass.Heavy => brickMass,
                BlockMass.Base => woodMass,
                BlockMass.Light => strawMass,
                _ => brickMass
            };
        }

        private void UpdateSprite(BlockType type)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = type switch
                {
                    BlockType.Brick => brickSprite,
                    BlockType.Wood => woodSprite,
                    BlockType.Straw => strawSprite,
                    _ => brickSprite
                };
            }
        }

        public void MakeKinematic()
        {
            body.bodyType = RigidbodyType2D.Kinematic;
            body.linearVelocity = Vector2.zero;
        }

        public void MakeDynamic()
        {
            body.bodyType = RigidbodyType2D.Dynamic;
        }

        public float GetWidth()
        {
            return boxCollider.bounds.size.x;
        }

        public Vector2 GetPosition()
        {
            return transform.position;
        }
    }

    public enum BlockType
    {
        Brick,
        Wood,
        Straw
    }

    public enum BlockMass
    {
        Aligned,
        Heavy,
        Base,
        Light
    }

    // B A C K L O G
    /*public enum BlockSize
    {
        Wide,
        Normal,
        Narrow
    }*/
}