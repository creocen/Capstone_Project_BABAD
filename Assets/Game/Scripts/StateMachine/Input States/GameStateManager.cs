using UnityEngine;
using Core.PlayerInput;
using Core.InputStates;
using Core.State_Machine;

using Core.Movement;
using Minigame.TowerBuilder;

namespace Core.InputStates
{
    public class GameStateManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Animator animator;
        [SerializeField] private InputReader inputReader;

        [Header("Player Controller References")]
        [SerializeField] private PlayerMovement playerBaseMovement;
        [SerializeField] private DropBlock playerBlockDropper;
        //[SerializeField] private (Player Controller) playerMSGCatcher;
        //[SerializeField] private (Player Controller) playerPaddle;

        private StateMachine stateMachine;
        private PlayerBaseState baseInputState;
        private TowerBuildingState towerBuildingState;

        void Awake()
        {
            stateMachine = new StateMachine();

            baseInputState = new PlayerBaseState(playerBaseMovement, animator, inputReader);
            towerBuildingState = new TowerBuildingState(playerBlockDropper, animator, inputReader);

            stateMachine.SetState(baseInputState);
        }

        void Update()
        {
            stateMachine.Update();
        }

        void FixedUpdate()
        {
            stateMachine.FixedUpdate();
        }

        public void StartTowerBuilding()
        {
            stateMachine.ChangeState(towerBuildingState);
            Debug.Log("Start Tower Builing State");
        }

        public void StopTowerBuilding()
        {
            stateMachine.ChangeState(baseInputState);
            Debug.Log("Exiting Tower Builing State");
        }

    }
}
