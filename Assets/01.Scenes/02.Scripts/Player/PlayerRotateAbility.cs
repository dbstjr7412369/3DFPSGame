using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotateAbility : MonoBehaviour
{
    // ��ǥ: ���콺�� �����ϸ� �÷��̾ �¿�������� ȸ�� ��Ű�� �ʹ�.
    // �ʿ� �Ӽ�:
    // - ȸ�� �ӵ�
    public float RotationSpeed = 200; // �ʴ� 200������ ȸ�� ������ �ӵ�
    // ������ x����
    private float _mx = 0;
    private void Start()
    {
        ResetX();
    }
    void Update()
    {
        if (GameManager.Instance.State != GameState.Go)
        {
            return;
        }

        if (!CameraManager.Focus)
        {
            return;
        }
        // 1. ���콺 �Է�(drag) �޴´�.
        float mouseX = Input.GetAxis("Mouse X");

        // 2. ���콺 �Է� ����ŭ x���� �����Ѵ�.
        _mx += mouseX * RotationSpeed * Time.deltaTime;
        //_mx = Mathf.Clamp(_mx, -270f, 270f);

        // 3. ������ ���� ���� ȸ���Ѵ�.
        transform.eulerAngles = new Vector3(0f, _mx, 0);
    }
    public void ResetX()
    {
        _mx = 0;
    }
}