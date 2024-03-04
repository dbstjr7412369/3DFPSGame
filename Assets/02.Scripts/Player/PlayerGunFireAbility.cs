using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGunFireAbility : MonoBehaviour
{
    public Gun CurrentGun;        // ���� ����ִ� ��
    private int _currentGunIndex; // ���� ����ִ� ���� ����

    private float _timer;

    private const int DefaultFOV = 60;
    private const int ZoomFOV = 20;
    private bool _isZoomMode = false;  // �� ����?
    private const float ZoomInDuration = 0.3f;
    private const float ZoomOutDuration = 0.2f;
    private float _zoomProgress; // 0 ~ 1

    private Animator _animator;

    public GameObject CrosshairUI;
    public GameObject CrosshairZoomUI;


    // ���� ��� �κ��丮
    public List<Gun> GunInventory;


    // ��ǥ: ���콺 ���� ��ư�� ������ �ü��� �ٶ󺸴� �������� ���� �߻��ϰ� �ʹ�.
    // �ʿ� �Ӽ�
    // - �Ѿ� Ƣ�� ����Ʈ ������
    public ParticleSystem HitEffect;


    // - �Ѿ� ���� �ؽ�Ʈ UI
    public TextMeshProUGUI BulletTextUI;

    private bool _isReloading = false;      // ������ ���̳�?
    public GameObject ReloadTextObject;

    // ���� �̹��� UI
    public Image GunImageUI;

    public List<GameObject> MuzzleFire;

    private void Start()
    {
        foreach (GameObject muzzleEffect in MuzzleFire)
        {
            muzzleEffect.SetActive(false);
        }

        _animator = GetComponentInChildren<Animator>();

        _currentGunIndex = 0;

        // �Ѿ� ���� �ʱ�ȭ
        RefreshUI();
        RefreshGun();
    }

    public void RefreshUI()
    {
        GunImageUI.sprite = CurrentGun.ProfileImage;
        BulletTextUI.text = $"{CurrentGun.BulletRemainCount:d2}/{CurrentGun.BulletMaxCount}";

        CrosshairUI.SetActive(!_isZoomMode);
        CrosshairZoomUI.SetActive(_isZoomMode);
    }

    private IEnumerator Reload_Coroutine()
    {
        _isReloading = true;

        // RŰ ������ 1.5�� �� ������, (�߰��� �� ��� ������ �ϸ� ������ ���)
        yield return new WaitForSeconds(CurrentGun.ReloadTime);
        CurrentGun.BulletRemainCount = CurrentGun.BulletMaxCount;
        RefreshUI();

        _isReloading = false;
    }

    //_animator
    // �� ��忡 ���� ī�޶� FOV �������ִ� �޼���
    private void RefreshZoomMode()
    {
        if (!_isZoomMode)
        {
            Camera.main.fieldOfView = DefaultFOV;
        }
        else
        {
            Camera.main.fieldOfView = ZoomFOV;
        }
    }
    private void Update()
    {
        if (GameManager.Instance.State != GameState.Go)
        {
            return;
        }

        // ���콺 �� ��ư ������ �� && ���� ���� ��������
        if (Input.GetMouseButtonDown(2) && CurrentGun.GType == GunType.Sniper)
        {
            _isZoomMode = !_isZoomMode; // �� ��� ������
            _zoomProgress = 0f;
            RefreshUI();
        }

        if (CurrentGun.GType == GunType.Sniper && _zoomProgress < 1)
        {
            if (_isZoomMode) // ����
            {
                _zoomProgress += Time.deltaTime / ZoomInDuration;
                Camera.main.fieldOfView = Mathf.Lerp(DefaultFOV, ZoomFOV, _zoomProgress);
            }
            else
            {
                _zoomProgress += Time.deltaTime / ZoomOutDuration;
                Camera.main.fieldOfView = Mathf.Lerp(ZoomFOV, DefaultFOV, _zoomProgress);
            }
        }



        if (Input.GetKeyDown(KeyCode.LeftBracket)) // '['
        {
            // �ڷΰ��� 
            _currentGunIndex--;
            if (_currentGunIndex < 0)
            {
                _currentGunIndex = GunInventory.Count - 1;
            }
            CurrentGun = GunInventory[_currentGunIndex];
            _isZoomMode = false;
            _zoomProgress = 1f;
            RefreshZoomMode();
            RefreshGun();
            RefreshUI();
        }
        else if (Input.GetKeyDown(KeyCode.RightBracket)) // ']'
        {
            // ������ ����
            _currentGunIndex++;
            if (_currentGunIndex >= GunInventory.Count)
            {
                _currentGunIndex = 0;
            }
            CurrentGun = GunInventory[_currentGunIndex];
            _isZoomMode = false;
            _zoomProgress = 1f;
            RefreshZoomMode();
            RefreshGun();
            RefreshUI();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _currentGunIndex = 0;
            CurrentGun = GunInventory[0];
            _isZoomMode = false;
            _zoomProgress = 1f;
            RefreshZoomMode();
            RefreshGun();
            RefreshUI();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _currentGunIndex = 1;
            CurrentGun = GunInventory[1];
            _isZoomMode = false;
            _zoomProgress = 1f;
            RefreshZoomMode();
            RefreshGun();
            RefreshUI();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _currentGunIndex = 2;
            CurrentGun = GunInventory[2];
            _isZoomMode = false;
            _zoomProgress = 1f;
            RefreshZoomMode();
            RefreshGun();
            RefreshUI();
        }

        if (Input.GetKeyDown(KeyCode.R) && CurrentGun.BulletRemainCount < CurrentGun.BulletMaxCount)
        {
            if (!_isReloading)
            {
                StartCoroutine(Reload_Coroutine());
            }
        }

        ReloadTextObject.SetActive(_isReloading);


        _timer += Time.deltaTime;

        // 1. ���࿡ ���콺 ���� ��ư�� ���� ���� && ��Ÿ���� �� ���� ���� && �Ѿ� ���� > 0
        if (Input.GetMouseButton(0) && _timer >= CurrentGun.FireCooltime && CurrentGun.BulletRemainCount > 0)
        {
            //�ǽ� ���� 45. �ڷ�ƾ�� ����ؼ� ���� �򶧸��� ���� ����Ʈ �� ������ �ϳ��� 0.1�ʵ��� ���̰� ������ �ϱ�


            // ������ ���
            if (_isReloading)
            {
                StopAllCoroutines();
                _isReloading = false;
            }

            CurrentGun.BulletRemainCount -= 1;
            RefreshUI();

            _timer = 0;

            //�� ����Ʈ�� �ϳ��� ���ش�
            //0.1�� ��...
            // ���ش�.
            StartCoroutine(MuzzleFireEffectOn_Coroutine());

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
                    DamageInfo damageInfo = new DamageInfo(DamageType.Normal, CurrentGun.Damage);
                    damageInfo.Position = hitInfo.point;
                    damageInfo.Normal = hitInfo.normal;

                    if (UnityEngine.Random.Range(0, 2) == 0)
                    {
                        Debug.Log("ũ��Ƽ��!");
                        damageInfo.DamageType = DamageType.Critical;
                        damageInfo.Amount *= 2;
                    }

                    hitObject.Hit(damageInfo);
                }
                // 5. �ε��� ��ġ�� (�Ѿ��� Ƣ��)����Ʈ�� ��ġ�Ѵ�.
                HitEffect.gameObject.transform.position = hitInfo.point;
                // 6. ����Ʈ�� �Ĵٺ��� ������ �ε��� ��ġ�� ���� ���ͷ� �Ѵ�.
                HitEffect.gameObject.transform.forward = hitInfo.normal;
                HitEffect.Play();
            }
        }
    }
    private IEnumerator MuzzleFireEffectOn_Coroutine()
    {
        //�� ����Ʈ�� �ϳ��� ���ش�
        int rendumIndex =  UnityEngine.Random.Range(0, MuzzleFire.Count);
        MuzzleFire[rendumIndex].SetActive(true);
        //0.1�� ��...
        yield return new WaitForSeconds(0.1f);
        // ���ش�.
        MuzzleFire[rendumIndex].SetActive(false);

    }
    private void RefreshGun()
    {
        foreach (Gun gun in GunInventory)
        {
            /**if (gun == CurrentGun)
            {
                gun.gameObject.SetActive(true);
            }
            else
            {
                gun.gameObject.SetActive(false);
            }**/
            gun.gameObject.SetActive(gun == CurrentGun);
        }
    }
}