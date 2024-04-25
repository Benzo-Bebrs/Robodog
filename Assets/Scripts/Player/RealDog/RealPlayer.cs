using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    public float moveSpeed = 5f;

    private SpriteRenderer spriteRenderer;
    private Vector3 lastMoveDirection = Vector3.right;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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

        transform.localScale = new Vector3(Mathf.Sign(lastMoveDirection.x), 1f, 1f);
    }
}