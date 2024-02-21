using FiniteStateMachine;
using PlayerFiniteStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Decision/WalkSprint")]
public class WalkSprintDecision : PlayerStateDecision
{
    [SerializeField] bool reversed = false;


    public override bool Decide(PlayerStateMachine machine) {
        bool response = Decision(machine);
        if (reversed) {
            return !response;
        } else {
            return response;
        }
    }

    bool Decision(PlayerStateMachine machine) {
        if (machine.CanSprint()) {
            return true;
        }
        return false;
    }
}
