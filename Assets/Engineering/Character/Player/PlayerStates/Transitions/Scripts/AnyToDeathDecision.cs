using PlayerFiniteStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Decision/AnyLaugh")]
public class AnyToDeathDecision : PlayerStateDecision
{
    [SerializeField] bool reversed = false;
    public override bool Decide(PlayerStateMachine machine) {
        if (reversed) {
            return !Decision(machine);
        }
        return Decision(machine);
    }

    bool Decision(PlayerStateMachine machine) {
        return machine.IsDead();
    }
}
