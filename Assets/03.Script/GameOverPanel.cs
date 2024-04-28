using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    public GameObject gameOverParticle;
    public GameObject gameOverPanel;
    public GameObject gameOverBlackPanel;

    public Button enrollRank, returnHome, retry;

    bool isUploadAble = true;

    public TMP_Text curScore;
    public void StartShow()
    {
        gameOverPanel.SetActive(false);
        StartCoroutine(PanelShowUp());
    }

    IEnumerator PanelShowUp()
    {
        GameManager.instance.TurnOffInteract();
        NetworkManager.instance.ownData.score =  GameManager.instance.score;
        
        curScore.text = GameManager.instance.score.ToString();
        gameOverBlackPanel.SetActive(true);
        yield return new WaitForSeconds(gameOverParticle.GetComponent<ParticleSystem>().main.duration);
        
        GameManager.instance.TurnOffInteract();
        gameOverBlackPanel.GetComponent<Image>().DOFade(0.5f,1.5f);
        yield return new WaitForSeconds(1.5f);
        
        GameManager.instance.TurnOffInteract();
        gameOverPanel.SetActive(true);
        SoundManager.instance.EndGame();
        

        if(isUploadAble == true)
        {
            var possibleShowingText = LangManager.IsEng() ? "Ranking Uploaded!" :"랭킹등록 성공!";
            var impossibleShowingText = LangManager.IsEng() ? "Coludn't Upload" :"랭킹을 등록할 수 없어요.";
            

            isUploadAble = false;
            if(NetworkManager.instance.onlineMode == true)
            {
                NetworkManager.instance.EnrollOwnData();
                NetworkManager.instance.ToastText(possibleShowingText);
            }else
            {
                NetworkManager.instance.ToastText(impossibleShowingText);
            }
        }

        
    }
}
