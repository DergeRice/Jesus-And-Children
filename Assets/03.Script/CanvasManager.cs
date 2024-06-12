using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Purchasing;
using UnityEngine.Events;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;
    public TMP_Text scoreText, highScoreText, notice;
    public RankingManager rankingManager;

    public List<GameObject> profileImgs = new List<GameObject>();

    public InfoPanel infoPanel;

    public Image profileImg;
    public TMP_Text nickNameText, churchNameText, lobbyGold, gameGold;

    public bool isGameScene;

    public TMP_Text dangerCount;

    public GameOverPanel gameOverPanel;

    public GameObject korTitle, engTitle;

    public SettingPanel settingPanel;

    public ContentSizeFitter highCsf, curCsf;

    public List<GameObject> iosRemoveObject = new List<GameObject>();

    public List<Button> purchaseBtns = new List<Button>();

    public List<CodelessIAPButton> purchaseIAP = new List<CodelessIAPButton>();

    public OnlineIndicator onlineIndicator;

    public bool isDefaultNickname = true;

    public GameObject agePanel , noticeObj;

    public RewardPanel rewardPanel;

    public string engNotice, korNotice;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {   
        instance = this;
        if(isGameScene == true) return;
        FirstStart();
        noticeObj.SetActive(false);
    }

    public void FirstStart()
    {
        if(PlayerPrefs.GetString("nickName") != null && PlayerPrefs.GetString("nickName") != "")
        {
            nickNameText.text = PlayerPrefs.GetString("nickName");
            isDefaultNickname = false;
        }
        if(PlayerPrefs.GetString("churchName") != null && PlayerPrefs.GetString("churchName") != "")
        {
            churchNameText.text = PlayerPrefs.GetString("churchName");
        }
        
        ResetInfoImg(profileImgs);
        profileImgs[PlayerPrefs.GetInt("profileIndex")].SetActive(true);

        foreach (var item in purchaseBtns)
        {
            item.onClick.AddListener(()=> NetworkManager.instance.loadingPanel.gameObject.SetActive(true));
        }

        
        var cardManager = FindObjectOfType<CardManager>();

        for(int i = 0; i <purchaseIAP.Count; i++)
        {
            int index = i;
            purchaseIAP[i].onPurchaseComplete.AddListener((x) => 
            {
                AdsInitializer.instance.RemoveAd(true);
                NetworkManager.instance.loadingPanel.gameObject.SetActive(false);
                if(index == 0)  cardManager.ShowPopup(1);
                if(index == 1)  cardManager.ShowPopup(5);
                if(index == 2)  cardManager.ShowPopup(10);
            }
            );

            purchaseIAP[i].onPurchaseFailed.AddListener((x,y)=>
            {
                NetworkManager.instance.loadingPanel.gameObject.SetActive(false);
                string failText = LangManager.instance.isEng ? "Purchase Failed" : "구매 되지 않았습니다.";
                NetworkManager.instance.ToastText(failText);
            });
        }

#if UNITY_IOS
        RemoveIosAds(); 
#endif
    }

    public void RemoveIosAds()
    {
        foreach (var item in iosRemoveObject)
        {
            item.SetActive(false);
        }
    }

    public void UpdateScoreText(int score)
    {
        scoreText.text = score.ToString();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)curCsf.transform);
    }

    public void SetHighScoreText(int score)
    {
        highScoreText.text = score.ToString();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)highCsf.transform);
    }
    public void ChangeEngTitle(bool isEng)
    {
        korTitle.SetActive(!isEng);
        engTitle.SetActive(isEng);
    }

    public void ShowInfoPanel()
    {
        ResetInfoImg(profileImgs);
        profileImgs[PlayerPrefs.GetInt("profileIndex")].SetActive(true);

        if(isDefaultNickname == false)
        {
            infoPanel.churchNameInput.text = churchNameText.text;
            infoPanel.nicknameInput.text = nickNameText.text;
        }else
        {
            infoPanel.churchNameInput.text = "";
            infoPanel.nicknameInput.text = "";
        }
        infoPanel.gameObject.SetActive(true);
    }

    public void CloseInfoPanel(RankingData rankingData)
    {
        if(nickNameText.text != "" && rankingData.name != null)
        {
            string temp = rankingData.name;
            
            nickNameText.text = temp.Replace(" ","");
        }else
        {
            infoPanel.nicknameInputAlert.SetActive(true);
            return;
        }
        
        if(churchNameText.text != ""&& rankingData.churchName != null)
        {
            string temp = rankingData.churchName;
            churchNameText.text = temp.Replace(" ","");
        }else
        {
            infoPanel.churchNameInputAlert.SetActive(true);
            return;
        }

        ResetInfoImg(profileImgs);
        
        isDefaultNickname = false;
        NetworkManager.instance.ownData.name = rankingData.name.Replace(" ","");
        NetworkManager.instance.ownData.churchName = rankingData.churchName.Replace(" ","");
        profileImgs[rankingData.profileIndex].SetActive(true);
        infoPanel.gameObject.SetActive(false);
    }

    public void ResetInfoImg(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i].SetActive(false);
        }
    }

    public void GameOverCanvas()
    {
        dangerCount.gameObject.SetActive(true);        
    }

    [ContextMenu("ResetPrefs")]
    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    public static void DisableChild(GameObject parent)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            parent.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void ChangeNotice(string eng,string kor)
    {
        engNotice = eng;
        korNotice = kor;

        notice.text = LangManager.instance.isEng ? engNotice : korNotice;
        noticeObj.SetActive(true);
    }
    public void ChangeNotice()
    {
        if(notice == null) return;
        notice.text = LangManager.instance.isEng ? engNotice : korNotice;
        noticeObj.SetActive(true);
    }

}
