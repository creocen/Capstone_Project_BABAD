using UnityEngine;

namespace Core.Interactions
{
    public class SampleInteraction : MonoBehaviour, IInteractable
    {
        [Header("Info")]
        [SerializeField] private string npcName;
        [SerializeField] private GameObject DialogueBox;


        public string InteractionPrompt => $"Press [E] to interact with {npcName}.";
        public Transform Transform => this.transform;

        public void Interact(GameObject player)
        {
            DialogueBox.SetActive(true);
        }
    }
}
