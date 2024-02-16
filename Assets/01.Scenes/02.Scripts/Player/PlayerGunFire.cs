using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class PlayerGunFire : MonoBehaviour
{

    private Coroutine _reloadCorutine;
    // 마우스 왼쪽버튼을 누르면 시선이 바라보는 방향으로 총을 발사하고 싶다

    // 필요속성 
    // 총알 튀는 이펙트 프리펩
    public ParticleSystem HitEffect;
    public Text BulletCountText;
    public Text ReloadBulletCountText;

    // 발사 쿨타임
    public float FireCoolTime = 0.2f;
    private float _timer;
    public int MaxBullet = 30;
    public int Bullet = 0;

    private void Start()
    {
        
        Bullet = MaxBullet;
        RefreshUI();
    }
    void Update()
    {
        RefreshUI();
        PlayerFire();
    }
    private void RefreshUI()
    {
        // 현재불렛 = 멕스 불렛
        BulletCountText.text = $"{Bullet:d2}/{MaxBullet}";
        BulletCountText.color = Color.white;
    }
    private void RefreshUI_Reload()
    {
        ReloadBulletCountText.text = $"재장전중...";
        ReloadBulletCountText.color = Color.red;
    }
    //실습 과제 14.총알 최대 개수 30개 적용 및 R키 누르면 초기화(재장전)
    public void PlayerFire()
    {
        //실습 과제 13. 마우스 왼쪽 버튼 누르고 있으면 연사 (쿨타임 적용)
        _timer += Time.deltaTime;
        // 1. 만약에 마우스 왼쪽 버튼을 누른 상태 && 쿨타임이 다 지난 상태
        if (Input.GetMouseButton(0) && _timer >= FireCoolTime && Bullet > 0)
        {
            _timer = 0;

            // 2. 레이(광선)을 생성하고, 위치와 방향을 설정한다
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            // 3.레이를 발사한다
            // 4. 레이가 부딛힌 대상의 정보를 받아온다
            RaycastHit hitInfo;

            bool IsHit = Physics.Raycast(ray, out hitInfo);
            if (IsHit)
            {
                // 5. 부딛힌 위치에 (총알이 튀는)이펙트를 생성한다
                HitEffect.gameObject.transform.position = hitInfo.point;
                // 6. 이펙트가 쳐다보는 방향을 부딛힌 위치의 법선 백터로 한다
                HitEffect.gameObject.transform.forward = hitInfo.normal;
                HitEffect.Play();
            }
            Bullet--;
        }
        if (Input.GetKey(KeyCode.R))
        {
            StopCoroutine(_reloadCorutine);
            Bullet = MaxBullet;
            StartCoroutine(Reload_Coroutine(1.5f));
        }
        //실습 과제 16.R키 누르면 1.5초 후 재장전, (중간에 총 쏘는 행위를 하면 재장전 취소)
    }
    private IEnumerator Reload_Coroutine(float reloadTime)
    {
        yield return new WaitForSeconds(reloadTime);
        Debug.Log("재장전중...");
        yield break;
    }
}
