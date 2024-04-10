using UnityEngine;

public class DoorController : Sounds
{
    public float moveDistance = 1f; // ����������, �� ������� ����� ��������� �����
    public float moveTime = 1f; // ����� ����������� �����
    public KeyCode activationKey = KeyCode.Space; // ������ ��� ���������
    private bool doorOpen = false; // ����, ����������� �� ��������� �����
    private Vector3 initialPosition; // �������� ������� �����
    private Vector3 targetPosition; // ������� ������� �����
    private float timer = 0f; // ������ ��� �������� �����������
    private bool isMoving = false; // ����, ����������� �� ������� ��������� �����������
    private bool used = false; // ����, ����������� �� ������������� �����

     // ��������� AudioSource ��� ��������������� ������

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