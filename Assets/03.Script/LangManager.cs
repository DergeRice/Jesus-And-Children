using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LangManager : MonoBehaviour
{
    public static LangManager instance;
    public LangConvert[] langConverts = new LangConvert[0];

    public bool isEng;

    private void Awake()
    {
        if(instance != null) Destroy(instance.gameObject);
        if(instance == null ) instance = this;

       
    }
    void Start()
    {
        isEng = PlayerPrefs.GetInt("LangEng") == 1;

        for (int i = 0; i < langConverts.Length; i++)
        {
            int index = i;
            langConverts[index].text = langConverts[index].GetComponent<TMP_Text>();
            if(langConverts[index].isField != true) langConverts[index].isText = true;
        }

         if(isEng == true)  
         {
            ConvertAll();
            
         }
    }

    public void ChanageLang()
    {
        isEng = !isEng;
        ConvertAll();
        PlayerPrefs.SetInt("LangEng",isEng? 1:0);
        
        // CanvasManager.instance.settingPanel.SetToggleImg(settingButtons[3],orignImgs[3],disalbeImgs[3]);

        
    }

    public void ConvertAll()
    {
        var canvasManager = CanvasManager.instance;
        canvasManager.settingPanel.buttonText.text = isEng ? "한글" : "Eng";
        if(canvasManager.isGameScene != true)
        {
            CanvasManager.instance.ChangeEngTitle(isEng);

        }
        
        for (int i = 0; i < langConverts.Length; i++)
        {
            int index = i;
            langConverts[index].SetText(isEng);
        }
    }

    public static bool IsEng()
    {
        return LangManager.instance.isEng;
    }

    
}
