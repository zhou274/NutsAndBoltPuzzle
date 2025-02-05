using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine.Rendering.Universal;
using StarkSDKSpace;
using TTSDK.UNBridgeLib.LitJson;
using TTSDK;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

//    public bool TransferWorking;
    // public bool RingExpanding;

    [HideInInspector] public float delayBetweenRings = .1f;

    public GameObject FirstPole;
    public List<GameObject> MovingRingList;

    public int NumberOfRingsCanBeMoved;
    public int NumberOfRingsAvailable;

    public int NumberOfColor;
    public int NumberOfColorCompleted = 0;

    public GameObject WinningCanvas;
    public int NumberOfChildInPole = 4;

    //public bool nextClick;

    public bool Training = false;

    [SerializeField] public List<GameObject> FinishedPoleList;
    public bool hiidenRings = false;

    private GameObject levelManagerObject;
    private LevelManager levelManager;
    public bool Upward;
    private StarkAdManager starkAdManager;

    public string clickid;
    private void Awake()
    {
        instance = this;
    }
    /// <summary>
    /// 播放插屏广告
    /// </summary>
    /// <param name="adId"></param>
    /// <param name="errorCallBack"></param>
    /// <param name="closeCallBack"></param>
    public void ShowInterstitialAd(string adId, System.Action closeCallBack, System.Action<int, string> errorCallBack)
    {
        starkAdManager = StarkSDK.API.GetStarkAdManager();
        if (starkAdManager != null)
        {
            var mInterstitialAd = starkAdManager.CreateInterstitialAd(adId, errorCallBack, closeCallBack);
            mInterstitialAd.Load();
            mInterstitialAd.Show();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        // RingExpanding = false;
        // Upward = false;
        Application.targetFrameRate = 120;
        levelManagerObject = GameObject.Find("LevelManager");
        levelManager = levelManagerObject.GetComponent<LevelManager>();
        // nextClick = true;
        WinningCanvas.SetActive(false);
        NumberOfColorCompleted = 0;

        if (NumberOfColor == 0)
        {
            Debug.Log("add number of color need to be sorted");
        }

        // GameObject DirectionalLight = GameObject.Find("Directional Light");
        // Light light = DirectionalLight.GetComponent<Light>();
        // light.intensity = 1.4f;
        //Camera.main.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = true;
        Vibration.Init();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.D))
        {
            var ss = GetScreenShot();
            Debug.Log("SS");
            ScreenCapture.CaptureScreenshot(Path.Combine(Application.persistentDataPath, $"SS_{ss}.png"));
            SetScreenShot(GetScreenShot() + 1);
        }
        return;
        int GetScreenShot() => EditorPrefs.GetInt("SS", 1);

        void SetScreenShot(int val) => EditorPrefs.SetInt("SS", val);*/
    }


    public void UpdateRingList()
    {
        int listcount = RingsNeedToMove();

        for (int i = MovingRingList.Count - 1; i >= listcount; i--)
        {
            MovingRingList.RemoveAt(i);
        }

        Debug.Log(MovingRingList.Count);
    }

    public void RingTransfer(GameObject Pole, int index)
    {
        PoleScript TarGetPole_script = Pole.GetComponent<PoleScript>();

        if (TransferData.instance.stackList[index].RingsMoved.Count > 0)
        {
            StartCoroutine(TarGetPole_script.MoveRingsWithDelay());
        }
    }

    //public void MoovingStarted()
    //{
    //    PoleScript poleScript = FirstPole.GetComponent<PoleScript>();
    //    poleScript.moving = false;
    //}

    public int RingsNeedToMove()
    {
        Debug.Log(" NumberOfRingsAvailable--" + NumberOfRingsAvailable);
        Debug.Log("NumberOfRingsCanBeMoved--" + NumberOfRingsCanBeMoved);

        if (NumberOfRingsCanBeMoved >= NumberOfRingsAvailable)
        {
            return NumberOfRingsAvailable;
        }
        else
        {
            return NumberOfRingsCanBeMoved;
        }
    }


    public IEnumerator PoleCompleted()
    {
        NumberOfColorCompleted++;

        if (NumberOfColorCompleted == NumberOfColor)
        {
            LevelManager.instance.SaveLevel();
            SoundManager.instance.winningSound.Play();
            if (ColliderSwitcherHelp.Instance) ColliderSwitcherHelp.Instance.LevelComplete();
            levelManager.ButtonsPanel.SetActive(false);
            levelManager.HomePanel.SetActive(false);
            yield return new WaitForSeconds(1f);
            WaveAnimation();

            yield return new WaitForSeconds(2f);
            ShowInterstitialAd("1lcaf5895d5l1293dc",
            () => {
                Debug.LogError("--插屏广告完成--");

            },
            (it, str) => {
                Debug.LogError("Error->" + str);
            });
            WinningCanvas.SetActive(true);
            Vibration.Vibrate(30);
        }
    }


    public void WaveAnimation()
    {
        for (int k = FinishedPoleList.Count - 1; k >= 0; k--)
        {
            List<Transform> children = new List<Transform>();
            GameObject pole = FinishedPoleList[k];

            PoleScript poleScript = pole.GetComponent<PoleScript>();
            poleScript.Confetti.SetActive(false);
            //poleScript.Confetti.transform.localPosition += new Vector3(0, -2, 0);
            //poleScript.Confetti.transform.localScale += new Vector3(2, 2, 2);
            ////ParticleSystem particleSystem = poleScript.Confetti.GetComponent<ParticleSystem>();
            //particleSystem.startSpeed = 15;
            //ParticleSystem.ShapeModule shapeModule = particleSystem.shape;

            //// Setting the angle of the cone shape to 20 degrees
            //shapeModule.angle = 20f;
            ////List<GameObject> Rings = poleScript.Rings;
            //for( int i =Rings.Count - 1; i >=0; i--)
            //{
            //    GameObject ring = Rings[i];
            //    Transform ringTransform = ring.transform;
            //    int childCount = ringTransform.childCount;

            //    for (int j = childCount - 1; j >= 0; j--)
            //    {
            //        Transform child = ringTransform.GetChild(j);
            //        if(child.gameObject.activeSelf == true)
            //        {
            //            children.Add(child);
            //        }

            //    }
            //}
            //StartCoroutine(ColorChanging(children));
            poleScript.Confetti.SetActive(true);
        }
    }

    public IEnumerator ColorChanging(List<Transform> Children)
    {
        for (int i = 0; i < Children.Count; i++)
        {
            Transform child = Children[i];
            child.DOKill();
            child.DOShakeScale(.2f, .4f).SetEase(Ease.InOutElastic).OnComplete(() =>
            {
                child.DOKill();
                Color rainbowColor = Color.HSVToRGB((float)(i) / Children.Count, 1f, 1f);

                Transform childRenderer = child.GetChild(0);
                Renderer renderer = childRenderer.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.DOColor(rainbowColor, 0f);
                }
                else
                {
                    Debug.LogWarning("No Renderer component found on child object.");
                }
            });
            yield return new WaitForSeconds(0.04f);
        }
    }

    public IEnumerator WaveForSinglePole(PoleScript poleScript)
    {
        List<GameObject> Rings = poleScript.Rings;
        for (int i = Rings.Count - 1; i >= 0; i--)
        {
            GameObject ring = Rings[i];
            Transform ringTransform = ring.transform;
            int childCount = ringTransform.childCount;

            for (int j = childCount - 1; j >= 0; j--)
            {
                Transform child = ringTransform.GetChild(j);
                if (child.gameObject.activeSelf == true)
                {
                    child.DOKill();
                    child.DOShakeScale(.2f, .5f).OnComplete(() => { child.DOKill(); });
                    yield return new WaitForSeconds(0.01f);
                    ;
                }
            }
        }
    }

    public void DeactivatePoles()
    {
        foreach (GameObject poles in levelManager.Poles)
        {
            poles.gameObject.transform.localScale = new Vector3(0, 0, 0);
        }
    }
}