using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class InfoPanel : MonoBehaviour
{
    public int profileIndex;

    public TMP_InputField churchNameInput, nicknameInput;

    public Button closeBtn;

    public List<Button> galleryUIs = new List<Button>();
    public List<GameObject> profiles = new List<GameObject>();

    public BadWordManager badWordManager;

    public GameObject nicknameInputAlert, churchNameInputAlert;

    private void Start()
    {
        closeBtn.onClick.AddListener(()=>{ApplyMyDatas();});

       profileIndex = NetworkManager.instance.ownData.profileIndex;
        for (int i = 0; i < galleryUIs.Count; i++)
        {
            int index = i;
            galleryUIs[index].onClick.AddListener(()=>
            {
                this.profileIndex =index;
                PlayerPrefs.SetInt("profileIndex",index);
                NetworkManager.instance.ownData.profileIndex = index;
                ShowInfoImg(index);
            });
        }
        Debug.Log("Info");
        ShowInfoImg(PlayerPrefs.GetInt("profileIndex"));

    }


    public void ApplyMyDatas()
    {
        RankingData rankingData = new RankingData();

        if(churchNameInput.text != "")
        {
            string trimedName = churchNameInput.text.Replace(" ","");

            if(badWordManager.IsPossbieNickName(trimedName) == false) return;
            if(churchNameInput.text == null || (!churchNameInput.text.Contains("교회")&&!LangManager.instance.isEng))
            {
                churchNameInputAlert.SetActive(true);
                return;
            }
        
            LobbyManager.instance.rankingData.churchName = trimedName;
            rankingData.churchName = trimedName;  
            PlayerPrefs.SetString("churchName",trimedName);
        }

        if(nicknameInput.text != "")
        {
            string trimedName = nicknameInput.text.Replace(" ","");

            if(badWordManager.IsPossbieNickName(trimedName) == false) return;
            if(nicknameInput.text == null)
            {
                nicknameInputAlert.SetActive(true);
                return;
            }

            LobbyManager.instance.rankingData.name = trimedName;
            rankingData.name = trimedName;
            PlayerPrefs.SetString("nickName",trimedName);
        }

        LobbyManager.instance.rankingData.profileIndex = profileIndex;
        rankingData.profileIndex = profileIndex;
        PlayerPrefs.SetInt("profileIndex",profileIndex);

        ShowInfoImg(profileIndex);
        
        nicknameInputAlert.SetActive(false);
        churchNameInputAlert.SetActive(false);
        CanvasManager.instance.CloseInfoPanel(rankingData);
    }

    public void ShowInfoImg(int index)
    {
        ResetImgs();
        profiles[index].SetActive(true);
    }

    public void ResetImgs()
    {
        for (int i = 0; i < profiles.Count; i++)
        {
            profiles[i].SetActive(false);
        }
    }

}
