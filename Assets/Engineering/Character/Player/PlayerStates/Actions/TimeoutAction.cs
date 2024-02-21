using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine 
{
    [CreateAssetMenu(menuName = "Player/Actions/Timeout")]
    public class TimeoutAction : PlayerStateAction
    {
        [SerializeField] private float time;
        [SerializeField] PlayerState targetState;
        float timer;
        public override void Enter(PlayerStateMachine machine) {
            timer = time;
        }

        public override void Execute(PlayerStateMachine machine) {
            timer -= Time.deltaTime;
            if (timer <= 0) {
                machine.ChangeState(targetState);
            }
        }

        public override void Exit(PlayerStateMachine machine) {
        }

    }

}
