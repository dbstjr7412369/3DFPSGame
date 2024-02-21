using UnityEngine;

public enum GunType
{
    Rifle,  // ������
    Sniper, // ������ 
    Pistol, // ����
}

public class Gun : MonoBehaviour
{
    public GunType GType;

    // - ��ǥ �̹���
    public Sprite ProfileImage;

    // - ���ݷ�
    public int Damage = 10;

    // - �߻� ��Ÿ��
    public float FireCooltime = 0.2f;


    // - �Ѿ� ����
    public int BulletRemainCount;
    public int BulletMaxCount = 30;

    // - ������ �ð�
    public float ReloadTime = 1.5f;

    private void Start()
    {
        // �Ѿ� ���� �ʱ�ȭ
        BulletRemainCount = BulletMaxCount;
    }
}