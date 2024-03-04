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
        // 사운드 효과음이라던지
        // 초기화 함수
        gameObject.SetActive(true);
    }

    public void Close()
    {
        // 사운드 효과음이라던지...
        // 여러 가지
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void OnClickContinueButton()
    {
        Debug.Log("계속하기");

        GameManager.Instance.Continue();

        Close();
    }

    public void OnClickResumeButton()
    {
        // 씬매니저야.(현재 열려 있는 씬)번 씬을 로드해라.
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        Debug.Log("다시하기");
    }

    public void OnClickExitButton()
    {
        Debug.Log("게임종료");

        // 빌드 후 실행했을 경우 종료하는 방법
        Application.Quit();

#if UNITY_EDITOR
        // 유니티 에디터에서 실행했을 경우 종료하는 방법
        UnityEditor.EditorApplication.isPlaying = false;
#endif        
    }
}
