using UnityEngine;

// 3��Ī ���� (Third Person Shooter)
// ���ӻ��� ĳ���Ͱ� ���� ������ �ƴ�, ĳ���͸� ���� ���� ��, 3��Ī ������ ������ ī�޶�
public class TPSCamera : MonoBehaviour
{
    public Transform Target;
    public Vector3 Offset = new Vector3(0, 3f, -3f);


    public float a;
    private void LateUpdate()
    {
        transform.localPosition = Target.position + Offset;
        transform.LookAt(Target);

        Vector2 xy = CameraManager.Instance.XY;
        transform.RotateAround(Target.position, Vector3.up, xy.x);
        transform.RotateAround(Target.position, transform.right, -xy.y);


        transform.localPosition = Target.position - transform.forward * Offset.magnitude + Vector3.up * (Offset.y - a);
    }
}