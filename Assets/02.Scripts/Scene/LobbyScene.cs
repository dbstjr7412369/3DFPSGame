using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SceneNames
{
    Lobby,   // 0
    Loading, // 1
    Main     // 2
}

public class LobbyScene : MonoBehaviour
{
    // ����� ������ ���� �����ϰų�(ȸ������), ����� �����͸� �о�
    // ����� �Է°� ��ġ�ϴ��� �˻�(�α���)�Ѵ�.
    public TMP_InputField IDInputField;         // ���̵� �Է�â
    public TMP_InputField PasswordInputField;   // ��й�ȣ �Է�â
    public TextMeshProUGUI NotifyTextUI;         // �˸� �ؽ�Ʈ

    private void Start()
    {
        IDInputField.text = string.Empty;
        PasswordInputField.text = string.Empty;
        NotifyTextUI.text = string.Empty;
    }

    // ȸ������ ��ư Ŭ��
    public void OnClickRegisterButton()
    {
        string id = IDInputField.text;
        string pw = PasswordInputField.text;
        // 0. ���̵� �Ǵ� ��й�ȣ�� �Է����� ���� ���
        if (id == string.Empty || pw == String.Empty)
        {
            NotifyTextUI.text = "���̵�� ��й�ȣ�� ��Ȯ�ϰ� �Է����ּ���.";
            return;
        }
        // 1. �̹� ���� �������� ȸ�������� �Ǿ��ִ� ���
        if (PlayerPrefs.HasKey(id))
        {
            NotifyTextUI.text = "�̹� �����ϴ� �����Դϴ�.";
        }
        // 2. ȸ�����Կ� �����ϴ� ���    
        else
        {
            NotifyTextUI.text = "ȸ�������� �Ϸ��߽��ϴ�.";
            PlayerPrefs.SetString(id, pw);
        }

        IDInputField.text = string.Empty;
        PasswordInputField.text = string.Empty;
    }

    // �α��� ��ư Ŭ��
    public void OnClickLoginButton()
    {
        string id = IDInputField.text;
        string pw = PasswordInputField.text;
        // 0. ���̵� �Ǵ� ��й�ȣ �Է� X -> "���̵�� ��й�ȣ�� ��Ȯ�ϰ� �Է����ּ���."
        if (id == string.Empty || pw == String.Empty)
        {
            NotifyTextUI.text = "���̵�� ��й�ȣ�� ��Ȯ�ϰ� �Է����ּ���.";
            return;
        }

        if (!PlayerPrefs.HasKey(id))
        {
            // 1. ���� ���̵�               -> "���̵� Ȯ�����ּ���."
            NotifyTextUI.text = "���̵�� ��й�ȣ�� Ȯ�����ּ���.";
        }
        else if (PlayerPrefs.GetString(id) != pw)
        {
            // 2. Ʋ�� ��й�ȣ             -> "��й�ȣ�� Ȯ�����ּ���."
            NotifyTextUI.text = "���̵�� ��й�ȣ�� Ȯ�����ּ���.";
        }
        else
        {
            // 3. �α��� ����               -> ���� ������ �̵� 
            SceneManager.LoadScene((int)SceneNames.Loading);
        }
    }

}