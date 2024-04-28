using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance;
    public int highScore;

    public RankingData rankingData;

    public GameObject allRoot, tutorial;
    
    private void Awake()
    {
        
        if(instance == null) instance = this;

        if(PlayerPrefs.HasKey("highScore") == false)
        {
            PlayerPrefs.SetInt("highScore",0);
        }

        highScore = PlayerPrefs.GetInt("highScore");
        CanvasManager.instance.highScoreText.text = highScore.ToString();


        if(PlayerPrefs.HasKey("isFirstTime") == true)
        {
        }
        else
        {
            PlayerPrefs.SetInt("isFirstTime",1);
            tutorial.SetActive(true);
        }
    }
    public void Start()
    {
        SoundManager.instance.ChangeBgm(0);
    }
    public void StartGameScene()
    {
        
        StartCoroutine(LoadGameSceneAsync());

    }

    public void StartGameInvoke()
    {
        SceneManager.LoadScene("GameScene");

    }

    private IEnumerator LoadGameSceneAsync()
    {
        allRoot.transform.DOMoveY(12000,5f);
        // 비동기 씬 로딩 시작
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameScene");
        
        // 로딩 진행 상태 확인
        while (!asyncLoad.isDone)
        {
            // 진행 상태를 보여주는 기능 추가 가능
            yield return null;
        }

        // 씬 로딩 완료 후 로딩 화면 숨기기
        // loadingScreen.SetActive(false);
    }

    public void ShowPolicy()
    {
        Application.OpenURL("https://fascinated-frog-c0b.notion.site/bcdf5b7223214d4a972b6ac89ad27212?pvs=4");
    }

    public void OpenEmail()
    {

    string mailto = "leesangjin2372@gmail.com"; 
    string subject = EscapeURL("[예수님과 아이들]게임 문의"); 
    string body = EscapeURL("내용을 입력해주세요."); 
  
    Application.OpenURL("mailto:" + mailto + "?subject=" + subject + "&body=" + body); 
    } 
    string EscapeURL(string url) 
    { 
        return WWW.EscapeURL(url).Replace("+", "%20"); 
    }

    
    
}
