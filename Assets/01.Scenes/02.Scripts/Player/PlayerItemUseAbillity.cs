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
            // 체력 아이템 사용
           bool result = ItemManager.Instance.TryUseItem(ItemType.Health);
            if (result)
            {
                // todo: 아이템 효과음 재생
                // todo: 파티클 시스템 재생
                ItemManager.Instance.RefreshUI();
            }
            else
            {
                // todo: 알림창(아이템이 부족합니다)
                
            }

        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            bool result = ItemManager.Instance.TryUseItem(ItemType.Bullet);
            if(result)
            {
                Debug.Log(0);
                ItemManager.Instance.RefreshUI();
                // 스테미나 아이템 사용
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
                // 총알 아이템 사용
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
