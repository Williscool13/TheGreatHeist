using System.Collections.Generic;
using UnityEngine;
using EnemyFiniteStateMachine;
using Sirenix.OdinInspector;
using EnemyAI;
using System.Linq;
using System;
using Unity.Android.Gradle;

public class EnemyStateMachine : MonoBehaviour
{
    [field: SerializeField]
    [field: ReadOnly]
    public EnemyState CurrentState { get; private set; }

    [Title("State Machine Properties")]
    [SerializeField] EnemyState startingState;
    [SerializeField] private Patrol patrolRoute;

    [Title("Components")]
    [SerializeField] CharacterMovement movement;
    [SerializeField] EnemyAim aim;
    [SerializeField] HealthSystem healthSystem;

    [Title("Vision")]
    [SerializeField] float fieldOfView = 90;
    [SerializeField] float viewDistance = 10;
    [SerializeField] LayerMask targetLayerMask;
    [SerializeField] LayerMask flareLayerMask;
    [SerializeField] LayerMask obstacleTargetLayerMask;
    [SerializeField] LayerMask obstacleFlareLayerMask;

    [Title("Movement")]
    [SerializeField] float moveSpeed = 1;
    [SerializeField] float investigateMoveSpeed = 2;
    [SerializeField] float initialPatrolRotateTime = 2f;
    [SerializeField] float patrolRotateSpeedTime = 2f;

    [Title("Idle Properties")]
    [SerializeField] Vector2 patrolIdleTime = new Vector2(2.5f, 4.5f);
    float patrolIdleTimer = 0;
    public float PatrolIdleTimer { get => patrolIdleTimer; set => patrolIdleTimer = value; }
    public float PatrolIdleTime { get => UnityEngine.Random.Range(patrolIdleTime.x, patrolIdleTime.y); }

    [Title("Shoot Properties")]
    [SerializeField] float shootCooldown = 1.0f;
    [SerializeField] float shootRange = 10;

    [Title("Investigate Properties")]
    [SerializeField] float investigateTimeout = 5;

    Vector2 lastKnownPosition;
    IHitbox targetHitbox;
    Transform targetTransform;
    int currentHitboxHitpointIndex = 0;

    public int CurrentHitboxHitpointIndex { get => currentHitboxHitpointIndex; set => currentHitboxHitpointIndex = value; }
    public IHitbox TargetHitbox => targetHitbox;
    public Transform TargetTransform => targetTransform;
    public Vector2 LastKnownPosition => lastKnownPosition;

    public CharacterMovement Movement => movement;
    public EnemyAim Aim => aim;
    public HealthSystem HealthSystem => healthSystem;

    public Patrol PatrolRoute => patrolRoute;
    public int PatrolIndex { get; private set; }
    public PatrolSequence CurrentPatrolSequence { get; private set; }

    public float ShootRange => shootRange;
    public float MoveSpeed => moveSpeed;
    public float InvestigateMoveSpeed => investigateMoveSpeed;
    public float ViewDistance => viewDistance;
    public float FieldOfView => fieldOfView;
    public LayerMask TargetLayerMask => targetLayerMask;
    public LayerMask FlareLayerMask => flareLayerMask;
    public LayerMask ObstacleTargetLayerMask => obstacleTargetLayerMask;
    public LayerMask ObstacleFlareLayerMask => obstacleFlareLayerMask;

    List<GameObject> previousInvestigatedFlares = new List<GameObject>();

    float investigateTimestamp = 0;
    public float InvestigateTimestamp { get => investigateTimestamp; set => investigateTimestamp = value; }
    public int InvestigateTimeout => (int)investigateTimeout;
    public List<GameObject> PreviousInvestigatedFlares { get => previousInvestigatedFlares; set => previousInvestigatedFlares = value; }

    public void OnFlareReleased_RemoveFromInvestigatedFlares(object o, LightFlare flare) {
        if (previousInvestigatedFlares.Contains(flare.gameObject)){
            previousInvestigatedFlares.Remove(flare.gameObject);
        }
        if (o is LightFlare l) {
            l.OnFlareRelease -= OnFlareReleased_RemoveFromInvestigatedFlares;
        }
    }

    float shootTimestamp = 0;

    #region State Machine Functions
    private void Start() {
        ChangeState(startingState);
    }
    void Update()
    {
        CurrentState.Execute(this);
    }
    public void ChangeState(EnemyState state) {
        if (CurrentState != null) {
            CurrentState.Exit(this);
        }
        CurrentState = state;
        CurrentState.Enter(this);
    }
    #endregion



    #region Shoot Functions
    public bool CanShoot() {
        return shootTimestamp + shootCooldown < Time.time;
    }
    public void Shoot() {
        shootTimestamp = Time.time;

    }

    public Vector2 GetWorldShootPoint() {
        return transform.position;
    }
    #endregion


    #region Investigate Functions
    public void SetInvestigationPoint(Vector2 v) {
        lastKnownPosition = v;
    }
    public void SetTargetInformation(Transform t, IHitbox h) {
        targetTransform = t;
        targetHitbox = h;
    }
    #endregion

    #region Patrol Functions
    public bool HasPatrolRoute() {
        return patrolRoute != null;
    }
    public bool IsPatrolling() {
        return CurrentPatrolSequence != null && CurrentPatrolSequence.Active;
    }
    public void IncremenetPatrolIndex() {
        PatrolIndex = (PatrolIndex + 1) % patrolRoute.patrolData.Length;
    }

    public void Patrol(Vector2 target, Vector2 finalFaceDirection) {
        Debug.Assert(!IsPatrolling(), "Patrol sequence is already active");

        CurrentPatrolSequence = new PatrolSequence(movement, aim, transform, target, finalFaceDirection, moveSpeed, initialPatrolRotateTime, patrolRotateSpeedTime);
        CurrentPatrolSequence.Start();
    }


    public void StopPatrol() {
        if (IsPatrolling()) {
            CurrentPatrolSequence.Stop();
        }
    }
    #endregion

    #region Backtrack Functions
    PatrolSequence currentBacktrackSequence;
    [ReadOnly][SerializeField] Stack<Vector2> backtrackHistory = new Stack<Vector2>();
    public int backtrackCount => backtrackHistory.Count;
    [ReadOnly]
    [SerializeField] List<Vector2> investigatePath = new List<Vector2>();
    public void AddInvestigatePath(Vector2 pos) {
        Debug.Log("Adding " + pos + " to the queue of move commands");
        backtrackHistory.Push(pos);
        investigatePath.Add(pos);
    }
    public bool IsBacktracking() {
        return currentBacktrackSequence != null && currentBacktrackSequence.Active;
    }

    public void Backtrack() {
        Debug.Assert(!IsBacktracking(), "Backtrack sequence is already active");

        if (backtrackHistory.Count == 0) {
            Debug.Log("No backtrack history, returning to patrol state (using state transition)");
            return;
        }

        Vector2 peek = backtrackHistory.Peek();
        investigatePath.RemoveAt(0);
        currentBacktrackSequence = new PatrolSequence(movement, aim, transform, peek, peek - (Vector2)transform.position, moveSpeed, initialPatrolRotateTime, patrolRotateSpeedTime);
        currentBacktrackSequence.OnComplete += OnBacktrackSequenceComplete;
        currentBacktrackSequence.Start();
    }

    void OnBacktrackSequenceComplete(object o, EventArgs e) {
        backtrackHistory.Pop();
        if (o is PatrolSequence p) {
            p.OnComplete -= OnBacktrackSequenceComplete;
        }
    }

    public void StopBacktrack() {
        if (IsBacktracking()) {
            currentBacktrackSequence.Stop();
        }
    }

    #endregion

    private EnemyCollisionData previousCollisionData = new EnemyCollisionData() { isColliding = false };
    public EnemyCollisionData PreviousCollisionData => previousCollisionData;

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.collider.CompareTag("Player")) {
            previousCollisionData = new EnemyCollisionData() {
                isColliding = true,
                collisionTimestamp = Time.time,
                collision = collision,
                collisionPoint = collision.GetContact(0).point,
                collisionNormal = collision.GetContact(0).normal,
                collisionDirection = collision.GetContact(0).point - (Vector2)transform.position
            };
            // contact with player, add to potential investigation data
        }
    }

#if UNITY_EDITOR
    public void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, lastKnownPosition);
    }
#endif

    // to tell other components if enemy is colliding with player
    public struct EnemyCollisionData 
    {
        public bool isColliding;
        public float collisionTimestamp;
        public Collision2D collision;

        public Vector2 collisionPoint;
        public Vector2 collisionNormal;
        public Vector2 collisionDirection;
        
    }

    public struct InvestigateData
    {
        public bool targetFound;
        public Vector2 targetPosition;
        public Transform target;
        public IHitbox targetHitbox;
    }
}