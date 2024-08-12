using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AddFriendPanel : MonoBehaviour
{
    public TMP_InputField recommendCodeInput;
    public TMP_Text myRecommendCodeText, recommendedCodeText, recommendCodeTextConfirm;

    public string myRecommendCode;

    public string recommendCode;

    public Button  firstRecommendButton ,realConfirmRecommend;

    public GameObject reCheckPanel;
    // Start is called before the first frame update
    void Start()
    {
        firstRecommendButton.onClick.AddListener(FirstConfirmRecommend);
        realConfirmRecommend.onClick.AddListener(RealConfirmRecommend);


    }   

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMyRecommenCode(string _text)
    {
        myRecommendCode = _text;
        myRecommendCodeText.text = _text;
    }

    public void SetAlreadyRecommeded()
    {
        recommendCodeInput.text = PlayerPrefs.GetString("Recommeded");
        recommendCodeInput.interactable = false;
        firstRecommendButton.gameObject.SetActive(false);

    }

    public void FirstConfirmRecommend()
    {
        if(!myRecommendCode.Equals(recommendCodeInput.text))
        {
            recommendCodeTextConfirm.text = recommendCodeInput.text;
            reCheckPanel.SetActive(true);
        }else
        {
            string text = LangManager.IsEng()? "Set unequal my referral code" : "내 코드는 추천인으로 사용할 수 없어요.";
            NetworkManager.instance.ToastText(text);
        }
    }

    public void RealConfirmRecommend()
    {
        // NetworkManager.instance

        recommendCode = recommendCodeInput.text ;
        PlayerPrefs.SetString("Recommeded",recommendCode);
        NetworkManager.instance.RecommendAdd(recommendCode);
        
        
    }
}

