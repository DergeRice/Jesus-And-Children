using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public enum EVibrate
{
    weak,
    strong,
    nope
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public CanvasManager canvasManager;


    public bool testMode = false;


    public int score = 0;
    public int highScore = 0;
    public GameObject ball,holdingBall;

    public List<GameObject> particles = new List<GameObject>();

    GameObject currentBall, showingBall;

    public List<GameObject> ballPrefabs = new List<GameObject>();
    public GameObject ballParent;

    public Image nextBallIndi;

    public bool isPossibleCreate = false;

    bool mergeAble = true;

    int ballInstiateCount;

    int nextBallIndex;
    Vector3 ballPos;

    public List<int> pointList = new List<int>();
    private Vector3 holdingBallPos;
    public GameObject newRecodePanel, curScoreCrown,highScoreCrown;

    public GameObject mainCamera;

    public bool isTriggered = false;

    public CanvasGroup canvasGroup;

    public List<GameObject> jesusComeParticle = new List<GameObject>();
    public List<GameObject> spawnedBalls = new List<GameObject>();
    
    public ItemManager itemManager;

    private void Awake()
    {
        
        if(instance == null){ instance = this;}
    }

    void Start()
    {
        Init();
    }

    void Init()
    {
        score = 0;

        ballInstiateCount = 0;
        // currentBall = ballHolding;

        highScore = PlayerPrefs.GetInt("highScore");
        canvasManager.SetHighScoreText(highScore);
        
        currentBall = ballPrefabs[0];
        holdingBall.GetComponent<SpriteRenderer>().sprite = currentBall.GetComponent<Ball>().ballImg;
        holdingBallPos = holdingBall.transform.position;
        holdingBall.transform.localScale = currentBall.transform.localScale;
        isPossibleCreate = true;
        ShowNextImg();
        
        SoundManager.instance.ChangeBgm(1);

        
        

        float desiredHeight = Camera.main.orthographicSize;


        Camera.main.orthographicSize = (9*Screen.height*desiredHeight)/(16*Screen.width);
        
        mainCamera.transform.position = new Vector3(0,20.27f,-10);
        mainCamera.transform.DOMoveY(-0.1f,0.6f);
        Invoke(nameof(FadeInvoke),0.6f);
    }

    public void FadeInvoke()
    {
        canvasGroup.DOFade(1,0.5f);
        if(AdsInitializer.instance.isRemovedAds == false)
        {
            AdsInitializer.instance.bannerAd.ShowBannerAd();
        }
    }

    public void SetIsPossible()
    {
        StartCoroutine(SetInteractiveCoroutine());
    }

    private IEnumerator SetInteractiveCoroutine()
    {
        yield return new WaitForSeconds(0.3f);
        isPossibleCreate = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
             // 카메라와의 거리 설정 (원하는 값으로 변경)

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        ballPos = new Vector3(worldPosition.x,3.5f,0);

        if(isPossibleCreate == true)
        {
            if(Input.GetMouseButton(0))
            {
                holdingBall.transform.position = holdingBallPos;
                holdingBall.transform.position = ballPos;
            }
            if(Input.GetMouseButtonUp(0))
            {
                GameObject newBall = null;

                newBall = Instantiate(currentBall, holdingBall.transform.position, Quaternion.identity);
                newBall.transform.parent = ballParent.transform;
                spawnedBalls.Add(newBall);

                SoundManager.instance.PlayDropSound();
                var currentBallScript = currentBall.GetComponent<Ball>();

                isPossibleCreate = false;

                float reloadTime = testMode ? 0.01f : 1f;

                Invoke(nameof(ReloadBall),reloadTime);
                holdingBall.GetComponent<SpriteRenderer>().enabled = false;
                
                holdingBall.transform.localScale = Vector3.zero;
                holdingBall.GetComponent<LineRenderer>().enabled = false;
                ballInstiateCount++;
            }
        }else
        {
            holdingBall.transform.position = holdingBallPos;
        }
    }


    public void TurnOffInteract()
    {
        isPossibleCreate = false;
    }
    public void TurnOnInteract()
    {
        Invoke(nameof(TurnOnInteractInvoke),0.02f);
    }
    public void TurnOnInteractInvoke()
    {
        isPossibleCreate = true;
    }


    public void ReturnLobbyScene(bool showAd)
    {
        RecodeScore();

        if(showAd == true )
        {
            AdsInitializer.instance.interstitialAd.ShowAd();
        }
        SceneManager.LoadScene("LobbyScene");
    }

    
    public void ReloadBall()
    {
        ChangeCurrentBall(nextBallIndex);
        ChangeNextBall();
        holdingBall.GetComponent<SpriteRenderer>().sprite = currentBall.GetComponent<Ball>().ballImg;
        holdingBall.GetComponent<SpriteRenderer>().enabled = true;
        holdingBall.GetComponent<LineRenderer>().enabled = true;
        holdingBall.transform.position = holdingBallPos;
        holdingBall.transform.DOScale(currentBall.transform.localScale,0.08f);
        
        if(isTriggered == false) isPossibleCreate = true;
        
    }

    public void ChangeNextBall()
    {
        int maxBall = 1;
        int minBall = 0;

        if(ballInstiateCount > 70) maxBall = 6;
        else if(ballInstiateCount > 53) maxBall = 5;
        else if(ballInstiateCount > 35) maxBall = 4;
        else if(ballInstiateCount > 17) maxBall = 3;
        else if(ballInstiateCount > 4) maxBall = 2;

        if(UnityEngine.Random.value > 0.5f) minBall = 0;

        nextBallIndex = UnityEngine.Random.Range(minBall,maxBall);
        
        ShowNextImg();
    }

    public void ChangeCurrentBall(int index)
    {
        currentBall = ballPrefabs[index];
    }

    public void ShowNextImg()
    {
        nextBallIndi.sprite = ballPrefabs[nextBallIndex].GetComponent<Ball>().ballImg;
    }

    public void BallMerge(Ball one, Ball two, Vector3 pos,Ball target)
    {
        if(mergeAble == false) return;

        one.mergeAble = false;
        two.mergeAble = false;
        
        if(!testMode) 
        {
            if(one.ballLevel != 10) SoundManager.VibrateGame(EVibrate.weak);
            else SoundManager.VibrateGame(EVibrate.strong);
        }

        if(ballPrefabs.Count < one.ballLevel) return;

        Quaternion qu = two.transform.rotation;

        score += pointList[one.ballLevel-1];
        UpdateUI();

        spawnedBalls.Remove(one.gameObject);
        spawnedBalls.Remove(two.gameObject);
        Destroy(one.gameObject);
        Destroy(two.gameObject);
         
       
        StartCoroutine(SetMergeAble(one,two,pos));
    }

    IEnumerator SetMergeAble(Ball one, Ball two,Vector3 pos)
    {
        yield return new WaitForSeconds(0.01f);

        if(one == null && two == null )
        {
            if(!testMode) SoundManager.instance.PlayPopSound();

            int particleNum = 0;
            Instantiate(particles[particleNum],pos,Quaternion.identity);

            if(one.ballLevel == 11)
            {
                yield return null;
            }
            var temp = Instantiate(ballPrefabs[one.ballLevel],pos,quaternion.identity);


            temp.transform.parent = ballParent.transform;
            spawnedBalls.Add(temp);

            var tempBall = temp.GetComponent<Ball>();

            temp.transform.localScale = new Vector3(0f,0f,1);

            float scaleDuration =   ( tempBall.initScale.x) * 0.7f;
            temp.transform.DOScale(tempBall.initScale,scaleDuration);

            yield return new WaitForSeconds(0.02f);
            one.mergeAble = true;
            two.mergeAble = true;

        }
    }

    public void JesusCome()
    {
        StartCoroutine(ActivateParticles());
    }

    IEnumerator ActivateParticles()
    {
        foreach (GameObject particle in jesusComeParticle)
        {
            particle.SetActive(true);
            yield return new WaitForSeconds(0.8f);
        }
    }

    void UpdateUI()
    {
        canvasManager.UpdateScoreText(score);
        
        if(score > highScore) // new high Score;
        {
            highScore = score;
            canvasManager.SetHighScoreText(highScore);
            NewRecodeEvent();
        }
        
    }
    void NewRecodeEvent()
    {
        newRecodePanel.SetActive(true);
        curScoreCrown.SetActive(true);
        newRecodePanel.GetComponent<CanvasGroup>().DOFade(0,2.5f).SetEase(Ease.InSine);
    }

    public void SetMergeAble()
    {
        mergeAble = true;
    }

    public void GameOver()
    {
        
        isTriggered = true;
        StartCoroutine(DestroyBallsWithDelay());
        
        ShowGameOverPanel();
        RecodeScore();
        isPossibleCreate = false;
    }

    IEnumerator DestroyBallsWithDelay()
    {
        List<GameObject> allBalls = new List<GameObject>();

        for (int i = 0; i < ballParent.transform.childCount; i++)
        {
            allBalls.Add(ballParent.transform.GetChild(i).gameObject);
        }

        foreach (var obj in allBalls)
        {
            if(obj.gameObject != null)
            {
                Vector3 pos = obj.transform.position;
                Instantiate(particles[0], pos, Quaternion.identity);
                SoundManager.instance.PlayPopSound();
                SoundManager.VibrateGame(EVibrate.weak);
                Destroy(obj.gameObject);
            }

            // 0.1초 대기
            yield return new WaitForSeconds(0.03f);
        }

        if(ballParent.transform.childCount !=0) StartCoroutine(DestroyBallsWithDelay());
    }
    public void ShowGameOverPanel()
    {
        RecodeScore();
        // SoundManager.instance.EndGame();
        
        NetworkManager.instance.ownData.score = score;
        canvasManager.gameOverPanel.gameObject.SetActive(true);
        canvasManager.gameOverPanel.StartShow();
    }
    public void RecodeScore()
    {
        PlayerPrefs.SetInt("highScore",highScore);
    }

    public void ReloadGame()
    {
        RecodeScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
        // if(AdsInitializer.instance.isRemovedAds == false)
        // {
        //     AdsInitializer.instance.interstitialAd.ShowAd();
        // }
    }

    public void SpotLightAction()
    {
        if(itemManager.CheckAvailAble())
        {
            List<GameObject> SpotedBalls = new List<GameObject>();

            for (int i = spawnedBalls.Count - 1; i >= 0; i--)
            {
                if (spawnedBalls[i].GetComponent<Ball>().isSelected == true)
                {
                    SpotedBalls.Add(spawnedBalls[i]);
                    spawnedBalls.RemoveAt(i);
                }
            }

            if(SpotedBalls.Count == 0)
            {
                string noBalls = LangManager.instance.isEng ? "Noting Selected": "공이 선택되지 않았어요."; 
                NetworkManager.instance.ToastText(noBalls);
                return;
            }

            foreach (var item in SpotedBalls)
            {
                // float scaleDuration = item.GetComponent<Ball>().initScale.x * 2f;
                item.GetComponent<Collider2D>().enabled = false;
                item.GetComponent<Rigidbody2D>().gravityScale = 0;
                item.transform.DOLocalRotate(item.transform.rotation.eulerAngles+(Vector3.back*800),1.5f,RotateMode.FastBeyond360);
                item.transform.DOScale(Vector3.zero, 1.5f);
                SoundManager.instance.PlayMagicSound();
                
                Destroy(item, 1.5f);
            }
            
            SpotedBalls.Clear();
            itemManager.UseItem();
        }
    }



    public void ShowPolicy()
    {
        Application.OpenURL("https://fascinated-frog-c0b.notion.site/bcdf5b7223214d4a972b6ac89ad27212?pvs=4");
    }


}
