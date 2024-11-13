using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AYellowpaper.SerializedCollections;

[System.Serializable]
public class CharSkinData
{
    public Sprite skinImg;
    public string skinName;
    public int unlockAmount;
    public bool isSelected;
    public bool isUnlocked;
    public int cost;
}
[System.Serializable]
public class CharSkinDataList // 한줄을 주는거임 11줄 -- 예수 총 11줄 있음
{
    public List<CharSkinData> charSkinDatas = new List<CharSkinData>();
}

public class SkinPanel : MonoBehaviour
{
    public GameObject root;
    public GameObject selectPanel;
    public List<Button> charButtons;
    public List<CharSkinDataList> charSkinDatas;

    [Header("SkinInnerContents")]
    
    public Image mainImg;
    public GameObject skinPrefab;

    public List<GameObject> skinSelectButtons;

    public int currentCharIndex = 0;

    public GameObject prefebParent;


    public TMP_Text skinNameText;
    public TMP_Text conditionTextKor;
    public TMP_Text conditionTextEng;

    public SerializedDictionary<string,CharSkinData> skinDataDictionary =  new SerializedDictionary<string, CharSkinData>();
    public SkinManager skinManager;
    private void Start()
    {
        for (int i = 0; i < charButtons.Count; i++)
        {
            int index = i;
            charButtons[i].onClick.AddListener(()=>
            {
                selectPanel.SetActive(true);
                currentCharIndex = index; 
                SetSkinSelectPanel(index);
            });
        }

        for (int i = 0; i < charSkinDatas.Count; i++)
        {
            skinManager.GetUnlockData(charSkinDatas[i]);
        }
    }

    public void SetSkinSelectPanel(int charIndex)
    {
        // mainImg.sprite = charSkinDatas[charIndex].[].skinImg;
        // main img is set before;
        skinSelectButtons = new List<GameObject>();
        FindCurrentDefaultImage();
        for (int i = 0; i < prefebParent.transform.childCount; i++)
        {
            int index = i;
            Destroy(prefebParent.transform.GetChild(index).gameObject);
        }

        for (int i = 0; i < charSkinDatas[charIndex].charSkinDatas.Count; i++)
        {
            int index = i;
            var charBtn = Instantiate(skinPrefab,prefebParent.transform);
            skinSelectButtons.Add(charBtn);
            CharSkin charSkin = charBtn.GetComponent<CharSkin>();
            charSkin.mainImg.sprite = charSkinDatas[charIndex].charSkinDatas[index].skinImg;
            // charBtn.GetComponent<CharSkin>().mainImg.SetNativeSize();

            SetGalleryRatio(charSkin.mainImg.sprite);
            // charSkin.mainImg. = 350f;
        } 

        for (int i = 0; i < skinSelectButtons.Count; i++)
        {
            int index = i;
            skinSelectButtons[index].GetComponent<CharSkin>().button.onClick.AddListener(()=>
            {
                SetMyCharNewSkin(index);
            });
        }
    }
    

    public void SetMyCharNewSkin(int charSkinSelectedNum)
    {
        charSkinDatas[currentCharIndex].charSkinDatas[charSkinSelectedNum].isSelected = true;
        mainImg.sprite = charSkinDatas[currentCharIndex].charSkinDatas[charSkinSelectedNum].skinImg;
        mainImg.SetNativeSize();
    }

    public void FindCurrentDefaultImage()
    {
        for (int i = 0; i < charSkinDatas[currentCharIndex].charSkinDatas.Count; i++)
        {
            if(charSkinDatas[currentCharIndex].charSkinDatas[i].isSelected == true)
            {
                mainImg.sprite = charSkinDatas[currentCharIndex].charSkinDatas[i].skinImg;
                mainImg.SetNativeSize();
                return;
            }
        }
    }

    public void SetImageRatio(GameObject targetUi, Sprite newSprite)
    {
        // RectTransform을 통해 targetUi의 현재 너비를 가져옴
        RectTransform rectTransform = targetUi.GetComponent<RectTransform>();
        float targetWidth = rectTransform.rect.width;  // targetUi의 현재 너비

        // newSprite의 가로 세로 비율 계산
        float spriteAspectRatio = (float)newSprite.rect.width / newSprite.rect.height;

        // targetUi의 너비에 맞춘 높이 계산
        float targetHeight = targetWidth / spriteAspectRatio;

        // RectTransform의 사이즈를 비율에 맞춰 설정
        rectTransform.sizeDelta = new Vector2(targetWidth, targetHeight);
    }

    public void SetGalleryRatio(Sprite newSprite)
    {
        // 고정된 너비 (x값)
        float targetWidth = prefebParent.GetComponent<GridLayoutGroup>().cellSize.x;

        // newSprite의 가로 세로 비율 계산
        float spriteAspectRatio = (float)newSprite.rect.width / newSprite.rect.height;

        // 너비에 맞춘 높이 계산
        float targetHeight = targetWidth / spriteAspectRatio;

        // galleryLayoutGroup의 셀 크기를 비율에 맞춰 설정
        prefebParent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(targetWidth, targetHeight);
    }


}
