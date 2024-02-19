using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    // ��ǥ: ����ź ���� ���� ������ ��� ����
    // �ʿ� �Ӽ�:
    // - ����
    public float ExplosionRadius = 3;
    // ���� ����:


    public int Damage = 60;

    public GameObject BombEffectPrefab;

    // 1. ���� ��
    private void OnCollisionEnter(Collision other)
    {
        gameObject.SetActive(false); // â�� �ִ´�.

        GameObject effect = Instantiate(BombEffectPrefab);
        effect.transform.position = this.gameObject.transform.position;

        // 2. �����ȿ� �ִ� ��� �ݶ��̴��� ã�´�.
        // -> ������.������ �Լ��� Ư�� ����(radius) �ȿ� �ִ� Ư�� ���̾���� ���� ������Ʈ��
        //    �ݶ��̴� ������Ʈ���� ��� ã�� �迭�� ��ȯ�ϴ� �Լ�
        // ������ ����: ���Ǿ�, ť��, ĸ��
        int layer =/* LayerMask.GetMask("Player") |*/ LayerMask.GetMask("Monster");
        Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius, layer);
        Debug.Log(colliders.Length);
        // 3. ã�� �ݶ��̴� �߿��� Ÿ�� ������(IHitable) ������Ʈ�� ã�Ƽ� Hit()�Ѵ�.
        foreach (Collider c in colliders)
        {
            IHitable hitable = c.GetComponent<IHitable>();
            if (hitable != null)
            {
                hitable.Hit(Damage);
            }
        }
    }
}