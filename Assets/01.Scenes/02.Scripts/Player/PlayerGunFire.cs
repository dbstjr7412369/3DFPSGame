using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGunFire : MonoBehaviour
{
    public int Damage = 1;

    // ��ǥ: ���콺 ���� ��ư�� ������ �ü��� �ٶ󺸴� �������� ���� �߻��ϰ� �ʹ�.
    // �ʿ� �Ӽ�
    // - �Ѿ� Ƣ�� ����Ʈ ������
    public ParticleSystem HitEffect;

    // - �߻� ��Ÿ��
    public float FireCooltime = 0.2f;
    private float _timer;

    // - �Ѿ� ����
    public int BulletRemainCount;
    public int BulletMaxCount = 30;

    // - �Ѿ� ���� �ؽ�Ʈ UI
    public Text BulletTextUI;

    private const float RELOAD_TIME = 1.5f; // ������ �ð�
    private bool _isReloading = false;      // ������ ���̳�?
    public GameObject ReloadTextObject;

    private void Start()
    {
        // �Ѿ� ���� �ʱ�ȭ
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

        // RŰ ������ 1.5�� �� ������, (�߰��� �� ��� ������ �ϸ� ������ ���)
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

        // 1. ���࿡ ���콺 ���� ��ư�� ���� ���� && ��Ÿ���� �� ���� ���� && �Ѿ� ���� > 0
        if (Input.GetMouseButton(0) && _timer >= FireCooltime && BulletRemainCount > 0)
        {
            // ������ ���
            if (_isReloading)
            {
                StopAllCoroutines();
                _isReloading = false;
            }

            BulletRemainCount -= 1;
            RefreshUI();

            _timer = 0;

            // 2. ����(����)�� �����ϰ�, ��ġ�� ������ �����Ѵ�.
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            // 3. ���̸� �߻��Ѵ�.
            // 4. ���̰� �ε��� ����� ������ �޾ƿ´�.
            RaycastHit hitInfo;
            bool isHit = Physics.Raycast(ray, out hitInfo);
            if (isHit)
            {
                //�ǽ� ���� 18. �������� ���Ϳ��� ���� �� ���� ü�� ��� ��� ����
                IHitable hitObject = hitInfo.collider.GetComponent<IHitable>();
                if (hitObject != null)  // ���� �� �ִ� ģ���ΰ���?
                {
                    hitObject.Hit(Damage);
                }


                // 5. �ε��� ��ġ�� (�Ѿ��� Ƣ��)����Ʈ�� ��ġ�Ѵ�.
                HitEffect.gameObject.transform.position = hitInfo.point;
                // 6. ����Ʈ�� �Ĵٺ��� ������ �ε��� ��ġ�� ���� ���ͷ� �Ѵ�.
                HitEffect.gameObject.transform.forward = hitInfo.normal;
                HitEffect.Play();
            }
        }

    }
}