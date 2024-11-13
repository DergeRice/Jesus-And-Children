using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MergeCount
{
    int charIndex;
    int mergeCount;
}

public class SkinManager : MonoBehaviour
{
    [SerializeField] SkinPanel skinPanel;
    public void GetUnlockData(CharSkinDataList charSkinDataList)
    {   
        for (int i = 0; i < charSkinDataList.charSkinDatas.Count; i++)
        {
            int index = i;
            charSkinDataList.charSkinDatas[index].isUnlocked =  PlayerPrefs.GetInt(charSkinDataList.charSkinDatas[index].skinName+"Unlock") == 1 ? true : false;
            charSkinDataList.charSkinDatas[index].isSelected =  PlayerPrefs.GetInt(charSkinDataList.charSkinDatas[index].skinName+"Selected") == 1 ? true : false;

            if(charSkinDataList.charSkinDatas[index].isSelected == true)
            {
               ChangeSkinToUse(charSkinDataList.charSkinDatas[index]); 
            }
        }
         
    }

    public void SaveSkinUnlockData(CharSkinData charSkinData)
    {
        PlayerPrefs.SetInt(charSkinData.skinName+"Unlock",1);
    }

    public void ChangeSkinToUse(CharSkinData charSkinData)
    {
        PlayerPrefs.SetInt(charSkinData.skinName+"Unlock",1);
    }

    public void CheckUnlockSkin(MergeCount mergeCount)
    {
        for (int i = 0; i < skinPanel.charSkinDatas.Count; i++)
        {
            for (int j = 0; j < skinPanel.charSkinDatas[i].charSkinDatas.Count; j++)
            {
                // if(skinPanel.charSkinDatas[i].charSkinDatas[j].)
            }
            
        }
    }
}
