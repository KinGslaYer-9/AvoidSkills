using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f;
    
    public float knockBackForce = 2f;
    
    public float maxEndureTime = 2f;
    public float secondPerDamage = 1f;

    public float protectedDuration = 5f;

    public float hitImmovableTime = 1.5f;

    [HideInInspector]
    public bool isHit;

    [SerializeField]
    private ParticleSystem scoreEffect = null;
    [SerializeField]
    private ParticleSystem healEffect = null;
    [SerializeField]
    private ParticleSystem shieldEffect = null;
    [SerializeField]
    private ParticleSystem damageEffect = null;
    [SerializeField]
    private ParticleSystem burningEffect = null;
    
    [Space(10f)]
    [SerializeField]
    private Renderer headRenderer = null;
    [SerializeField]
    private Renderer bodyRenderer = null;

    private Animator playerAnimator;
    private Rigidbody playerRigidbody;
    private PlayerController playerController;
    
    private float checkBurningTime;
    private bool isBurning;
    
    private bool isShielding;
    
    private static readonly int HitTrigger = Animator.StringToHash("Hit");
    private static readonly int DieTrigger = Animator.StringToHash("Die");

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        if (transform.position.y <= -5)
        {
            gameObject.SetActive(false);
            Die();
        }
        
        if (isBurning)
        {
            checkBurningTime += Time.deltaTime;

            if (burningEffect.isStopped)
            {
                burningEffect.Play();
                StartCoroutine("ApplyBurnDamageCoroutine");
            }
            
            if (checkBurningTime >= maxEndureTime)
            {
                checkBurningTime = 0f;
                isBurning = false;
                burningEffect.Stop();
                StopCoroutine("ApplyBurnDamageCoroutine");
            }
        }
    }

    private IEnumerator ApplyBurnDamageCoroutine()
    {
        while (true)
        {
            TakeDamage(secondPerDamage, true);
            yield return new WaitForSeconds(1f);
        }
    }

    public void TakeDamage(float damage, bool isDurationDamage)
    {
        if (isShielding)
        {
            return;
        }
        
        health -= damage;

        if (isDurationDamage)
        {
            StartCoroutine(ChangePlayerCurrentColor());
        }
        else
        {
            CameraShake.Instance.ShakeCamera(5f, .1f);
            
            damageEffect.Play();
            playerRigidbody.AddForce(transform.forward * -knockBackForce, ForceMode.Impulse);
            playerAnimator.SetTrigger(HitTrigger);
            
            StartCoroutine(ImmovablePlayer());
        }
        
        if (health <= 0)
        {
            health = 0;
            Die();
        }
        
        InGameUIManager.Instance.UpdateHealthText(health);
    }

    public void RestoreHealth(float newHealth)
    {
        health += newHealth;
        
        InGameUIManager.Instance.UpdateHealthText(health);
    }
    
    private void Die()
    {
        if (isBurning)
        {
            isBurning = false;
            burningEffect.Stop();
            StopCoroutine("ApplyBurnDamageCoroutine");
        }
        
        playerAnimator.SetTrigger(DieTrigger);

        playerController.enabled = false;
        
        GameManager.Instance.EndGame();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.State != GameManager.GameState.Play)
        {
            return;
        }
        
        var item = other.GetComponent<IItem>();

        if (item != null)
        {
            item.Use(gameObject, out var type);

            AudioManager.Instance.Play("ItemPickUp");

            switch (type)
            {
                case IItem.ItemType.Health:
                    healEffect.Play();
                    break;
                case IItem.ItemType.Score:
                    scoreEffect.Play();
                    break;
                case IItem.ItemType.Shield:
                    shieldEffect.gameObject.SetActive(true);
                    shieldEffect.Play();
                    StartCoroutine(ProtectPlayerPerTime());
                    break;
                default:
                    Debug.LogError("Not Supported Type");
                    break;
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(GameManager.Instance.State == GameManager.GameState.Play)
        {
            if (other.gameObject.CompareTag("Damage Zone") && isShielding == false)
            {
                isBurning = true;
            }
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if(GameManager.Instance.State == GameManager.GameState.Play)
        {
            if (other.gameObject.CompareTag("Damage Zone") && isShielding == false)
            {
                isBurning = true;
            }
        }
    }

    private IEnumerator ProtectPlayerPerTime()
    {
        isShielding = true;

        yield return new WaitForSecondsRealtime(protectedDuration);

        shieldEffect.Stop();
        shieldEffect.gameObject.SetActive(false);
        isShielding = false;
    }
    
    private IEnumerator ChangePlayerCurrentColor()
    {
        headRenderer.material.color = Color.red;
        bodyRenderer.material.color = Color.red;
        yield return new WaitForSecondsRealtime(0.2f);
        headRenderer.material.color = Color.white;
        bodyRenderer.material.color = Color.white;
    }

    private IEnumerator ImmovablePlayer()
    {
        isHit = true;
        yield return new WaitForSecondsRealtime(hitImmovableTime);
        isHit = false;
    }
}
