using PlayerFiniteStateMachine;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Decision/WalkDash")]
public class WalkDashDecision : PlayerStateDecision
{
    public override bool Decide(PlayerStateMachine machine) {
        if (machine.CanDash()) {
            // cooldown
            return true;
        }
        return false;
    }

}
