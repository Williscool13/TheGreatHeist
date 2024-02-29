using DG.Tweening;
using ScriptableObjectDependencyInjection;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using UnityEngine.Video;

public class HealthSystem : MonoBehaviour, ITarget, IHitbox
{
    [Title("Health")]
    [SerializeField] private HealthData healthData;
    [SerializeField] private bool disableHitboxOnDeath = true;
    [ShowIf("disableHitboxOnDeath")][SerializeField] private Collider2D hitbox;
    [SerializeField] private bool disableColliderOnDeath = true;
    [ShowIf("disableColliderOnDeath")][SerializeField] private Collider2D[] colliders;
    [ReadOnly][SerializeField] int _currentHealth;

    [SerializeField] private bool useScriptableObjectHealth;
    [ShowIf("useScriptableObjectHealth")][SerializeField] private FloatVariable currentHealth;

    [Title("Blood Mist")]
    [SerializeField][AssetsOnly] private GameObject bloodMistPrefab;

    [Title("Blood Splatter")]
    [SerializeField][AssetsOnly] private GameObject bloodSplatterPrefab;
    [SerializeField] private Vector2 bloodSplatterOffset = new Vector2(0.25f, 0.25f);

    [Title("Death")]
    [SerializeField][AssetsOnly] private GameObject deathParticlesPrefab;
    [SerializeField] private bool bulletTimeOnDeath;


    [Title("Sound")]
    [SerializeField] private AudioClip[] sharpHitSounds;
    [SerializeField] private AudioClip[] bluntHitSounds;
    [SerializeField] private AudioClip[] criticalHitSounds;
    [SerializeField] private AudioClip[] deathSounds;
    [SerializeField] private AudioSource audioSource;

    public event EventHandler<DamageData> OnDamagePre;
    public event EventHandler OnDeath;
    public UnityEvent OnDeathUnityEvent;

    public bool IsDead => dead; 
    public HealthData HealthData => healthData;
    public DamageData LastDamageData => lastDamageData;



    // pool for blood mist particles
    ObjectPool<ParticleSystem> bloodMistPool;
    ObjectPool<ParticleSystem> bloodSplatterPool;
    ParticleSystem deathParticles;
    
    bool critImmune;
    bool dead = false;
    DamageData lastDamageData = null;
    void Start()
    {
        if (!useScriptableObjectHealth) {
            _currentHealth = healthData.maxHealth;
        }
        else {
            currentHealth.Value = healthData.maxHealth;
        }
        critImmune = healthData.critImmune;
        
        bloodMistPool = new ObjectPool<ParticleSystem>(BloodMistCreate, BloodMistOnTakeFromPool, BloodMistOnReleaseToPool, BloodMistDestroyPoolObject);
        bloodSplatterPool = new ObjectPool<ParticleSystem>(BloodSplatterCreate, BloodSplatterTakeFromPool, BloodSplatterReleaseToPool, BloodSplatterDestroyPoolObject);
    }

    public void Damage(DamageData data) {

        // Blood Mist
        ParticleSystem bloodMist = bloodMistPool.Get();
        bloodMist.transform.position = data.position + data.direction.normalized * 0.1f; 
        // rotate to face direction
        bloodMist.transform.eulerAngles = new Vector3(0, 0, (Mathf.Atan2(data.direction.y, data.direction.x) * Mathf.Rad2Deg) + 90.0f);
        bloodMist.Play();

        // Blood Splatter
        if (data.isCritical) {
            ParticleSystem bloodSplatter = bloodSplatterPool.Get();
            bloodSplatter.transform.position = data.position + new Vector3(UnityEngine.Random.Range(-bloodSplatterOffset.x, bloodSplatterOffset.x), UnityEngine.Random.Range(-bloodSplatterOffset.y, bloodSplatterOffset.y), 0f);
            bloodSplatter.transform.eulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(0, 360));
            bloodSplatter.Play();

            audioSource.PlayOneShot(criticalHitSounds[UnityEngine.Random.Range(0, criticalHitSounds.Length)]);
            if (!useScriptableObjectHealth) {
                _currentHealth -= data.amount * 2;
            } else {
                currentHealth.Value -= data.amount * 2;
            }
        } else {
            switch (data.impactType) {
                case DamageImpactType.Sharp:
                    audioSource.PlayOneShot(sharpHitSounds[UnityEngine.Random.Range(0, sharpHitSounds.Length)]);
                    break;
                case DamageImpactType.Blunt:
                    audioSource.PlayOneShot(bluntHitSounds[UnityEngine.Random.Range(0, bluntHitSounds.Length)]);
                    break;
            }
            if (!useScriptableObjectHealth) {
                _currentHealth -= data.amount;
            }
            else {
                currentHealth.Value -= data.amount;
            }
        }

        bool outOfHealth;
        if (!useScriptableObjectHealth) {
            outOfHealth = _currentHealth <= 0;
        } else {
            outOfHealth = currentHealth.Value <= 0;
        }
        if (outOfHealth && !dead) {
            OnDeath?.Invoke(this, EventArgs.Empty);
            // infinitely emit laughing particles, will likely change
            if (deathParticles == null) {
                deathParticles = Instantiate(deathParticlesPrefab, transform).GetComponent<ParticleSystem>();
                deathParticles.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            }
            
            deathParticles.Play();
            audioSource.PlayOneShot(deathSounds[UnityEngine.Random.Range(0, deathSounds.Length)]);

            if (disableHitboxOnDeath) {
                hitbox.enabled = false;
            }
            if (disableColliderOnDeath) {
                foreach (Collider2D c in colliders) {
                    c.enabled = false;
                }
            }
            if (bulletTimeOnDeath) {
                TimescaleManager.Instance.BulletTime(2.0f, true);
            }
            OnDeathUnityEvent.Invoke();

            dead = true;
        }
    }

    public void OnHit(DamageData data) {
        OnDamagePre?.Invoke(this, data);
        
        if (critImmune && data.isCritical) {
            data.isCritical = false;
        }
        lastDamageData = data;
        Damage(data);
    }
    public void AcknowledgeDamageData() {
        LastDamageData.acknowledged = true;
    }

    public void SetHealth(int value) { 
        if (!useScriptableObjectHealth) {
            _currentHealth = value;
        } else {
            currentHealth.Value = value;
        }
        dead = value <= 0;
        if (deathParticles != null) {
            deathParticles.Stop();
        }

        bool colEnabled = value > 0;
        if (disableHitboxOnDeath) {
            hitbox.enabled |= colEnabled;
        }
        if (disableColliderOnDeath) {
            foreach (Collider2D c in colliders) {
                c.enabled |= colEnabled;
            }
        }
    }

    #region Blood Mist
    ParticleSystem BloodMistCreate() {
        GameObject bloodMist = Instantiate(bloodMistPrefab, Vector3.one * -100.0f, Quaternion.identity);
        bloodMist.hideFlags = HideFlags.HideInHierarchy;

        return bloodMist.GetComponent<ParticleSystem>();
    }

    void BloodMistOnTakeFromPool(ParticleSystem bloodMist) {
        bloodMist.Clear();

        DOTween.Sequence()
            .AppendInterval(bloodMist.main.duration)
            .AppendCallback(() => bloodMistPool.Release(bloodMist));
    }

    void BloodMistOnReleaseToPool(ParticleSystem bloodMist) {
        bloodMist.Clear();
        bloodMist.Stop();
    }

    void BloodMistDestroyPoolObject(ParticleSystem bloodMist) {
        Destroy(bloodMist.gameObject);
    }
    #endregion

    #region Blood Splatter
    ParticleSystem BloodSplatterCreate() {
        GameObject bloodSplatter = Instantiate(bloodSplatterPrefab, Vector3.one * -100.0f, Quaternion.identity);
        bloodSplatter.hideFlags = HideFlags.HideInHierarchy;
        return bloodSplatter.GetComponent<ParticleSystem>();
    }

    void BloodSplatterTakeFromPool(ParticleSystem bloodSplatter) {
        bloodSplatter.Clear();

        DOTween.Sequence()
            .AppendInterval(bloodSplatter.main.duration * 1.1f)
            .AppendCallback(() => bloodSplatterPool.Release(bloodSplatter));
    }

    void BloodSplatterReleaseToPool(ParticleSystem bloodSplatter) {
        bloodSplatter.Clear();
        bloodSplatter.Stop();
    }

    void BloodSplatterDestroyPoolObject(ParticleSystem bloodSplatter) {
        Destroy(bloodSplatter.gameObject);
    }
    #endregion


    Vector2[] centerPointOffsets = new Vector2[] {
        new(0, 0),
        new(0.5f, 0),
        new(-0.5f, 0),
        new(0, 0.5f),
        new(0, -0.5f),
    };

    Vector2[] edgePointOffsets = new Vector2[] {
        new Vector2(1.0f, 0),
        new Vector2(-1.0f, 0),
        new Vector2(0, 1.0f),
        new Vector2(0, -1.0f),
    };

    public Vector2[] GetCenterHitPoints() {
        Vector2 pivot = (Vector2)transform.position;
        Vector2 extents = hitbox.bounds.extents * 0.9f;

        Vector2[] outputs = new Vector2[centerPointOffsets.Length];
        for (int i = 0; i < centerPointOffsets.Length; i++) {
            outputs[i] = pivot + new Vector2(extents.x * centerPointOffsets[i].x, extents.y * centerPointOffsets[i].y);
        }

        return outputs;
    }

    public Vector2[] GetEdgeHitPoints() {
        Vector2 pivot = (Vector2)transform.position;
        Vector2 extents = hitbox.bounds.extents * 0.9f;

        Vector2[] outputs = new Vector2[edgePointOffsets.Length];
        for (int i = 0; i < edgePointOffsets.Length; i++) {
            outputs[i] = pivot + new Vector2(extents.x * edgePointOffsets[i].x, extents.y * edgePointOffsets[i].y);
        }

        return outputs;
    }

    public Vector2[] GetAllHitPoints() {
        return GetCenterHitPoints().Concat(GetEdgeHitPoints()).ToArray();
    }



#if UNITY_EDITOR
    [Title("Debug")]
    [SerializeField] private bool crit;
    [SerializeField] private bool instakill;
    [Button("Shoot")]
    public void TestHit() {
        OnHit(new DamageData(instakill ? 1000 : 1, transform.position, Vector3.zero, 10.0f, crit, Time.time, DamageImpactType.Sharp));
    }
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Vector2[] points2 = GetAllHitPoints();
        for (int i = 0; i < points2.Length; i++) {
            Gizmos.DrawSphere(points2[i], 0.02f);
        }

    }
#endif
}


public class DamageData
{
    public int amount;
    public Vector3 position;
    public Vector3 direction;
    public bool isCritical;
    public float bulletShotTimestamp;
    public DamageImpactType impactType;
    public bool acknowledged = false;
    public DamageData(int amount, Vector3 position, Vector3 direction, float distance, bool isCritical, float damageTimestamp, DamageImpactType impactType) {
        this.amount = amount;
        this.position = position;
        this.direction = direction;
        this.isCritical = isCritical;
        this.bulletShotTimestamp = damageTimestamp;
        this.impactType = impactType;
    }
}

public enum DamageImpactType
{
    Blunt,
    Sharp
}

public interface IHitbox
{
    public Vector2[] GetCenterHitPoints();
    public Vector2[] GetEdgeHitPoints();
    public Vector2[] GetAllHitPoints();
}