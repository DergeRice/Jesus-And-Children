using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.Networking;
using System;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance;
    public int highScore, gold;

    public RankingData rankingData;

    public GameObject allRoot, tutorial,noNamePanel,agePanel;
    
    private void Awake()
    {
        
        if(instance == null) instance = this;

        if(PlayerPrefs.HasKey("highScore") == false)
        {
            PlayerPrefs.SetInt("highScore",0);
        }



        highScore = PlayerPrefs.GetInt("highScore");
        CanvasManager.instance.highScoreText.text = highScore.ToString();

        if(PlayerPrefs.HasKey("gold") == false)
        {
            PlayerPrefs.SetInt("gold",500);
        }
        
        gold = PlayerPrefs.GetInt("gold");
        CanvasManager.instance.lobbyGold.text = gold.ToString();


        InitRecommendCode();

        if(PlayerPrefs.HasKey("isFirstTime") == true)
        {
        }
        else
        {
            tutorial.SetActive(true);
            #if UNITY_ANDROID
            agePanel.SetActive(true);
            #endif
        }
        AdsInitializer.instance.interstitialAd.LoadAd();
    }

    public void EndTutorial()
    {
        PlayerPrefs.SetInt("isFirstTime",1);
    }
    public void Start()
    {
        SoundManager.instance.ChangeBgm(0);
        SoundManager.instance.bgmAudioSource.Play();
        AdsInitializer.instance.bannerAd.HideBannerAd();
        NetworkManager.instance.OnlineTest();
    }

    public void InitRecommendCode()
    {
        if(PlayerPrefs.HasKey("MyRecommend") == false)
        {
            PlayerPrefs.SetString("MyRecommend",RandomHashCodeGenerator.GenerateRandomHashCode());
            CanvasManager.instance.addFriendPanel.SetMyRecommenCode(PlayerPrefs.GetString("MyRecommend"));
        }else
        {
            CanvasManager.instance.addFriendPanel.SetMyRecommenCode(PlayerPrefs.GetString("MyRecommend"));
        }

        if(PlayerPrefs.HasKey("Recommeded") == true)
        {
            CanvasManager.instance.addFriendPanel.SetAlreadyRecommeded();
        }
    }

    public void StartGameScene()
    {
        string nickName = PlayerPrefs.GetString("nickName");
        string churchName = PlayerPrefs.GetString("churchName");
        if(LangManager.instance.isEng == false && !churchName.Contains("교회"))
        {
            noNamePanel.SetActive(true);
            return;
        }
        if(string.IsNullOrEmpty(nickName)||string.IsNullOrEmpty(churchName))
        {
            noNamePanel.SetActive(true);
            return;
        }
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
        
        string engEmailText = LangManager.instance.isEng ?  "Send Email to leesangjin2372@gmail.com." : "leesangjin2372@gmail.com으로 메일을 보냅니다.";
        NetworkManager.instance.ToastText(engEmailText);
    } 
    string EscapeURL(string url) 
    { 
        return UnityWebRequest.EscapeURL(url).Replace("+", "%20"); 
    }

    public void AndroidQuit()
    {
        AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        activity.Call<bool>("moveTaskToBack", true);
        Application.Quit();
        Debug.Log("Quit App");
    }

    
    
}

public class RandomHashCodeGenerator 
{
    private static readonly char[] chars = "abcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
    
    private static System.Random random = new System.Random();

    // 6자리의 랜덤 해시코드를 생성하는 메서드
    public static string GenerateRandomHashCode(int length = 6)
    {
        char[] result = new char[length];
        for (int i = 0; i < length; i++)
        {
            result[i] = chars[random.Next(chars.Length)];
        }
        return new string(result);
    }
}
