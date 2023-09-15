using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeMove : MonoBehaviour
{
    public Transform player; // 플레이어 Transform

    Volume volume;
    Vignette vignette;

    private void Start()
    {
        volume = GetComponent<Volume>();
        if (volume.profile.TryGet(out vignette))
        {
            // 시작할 때 초기 Center 값을 저장
            vignette.center.value = new Vector2(0.5f, 0.5f);
        }
    }

    private void Update()
    {
        if (player != null && vignette != null)
        {
            // 플레이어 위치에 따라 Vignette의 Center 값을 업데이트
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(player.position);
            vignette.center.value = new Vector2(viewportPos.x, viewportPos.y);
        }
    }
}