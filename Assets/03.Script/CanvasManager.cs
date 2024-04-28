using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;
    public TMP_Text scoreText, highScoreText;
    public RankingManager rankingManager;

    public List<GameObject> profileImgs = new List<GameObject>();

    public InfoPanel infoPanel;

    public Image profileImg;
    public TMP_Text nickNameText, churchNameText;

    public bool isGameScene;

    public TMP_Text dangerCount;

    public GameOverPanel gameOverPanel;

    public GameObject korTitle, engTitle;

    public SettingPanel settingPanel;

    public ContentSizeFitter highCsf, curCsf;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {   
        instance = this;
        if(isGameScene == true) return;
        FirstStart();
    }

    public void FirstStart()
    {
        if(PlayerPrefs.GetString("nickName") != null && PlayerPrefs.GetString("nickName") != "")
        {
            nickNameText.text = PlayerPrefs.GetString("nickName");
        }
        if(PlayerPrefs.GetString("churchName") != null && PlayerPrefs.GetString("churchName") != "")
        {
            churchNameText.text = PlayerPrefs.GetString("churchName");
        }
        
        ResetInfoImg(profileImgs);
        profileImgs[PlayerPrefs.GetInt("profileIndex")].SetActive(true);
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

        infoPanel.churchNameInput.text = churchNameText.text;
        infoPanel.nicknameInput.text = nickNameText.text;
        infoPanel.gameObject.SetActive(true);
    }

    public void CloseInfoPanel(RankingData rankingData)
    {
        if(nickNameText.text != "")
        {
            nickNameText.text = rankingData.name.Replace(" ","");
        }
        
        if(churchNameText.text != "")
        {
            churchNameText.text = rankingData.churchName.Replace(" ","");
        }

        ResetInfoImg(profileImgs);
        
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

}
