using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{
    ClassicMode, 
    SurvivalMode,
    TimeAttackmode
}

public class SelectGameModePanel : MonoBehaviour
{
    public GameMode gameMode;
    public List<GameObject> gamemodeObjs = new List<GameObject>();

    public Button TimeAttackBtn;
    public Button ClassicBtn;


    public void SetPanelObjects(int gameModeIndex)
    {
        for (int i = 0; i < gamemodeObjs.Count; i++)
        {
            gamemodeObjs[i].SetActive(false);
        }
        gamemodeObjs[gameModeIndex].SetActive(true);
    }

    private void Start()
    {
        TimeAttackBtn.onClick.AddListener(()=>NetworkManager.instance.SetTimeAttackMode(true));
        ClassicBtn.onClick.AddListener(()=>NetworkManager.instance.SetTimeAttackMode(false));
    }

}
