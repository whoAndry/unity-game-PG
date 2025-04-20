using UnityEngine;

public class MonkeyController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float buttonTime = 0.3f;
    private float jumpTime;
    private bool jumping;
    private bool isJumping;

    private Vector3 originalScale; // Menyimpan skala asli karakter

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Mengambil komponen Animator
        originalScale = transform.localScale; // Simpan skala awal karakter
    }

    void Update()
    {
        Move();
        HandleJump();
        UpdateAnimation();
    }

    void Move()
    {
        float moveInput = 0;

        if (Input.GetKey(KeyCode.A))
        {
            moveInput = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveInput = 1;
        }

        // Menggerakkan karakter secara horizontal
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Flip karakter sesuai arah gerakan
        if (moveInput != 0)
        {
            transform.localScale = new Vector3(originalScale.x * Mathf.Sign(moveInput), originalScale.y, originalScale.z);
        }
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            jumping = true;
            jumpTime = 0;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = true;
        }

        if (jumping)
        {
            jumpTime += Time.deltaTime;

            if (jumpTime > buttonTime || !Input.GetKey(KeyCode.Space))
            {
                jumping = false;
            }
        }

        if (rb.velocity.y == 0)
        {
            isJumping = false;
        }
    }

    void UpdateAnimation()
    {
        float moveInput = Mathf.Abs(rb.velocity.x);

        if (rb.velocity.y == 0)
        {
            // Set animasi saat di tanah
            if (moveInput > 0)
            {
                animator.SetBool("isRunning", true); // Animasi Run
                animator.SetBool("isIdle", false);  // Nonaktifkan animasi Idle
            }
            else
            {
                animator.SetBool("isRunning", false); // Nonaktifkan animasi Run
                animator.SetBool("isIdle", true);  // Animasi Idle
            }

            animator.SetBool("isJump", false); // Nonaktifkan animasi lompat saat di tanah
        }
        else
        {
            // Set animasi saat di udara
            animator.SetBool("isRunning", false);
            animator.SetBool("isIdle", false);
            animator.SetBool("isJump", true);
        }
    }
}
