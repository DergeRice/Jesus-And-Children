using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;
using DG.Tweening;
using TMPro;
using Newtonsoft.Json.Linq;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;
    public string severSelectURL;
    public string severInsertURL;

    public string awsIP ;
    public string selectServerURL; 
    public string insertServerURL; 


    public List<RankingData> rankingDatas ;

    public RankingData ownData;

    public LoadingPanel loadingPanel;

    public Transform toastRoot;
    // public CanvasGroup rankSuccessPanel;

    public bool onlineMode = false;

    private TMP_Text toastText;

    public GameObject toastUIPrefeb;

    public string noticeText;


/// <summary>
/// Awake is called when the script instance is being loaded.
/// </summary>
    private void Awake()
    {
        gameObject.transform.SetParent(null);
        if(instance != null) Destroy(gameObject);
        if(instance == null){ instance = this;}
       

        DontDestroyOnLoad(this);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        selectServerURL = awsIP + severSelectURL;
        insertServerURL = awsIP + severInsertURL;
        UpdateOwnData();
        OnlineTest();
    }

    public void UpdateOwnData()
    {
        ownData.name = PlayerPrefs.GetString("nickName");
        ownData.churchName = PlayerPrefs.GetString("churchName");
        ownData.profileIndex = PlayerPrefs.GetInt("profileIndex");
    }



    [ContextMenu("TestInsert")]
    public void EnrollOwnData()
    {
        NetworkManager.instance.InsertData(ownData);
    }

    [ContextMenu("TestSelect")]
    public void SelectData()
    {
        NetworkManager.instance.GetData();
    }

    public void ToastText(string text)
    {
        var toastPanel = Instantiate(toastUIPrefeb,toastRoot).GetComponent<CanvasGroup>();
        
        toastText = toastPanel.transform.GetChild(0).GetComponent<TMP_Text>();

        toastText.text = text;
        Destroy(toastPanel.gameObject,2f);
    }


    public void RankSuccess()
    {
        // rankSuccessPanel.alpha =1;
        // rankSuccessPanel.DOFade(0,5f);
    }

    public void InsertData(RankingData rankingData,Action action = null,Action failAction = null)
    {
        loadingPanel.gameObject.SetActive(true);

        if(rankingData.name == "") rankingData.name = "Name";
        if(rankingData.churchName == "") rankingData.churchName = "ChurchName";
        action += () => {
            loadingPanel.gameObject.SetActive(false);
            RankSuccess();
            };

        failAction += () => loadingPanel.gameObject.SetActive(false);
        failAction += () => ToastText(LangManager.IsEng()? "Could't Upload": "랭킹을 등록할 수 없어요.");
        StartCoroutine(InsertDataToServer(rankingData,action,failAction));
    }

    public void GetData(Action action = null, Action failAction = null)
    {

        
        loadingPanel.gameObject.SetActive(true);
        action += () => loadingPanel.gameObject.SetActive(false);

        failAction += () => loadingPanel.gameObject.SetActive(false);

        string showingText = LangManager.IsEng() ? "Can not upload ranking" :"랭킹을 등록할 수 없어요.";
        failAction += () => ToastText(showingText);
        StartCoroutine(GetDataFromServer(action,failAction));
    }

    IEnumerator GetDataFromServer(Action successAction,Action failAction)
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://52.79.46.242:3000/select"))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
                failAction.Invoke();
            }
            else
            {
                // 서버로부터 받은 응답을 출력합니다
                string responseData = www.downloadHandler.text;
                rankingDatas = JsonConvert.DeserializeObject<List<RankingData>>(responseData);
              
                successAction?.Invoke();
                
            }
        }

        
    }


    IEnumerator InsertDataToServer(RankingData data, Action successAction,Action failAction)
    {
        // RankingData 객체를 JSON 형식의 문자열로 변환합니다.
        string jsonData = JsonUtility.ToJson(data);

        // JSON 데이터를 바이트 배열로 변환합니다.
        byte[] jsonDataBytes = System.Text.Encoding.UTF8.GetBytes(jsonData);

        // POST 요청을 생성합니다.
        UnityWebRequest www = new UnityWebRequest("http://52.79.46.242:3000/insert/", "POST");
        www.uploadHandler = new UploadHandlerRaw(jsonDataBytes);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        Debug.Log("insert");

        // 요청을 보냅니다.
        yield return www.SendWebRequest();

        // 응답을 확인합니다.
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(www.error);
            failAction.Invoke();
        }
        else
        {
            // 서버로부터 받은 응답을 출력합니다.
            successAction?.Invoke();
            Debug.Log("Response: " + www.downloadHandler.text);
        }
    }

    public void OnlineTest(Action action = null, Action failAction = null)
    {
        loadingPanel.gameObject.SetActive(true);
        action += () => loadingPanel.gameObject.SetActive(false);

        string showingText = LangManager.IsEng() ? "Conneted to server" :"온라인 상태입니다.";
        

        action += () => ToastText(showingText);
        action += () => 
        {
            onlineMode = true;
            CanvasManager.instance.onlineIndicator.SetOnlineState(onlineMode);
            
        };

        failAction += () => loadingPanel.gameObject.SetActive(false);

        string showingFailText = LangManager.IsEng() ? "Can Not Conneted to server" :"오프라인 상태입니다.";
        failAction += () => ToastText(showingFailText);
        StartCoroutine(OnlineTestFromServer(action,failAction));
    }

    [ContextMenu("NoticeUpload")]
    public void NoticeUpload()
    {
        StartCoroutine(UploadNotice());
    }

    IEnumerator OnlineTestFromServer(Action successAction,Action failAction)
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://52.79.46.242:3000/notice"))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
                failAction.Invoke();
            }
            else
            {
                JObject jsonObject = JObject.Parse(www.downloadHandler.text);

                string key = "";
                foreach (var pair in jsonObject)
                {
                    key = pair.Key;
                }
                string[] splitText = key.Split('^');

                    // 앞부분은 kor, 뒷부분은 eng로 할당합니다.
                string kor = splitText[0];
                string eng = splitText[1];
                
                Debug.Log(www.downloadHandler.text);
                CanvasManager.instance.ChangeNotice(eng , kor);
                successAction?.Invoke();
            }
        }
    }
    IEnumerator UploadNotice()
    {

        // POST 요청을 생성합니다.
        UnityWebRequest www = UnityWebRequest.PostWwwForm("http://52.79.46.242:3000/noticeupload",noticeText);

        Debug.Log("Sending insert request");

        // 요청을 보냅니다.
        yield return www.SendWebRequest();

        // 응답을 확인합니다.
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(www.error);
        }
        else
        {
            Debug.Log("Response: " + www.downloadHandler.text);
        }
    }

    

    public void GoldChange(int value)
    {
        var gold = PlayerPrefs.GetInt("gold");

        gold  += value;

        PlayerPrefs.SetInt("gold",gold);
    }
    public int GetCurGold()
    {
        
        return PlayerPrefs.GetInt("gold");
    }
}
