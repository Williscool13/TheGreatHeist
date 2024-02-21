using PlayerFiniteStateMachine;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Player/Actions/Laugh")]
    public class LaughAction : PlayerStateAction
    {
        [SerializeField][AssetsOnly] GameObject laughParticlesPrefab;
        ParticleSystem laughParticles;
        public override void Enter(PlayerStateMachine machine) {
            laughParticles = Instantiate(laughParticlesPrefab, machine.transform).GetComponent<ParticleSystem>();
            laughParticles.transform.localPosition = Vector3.zero;
            laughParticles.transform.localRotation = Quaternion.identity;
            laughParticles.Play();
        }

        public override void Execute(PlayerStateMachine machine) {
        }

        public override void Exit(PlayerStateMachine machine) {
            laughParticles.Stop();
            Destroy(laughParticles.gameObject, laughParticles.main.duration * 1.1f);
        }
    }

}
