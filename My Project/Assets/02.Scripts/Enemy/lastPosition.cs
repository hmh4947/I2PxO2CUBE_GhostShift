using UnityEngine;

public class lastPosition : MonoBehaviour
{
    private Vector2 last_Position;

    void Update()
    {
        // 오브젝트의 위치를 업데이트
       last_Position = transform.position;
        
    }

    // 마지막 위치를 반환하는 함수
    public Vector2 GetLastPosition()
    {
        return last_Position;
    }
}