using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemUseAbillity : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log(0);
            // ü�� ������ ���
           bool result = ItemManager.Instance.TryUseItem(ItemType.Health);
            if (result)
            {
                // todo: ������ ȿ���� ���
                // todo: ��ƼŬ �ý��� ���
                ItemManager.Instance.RefreshUI();
            }
            else
            {
                // todo: �˸�â(�������� �����մϴ�)
                
            }

        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            bool result = ItemManager.Instance.TryUseItem(ItemType.Bullet);
            if(result)
            {
                Debug.Log(0);
                ItemManager.Instance.RefreshUI();
                // ���׹̳� ������ ���
                ItemManager.Instance.TryUseItem(ItemType.Bullet);
            }
            else
            { 
            
            }



        }

        else if (Input.GetKeyDown(KeyCode.U))
        {
            bool result = ItemManager.Instance.TryUseItem(ItemType.Stamina);
            if (result)
            {
                Debug.Log(0);
                // �Ѿ� ������ ���
                ItemManager.Instance.RefreshUI();
                ItemManager.Instance.TryUseItem(ItemType.Stamina);
            }
            else
            {
                Debug.Log(0);            
            }

        }

    }
}
