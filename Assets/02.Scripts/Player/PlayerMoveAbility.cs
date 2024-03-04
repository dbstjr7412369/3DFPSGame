using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoveAbility : MonoBehaviour, IHitable
{
    // ��ǥ: Ű���� ����Ű(wasd)�� ������ ĳ���͸� �ٶ󺸴� ���� �������� �̵���Ű�� �ʹ�. 
    // �Ӽ�:
    // - �̵��ӵ�
    public float MoveSpeed = 5;     // �Ϲ� �ӵ�
    public float RunSpeed = 10;    // �ٴ� �ӵ�

    public float Stamina = 100;             // ���¹̳�
    public const float MaxStamina = 100;    // ���¹̳� �ִ뷮
    public float StaminaConsumeSpeed = 33f; // �ʴ� ���¹̳� �Ҹ�
    public float StaminaChargeSpeed = 50;  // �ʴ� ���¹̳� ������

    [Header("���¹̳� �����̴� UI")]
    public Slider StaminaSliderUI;

    private CharacterController _characterController;
    private Animator _animator;


    // ��ǥ: �����̽��ٸ� ������ ĳ���͸� �����ϰ� �ʹ�.
    // �ʿ� �Ӽ�:
    // - ���� �Ŀ� ��
    public float JumpPower = 10f;
    public int JumpMaxCount = 2;
    public int JumpRemainCount;
    private bool _isJumping = false;
    // ���� ����:
    // 1. ���࿡ [Spacebar] ��ư�� ������..
    // 2. �÷��̾�� y�࿡ �־� ���� �Ŀ��� �����Ѵ�



    // ��ǥ: ĳ���Ϳ� �߷��� �����ϰ� �ʹ�.
    // �ʿ� �Ӽ�:
    // - �߷� ��
    private float _gravity = -20;
    // - ������ �߷� ����: y�� �ӵ�
    public float _yVelocity = 0f;
    // ���� ����:
    // 1. �߷� ���ӵ��� �����ȴ�.
    // 2. �÷��̾�� y�࿡ �־� �߷��� �����Ѵ�.



    // ��ǥ: ���� ��� �ִ� ���¿��� �����̽��ٸ� ������ ��Ÿ�⸦ �ϰ� �ʹ�.
    // �ʿ� �Ӽ�:
    // - ��Ÿ�� �Ŀ�
    public float ClimbingPower = 7f;
    // - ��Ÿ�� ���¹̳� �Ҹ� ����
    public float ClimbingStaminaCosumeFactor = 1.5f;
    // - ��Ÿ�� ����
    private bool _isClimbing = false;
    // ���� ����
    // 1. ���� ���� ��� �ִµ�
    // 2. [Spacebar] ��ư�� ������ ������
    // 3. ���� Ÿ�ڴ�.

    public int Health;
    public int MaxHealth = 1000000;
    public Slider HealthSliderUI;


    public Image HitEffectImageUI;


    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        Stamina = MaxStamina;
        Health = MaxHealth;
    }

    // ���� ����
    // 1. Ű �Է� �ޱ�
    // 2. 'ĳ���Ͱ� �ٶ󺸴� ����'�� �������� ���ⱸ�ϱ�
    // 3. �̵��ϱ�

    void Update()
    {
        HealthSliderUI.value = (float)Health / (float)MaxHealth;  // 0 ~ 1

        if (GameManager.Instance.State != GameState.Go)
        {
            return;
        }

        // 1. ���� ���� ��� �ִµ� && ���¹̳ʰ� > 0
        if (Stamina > 0 && _characterController.collisionFlags == CollisionFlags.Sides)
        {
            // 2. [Spacebar] ��ư�� ������ ������
            if (Input.GetKey(KeyCode.Space))
            {
                // 3. ���� Ÿ�ڴ�.
                _isClimbing = true;
                _yVelocity = ClimbingPower;

            }
        }



        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            // FPS ī�޶� ���� ��ȯ
            CameraManager.Instance.SetCameraMode(CameraMode.FPS);
       
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            // TPS ī�޶� ���� ��ȯ
            CameraManager.Instance.SetCameraMode(CameraMode.TPS);
        }


        // 1. Ű �Է� �ޱ�
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 2. 'ĳ���Ͱ� �ٶ󺸴� ����'�� �������� ���ⱸ�ϱ�
        Vector3 dir = new Vector3(h, 0, v);             // ���� ��ǥ�� (������ ��������) 
        Vector3 unNormalizedDir = dir;
        dir.Normalize();
        // Transforms direction from local space to world space.
        dir = Camera.main.transform.TransformDirection(dir); // �۷ι� ��ǥ�� (������ ��������)
        // �ǽ� ���� 1. Shift ������ ������ ���� �ٱ�
        float speed = MoveSpeed; // 5
        if (_isClimbing || Input.GetKey(KeyCode.LeftShift)) // �ǽ� ���� 2. ���¹̳� ����
        {
            // - Shfit ���� ���ȿ��� ���¹̳��� ������ �Ҹ�ȴ�. (3��)

            // ����(����) ������
            // -> ���ǽ��� ����ؼ� ���ǽ��� ��, ���� ���ο� ���� �ٸ� ������� ����
            // ���ǽ� ? ���ǽ��� ���϶��� �� : ���ǽ��� �����϶��� ��

            float factor = _isClimbing ? ClimbingStaminaCosumeFactor : 1f;
            Stamina -= StaminaConsumeSpeed * factor * Time.deltaTime;


            // Ŭ���̹� ���°� �ƴҶ��� ���ǵ� up!
            if (!_isClimbing && Stamina > 0)
            {
                speed = RunSpeed;
            }
        }
        else
        {
            // - �ƴϸ� ���¹̳��� �Ҹ� �Ǵ� �ӵ����� ���� �ӵ��� �����ȴ� (2��)
            Stamina += StaminaChargeSpeed * Time.deltaTime; // �ʴ� 50�� ����
        }

        Stamina = Mathf.Clamp(Stamina, 0, 100);
        StaminaSliderUI.value = Stamina / MaxStamina;  // 0 ~ 1;//

        // ���� ������� 
        if (_characterController.isGrounded)
        {
            if (_yVelocity < -20)
            {
                DamageInfo damageInfo = new DamageInfo(DamageType.Normal, 10 * (int)(_yVelocity / -10f));
                Hit(damageInfo);
            }

            _isJumping = false;
            _isClimbing = false;
            _yVelocity = 0f;
            JumpRemainCount = JumpMaxCount;
        }

        // ���� ����
        // 1. ���࿡ [Spacebar] ��ư�� ������ ���� && (���̰ų� or ���� Ƚ���� �����ִٸ�)
        if (Input.GetKeyDown(KeyCode.Space) && (_characterController.isGrounded || (_isJumping && JumpRemainCount > 0)))
        {
            _isJumping = true;

            JumpRemainCount--;

            // 2. �÷��̾�� y�࿡ �־� ���� �Ŀ��� �����Ѵ�.
            _yVelocity = JumpPower;
        }


        // 3-1. �߷� ����
        // 1. �߷� ���ӵ��� �����ȴ�.
        _yVelocity += _gravity * Time.deltaTime;

        // 2. �÷��̾�� y�࿡ �־� �߷��� �����Ѵ�.

        dir.y = _yVelocity;

        // 3-2. �̵��ϱ�
        //transform.position += speed * dir * Time.deltaTime;
        _characterController.Move(dir * speed * Time.deltaTime);
        _animator.SetFloat("Move", unNormalizedDir.magnitude);
    }

    public void Hit(DamageInfo damageInfo)
    {
        StartCoroutine(HitEffect_Coroutine(0.2f));
        CameraManager.Instance.CameraShake.Shake();

        Health -= damageInfo.Amount;

        RefreshAnimation();

        if (Health <= 0)
        {
            HealthSliderUI.value = 0f;
            gameObject.SetActive(false);

            GameManager.Instance.GameOver();
        }
    }

    public void RefreshAnimation()
    {
        _animator.SetLayerWeight(1, 1 - Health / (float)MaxHealth);
    }

    private IEnumerator HitEffect_Coroutine(float delay)
    {
        // ���� 40. ��Ʈ ����Ʈ �̹��� 0.3�ʵ��� ���̰� ����
        HitEffectImageUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        HitEffectImageUI.gameObject.SetActive(false);
    }
}