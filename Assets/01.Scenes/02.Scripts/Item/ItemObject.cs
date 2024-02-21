using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public ItemType ItemType;

    // Todo 1. ������ �������� 3��(Health, Stamina, Bullet) �����. (�����̳� ���� �ٸ����ؼ� �����ǰ�)
    // Todo 2. �÷��̾�� ���� �Ÿ��� �Ǹ� �������� �Ծ����� �������.
    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log(12111);
        if (collider.CompareTag("Player"))
        {
            //�÷��̾�� ���� �Ÿ��� �˰� �ʹ�
            float distance = Vector3.Distance(collider.transform.position, transform.position);
            Debug.Log(distance);
            // 1. ������ �Ŵ���(�κ��丮)�� �߰��ϰ�,

            // 2. �������.
            Destroy(gameObject);
        }
    }

    // �ǽ� ���� 31. ���Ͱ� ������ �������� ���(Health: 20%, Stamina: 20%, Bullet:10%)
    //�ǽ� ���� 32. ���� �Ÿ��� �Ǹ� �������� Slerp �̿��ؼ� ������� �ϱ�(�ɽ��ϸ� ������ ���)
}