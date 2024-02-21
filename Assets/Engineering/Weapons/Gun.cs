using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Gun : Weapon
{

    [SerializeField] GunStats stats;
    [SerializeField] LayerMask shootLayerWhitelist;

    [Title("Bullet Trail")]
    [SerializeField][AssetsOnly] GameObject bulletTrailPrefab;


    
    [ReadOnly][SerializeField] int currentAmmo;
    float currTimeSinceLastShot = 0;


    ObjectPool<TrailRenderer> trailRendererPool;

    private void Start() {
        currentAmmo = stats.maxAmmo;


        trailRendererPool = new ObjectPool<TrailRenderer>(BulletTrailCreate, BulletTrailOnTakeFromPool, BulletTrailOnReleaseToPool, BulletTrailDestroyPoolObject);
    }

    private void Update() {
        currTimeSinceLastShot += Time.deltaTime;
    }

    public override bool CanShoot(bool press) {

        if (!stats.automatic) {
            if (!press) {
                return false;
            }
        }

        if (currentAmmo <= 0)
        {
            // play click sound
            return false;
        }

        if (currTimeSinceLastShot < 1 / stats.fireRate) {
            return false;
        }

        return true;
    }

    public override void Shoot(Vector2 targetPos) {
        int currBounceCount = stats.bounceCount + 1;
        Vector2 raycastSource = muzzlePosition.position;
        Vector2 muzzleDirection = muzzlePosition.up.normalized;
        Vector2 desiredDirection = (targetPos - (Vector2)muzzlePosition.position).normalized;
        Vector2 bulletDirection;
        if (Vector2.Dot(desiredDirection, muzzleDirection) > 0.95) {
            bulletDirection = desiredDirection;
        } else {
            bulletDirection = muzzleDirection;
        }



            muzzleFlash.Play();

        do {
            RaycastHit2D hit = Physics2D.Raycast(raycastSource, bulletDirection, Mathf.Infinity, shootLayerWhitelist);
            if (hit) {
                // withdraw from pool and draw line from muzzle position to hit point
                TrailRenderer t = trailRendererPool.Get();
                t.transform.position = raycastSource;
                t.emitting = true;

                t.AddPosition(raycastSource);
                t.AddPosition(hit.point);

                DOTween.Sequence()
                    .AppendInterval(t.time)
                    .onComplete += () => {
                        trailRendererPool.Release(t);
                    };


                // if hit ITarget, call OnHit
                if (hit.collider.TryGetComponent(out ITarget target)) {
                    bool crit = Random.Range(0f, 1f) < stats.criticalChance;
                    if (stats.alwaysCritFromBehind) {
                        if (Vector2.Dot(hit.transform.right, -bulletDirection) < 0.66f) {
                            crit = true;
                        }
                    }
                    target.OnHit(new DamageData(stats.damage, hit.point, bulletDirection, hit.distance, crit, Time.time, DamageImpactType.Sharp));
                    break;
                }
            }
            else {
                TrailRenderer t = trailRendererPool.Get();
                t.transform.position = raycastSource;
                t.emitting = true;

                t.AddPosition(raycastSource);
                t.AddPosition(raycastSource + bulletDirection * 100);

                DOTween.Sequence()
                    .AppendInterval(t.time)
                    .onComplete += () => {
                        trailRendererPool.Release(t);
                    };

                break;
            }

            // Reflected Vector
            bulletDirection = Vector2.Reflect(bulletDirection, hit.normal).normalized;
            raycastSource = hit.point + bulletDirection * 0.01f;

            currBounceCount--;
        }
        while (currBounceCount > 0);

        currentAmmo--;
        currTimeSinceLastShot = 0;
    }


    public override void Equip() {
        Debug.Log("Equipping Gun: " + this.name);
    }

    public override void Unequip() {
        Debug.Log("Unequipping Gun: " + this.name);
    }


    #region Bullet Trail Pooling
    TrailRenderer BulletTrailCreate() {
        GameObject gameObject = Instantiate(bulletTrailPrefab);
        TrailRenderer trailRenderer = gameObject.GetComponent<TrailRenderer>();

        return trailRenderer;
    }

    void BulletTrailOnReleaseToPool(TrailRenderer trailRenderer) {
        trailRenderer.Clear();
        trailRenderer.emitting = false;
        trailRenderer.gameObject.SetActive(false);

    }

    void BulletTrailOnTakeFromPool(TrailRenderer trailRenderer) {
        trailRenderer.gameObject.SetActive(true);
    }

    void BulletTrailDestroyPoolObject(TrailRenderer trailRenderer) {
        Destroy(trailRenderer.gameObject);
    }
    #endregion
}
