using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    // 목표: 수류탄 폭발 범위 데미지 기능 구현
    // 필요 속성:
    // - 범위
    public float ExplosionRadius = 3;
    // 구현 순서:


    public int Damage = 60;

    public GameObject BombEffectPrefab;

    // 1. 터질 때
    private void OnCollisionEnter(Collision other)
    {
        gameObject.SetActive(false); // 창고에 넣는다.

        GameObject effect = Instantiate(BombEffectPrefab);
        effect.transform.position = this.gameObject.transform.position;

        // 2. 범위안에 있는 모든 콜라이더를 찾는다.
        // -> 피직스.오버랩 함수는 특정 영역(radius) 안에 있는 특정 레이어들의 게임 오브젝트의
        //    콜라이더 컴포넌트들을 모두 찾아 배열로 반환하는 함수
        // 영역의 형태: 스피어, 큐브, 캡슐
        int layer =/* LayerMask.GetMask("Player") |*/ LayerMask.GetMask("Monster");
        Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius, layer);
        Debug.Log(colliders.Length);
        // 3. 찾은 콜라이더 중에서 타격 가능한(IHitable) 오브젝트를 찾아서 Hit()한다.
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