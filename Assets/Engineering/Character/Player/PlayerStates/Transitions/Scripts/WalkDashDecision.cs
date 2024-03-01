using PlayerFiniteStateMachine;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Decision/WalkDash")]
public class WalkDashDecision : PlayerStateDecision
{
    public override bool Decide(PlayerStateMachine machine) {
        if (machine.CanDash()) {
            machine.Dash();
            return true;
        }
        return false;
    }

}
