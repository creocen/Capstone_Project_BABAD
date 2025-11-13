using UnityEngine;
using System.Collections;

namespace Minigame.TowerBuilder
{
    public class Wolf : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private BulletPooling bulletPooling;

        [Header("Switch Config")]
        [SerializeField] private Transform positionLeft;
        [SerializeField] private Transform positionRight;
        [SerializeField] private float switchInterval = 3f;
        [SerializeField] private bool atPositionRight = true;

        private void Start()
        {
            StartCoroutine(SwitchRoutine());
        }

        private void Update()
        {
            
        }

        private IEnumerator SwitchRoutine()
        {
            /*if (bulletPooling != null)
            {
                bulletPooling.canAttack = false;
            }*/

            while (true)
            {
                yield return new WaitForSeconds(switchInterval);

                transform.position = atPositionRight ? positionLeft.position : positionRight.position;

                Vector3 currRotation = transform.eulerAngles;
                currRotation.y = atPositionRight ? 0f : 180f;
                transform.eulerAngles = currRotation;

                atPositionRight = !atPositionRight;

                bulletPooling.canAttack = true;
            }

        }
    }
}

