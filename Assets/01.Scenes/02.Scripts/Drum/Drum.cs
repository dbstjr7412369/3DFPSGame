using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Drum : MonoBehaviour, IHitable
{
    // �ǽ� ���� 19. ������ �巳�� 3�� ���� �� ������� ����
    private int _hitCount = 0;
    public GameObject ExplosionPaticlePrefab;
    private Rigidbody _rigidbody;
    public float UpPower = 50f;

    public int Damage = 70;
    public float ExplosionRadius = 10f;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }


    public void Hit(int damage)
    {
        _hitCount += 1;
        if (_hitCount >= 3)
        {
            Kill();
        }
    }

    private void Kill()
    {
        GameObject explosion = Instantiate(ExplosionPaticlePrefab);
        explosion.transform.position = this.transform.position;
        _rigidbody.AddForce(Vector3.up * UpPower, ForceMode.Impulse);
        _rigidbody.AddTorque(new Vector3(1, 0, 1) * UpPower / 2f);

        // �ǽ� ���� 22. �巳�� ������ �� �ֺ� Hitable�� Monster�� Player���� ������ 70
        // 1. ���� ���� �� �ݶ��̴� ã��
        int findLayer = LayerMask.GetMask("Player") | LayerMask.GetMask("Monster");
        Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius, findLayer);

        // 2. �ݶ��̴� ������ Hitable ã��
        foreach (Collider c in colliders)
        {
            IHitable hitable = null;
            if (c.TryGetComponent<IHitable>(out hitable))
            {

                // 3. ������ �ֱ�
                hitable.Hit(Damage);
            }
        }


        Destroy(gameObject, 3f);
        
    }
    // �ǽ� ���� 23. �巳�� ������ �� �ֺ� �巳�뵵 ���� ���ߵǰ� ����
    // ���� ���� ���� �ݶ��̴� ���� �巳���� ��������� �巳���� _hitCount =3���� ����


}