using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float MoveSpeed => moveSpeed;
    [ReadOnly][SerializeField] float moveSpeed;
    [ReadOnly][SerializeField] Movement currentMovementType;
    [ReadOnly][SerializeField] Vector2 forcedMovementDirection;
    [SerializeField] Rigidbody2D rb;
    public Movement CurrentMovementType => currentMovementType;


    public event EventHandler<MovementChangeEventArgs> OnMovementTypeChanged;
    public enum Movement
    {
        None,
        Standard,
        Sprinting,
        Forced,
        Stunned,
    }



    public void SetMovementProperties(float speed, Movement type) {
        MovementChangeEventArgs args = new MovementChangeEventArgs() { previous = currentMovementType, current = type };
        moveSpeed = speed; 
        currentMovementType = type;
        OnMovementTypeChanged?.Invoke(this, args);

    }

    public void SetForcedMovementDirection(Vector2 dir) {
        forcedMovementDirection = dir.normalized;
    }

    /// <summary>
    /// Returns the last direction the player moved in. Used to set facing direction if not aiming.
    /// </summary>
    /// <returns></returns>
    public Vector2 GetLastMovementDirection() {
        return lastMoveDirection;
    }



    Vector2 lastMoveDirection;
    /// <summary>
    /// /// Commands the player to move in a direction. 
    /// Returns the actual distance moved.
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public void Move(Vector2 moveInputs) {
        if (currentMovementType == Movement.Forced) {
            moveInputs = forcedMovementDirection.normalized;
        }

        //lastMoveDirection = rb.velocity;
        //rb.velocity = moveInputs * moveSpeed;
        
        targetVel = moveInputs * moveSpeed;
    }
    Vector2 targetVel = Vector2.zero;
    private void FixedUpdate() {
        rb.AddForce(targetVel - rb.velocity, ForceMode2D.Impulse);
        //rb.velocity = targetVel;
        lastMoveDirection = rb.velocity;
        targetVel = Vector2.zero;
    }

    public void StopMovement() {
        rb.velocity = Vector2.zero;
    }

    public void SetKinematic(bool kin) {
        rb.isKinematic = kin;
    }

    public class MovementChangeEventArgs : EventArgs {
        public Movement previous;
        public Movement current;
    }
}

