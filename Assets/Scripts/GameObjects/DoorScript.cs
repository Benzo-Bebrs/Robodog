using UnityEngine;

public class DoorController : Sounds
{
    public float moveDistance = 1f; // Расстояние, на которое будет двигаться дверь
    public float moveTime = 1f; // Время перемещения двери
    public KeyCode activationKey = KeyCode.Space; // Кнопка для активации
    private bool doorOpen = false; // Флаг, указывающий на состояние двери
    private Vector3 initialPosition; // Исходная позиция двери
    private Vector3 targetPosition; // Целевая позиция двери
    private float timer = 0f; // Таймер для плавного перемещения
    private bool isMoving = false; // Флаг, указывающий на текущее состояние перемещения
    private bool used = false; // Флаг, указывающий на использование двери

     // Компонент AudioSource для воспроизведения звуков

    private void Start()
    {
        initialPosition = transform.position;
        targetPosition = transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(activationKey) && !isMoving && !used)
        {
            ToggleDoor();
            used = true;
        }

        if (isMoving)
        {
            timer += Time.deltaTime;
            float normalizedTime = timer / moveTime;
            transform.position = Vector3.Lerp(initialPosition, targetPosition, normalizedTime);

            if (normalizedTime >= 1f)
            {
                isMoving = false;
                timer = 0f;
            }
        }
    }

    private void ToggleDoor()
    {
        doorOpen = !doorOpen;

        if (doorOpen)
        {
            targetPosition = initialPosition + new Vector3(0f, moveDistance, 0f);
        }

        PlaySound(0);
        isMoving = true;
    }
}