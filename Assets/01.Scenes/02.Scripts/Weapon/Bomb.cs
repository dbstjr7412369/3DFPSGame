using System;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    // �ǽ� ���� 8. ����ź�� ������ ��(����� ��) ���� ����Ʈ�� �ڱ� ��ġ�� �����ϱ�
    public GameObject BombEffectPrefab;

    private void OnCollisionEnter(Collision other)
    {
        gameObject.SetActive(false);

        GameObject effect = Instantiate(BombEffectPrefab);
        effect.transform.position = this.gameObject.transform.position;
    }
}
