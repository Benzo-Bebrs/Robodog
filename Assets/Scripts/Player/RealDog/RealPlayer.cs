using UnityEngine;

public class RealPlayer : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameObject mouseBall;

    private Animator animator;
    private Vector3 lastMoveDirection = Vector3.right;
    private bool canUpBall;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        mouseBall.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            canUpBall= true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            canUpBall = false;
        }
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontalInput * moveSpeed * Time.deltaTime);
        CamController.cameraShake?.Invoke(1f, 1f, 1f);

        if (horizontalInput > 0)
        {
            lastMoveDirection = Vector3.right;
        }
        else if (horizontalInput < 0)
        {
            lastMoveDirection = Vector3.left;
        }

        animator.SetInteger("Condition", horizontalInput != 0 ? 1 : 0);

        if (canUpBall && Input.GetKeyDown(KeyCode.F))
        {
            animator.SetInteger("Condition", 2);
            Invoke(nameof(UpBall), 0.4f);
        }

        transform.localScale = new Vector3(Mathf.Sign(lastMoveDirection.x), 1f, 1f);
    }

    private void UpBall()
    {
        mouseBall.SetActive(true);
    }
}