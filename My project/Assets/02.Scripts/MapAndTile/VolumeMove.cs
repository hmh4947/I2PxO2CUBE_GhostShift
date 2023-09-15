using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeMove : MonoBehaviour
{
    public Transform player; // �÷��̾� Transform

    Volume volume;
    Vignette vignette;

    private void Start()
    {
        volume = GetComponent<Volume>();
        if (volume.profile.TryGet(out vignette))
        {
            // ������ �� �ʱ� Center ���� ����
            vignette.center.value = new Vector2(0.5f, 0.5f);
        }
    }

    private void Update()
    {
        if (player != null && vignette != null)
        {
            // �÷��̾� ��ġ�� ���� Vignette�� Center ���� ������Ʈ
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(player.position);
            vignette.center.value = new Vector2(viewportPos.x, viewportPos.y);
        }
    }
}