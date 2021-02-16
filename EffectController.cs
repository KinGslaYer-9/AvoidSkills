using System.Collections;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    public float EffectDisableTime { get; private set; }

    [SerializeField]
    private float damage;
    
    private Collider damageApplyArea;

    private float enableColliderTime;
    private float disableColliderTime;

    private void Awake()
    {
        damageApplyArea = GetComponent<Collider>();
        
        var ps = GetFirstParticleSystem(gameObject);
        
        EffectDisableTime = ps.main.duration + ps.main.startLifetime.constantMax;
        
        enableColliderTime = ps.main.startLifetime.constantMax;
        disableColliderTime = 0.2f;
    }

    private void OnEnable()
    {
        StartCoroutine(ActiveSkillCollider(enableColliderTime, disableColliderTime));
    }

    private IEnumerator ActiveSkillCollider(float colliderEnableTime, float colliderDisableTime)
    {
        yield return new WaitForSeconds(colliderEnableTime);

        damageApplyArea.enabled = true;
        AudioManager.Instance.Play("Explosion" + Random.Range(1, 6));
        
        yield return new WaitForSeconds(colliderDisableTime);

        damageApplyArea.enabled = false;
    }

    private ParticleSystem GetFirstParticleSystem(GameObject vfx)
    {
        var ps = vfx.GetComponent<ParticleSystem>();
        if (ps == null && vfx.transform.childCount > 0)
        {
            foreach (Transform t in vfx.transform)
            {
                ps = t.GetComponent<ParticleSystem> ();
                if (ps != null)
                {
                    return ps;   
                }
            }
        }
        return ps;
    }

    private void OnTriggerEnter(Collider other)
    {
        // if (!GameManager.Instance.IsGameover)
        if (GameManager.Instance.State == GameManager.GameState.Play)
        {
            var playerHealth = other.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                // playerHealth.ApplyDamage(damage);
                playerHealth.TakeDamage(damage, false);
            }
        }
    }
}
