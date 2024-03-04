using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ����: ���� ���� �񵿱� ������� �ε��Ѵ�.
// �׸��� �ε� ������� �ǽð����� ǥ���Ѵ�.
public class LoadingScene : MonoBehaviour
{
    public SceneNames NextScene;

    public Slider LoadingSliderUI;
    public TextMeshProUGUI LoadingTextUI;

    void Start()
    {
        StartCoroutine(LoadNextScene_Coroutine());
    }

    private IEnumerator LoadNextScene_Coroutine()
    {
        // ������ ���� "�񵿱�" ������� �ε��Ѵ�.
        AsyncOperation ao = SceneManager.LoadSceneAsync((int)NextScene);

        // �ε�Ǵ� ���� ����� ȭ�鿡 ������ �ʰ� �Ѵ�.
        ao.allowSceneActivation = false;

        // �ε��� �Ϸ�� ������... �ݺ�
        while (!ao.isDone)
        {
            // �ε��ٵ� �̵���Ű��,
            // �ε� �ؽ�Ʈ�� �����ϰ�..
            LoadingSliderUI.value = ao.progress;  // 0 ~ 1;
            LoadingTextUI.text = $"{ao.progress * 100f}%";

            /**
              ��������� �ؼ� �����͸� �޾ƿ��⵵ �Ѵ�.
              - ��ȹ ������
                - �뷱�� ������
                - ����  ������
                - ����  ������
            **/

            if (ao.progress >= 0.9f)
            {
                ao.allowSceneActivation = true;
            }

            // ���� �����ӱ��� ����.
            yield return null;
        }
    }
}