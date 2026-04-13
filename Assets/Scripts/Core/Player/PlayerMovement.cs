using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Stats Set Up")]
    [SerializeField] private Transform viTriSpriteChar;
    [SerializeField] private Animator animator;

    [Header("--- HIỆU ỨNG (VFX) ---")]
    [SerializeField] private ParticleSystem hieuUngKhoiBui;
    [SerializeField] private float lucVayBuiNguoc = 2f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    public Vector2 HuongDiChuyenCuoi { get; private set; } = Vector2.right;

    [HideInInspector] public bool isCutsceneControlled = false;

    private readonly int moveXPara = Animator.StringToHash("MoveX");
    private readonly int moveYPara = Animator.StringToHash("MoveY");
    private readonly int isMovingPara = Animator.StringToHash("IsMoving");

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;

        if (!isCutsceneControlled && (PlayerHealth.Instance == null || PlayerHealth.Instance.GetCurrentHP() > 0))
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
        }
        else if (isCutsceneControlled)
        {
            if (rb.linearVelocity.sqrMagnitude > 0.01f)
            {
                moveInput = rb.linearVelocity.normalized;
            }
            else
            {
                moveInput = Vector2.zero;
            }
        }
        else
        {
            moveInput = Vector2.zero;
        }
        if (moveInput != Vector2.zero)
        {
            HuongDiChuyenCuoi = moveInput.normalized;
        }
        HandleAnimations();
    }

    private void HandleAnimations()
    {
        bool dangDiChuyen = moveInput.sqrMagnitude > 0.01f;

        if (animator != null)
        {
            animator.SetBool(isMovingPara, dangDiChuyen);

            if (dangDiChuyen)
            {
                animator.SetFloat(moveXPara, moveInput.x);
                animator.SetFloat(moveYPara, moveInput.y);

                if (moveInput.x != 0)
                {
                    float huong = Mathf.Sign(moveInput.x);
                    viTriSpriteChar.localScale = new Vector3(huong, 1, 1);
                }
            }
        }

        if (hieuUngKhoiBui != null)
        {
            if (dangDiChuyen)
            {
                if (!hieuUngKhoiBui.isPlaying) hieuUngKhoiBui.Play();

                var velocityModule = hieuUngKhoiBui.velocityOverLifetime;

                Vector2 huongBuiVang = -moveInput.normalized;
                velocityModule.x = new ParticleSystem.MinMaxCurve(huongBuiVang.x * lucVayBuiNguoc);
                velocityModule.y = new ParticleSystem.MinMaxCurve(huongBuiVang.y * lucVayBuiNguoc);
            }
            else
            {
                if (hieuUngKhoiBui.isPlaying) hieuUngKhoiBui.Stop();
            }
        }
    }

    void FixedUpdate()
    {
        if (isCutsceneControlled) return;

        float tocDoHienTai = PlayerStats.Instance != null ? PlayerStats.Instance.GetMoveSpeed() : 5f;
        rb.linearVelocity = moveInput.normalized * tocDoHienTai;
    }

    public void ForceMoveTo(Vector2 huong)
    {
        isCutsceneControlled = true;
        rb.linearVelocity = huong;
    }

    public void StopForceMove()
    {
        isCutsceneControlled = false;
        rb.linearVelocity = Vector2.zero;
        moveInput = Vector2.zero;
        HandleAnimations();
    }
}