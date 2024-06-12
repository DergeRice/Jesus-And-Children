using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Phrase
{
   public string korPhrase;
   public string engPhrase;

   public string korDictionary;

   public string engDictionary;
}

public class CardManager : MonoBehaviour
{

    public GameObject cardObj,root;

    public int popupCount;

    public Text phraseText;
    public TMP_Text  dictionaryText;

    public List<Phrase> phraseList = new List<Phrase>();
    public void ShowPopup(int popupCount)
    {
        root.SetActive(true);
        cardObj.SetActive(false);
        Invoke(nameof(InvokePopup),0.2f);
        this.popupCount = popupCount;
    }
    
    public void InvokePopup()
    {
        cardObj.SetActive(true);
        int randomIndex =Random.Range(0,phraseList.Count);

        if(randomIndex == 15 && LangManager.instance.isEng)randomIndex =16;
        
        var showing = phraseList[randomIndex];
        Debug.Log(randomIndex);
        
        phraseText.text = LangManager.instance.isEng ? showing.engPhrase : showing.korPhrase;
        // phraseText.text.Replace(' ', '\u00A0');

        dictionaryText.text = LangManager.instance.isEng ? showing.engDictionary : showing.korDictionary;
        // dictionaryText.text.Replace(' ', '\u00A0');
    }

    public void CheckPopupEnd()
    {
        popupCount --;
        if(popupCount > 0)
        {
            
            ShowPopup(popupCount);
        }else
        {
            root.SetActive(false);
        }
    }
}
