using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Player/Actions/Recover")]
    public class RecoverAction : PlayerStateAction
    {
        [SerializeField] float recoverThreshold = 0.5f;
        [SerializeField] float recoverPerInput = 0.1f;

        [SerializeField][ReadOnly] float currentRecover = 0;
        public override void Enter(PlayerStateMachine machine) {
            currentRecover = 0;
        }

        public override void Execute(PlayerStateMachine machine) {
            // player spamming recover input
            if (machine.IsRecovering()) { currentRecover += recoverPerInput; }

            // player successfully recovers
            if (currentRecover >= recoverThreshold) {
                machine.EndStun();
            }
        }

        public override void Exit(PlayerStateMachine machine) {
            machine.EndStun();
        }
    }
}
