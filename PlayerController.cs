using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 8f;
    private Vector3 moveDirection;

    private PlayerHealth playerHealth;
    private Animator playerAnimator;
    private Rigidbody playerRigidbody;

    private FixedJoystick joystick;
    
    private static readonly int MoveRatio = Animator.StringToHash("MoveRatio");

    private void Start()
    {
        joystick = FindObjectOfType<FixedJoystick>();
        
        playerHealth = GetComponent<PlayerHealth>();
        playerAnimator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if ((GameManager.Instance != null) && (GameManager.Instance.State != GameManager.GameState.Play))
        {
            moveDirection = Vector3.zero;
            return;
        }

        if (playerHealth.isHit)
        {
            return;
        }

        moveDirection = new Vector3(joystick.Horizontal, 0, joystick.Vertical);

        Move();
        Rotate();

        playerAnimator.SetFloat(MoveRatio, moveDirection.magnitude);
    }

    private void Move()
    {
        var moveDistance = moveDirection * moveSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }

    private void Rotate()
    {
        if (moveDirection != Vector3.zero)
        { 
            playerRigidbody.MoveRotation(Quaternion.LookRotation(moveDirection));
        }
    }
}
