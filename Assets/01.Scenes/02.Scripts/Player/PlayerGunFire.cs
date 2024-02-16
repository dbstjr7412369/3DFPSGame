using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class PlayerGunFire : MonoBehaviour
{

    private Coroutine _reloadCorutine;
    // ���콺 ���ʹ�ư�� ������ �ü��� �ٶ󺸴� �������� ���� �߻��ϰ� �ʹ�

    // �ʿ�Ӽ� 
    // �Ѿ� Ƣ�� ����Ʈ ������
    public ParticleSystem HitEffect;
    public Text BulletCountText;
    public Text ReloadBulletCountText;

    // �߻� ��Ÿ��
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
        // ����ҷ� = �߽� �ҷ�
        BulletCountText.text = $"{Bullet:d2}/{MaxBullet}";
        BulletCountText.color = Color.white;
    }
    private void RefreshUI_Reload()
    {
        ReloadBulletCountText.text = $"��������...";
        ReloadBulletCountText.color = Color.red;
    }
    //�ǽ� ���� 14.�Ѿ� �ִ� ���� 30�� ���� �� RŰ ������ �ʱ�ȭ(������)
    public void PlayerFire()
    {
        //�ǽ� ���� 13. ���콺 ���� ��ư ������ ������ ���� (��Ÿ�� ����)
        _timer += Time.deltaTime;
        // 1. ���࿡ ���콺 ���� ��ư�� ���� ���� && ��Ÿ���� �� ���� ����
        if (Input.GetMouseButton(0) && _timer >= FireCoolTime && Bullet > 0)
        {
            _timer = 0;

            // 2. ����(����)�� �����ϰ�, ��ġ�� ������ �����Ѵ�
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            // 3.���̸� �߻��Ѵ�
            // 4. ���̰� �ε��� ����� ������ �޾ƿ´�
            RaycastHit hitInfo;

            bool IsHit = Physics.Raycast(ray, out hitInfo);
            if (IsHit)
            {
                // 5. �ε��� ��ġ�� (�Ѿ��� Ƣ��)����Ʈ�� �����Ѵ�
                HitEffect.gameObject.transform.position = hitInfo.point;
                // 6. ����Ʈ�� �Ĵٺ��� ������ �ε��� ��ġ�� ���� ���ͷ� �Ѵ�
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
        //�ǽ� ���� 16.RŰ ������ 1.5�� �� ������, (�߰��� �� ��� ������ �ϸ� ������ ���)
    }
    private IEnumerator Reload_Coroutine(float reloadTime)
    {
        yield return new WaitForSeconds(reloadTime);
        Debug.Log("��������...");
        yield break;
    }
}
