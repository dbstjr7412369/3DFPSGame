using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

// ����: ���� ������
// -> ���� ��ü�� ���¸� �˸���, ���۰� ���� �ؽ�Ʈ�� ��Ÿ����.
public enum GameState
{
    Ready, // �غ�
    Go, // ����
    Over,  // ����
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // ������ ���´� ó���� "�غ�" ����
    public GameState State { get; private set; } = GameState.Ready;

    public TextMeshProUGUI StateTextUI;

    public Color GoStateColor;

    public PopupOption OptionUI;
  
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(Start_Coroutine());
    }

    private IEnumerator Start_Coroutine()
    {
        // ���� ����
        // 1. ���� "�غ�" ���� (Ready...)
        State = GameState.Ready;
        StateTextUI.gameObject.SetActive(true);
        Refresh();

        // 2. 1.6�� �Ŀ� ���� "����" ���� (Start!)
        yield return new WaitForSeconds(1.6f);
        State = GameState.Go;
        Refresh();

        // 3. 0.4�� �Ŀ� �ؽ�Ʈ �������...
        yield return new WaitForSeconds(0.4f);
        StateTextUI.gameObject.SetActive(false);
    }

    // 4. �÷��̸� �ϴٰ�
    // 5. �÷��̾� ü���� 0�� �Ǹ� "���� ����" ����
    public void GameOver()
    {
        State = GameState.Over;
        StateTextUI.gameObject.SetActive(true);
        Refresh();
    }
    public void Refresh()
    {
        switch (State)
        {
            case GameState.Ready:
            {
                StateTextUI.text = "Ready...";
                StateTextUI.color = new Color(0 / 255f, 16 / 255f, 195 / 255f, 255 / 255f);

                break;
            }

            case GameState.Go:
            {
                StateTextUI.text = "Go!";
                StateTextUI.color = GoStateColor;

                break;
            }

            case GameState.Over:
            {
                StateTextUI.text = "Game Over";
                StateTextUI.color = new Color32(195, 0, 33, 255);

                break;
            }

        }
    }

    public void OnOptionButtonClicked()
    {
        Debug.Log("�ɼ� ��ư Ŭ��");
        OptionUI.Open();
    }
}