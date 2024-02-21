using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Player/Actions/Move")]
    public class MoveAction : PlayerStateAction
    {
        [SerializeField] private float movementSpeed;
        [SerializeField] private CharacterMovement.Movement movement;

        public override void Enter(PlayerStateMachine machine) {
            machine.SetMovementProperties(movementSpeed, movement);

            if (movement == CharacterMovement.Movement.Forced) {
                Vector2 dir = machine.GetLastMovementDirection();
                machine.SetMovementForcedDirection(dir);
                Debug.Log("Set forced movement direction to " + dir);
            }
        }

        public override void Execute(PlayerStateMachine machine) {
        }

        public override void Exit(PlayerStateMachine machine) {
        }
    }
}
