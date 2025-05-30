using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    public List<SettingButton> settingButtons = new List<SettingButton>();

    List<Sprite> orignImgs = new List<Sprite>();

    public List<Sprite> disalbeImgs = new List<Sprite>();

    public TMP_Text buttonText;

    public int easterEggPanelCount;

    public GameObject easterEggPanel;


    private void Start()
    {
        SoundManager.instance.settingPanel = this;
        AddEvent();
        for (int i = 0; i < settingButtons.Count; i++)
        {
            int index = i;
            settingButtons[index].Init();
            orignImgs.Add(settingButtons[index].icon.sprite);
            settingButtons[index].button.onClick.AddListener(
                () => {
                    
                    SetToggleImg(settingButtons[index],orignImgs[index],disalbeImgs[index]);
                    }
                );

        }
        settingButtons[3].disabled = (PlayerPrefs.GetInt("LangEng") == 1);
    
        SetToggleJustImg(0);
        SetToggleJustImg(1);
        SetToggleJustImg(2);
        SetToggleJustImg(3);


        settingButtons[0].action += () => 
        {
            easterEggPanelCount++;
            
            if(easterEggPanelCount > 20)
            {
                easterEggPanel.SetActive(true);
                easterEggPanelCount = 0;
            }
        };
    } 



    public void SetToggleImg(SettingButton settingButton, Sprite enabledImg, Sprite disabledImg)
    {
        settingButton.action?.Invoke();
        settingButton.icon.sprite = settingButton.disabled ? enabledImg : disabledImg; 

        settingButton.disabled = !settingButton.disabled;
    }
    public void SetToggleJustImg(int index)
    {
        settingButtons[index].icon.sprite = settingButtons[index].disabled ? disalbeImgs[index] : orignImgs[index]; 
    }

    public void AddEvent()
    {
        settingButtons[0].action += () => SoundManager.instance.ToggleBgm();
        settingButtons[1].action += () => SoundManager.instance.ToggleSfx();
        settingButtons[2].action += () => SoundManager.instance.ToggleVibrate();
        settingButtons[3].action += () => LangManager.instance.ChanageLang();


        settingButtons[0].disabled  = SoundManager.instance.isBgmOff; 
        settingButtons[1].disabled  = SoundManager.instance.isSfxOff; 
        settingButtons[2].disabled  = SoundManager.instance.isVibrateOff; 
        settingButtons[3].disabled  = LangManager.instance.isEng;

    }

    
}
