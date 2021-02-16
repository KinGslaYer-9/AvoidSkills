using System.Collections;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdmobManager : MonoBehaviour
{
    #region Singleton

    private static AdmobManager _instance;
    public static AdmobManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var obj = FindObjectOfType<AdmobManager>();
                if (obj != null)
                {
                    _instance = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<AdmobManager>();
                    _instance = newObj;
                }
            }
            return _instance;
        }
    }

    #endregion
    
    private const string UnitID = "ca-app-pub-3940256099942544/1033173712";
    private const string TestUnitID = "ca-app-pub-4555023552616472/5758257784";
    private const string TestDeviceID = "4917842B2184F494";
    
    private InterstitialAd frontAd;

    private void InitAd()
    {
        string id = Debug.isDebugBuild ? TestUnitID : UnitID;

        frontAd = new InterstitialAd(id);

        AdRequest request;

        if (Debug.isDebugBuild)
        {
            request = new AdRequest.Builder().AddTestDevice(TestDeviceID).Build();
        }
        else
        {
            request = new AdRequest.Builder().Build();
        }

        frontAd.LoadAd(request);
        frontAd.OnAdClosed += (sender, e) => Debug.Log("screen Ads Closed");
        frontAd.OnAdLoaded += (sender, e) => Debug.Log("screen Ads Loaded");
    }

    public void Show()
    {
        int draw = Random.Range(0, 5);
        if (draw == 0)
        {
            StartCoroutine("ShowFrontAd");
        }
    }

    private IEnumerator ShowFrontAd()
    {
        while (!frontAd.IsLoaded())
        {
            yield return null;
        }
        
        frontAd.Show();
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitAd();
    }
}
