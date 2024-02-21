using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Health,  // ü���� ������.
    Stamina, // ���¹̳� ������.
    Bullet   // ���� ����ִ� ���� �Ѿ��� ������.
}

public class Item
{
    public ItemType ItemType;
    public int Count;

    public Item(ItemType itemType, int count)
    {
        ItemType = itemType;
        Count = count;
    }
    
    public bool TryUse()
    {
        if (Count == 0)
        {
            return false;
        }

        Count -= 1;

        switch (ItemType)
        {
            case ItemType.Health:
            {
                //Todo: �÷��̾� ü�� ������
                PlayerMoveAbility playerMoveAbility = GameObject.FindWithTag("Player").GetComponent<PlayerMoveAbility>();
                playerMoveAbility.Health = playerMoveAbility.MaxHealth;
                //null �����˻� �� ��
                break;
            }

            case ItemType.Stamina:
            {
                // Todo: �÷��̾� ���¹̳� ������
                PlayerMoveAbility playerMoveAbility = GameObject.FindWithTag("Player").GetComponent<PlayerMoveAbility>();
                playerMoveAbility.Stamina = playerMoveAbility.GetStamina();
             
                break;
            }

            case ItemType.Bullet:
            {
                // Todo: �÷��̾ ���� ����ִ� ���� �Ѿ��� ������.
                PlayerGunFireAbility ability = GameObject.FindWithTag("Player").GetComponent<PlayerGunFireAbility>();
                ability.CurrentGun.BulletRemainCount = ability.CurrentGun.BulletMaxCount;
                //ability.RefreshUI();
                break;
            }
        }

        return true;
    }
}