using UnityEngine;

public class lastPosition : MonoBehaviour
{
    private float last_Position;

    void Update()
    {
        // ������Ʈ�� ��ġ�� ������Ʈ
        last_Position = transform.position.x;
    }

    // ������ ��ġ�� ��ȯ�ϴ� �Լ�
    public float GetLastPosition()
    {
        return last_Position;
    }
}