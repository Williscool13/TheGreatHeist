using DG.Tweening;
using System;
using UnityEngine;

public class PatrolSequence
{
    public bool Active { get; private set; }
    public event EventHandler OnComplete;

    Sequence initialRotateRequest;
    Sequence initialRotate;

    Sequence moveSequence;

    Sequence finalRotateRequest;
    Sequence finalRotateSequence;

    EnemyAim unitAim;
    CharacterMovement unitMovement;
    Transform unitTransform;

    Vector2 finalTargetPosition;
    public Vector2 FinalTargetPosition => finalTargetPosition;
    public PatrolSequence(CharacterMovement movement, EnemyAim aim, Transform unit, Vector2 target, Vector2 finalFaceDirection, float speed, float initialPatrolRotateTime, float patrolRotateTime) {
        Vector2 moveDirection = (target - (Vector2)unit.transform.position).normalized;
        float initialTimeToRotate = ((Vector2.Dot(unit.transform.right, moveDirection) - 1.0f) / -2f) * initialPatrolRotateTime;
        float finalTimeToRotate = ((Vector2.Dot(moveDirection, finalFaceDirection.normalized) - 1.0f) / -2f) * patrolRotateTime;

        finalTargetPosition = target;

        unitAim = aim;
        unitMovement = movement;

        movement.SetMovementProperties(speed, CharacterMovement.Movement.Standard);


        initialRotateRequest = DOTween.Sequence()
            .AppendCallback(() => {
                if (aim.TweenRotate(moveDirection, initialTimeToRotate)) {
                    initialRotate.Play();
                    initialRotateRequest.Kill();
                    initialRotateRequest = null;
                }
            })
            .SetLoops(-1);

        initialRotate = DOTween.Sequence()
            .AppendInterval(initialTimeToRotate)
            .OnKill(() => {
                initialRotate = null;
                moveSequence.Play();
            });


        Vector2 basePos = unit.transform.position;
        float targetDistanceFromBase = Vector2.Distance(target, basePos);
        moveSequence = DOTween.Sequence()
            .AppendInterval(Time.deltaTime)
            .AppendCallback(() => {
                Vector2 dir = (target - (Vector2)unit.transform.position).normalized;
                movement.Move(dir);
                aim.RotateTowards(dir);
            })
            .AppendCallback(() => {
                if (Vector2.Distance(unit.transform.position, target) < 0.1f) {
                    movement.SetMovementProperties(0, CharacterMovement.Movement.None);
                    moveSequence.Kill();
                }
                /*float currentDistanceFromBase = Vector2.Distance(unit.transform.position, basePos);
                Debug.Log("target dist from base " + targetDistanceFromBase + " current dist from base " + currentDistanceFromBase + 0.3f);
                if (targetDistanceFromBase < currentDistanceFromBase + 0.3f) {
                    movement.SetMovementProperties(0, CharacterMovement.Movement.None);
                    moveSequence.Kill();
                }*/
            })
            .SetLoops(-1)
            .OnKill(() => {
                moveSequence = null;
                movement.SetMovementProperties(0, CharacterMovement.Movement.None);
                movement.StopMovement();
                finalRotateRequest.Play();
            });



        finalRotateRequest = DOTween.Sequence()
            .AppendCallback(() => {
                if (aim.TweenRotate(finalFaceDirection, finalTimeToRotate)) {
                    finalRotateSequence.Play();
                    finalRotateRequest.Kill();
                    finalRotateRequest = null;
                }
            })
            .SetLoops(-1);

        finalRotateSequence = DOTween.Sequence()
            .AppendInterval(finalTimeToRotate)
            .OnKill(() => {
                Debug.Log("Patrol Sequence Finished");
                finalRotateSequence = null; 
                Active = false; 
                OnComplete?.Invoke(this, EventArgs.Empty);
            });


        initialRotateRequest.Pause();
        initialRotate.Pause();

        moveSequence.Pause();

        finalRotateRequest.Pause();
        finalRotateSequence.Pause();


    }

    public void Start() {
        Active = true;
        initialRotateRequest.Play();
    }

    public void Stop() {
        Active = false;
        moveSequence.Kill();
        initialRotateRequest.Kill();
        initialRotate.Kill();
        finalRotateSequence.Kill();
        finalRotateRequest.Kill();

        unitAim.StopAllAim();

        unitMovement.SetMovementProperties(0, CharacterMovement.Movement.None);
        unitMovement.StopMovement();
    }

}
