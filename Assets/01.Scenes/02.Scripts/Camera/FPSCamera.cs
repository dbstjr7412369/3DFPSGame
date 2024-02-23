using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

// 1��Ī ���� (First Person Shooter)
// ���ӻ��� ĳ������ ������ ���� ī�޶�
public class FPSCamera : MonoBehaviour
{
    public Transform Target;

    private void LateUpdate()
    {
        transform.localPosition = Target.position;

        Vector2 xy = CameraManager.Instance.XY;
        transform.eulerAngles = new Vector3(-xy.y, xy.x, 0);

    }
}