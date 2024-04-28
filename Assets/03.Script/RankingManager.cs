using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class RankingManager : MonoBehaviour
{
    public GameObject uiPrefeb;
    public Sprite[] rankingUiSprites = new Sprite[3];
    public  List<RankingUI> rankingUIs = new List<RankingUI>();

    public Transform uiContainer;

    public List<GameObject> profileImgs = new List<GameObject>();
    List<RankingData> globalrankingDatas;
    List<RankingData> churchRankingDatas;


    public Button churchTab, globalTab;

    [Header("Mydata")]
    public TMP_Text myScore, myRankIndex;

    Color disabledColor, enableColor;

    private void Awake()
    {
        disabledColor = churchTab.GetComponent<Image>().color;
        enableColor = globalTab.GetComponent<Image>().color;
    }

    private void Start()
    {
        churchTab.onClick.AddListener(ShowChurchRanking);
        globalTab.onClick.AddListener(ShowGlobalRanking);
    }

    public void ShowChurchRanking()
    {
        churchTab.GetComponent<Image>().color = enableColor;
        globalTab.GetComponent<Image>().color = disabledColor;

        SetMyData(churchRankingDatas);
        ShowUiList(churchRankingDatas);
    }
    public void ShowGlobalRanking()
    {
        churchTab.GetComponent<Image>().color = disabledColor;
        globalTab.GetComponent<Image>().color = enableColor;


        SetMyData(globalrankingDatas);
        ShowUiList(globalrankingDatas);
    }

    public void ShowUiList(List<RankingData> datas)
    {
        CleanBoard();
        

        for (int i = 0; i < datas.Count; i++)
        {
            int index = i;
            var temp = Instantiate(uiPrefeb,uiContainer).GetComponent<RankingUI>();
            temp.SettingUi(datas[index]);
            temp.rankNumText.text = (index+1).ToString();

            if(index < 3)
            {
                temp.medalImg.sprite = rankingUiSprites[index];
            }else
            {
                temp.medalImg.gameObject.SetActive(false);
            }
        }
    }

    public void SetMyData(List<RankingData> datas)
    {
        var matchingData = datas.FirstOrDefault(data => data.name == NetworkManager.instance.ownData.name && data.churchName == NetworkManager.instance.ownData.churchName);
        profileImgs[NetworkManager.instance.ownData.profileIndex].SetActive(true);

        if(matchingData != null)
        {
            myScore.text = matchingData.score.ToString();
            int rankingIndex = datas.IndexOf(matchingData) +1;
            string additionText;
            
            if(rankingIndex == 1) additionText = "st";
            else if(rankingIndex == 1) additionText = "nd";
            else additionText = "th";

            myRankIndex.text = rankingIndex.ToString()+additionText;

            CanvasManager.DisableChild(profileImgs[0].transform.parent.gameObject);
            profileImgs[NetworkManager.instance.ownData.profileIndex].SetActive(true);
        }
    }


    public void CleanBoard()
    {
        for (int i = 0; i < uiContainer.childCount; i++)
        {
            Destroy(uiContainer.GetChild(i).gameObject);
        }
    }
    private void OnEnable()
    {
        NetworkManager.instance.GetData(()=>BringDatas(),()=>{gameObject.SetActive(false);});
    }

    private void BringDatas()
    {
        globalrankingDatas = NetworkManager.instance.rankingDatas;
        churchRankingDatas = globalrankingDatas.Where(data => data.churchName == NetworkManager.instance.ownData.churchName).ToList();
        SetMyData(globalrankingDatas);
        ShowUiList(globalrankingDatas);
    }
}
