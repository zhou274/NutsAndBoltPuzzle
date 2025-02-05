using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;
using Random = UnityEngine.Random;

public class LevelMapController : MonoBehaviour
{
    [SerializeField] private int firstLevelBuildIndex;
    private int currentUnlockedLevel;
    private int count = 0;
    public float ringScale;

    public PlatformsController[] allPlatforms;
    public List<Platform> platforms;
    public GameObject[] blastObjects;
    public PlayerRingController playerRing;
    public GameObject currentBlastObject;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
       
    }
    public Material completed, upcoming;
    [Header("Sound Fx")] public AudioClip tickClip;
    private AudioSource _audioSource;

    private void OnEnable()
    {
      
    }
    private void Start()
    {
        transform.position = new Vector3(transform.position.x, PlayerPrefs.GetFloat("mapY", transform.position.y), transform.position.z);
        if (playerRing.transform.position.y > 7)  
            transform.position = new Vector3(transform.position.x, transform.position.y - 5, transform.position.z);

        Debug.Log("Y Value is"+PlayerPrefs.GetFloat("mapY", transform.position.y));
        currentUnlockedLevel = PlayerPrefs.GetInt(PlayerPrefsManager.Level, 1);
        InitialiseLevelNumbers();
        InstantiateBlastObject();
        playerRing.transform.position = platforms[currentUnlockedLevel - 1].BlastObjectTransform.position;

        Transform target = platforms[currentUnlockedLevel].BlastObjectTransform;
        Debug.Log(blastObjects[currentUnlockedLevel - 1].name);
        StartCoroutine(playerRing.MoveRingToNextlevel(target, currentBlastObject, ringScale));
        StartCoroutine(MoveTransform());
    }


    private void InitialiseLevelNumbers()
    {
        if (currentUnlockedLevel < 50)
        {
            count = 0;
            for (int i = 0; i < allPlatforms.Length; i++)
            {
                for (int j = 0; j < allPlatforms[i].childPlatforms.Count(); j++)
                {
                    if (count == currentUnlockedLevel)
                    {
                        allPlatforms[i].childPlatforms[j].ChangePlatformColor(upcoming);
                        StartCoroutine(ScaleUpCurrentPlatform(allPlatforms[i].childPlatforms[j].plateTransform));
                        allPlatforms[i].childPlatforms[j].questionMark.SetActive(false);
                    }

                    count++;
                    allPlatforms[i].childPlatforms[j].SetLevelIdx(count);

                    if (count <= currentUnlockedLevel)
                    {
                        allPlatforms[i].childPlatforms[j].ChangePlatformColor(completed);
                        allPlatforms[i].childPlatforms[j].ToggleCheckMark(true);
                    }

                    platforms.Add(allPlatforms[i].childPlatforms[j]);
                }
            }
        }
    }

    public void PlayTickSound()
    {
        _audioSource.PlayOneShot(tickClip);
    }
    private void InstantiateBlastObject()
    {
      //  currentBlastObject = Instantiate(blastObjects[currentUnlockedLevel - 1], platforms[currentUnlockedLevel].BlastObjectTransform, true);
       // BlastObject obj = currentBlastObject.GetComponent<BlastObject>();s
       // ringScale = obj.levelMapRingScale;
       // obj.enabled = false;

        currentBlastObject.transform.position = platforms[currentUnlockedLevel].BlastObjectTransform.position;
        currentBlastObject.transform.GetChild(0).transform.localPosition = Vector3.zero;
        playerRing.transform.parent = currentBlastObject.transform;
    }
    private IEnumerator MoveTransform()
    {
        if (playerRing.transform.position.y < 2)
            yield break;

        yield return new WaitForSeconds(1.2f);

        transform.DOMoveY(transform.position.y - 1f, 1f).OnComplete(() =>
        {
            PlayerPrefs.SetFloat("mapY", transform.position.y);
            PlayerPrefs.Save();
            //Debug.Log("Saving Y Value is" + PlayerPrefs.GetFloat("mapY", transform.position.y));
        }); ;

        if (playerRing.transform.position.y > 7)
        {
            transform.DOMoveY(transform.position.y - 5, 1f).OnComplete(() =>
            {
                PlayerPrefs.SetFloat("mapY", transform.position.y);
                PlayerPrefs.Save();
               // Debug.Log("Saving Y Value is" + PlayerPrefs.GetFloat("mapY", transform.position.y));
            });
        }
    }
    private IEnumerator ScaleUpCurrentPlatform(Transform transform)
    {
        yield return new WaitForSeconds(1f);
        transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f);
    }
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(currentUnlockedLevel + firstLevelBuildIndex);
        Vibration.Vibrate(30);
    }
}
