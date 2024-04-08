using UnityEngine;

public class DoorController : MonoBehaviour
{
    public float moveDistance = 1f; // Расстояние, на которое будет двигаться дверь
    public float moveTime = 1f; // Время перемещения двери
    public KeyCode activationKey = KeyCode.Space; // Кнопка для активации

    public AudioClip buttonPressSound; // Звук нажатия кнопки

    private bool doorOpen = false; // Флаг, указывающий на состояние двери
    private Vector3 initialPosition; // Исходная позиция двери
    private Vector3 targetPosition; // Целевая позиция двери
    private float timer = 0f; // Таймер для плавного перемещения
    private bool isMoving = false; // Флаг, указывающий на текущее состояние перемещения
    private bool used = false; // Флаг, указывающий на использование двери

    private AudioSource audioSource; // Компонент AudioSource для воспроизведения звуков

    private void Start()
    {
        initialPosition = transform.position;
        targetPosition = transform.position;
        audioSource = GetComponent<AudioSource>();
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

        PlaySound(buttonPressSound);
        isMoving = true;
    }

    private void PlaySound(AudioClip sound)
    {
        audioSource.clip = sound;
        audioSource.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, чтобы дверь закрывалась только при входе другого объекта (например, игрока)
        if (other.CompareTag("Player"))
        {
            if (doorOpen && !used)
            {
                ToggleDoor();
                used = true;
            }
        }
    }
}