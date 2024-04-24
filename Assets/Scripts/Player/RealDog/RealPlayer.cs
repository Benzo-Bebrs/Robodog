using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    public float moveSpeed = 5f;

    private SpriteRenderer spriteRenderer;
    private Vector3 lastMoveDirection = Vector3.right; // Последнее направление движения

    void Start()
    {
        // Получаем компонент SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Получаем горизонтальную ось ввода
        float horizontalInput = Input.GetAxis("Horizontal");

        // Перемещаем игрока влево и вправо
        transform.Translate(Vector3.right * horizontalInput * moveSpeed * Time.deltaTime);

        // Определяем последнее направление движения
        if (horizontalInput > 0)
        {
            lastMoveDirection = Vector3.right;
        }
        else if (horizontalInput < 0)
        {
            lastMoveDirection = Vector3.left;
        }

        // Поворачиваем спрайт в сторону движения
        transform.localScale = new Vector3(Mathf.Sign(lastMoveDirection.x), 1f, 1f);
    }
}