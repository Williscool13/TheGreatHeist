using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerFiniteStateMachine;
using Sirenix.OdinInspector;
public class PlayerStateMachine : MonoBehaviour, IStunnable
{
    [field: SerializeField]
    [field: ReadOnly]
    public PlayerState CurrentState { get; private set; }

    [SerializeField] private PlayerState startingState;

    [SerializeField] private CharacterMovement movement;
    [SerializeField] private PlayerAim aim;
    [SerializeField] private PlayerLoadoutManager loadoutManager;
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private FlareManager flareManager;

    [SerializeField] private StatusEffectManager statusEffectManager;

    void Start()
    {
        ChangeState(startingState);
    }


    void Update()
    {
        CurrentState.Execute(this);
    }

    private void FixedUpdate() {
        movement.Move(move);
    }



    public void ChangeState(PlayerState newState) {
        if (CurrentState != null) {
            CurrentState.Exit(this);
        }
        CurrentState = newState;
        CurrentState.Enter(this);
    }

    #region Movement Functions
    public void SetMovementProperties(float speed, CharacterMovement.Movement movement) {
        this.movement.SetMovementProperties(speed, movement);
    }
    public void SetMovementForcedDirection(Vector2 dir) {
        movement.SetForcedMovementDirection(dir);
    }

    public bool CanSprint() {
        return sprintHeld && move.magnitude > 0.1f;
    }
    public bool CanDash() {
        return dashPressed && movement.GetLastMovementDirection().magnitude > 0.1f;
    }
    public Vector2 GetLastMovementDirection() {
        return movement.GetLastMovementDirection();
    }
    public bool IsMoveInput() {
        return move.magnitude > 0.1f;
    }

    #endregion

    #region Aim Functions
    public void Look() {
        aim.Look();
    }
    public void CustomLook(Vector2 lookDir, bool crosshairMove) {
        aim.CustomLook(lookDir, crosshairMove);
    }
    public void SetAimProperties(float speed, float rotationSpeed) {
        aim.SetAimProperties(speed, rotationSpeed);
    }
    #endregion

    #region Loadout Functions
    public bool CanShoot() {

        return loadoutManager.CanShoot(firePress, movement.CurrentMovementType == CharacterMovement.Movement.Sprinting);
    }

    public void Shoot() {
        loadoutManager.Shoot(aim.GetMousePositionWorld());
    }
    #endregion

    #region Flare Functions
    public bool CanFlare() {
        return flareManager.CanFlare(flarePressed);
    }

    public void Flare() {
        flareManager.Flare();
    }
    #endregion

    #region Status Effect Queries
    public bool IsDead() {
        return healthSystem.IsDead;
    }

    #region Stun
    public bool IsStunned() {
        return statusEffectManager.Stunned;
    }
    public bool IsInvulnerable() {
        return statusEffectManager.Invulnerable;
    }

    public void Stun(IStunnable.StunDuration duration) { statusEffectManager.StartStunTimer(float.MaxValue); }
    public void EndStun() { statusEffectManager.EndStunTimer(); }

    public bool IsRecovering() {
        return recoverPressed;
    }

    public void SetInvulnerable(bool v) {
        statusEffectManager.SetInvulnerable(v);
    }
    #endregion

    #endregion

    #region Footstep FUnctions
    [SerializeField] private FootstepManager footstepManager;
    float footstepTimer;
    public float FootstepTimer { get { return footstepTimer; } set {  footstepTimer = value; } }
    public FootstepManager FootstepManager { get { return footstepManager; } }
    public bool IsStairs() {
        return footstepManager.IsStairs;
    }
    public void PlayWalk() {
        footstepManager.PlayWalk();
    }

    public void PlayRun() {
        footstepManager.PlayRun();
    }

    #endregion


    bool firePress;
    bool fireHeld;
    bool recoverPressed;
    bool recoverHeld;
    bool flarePressed;
    bool flareHeld;
    bool sprintPressed;
    bool sprintHeld;
    bool dashPressed;
    bool dashHeld;
    Vector2 move;
    Vector2 look;


    public void SetInputs(
        Vector2 move, Vector2 look,
        bool firePress, bool fireHeld,
        bool recoverPressed, bool recoverHeld,
        bool flarePressed, bool flareHeld, 
        bool sprintPressed, bool sprintHeld,
        bool dashPressed, bool dashHeld) {
        
        this.move = move;
        this.look = look;
        this.firePress = firePress;
        this.fireHeld = fireHeld;
        this.recoverPressed = recoverPressed;
        this.recoverHeld = recoverHeld;
        this.flarePressed = flarePressed;
        this.flareHeld = flareHeld;
        this.sprintPressed = sprintPressed;
        this.sprintHeld = sprintHeld;
        this.dashPressed = dashPressed;
        this.dashHeld = dashHeld;
    }

}


public interface IStunnable
{
    public void Stun(StunDuration duration);


    public enum StunDuration
    {
        Short = 0,
        Medium = 1,
        Long = 2
    }
}