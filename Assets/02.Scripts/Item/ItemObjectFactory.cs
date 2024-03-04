using System.Collections.Generic;
using UnityEngine;

// ������ ������ ����: ������ ������Ʈ�� ������ å������.
// **���丮 ����**
// ��ü ������ ���� Ŭ������ �̿��� ĸ��ȭ ó���Ͽ� ��� "����"�ϰ� �ϴ� ������ ����
// ��ü ������ �ʿ��� ������ ���ø�ȭ �س��� �ܺο��� ���� ����Ѵ�.
// ����:
// 1. ������ ó�� ������ �и��Ͽ� ���յ��� ���� �� �ִ�. (���յ�: ������ ���� ��ȣ �������� �������� ����)
// 2. Ȯ�� �� ���������� ���ϴ�.
// 3. ��ü ���� �� �������� �� ���� �����ϵ��� �������� �� �ִ�.
// ����:
// 1. ��������� �ɲ� �� �����ϴ�.
// 2. �׷��� �����ؾ� �Ѵ�.
// 3. �װ� ����� ������ ũ�� ����.
public class ItemObjectFactory : MonoBehaviour
{
    public static ItemObjectFactory Instance { get; private set; }

    // (������)������ �����յ�
    public List<GameObject> ItemPrefabs;

    //������Ʈ Ǯ�� �����̶�� ���̳� ���ظ� ���� ������ â��� ǥ�� ��
    // ������ â��
    private List<ItemObject> _itemPool;
    public int PoolCount = 10;

    private void Awake()
    {
        Instance = this;

        _itemPool = new List<ItemObject>();

        for (int i = 0; i < PoolCount; ++i)            // 10��
        {
            foreach (GameObject prefab in ItemPrefabs) // 3��
            {
                // 1. �����
                GameObject item = Instantiate(prefab);
                // 2. â�� �ִ´�.
                item.transform.SetParent(this.transform);
                _itemPool.Add(item.GetComponent<ItemObject>());
                // 3. ��Ȱ��ȭ
                item.SetActive(false);
            }
        }
    }

    private ItemObject Get(ItemType itemType) // â�� ������
    {
        foreach (ItemObject itemObject in _itemPool) // â�� ������.
        {
            if (itemObject.gameObject.activeSelf == false
                && itemObject.ItemType == itemType)
            {
                return itemObject;
            }
        }

        return null;
    }

    // Ȯ�� ���� (�����! �����ڽ� �ֹ��Ұ�!)
    public void MakePercent(Vector3 position)
    {
        int percentage = UnityEngine.Random.Range(0, 100);
        if (percentage <= 20) // 20%
        {
            Make(ItemType.Health, position);
        }
        else if (percentage <= 40)
        {
            Make(ItemType.Stamina, position);
        }
        else if (percentage <= 50)
        {
            Make(ItemType.Bullet, position);
        }
    }

    // �⺻ ���� (�����! ���� ���ϴ°� �ֹ��Ұ�!)
    public void Make(ItemType itemType, Vector3 position)
    {
        ItemObject itemObject = Get(itemType);

        if (itemObject != null)
        {
            itemObject.transform.position = position;
            itemObject .Init();
            itemObject.gameObject.SetActive(true);
        }
    }
}