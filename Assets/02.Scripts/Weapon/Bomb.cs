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

    private Collider[] _colliders = new Collider[10];

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
        int count = Physics.OverlapSphereNonAlloc(transform.position, ExplosionRadius, _colliders, layer);
        // 3. ã�� �ݶ��̴� �߿��� Ÿ�� ������(IHitable) ������Ʈ�� ã�Ƽ� Hit()�Ѵ�.
        for (int i = 0; i < count; i++)
        {
            Collider c = _colliders[i];
            IHitable hitable = c.GetComponent<IHitable>();
            if (hitable != null)
            {
                DamageInfo damageInfo = new DamageInfo(DamageType.Normal, Damage);
                hitable.Hit(damageInfo);
            }
        }
    }
}