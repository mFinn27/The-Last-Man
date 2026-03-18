using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Stats Set Up")]
    [SerializeField] private float tocDoDiChuyen = 5f;
    [SerializeField] private Transform viTriSpriteChar;
    [SerializeField] private Animator animator;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    public Vector2 HuongDiChuyenCuoi { get; private set; } = Vector2.right;

    private readonly int moveXPara = Animator.StringToHash("MoveX");
    private readonly int moveYPara = Animator.StringToHash("MoveY");
    private readonly int isMovingPara = Animator.StringToHash("IsMoving");

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (moveInput != Vector2.zero)
        {
            HuongDiChuyenCuoi = moveInput.normalized;
        }

        HandleAnimations();
    }

    private void HandleAnimations()
    {
        if (animator == null) return;

        bool dangDiChuyen = moveInput.sqrMagnitude > 0.01f;
        animator.SetBool("IsMoving", dangDiChuyen);

        if (dangDiChuyen)
        {
            animator.SetFloat("MoveX", moveInput.x);
            animator.SetFloat("MoveY", moveInput.y);

            if (moveInput.x != 0)
            {
                float huongDiChuyen = Mathf.Sign(moveInput.x);
                viTriSpriteChar.localScale = new Vector3(huongDiChuyen, 1, 1);
            }
        }
    }

    void FixedUpdate() => rb.linearVelocity = moveInput.normalized * tocDoDiChuyen;
}