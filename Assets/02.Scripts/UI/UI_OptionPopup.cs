using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UI_OptionPopup : MonoBehaviour
{
    public void Open()
    {
        // ���� ȿ�����̶����
        // �ʱ�ȭ �Լ�
        gameObject.SetActive(true);
    }

    public void Close()
    {
        // ���� ȿ�����̶����...
        // ���� ����
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void OnClickContinueButton()
    {
        Debug.Log("����ϱ�");

        GameManager.Instance.Continue();

        Close();
    }

    public void OnClickResumeButton()
    {
        // ���Ŵ�����.(���� ���� �ִ� ��)�� ���� �ε��ض�.
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        Debug.Log("�ٽ��ϱ�");
    }

    public void OnClickExitButton()
    {
        Debug.Log("��������");

        // ���� �� �������� ��� �����ϴ� ���
        Application.Quit();

#if UNITY_EDITOR
        // ����Ƽ �����Ϳ��� �������� ��� �����ϴ� ���
        UnityEditor.EditorApplication.isPlaying = false;
#endif        
    }
}