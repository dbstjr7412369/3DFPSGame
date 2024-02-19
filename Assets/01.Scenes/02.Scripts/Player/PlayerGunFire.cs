using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGunFire : MonoBehaviour
{
    public int Damage = 1;

    // 목표: 마우스 왼쪽 버튼을 누르면 시선이 바라보는 방향으로 총을 발사하고 싶다.
    // 필요 속성
    // - 총알 튀는 이펙트 프리팹
    public ParticleSystem HitEffect;

    // - 발사 쿨타임
    public float FireCooltime = 0.2f;
    private float _timer;

    // - 총알 개수
    public int BulletRemainCount;
    public int BulletMaxCount = 30;

    // - 총알 개수 텍스트 UI
    public Text BulletTextUI;

    private const float RELOAD_TIME = 1.5f; // 재장전 시간
    private bool _isReloading = false;      // 재장전 중이냐?
    public GameObject ReloadTextObject;

    private void Start()
    {
        // 총알 개수 초기화
        BulletRemainCount = BulletMaxCount;
        RefreshUI();
    }

    private void RefreshUI()
    {
        BulletTextUI.text = $"{BulletRemainCount:d2}/{BulletMaxCount}";
    }

    private IEnumerator Reload_Coroutine()
    {
        _isReloading = true;

        // R키 누르면 1.5초 후 재장전, (중간에 총 쏘는 행위를 하면 재장전 취소)
        yield return new WaitForSeconds(RELOAD_TIME);
        BulletRemainCount = BulletMaxCount;
        RefreshUI();

        _isReloading = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && BulletRemainCount < BulletMaxCount)
        {
            if (!_isReloading)
            {
                StartCoroutine(Reload_Coroutine());
            }
        }

        ReloadTextObject.SetActive(_isReloading);


        _timer += Time.deltaTime;

        // 1. 만약에 마우스 왼쪽 버튼을 누른 상태 && 쿨타임이 다 지난 상태 && 총알 개수 > 0
        if (Input.GetMouseButton(0) && _timer >= FireCooltime && BulletRemainCount > 0)
        {
            // 재장전 취소
            if (_isReloading)
            {
                StopAllCoroutines();
                _isReloading = false;
            }

            BulletRemainCount -= 1;
            RefreshUI();

            _timer = 0;

            // 2. 레이(광선)을 생성하고, 위치와 방향을 설정한다.
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            // 3. 레이를 발사한다.
            // 4. 레이가 부딛힌 대상의 정보를 받아온다.
            RaycastHit hitInfo;
            bool isHit = Physics.Raycast(ray, out hitInfo);
            if (isHit)
            {
                //실습 과제 18. 레이저를 몬스터에게 맞출 시 몬스터 체력 닳는 기능 구현
                IHitable hitObject = hitInfo.collider.GetComponent<IHitable>();
                if (hitObject != null)  // 때릴 수 있는 친구인가요?
                {
                    hitObject.Hit(Damage);
                }


                // 5. 부딛힌 위치에 (총알이 튀는)이펙트를 위치한다.
                HitEffect.gameObject.transform.position = hitInfo.point;
                // 6. 이펙트가 쳐다보는 방향을 부딛힌 위치의 법선 벡터로 한다.
                HitEffect.gameObject.transform.forward = hitInfo.normal;
                HitEffect.Play();
            }
        }

    }
}