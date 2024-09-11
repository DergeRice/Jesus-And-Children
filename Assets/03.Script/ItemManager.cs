using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemManager : MonoBehaviour
{
    public int availableCount, curUsedCount;
    public TMP_Text availableCountText;
    public Button itemIcon;
    private void Start()
    {
        availableCount = 2;
        curUsedCount = 0;
        UpdateUI();
    }

    public bool CheckAvailAble()
    {
        if(NetworkManager.instance.GetCurGold() > 199 && availableCount > 0)
        {
            return true;
        }
        string noCoin = LangManager.instance.isEng ? "Not enough coin": "코인이 부족해요!"; 
        NetworkManager.instance.ToastText(noCoin);
        return false;
    }

    public void UseItem()
    {
        NetworkManager.instance.GoldChange(-200);
        availableCount --;
        curUsedCount ++;

        if(curUsedCount >= 3)
        {
            NetworkManager.instance.GoldChange(-99999);
            NetworkManager.instance.ToastText("코드벡터 관계자분들 감사합니다.");
            GameManager.instance.ReturnLobbyScene(false);
        }
        UpdateUI();
    }

    public void UpdateUI()
    {
        availableCountText.text = availableCount.ToString();
        if(availableCount <= 0)
        {
            itemIcon.interactable = false;
        }
    }
}
