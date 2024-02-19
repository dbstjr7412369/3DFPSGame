using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Drum : MonoBehaviour, IHitable
{
    // 실습 과제 19. 총으로 드럼통 3번 맞출 시 사라지게 구현
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

        // 실습 과제 22. 드럼통 폭발할 때 주변 Hitable한 Monster와 Player에게 데미지 70
        // 1. 폭발 범위 내 콜라이더 찾기
        int findLayer = LayerMask.GetMask("Player") | LayerMask.GetMask("Monster");
        Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius, findLayer);

        // 2. 콜라이더 내에서 Hitable 찾기
        foreach (Collider c in colliders)
        {
            IHitable hitable = null;
            if (c.TryGetComponent<IHitable>(out hitable))
            {

                // 3. 데미지 주기
                hitable.Hit(Damage);
            }
        }


        Destroy(gameObject, 3f);
        
    }
    // 실습 과제 23. 드럼통 폭발할 때 주변 드럼통도 같이 폭발되게 구현
    // 만약 폭발 범위 콜라이더 내에 드럼통이 들어있으면 드럼통의 _hitCount =3으로 변경


}