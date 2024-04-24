using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    public float moveSpeed = 5f;

    private SpriteRenderer spriteRenderer;
    private Vector3 lastMoveDirection = Vector3.right; // ��������� ����������� ��������

    void Start()
    {
        // �������� ��������� SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // �������� �������������� ��� �����
        float horizontalInput = Input.GetAxis("Horizontal");

        // ���������� ������ ����� � ������
        transform.Translate(Vector3.right * horizontalInput * moveSpeed * Time.deltaTime);

        // ���������� ��������� ����������� ��������
        if (horizontalInput > 0)
        {
            lastMoveDirection = Vector3.right;
        }
        else if (horizontalInput < 0)
        {
            lastMoveDirection = Vector3.left;
        }

        // ������������ ������ � ������� ��������
        transform.localScale = new Vector3(Mathf.Sign(lastMoveDirection.x), 1f, 1f);
    }
}