using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using TTSDK.UNBridgeLib.LitJson;
using TTSDK;
using StarkSDKSpace;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public List<GameObject> Poles;
    public List<GameObject> children;
    public List<Transform> NewPos;
    public bool FirstClick;
    public int activatedChildren;

    public GameObject HomePanel;
    public GameObject ButtonsPanel;
    public GameObject settingPanel;
    public Text HomelevelNumberText;
    public TextMeshProUGUI CanvaslevelNumberText;
    public int NewPoleCount => GetNewPoleCount();
    public int ReverseCount => GetUndoCount();


    // Event Variables
    private static int _levelAttempts;
    public string clickid;
    private StarkAdManager starkAdManager;
    private void Awake()
    {
        instance = this;
    }

    private int GetNewPoleCount() => PlayerPrefs.GetInt("NewPole", 1);
    private void SetNewPoleCount(int poleCount) => PlayerPrefs.SetInt("NewPole", poleCount);

    private int GetUndoCount() => PlayerPrefs.GetInt("UndoCount", 1);
    private void SetUndoCount(int undoCount) => PlayerPrefs.SetInt("UndoCount", undoCount);

    // Start is called before the first frame update
    void Start()
    {
        PoleScript poleScript = Poles[Poles.Count - 1].GetComponent<PoleScript>();

        // NewPoleCount = GetNewPoleCount();
        // ReverseCount = GetUndoCount();
        SoundManager.instance.ReverseCount.text = ReverseCount.ToString();
        SoundManager.instance.NewPoleCount.text = NewPoleCount.ToString();
        SoundManager.instance.ReverseActive = false;
        SoundManager.instance.UpdateReverseButton();
        CanvaslevelNumberText = SoundManager.instance.CanvaslevelNumberText;
        HomePanel.SetActive(false);
        settingPanel.SetActive(false);
        FirstClick = true;
        activatedChildren = 0;

        if (Poles == null || children == null || NewPos == null)
        {
            Debug.Log("New Pole Powerup Not possible");

            // Set the lists to null to release memory
            Poles = null;
            children = null;
            NewPos = null;
        }
        else
        {
            Poles[Poles.Count - 1].SetActive(false);
        }

        print($"UndoCount::{ReverseCount}++NewPoleCount::{NewPoleCount}::{GetUndoCount()}:::{GetNewPoleCount()}");
        if (ReverseCount == 0)
        {
            SoundManager.instance.ReverseButton.transform.GetChild(1).gameObject.SetActive(false);
            SoundManager.instance.ReverseButton.transform.GetChild(2).gameObject.SetActive(true);
        }

        if (NewPoleCount == 0)
        {
            SoundManager.instance.newPoleButton.transform.GetChild(1).gameObject.SetActive(false);
            SoundManager.instance.newPoleButton.transform.GetChild(2).gameObject.SetActive(true);
        }

        CanvaslevelNumberText.text = "关卡 " + PlayerPrefs.GetInt("levelnumber", 1).ToString();
        if (GAScript.Instance) GAScript.Instance.LevelStart(PlayerPrefs.GetInt("levelnumber", 1).ToString());
        //CanvaslevelNumberText.text = "Level " + (PlayerPrefs.GetInt(PlayerPrefsManager.Level,1) + 1).ToString();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Restart()
    {
        _levelAttempts++;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        Vibration.Vibrate(30);
    }

    public void NextScene()
    {
        if (PlayerPrefs.GetInt("level", 1) >= SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(Random.Range(5, SceneManager.sceneCountInBuildSettings - 1));
            PlayerPrefs.SetInt("level", (PlayerPrefs.GetInt("level", 1) + 1));
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("level", (PlayerPrefs.GetInt("level", 1) + 1));
        }

        PlayerPrefs.SetInt("levelnumber", PlayerPrefs.GetInt("levelnumber", 1) + 1);
        Vibration.Vibrate(30);
    }

    public void reverseButton()
    {
        ShowVideoAd("192if3b93qo6991ed0",
            (bol) => {
                if (bol)
                {
                    if (/*ReverseCount > 0 && */SoundManager.instance.ReverseActive)
                    {
                        TransferData.GameObjectData data = TransferData.instance.PopData();
                        Debug.Log("data received");
                        if (data == null)
                        {
                            Debug.LogError("No data found in the stack!");
                            return;
                        }

                        Vibration.Vibrate(30);
                        // ReverseCount--;
                        SetUndoCount(ReverseCount - 1);
                        SoundManager.instance.ReverseCount.text = ReverseCount.ToString();
                        if (ReverseCount <= 0)
                        {
                            SoundManager.instance.ReverseActive = false;
                            SoundManager.instance.UpdateReverseButton();
                        }

                        if (ReverseCount == 0)
                        {
                            SoundManager.instance.ReverseButton.transform.GetChild(1).gameObject.SetActive(false);
                            SoundManager.instance.ReverseButton.transform.GetChild(2).gameObject.SetActive(true);
                        }

                        StartCoroutine(ReverseMoving(data));
                    }
                    //else
                    //{
                    //    if (ISManager.instance)
                    //    {
                    //        ISManager.instance.adState = AdState.Undo;
                    //        ISManager.instance.ShowRewardedVideo();
                    //    }
                    //}



                    clickid = "";
                    getClickid();
                    apiSend("game_addiction", clickid);
                    apiSend("lt_roi", clickid);


                }
                else
                {
                    StarkSDKSpace.AndroidUIManager.ShowToast("观看完整视频才能获取奖励哦！");
                }
            },
            (it, str) => {
                Debug.LogError("Error->" + str);
                //AndroidUIManager.ShowToast("广告加载异常，请重新看广告！");
            });
        
    }

    public void UndoCallBack()
    {
        TransferData.GameObjectData data = TransferData.instance.PopData();
        Debug.Log("data received");
        if (data == null)
        {
            Debug.LogError("No data found in the stack!");
            return;
        }

        Vibration.Vibrate(30);
        SoundManager.instance.ReverseCount.text = ReverseCount.ToString();
        if (ReverseCount <= 0)
        {
            SoundManager.instance.ReverseActive = false;
            SoundManager.instance.UpdateReverseButton();
        }

        if (ReverseCount == 0)
        {
            SoundManager.instance.ReverseButton.transform.GetChild(1).gameObject.SetActive(false);
            SoundManager.instance.ReverseButton.transform.GetChild(2).gameObject.SetActive(true);
        }

        StartCoroutine(ReverseMoving(data));
    }

    int ReversingCount = 0;

    public IEnumerator ReverseMoving(TransferData.GameObjectData data)
    {
        if (GameManager.instance.Upward)
        {
            PoleScript poleScript = GameManager.instance.FirstPole.GetComponent<PoleScript>();
            StartCoroutine(poleScript.ReturnRing());
            if (data.firstPole == GameManager.instance.FirstPole)
            {
                yield return new WaitForSeconds(.3f);
            }

            for (int i = 0; i < LevelManager.instance.Poles.Count; i++)
            {
                Poles[i].GetComponent<CapsuleCollider>().enabled = false;
                Poles[i].GetComponent<BoxCollider>().enabled = false;
            }
        }
        else
        {
            for (int i = 0; i < LevelManager.instance.Poles.Count; i++)
            {
                PoleScript poleScript = LevelManager.instance.Poles[i].GetComponent<PoleScript>();
                Poles[i].GetComponent<CapsuleCollider>().enabled = false;
                Poles[i].GetComponent<BoxCollider>().enabled = false;

                if (poleScript.receving)
                {
                    yield return new WaitForSeconds(.7f);
                }
            }
        }

        ReversingCount++;

        GameObject poleToMove = data.firstPole;
        GameObject startPole = data.secondPole;
        List<GameObject> Rings = data.RingsMoved;

        PoleScript startPoleScript = startPole.GetComponent<PoleScript>();
        Vector3 startPoleTop = startPoleScript.StartingPos;

        PoleScript targetPoleScript = poleToMove.GetComponent<PoleScript>();
        Transform targetPoleTop = targetPoleScript.Top;


        for (int i = Rings.Count - 1; i >= 0; i--)
        {
            Transform targetMovePoint = targetPoleScript.ChildPolePos[targetPoleScript.FilledPoleCount];

            startPoleScript.Rings.Remove(Rings[i]);
            startPoleScript.FilledPoleCount--;
            targetPoleScript.Rings.Add(Rings[i]);
            targetPoleScript.FilledPoleCount++;
            //targetPoleScript.Expanding = true;
            Ring_Movement ring_Movement = Rings[i].GetComponent<Ring_Movement>();

            float moveDur = 0.2f;

            Transform child = Rings[i].transform;
            child.DOKill();
            SoundManager.instance.clickSound.PlayOneShot(SoundManager.instance.UpwardClickSound);
            child.DOMoveY(startPoleTop.y, moveDur).SetEase(Ease.Linear).OnComplete(() =>
            {
                Vector3 TargetTopPos = targetPoleScript.StartingPos;
                child.DOMove(TargetTopPos, moveDur).SetEase(Ease.Linear).OnComplete(() =>
                {
                    SoundManager.instance.clickSound.PlayOneShot(SoundManager.instance.DownwardClickSound);
                    child.DOMove(targetMovePoint.position, moveDur).SetEase(Ease.Linear);
                    child.DORotate(new Vector3(0, -360, 0), moveDur, RotateMode.FastBeyond360).SetEase(Ease.Linear)
                        .OnComplete(() =>
                        {
                            child.position = targetMovePoint.position;
                            ring_Movement.ChildPolePosition = targetMovePoint;
                            child.DOKill();
                        });
                });
            });
            yield return new WaitForSeconds(.45f);


            Vibration.Vibrate(15);
        }

        ReversingCount--;
        yield return new WaitForSeconds(.2f);
        if (ReversingCount == 0)
        {
            for (int j = 0; j < LevelManager.instance.Poles.Count; j++)
            {
                Poles[j].GetComponent<CapsuleCollider>().enabled = true;
                Poles[j].GetComponent<BoxCollider>().enabled = true;
            }
        }

        yield return null;
    }

    private bool NewPoleActivated = false;

    public void buttonNewPOle()
    {
        if (activatedChildren >= children.Count)
        {
            return;
        }
        ShowVideoAd("192if3b93qo6991ed0",
            (bol) => {
                if (bol)
                {
                    if (!NewPoleActivated)
                    {
                        int lastPoleIndex = Poles.Count - 1;

                        if (FirstClick)
                        {
                            NewPoleActivated = true;
                            bool IsReceving = false;
                            bool upWard = GameManager.instance.Upward;
                            if (upWard)
                            {
                                PoleScript poleScript = GameManager.instance.FirstPole.GetComponent<PoleScript>();
                                poleScript.CheckUpward = true;
                                StartCoroutine(poleScript.ReturnRing());
                            }

                            for (int i = 0; i < Poles.Count - 1; i++)
                            {
                                PoleScript poleScript = Poles[i].GetComponent<PoleScript>();

                                poleScript.PoleCollider.enabled = false;
                                Poles[i].GetComponent<BoxCollider>().enabled = false;
                                if (poleScript.receving && !IsReceving)
                                {
                                    poleScript.CheckReceving = true;
                                    IsReceving = true;
                                }
                            }

                            if (!IsReceving && !upWard)
                            {
                                StartCoroutine(FirstClickNewPOle());
                            }
                        }
                        else
                        {
                            if (activatedChildren >= children.Count)
                            {
                                Debug.Log("error in pole Powerup");
                            }
                            else
                            {
                                children[activatedChildren].gameObject.SetActive(true);
                                PoleScript Lastpole = Poles[lastPoleIndex].GetComponent<PoleScript>();
                                Lastpole.PoleCount++;
                                Lastpole.FogNewPole.SetActive(false);
                                Lastpole.FogNewPole.transform.position = children[activatedChildren].transform.position;
                                Lastpole.FogNewPole.SetActive(true);
                                Lastpole.ChildPolePos.Add(children[activatedChildren].transform);
                                activatedChildren++;
                            }
                        }

                        // NewPoleCount--;
                        SetNewPoleCount(NewPoleCount - 1);
                        SoundManager.instance.clickSound.PlayOneShot(SoundManager.instance.NewPoleSound);
                        SoundManager.instance.NewPoleCount.text = NewPoleCount.ToString();
                        if (NewPoleCount == 0)
                        {
                            SoundManager.instance.newPoleButton.transform.GetChild(1).gameObject.SetActive(false);
                            SoundManager.instance.newPoleButton.transform.GetChild(2).gameObject.SetActive(true);
                        }

                        Vibration.Vibrate(30);
                    }
                    //else
                    //{
                    //    Debug.Log("no more pole to add");
                    //    Vibration.Vibrate(30);
                    //    if (ISManager.instance)
                    //    {
                    //        ISManager.instance.adState = AdState.Slot;
                    //        ISManager.instance.ShowRewardedVideo();
                    //    }
                    //}



                    clickid = "";
                    getClickid();
                    apiSend("game_addiction", clickid);
                    apiSend("lt_roi", clickid);


                }
                else
                {
                    StarkSDKSpace.AndroidUIManager.ShowToast("观看完整视频才能获取奖励哦！");
                }
            },
            (it, str) => {
                Debug.LogError("Error->" + str);
                //AndroidUIManager.ShowToast("广告加载异常，请重新看广告！");
            });
        
    }


    public void NewPoleCallBack()
    {
        int lastPoleIndex = Poles.Count - 1;

        if (FirstClick)
        {
            NewPoleActivated = true;
            bool IsReceving = false;
            bool upWard = GameManager.instance.Upward;
            if (upWard)
            {
                PoleScript poleScript = GameManager.instance.FirstPole.GetComponent<PoleScript>();
                poleScript.CheckUpward = true;
                StartCoroutine(poleScript.ReturnRing());
            }

            for (int i = 0; i < Poles.Count - 1; i++)
            {
                PoleScript poleScript = Poles[i].GetComponent<PoleScript>();

                poleScript.PoleCollider.enabled = false;
                Poles[i].GetComponent<BoxCollider>().enabled = false;
                if (poleScript.receving && !IsReceving)
                {
                    poleScript.CheckReceving = true;
                    IsReceving = true;
                }
            }

            if (!IsReceving && !upWard)
            {
                StartCoroutine(FirstClickNewPOle());
            }
        }
        else
        {
            if (activatedChildren >= children.Count)
            {
                Debug.Log("error in pole Powerup");
            }
            else
            {
                children[activatedChildren].gameObject.SetActive(true);
                PoleScript Lastpole = Poles[lastPoleIndex].GetComponent<PoleScript>();
                Lastpole.PoleCount++;
                Lastpole.FogNewPole.SetActive(false);
                Lastpole.FogNewPole.transform.position = children[activatedChildren].transform.position;
                Lastpole.FogNewPole.SetActive(true);
                Lastpole.ChildPolePos.Add(children[activatedChildren].transform);
                activatedChildren++;
            }
        }
    }

    public IEnumerator FirstClickNewPOle()
    {
        int lastPoleIndex = Poles.Count - 1;
        yield return new WaitForSeconds(.05f);

        Poles[lastPoleIndex].gameObject.SetActive(true);

        PoleScript Lastpole = Poles[lastPoleIndex].GetComponent<PoleScript>();
        Lastpole.PoleCount = 1;
        Lastpole.ChildPolePos.Clear(); // Clear the list if you need to start fresh
        Lastpole.ChildPolePos.Add(children[0].transform); // Add the transform of the first child
        for (int i = 1; i <= children.Count - 1; i++)
        {
            children[i].gameObject.SetActive(false);
        }

        for (int i = 0; i <= Poles.Count - 1; i++)
        {
            PoleScript poleScript = Poles[i].GetComponent<PoleScript>();

            Poles[i].transform.position = NewPos[i].position;
            poleScript.StartingPos = poleScript.Top.position + Vector3.up * poleScript.HightOffset;

            for (int j = 0; j < poleScript.FilledPoleCount; j++)
            {
                poleScript.Rings[j].transform.position = poleScript.ChildPolePos[j].position;
            }

            poleScript.PoleCollider.enabled = true;
            Poles[i].GetComponent<BoxCollider>().enabled = true;
        }

        Lastpole.FogNewPole.transform.position = Lastpole.ChildPolePos[0].position;
        Lastpole.FogNewPole.SetActive(true);
        NewPoleActivated = false;
        activatedChildren++;
        FirstClick = false;
    }


    public void HomeButton()
    {
        HomePanel.SetActive(true);
        ButtonsPanel.SetActive(false);

        Transform numberTransform = HomePanel.transform.Find("circle/number");
        HomelevelNumberText = numberTransform.GetComponent<Text>();

        HomelevelNumberText.text = PlayerPrefs.GetInt("level", 1).ToString();
        Vibration.Vibrate(30);
    }

    public void SetingButton()
    {
        settingPanel.SetActive(true);
        ButtonsPanel.SetActive(false);
        Vibration.Vibrate(30);
    }

    public void Closebutton()
    {
        settingPanel.SetActive(false);
        ButtonsPanel.SetActive(true);
        Time.timeScale = 1f;
        Vibration.Vibrate(30);
    }

    public void LoadLevelMap()
    {
        _levelAttempts = 0;
        if (GAScript.Instance) GAScript.Instance.LevelEnd(true, PlayerPrefs.GetInt("levelnumber", 1).ToString());
        if (PlayerPrefs.GetInt("level", 1) >= SceneManager.sceneCountInBuildSettings - 2)
        {
            SceneManager.LoadScene(Random.Range(5, SceneManager.sceneCountInBuildSettings - 2));
            PlayerPrefs.SetInt("level", (PlayerPrefs.GetInt("level", 1) + 1));
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            //SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1);
            PlayerPrefs.SetInt("level", (PlayerPrefs.GetInt("level", 1) + 1));
        }

        PlayerPrefs.SetInt("levelnumber", PlayerPrefs.GetInt("levelnumber", 1) + 1);
    }

    public void SaveLevel()
    {
        int currentLevel = PlayerPrefs.GetInt(PlayerPrefsManager.Level);
        PlayerPrefs.SetInt(PlayerPrefsManager.Level, ++currentLevel);
    }
    public void getClickid()
    {
        var launchOpt = StarkSDK.API.GetLaunchOptionsSync();
        if (launchOpt.Query != null)
        {
            foreach (KeyValuePair<string, string> kv in launchOpt.Query)
                if (kv.Value != null)
                {
                    Debug.Log(kv.Key + "<-参数-> " + kv.Value);
                    if (kv.Key.ToString() == "clickid")
                    {
                        clickid = kv.Value.ToString();
                    }
                }
                else
                {
                    Debug.Log(kv.Key + "<-参数-> " + "null ");
                }
        }
    }

    public void apiSend(string eventname, string clickid)
    {
        TTRequest.InnerOptions options = new TTRequest.InnerOptions();
        options.Header["content-type"] = "application/json";
        options.Method = "POST";

        JsonData data1 = new JsonData();

        data1["event_type"] = eventname;
        data1["context"] = new JsonData();
        data1["context"]["ad"] = new JsonData();
        data1["context"]["ad"]["callback"] = clickid;

        Debug.Log("<-data1-> " + data1.ToJson());

        options.Data = data1.ToJson();

        TT.Request("https://analytics.oceanengine.com/api/v2/conversion", options,
           response => { Debug.Log(response); },
           response => { Debug.Log(response); });
    }


    /// <summary>
    /// </summary>
    /// <param name="adId"></param>
    /// <param name="closeCallBack"></param>
    /// <param name="errorCallBack"></param>
    public void ShowVideoAd(string adId, System.Action<bool> closeCallBack, System.Action<int, string> errorCallBack)
    {
        starkAdManager = StarkSDK.API.GetStarkAdManager();
        if (starkAdManager != null)
        {
            starkAdManager.ShowVideoAdWithId(adId, closeCallBack, errorCallBack);
        }
    }
}