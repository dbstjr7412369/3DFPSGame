using System.Collections;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public enum ItemState
    {
        Idle,  // ��� ����     (�÷��̾���� �Ÿ��� üũ�Ѵ�.)
        // �� (if ����� ����� ����..)
        Trace, // ������� ����  (N�ʿ� ���ļ� Slerp�� �÷��̾�� ����´�.)
    }
    public ItemType ItemType;
    private ItemState _itemState = ItemState.Idle;

    private Transform _player;
    public float EatDistance = 5f;

    private Vector3 _startPosition;
    private const float TRACE_DURATION = 0.3f;
    private float _progress = 0;

    private void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _startPosition = transform.position;
    }


    private void Update()
    {
        if (GameManager.Instance.State != GameState.Go)
        {
            return;
        }

        switch (_itemState)
        {
            case ItemState.Idle:
                Idle();
                break;

            case ItemState.Trace:
                Trace();
                break;
        }
    }

    public void Init()
    {
        _startPosition = transform.position;
        _progress = 0f;
        _traceCoroutine = null;
        _itemState = ItemState.Idle;
    }

    private void Idle()
    {
        // ��� ������ �ൿ: �÷��̾���� �Ÿ��� üũ�Ѵ�.
        float distance = Vector3.Distance(_player.position, transform.position);
        // ���� ����: ����� ����� ����..
        if (distance <= EatDistance)
        {
            _itemState = ItemState.Trace;
        }
    }

    private Coroutine _traceCoroutine;
    private void Trace()
    {
        if (_traceCoroutine == null)
        {
            _traceCoroutine = StartCoroutine(Trace_Coroutine());
        }
    }

    private IEnumerator Trace_Coroutine()
    {
        while (_progress < 0.6)
        {
            _progress += Time.deltaTime / TRACE_DURATION;
            transform.position = Vector3.Slerp(_startPosition, _player.position, _progress);

            yield return null;
        }

        // 1. ������ �Ŵ���(�κ��丮)�� �߰��ϰ�,
        ItemManager.Instance.AddItem(ItemType);
        ItemManager.Instance.Refresh();
        // 2. �������.
        gameObject.SetActive(false);
    }
}