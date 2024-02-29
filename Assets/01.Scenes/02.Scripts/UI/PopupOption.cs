using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PopupOption : MonoBehaviour
{
    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
    private void Awake()
    {
        gameObject.SetActive(false);
    }
   

    public void OnOptionButtonContinue()
    {
        Debug.Log("����ϱ�");

        GameManager.Instance.Continue();

        Close();
    }
    public void OnOptionButtonAgain()
    {
        Debug.Log("�ٽ��ϱ�");
    }

    public void OnOptionButtonTermination()
    {
        Debug.Log("��������");
    }
}
