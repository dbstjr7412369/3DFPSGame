using System.Collections;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public enum ItemState
    {
        Idle,  // 대기 상태     (플레이어와의 거리를 체크한다.)
        // ▼ (if 충분히 가까워 지면..)
        Trace, // 날라오는 상태  (N초에 걸쳐서 Slerp로 플레이어에게 날라온다.)
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
        // 대기 상태의 행동: 플레이어와의 거리를 체크한다.
        float distance = Vector3.Distance(_player.position, transform.position);
        // 전이 조건: 충분히 가까워 지면..
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

        // 1. 아이템 매니저(인벤토리)에 추가하고,
        ItemManager.Instance.AddItem(ItemType);
        ItemManager.Instance.Refresh();
        // 2. 사라진다.
        gameObject.SetActive(false);
    }
}