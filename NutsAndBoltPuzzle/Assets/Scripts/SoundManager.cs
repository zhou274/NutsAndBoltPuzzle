using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SoundManager : MonoBehaviour
{ public static SoundManager instance;

    public AudioSource clickSound;
    public AudioSource RingFalling;
    public AudioSource winningSound;
    public AudioSource poleCompletedSound;
    [SerializeField] private bool isSoundOn = true;
    public bool isVibrationOn = true;
    public TextMeshProUGUI CanvaslevelNumberText;
    public GameObject ButtonsPanel;
    public GameObject settingPanel;

    public Button soundButton;
    public Sprite soundOnSprite;
    public Button vibrationButton;
    public Sprite vibrationOnSprite;
    public Text ReverseCount;
    public Text NewPoleCount;

    public Button ReverseButton;
    public bool ReverseActive;

    public Button newPoleButton;
   // public GameObject crushUiPanel;
    //public Sprite soundOffSprite;
    public AudioClip UpwardClickSound;
    public AudioClip DownwardClickSound;
    public AudioClip NewPoleSound;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
           
            isVibrationOn = PlayerPrefs.GetInt("IsVibrationOn", 1) == 1 ? true : false;
            isSoundOn = PlayerPrefs.GetInt("isSoundOn", 1) == 1 ? true : false;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        soundOnSprite = soundButton.GetComponent<Image>().sprite;
        vibrationOnSprite = vibrationButton.GetComponent<Image>().sprite;
        UpdateSoundButton();
        UpdateButtonState();


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        AudioListener.pause = !isSoundOn;
        PlayerPrefs.SetInt("isSoundOn", isVibrationOn ? 1 : 0);
        PlayerPrefs.Save();// Mute or unmute all audio
        UpdateSoundButton();


    }
    private void UpdateSoundButton()
    {
        if (isSoundOn)
        {
            // Sound is on, set soundOnSprite and full alpha
            soundButton.image.sprite = soundOnSprite;
            ChangeButtonColor(soundButton.image, Color.white, 1f); // Full opacity and white color
            
            // Change child images as well
            foreach (Image childImage in soundButton.GetComponentsInChildren<Image>())
            {
                ChangeButtonColor(childImage, Color.white, 1f);
            }
        }
        else
        {
            // Sound is off, set soundOffSprite, lower alpha, and change color to gray
            soundButton.image.sprite = soundOnSprite;
            ChangeButtonColor(soundButton.image, Color.gray, 0.5f); // Half opacity and gray color

            // Change child images as well
            foreach (Image childImage in soundButton.GetComponentsInChildren<Image>())
            {
                ChangeButtonColor(childImage, Color.gray, 0.9f);
            }
        }
    }

    

    public void ToggleVibration()
    {
        isVibrationOn = !isVibrationOn;
        PlayerPrefs.SetInt("IsVibrationOn", isVibrationOn ? 1 : 0);
        PlayerPrefs.Save();

        UpdateButtonState();

    }
    private void UpdateButtonState()
    {
        if (isVibrationOn)
        {
            // Vibration is on, set vibrationOnSprite
            vibrationButton.image.sprite = vibrationOnSprite;
            ChangeButtonColor(vibrationButton.image, Color.white, 1f);
            ChangeButtonColor(vibrationButton.transform.GetChild(0).GetComponent<Image>(), Color.white, 1f);
        
        }
        else
        {
            // Vibration is off, set vibrationOffSprite
            vibrationButton.image.sprite = vibrationOnSprite;
            ChangeButtonColor(vibrationButton.image, Color.gray, 0.85f);
            ChangeButtonColor(vibrationButton.transform.GetChild(0).GetComponent<Image>(), Color.gray, 0.9f);
          
        }

       
       
    }
    private void ChangeButtonColor(Image image, Color color, float alpha)
    {
        Color newColor = color;
        newColor.a = alpha; // Set alpha value
        image.color = newColor;
    }


    public void SetingButton()
    {
        Time.timeScale = 0f;
        settingPanel.SetActive(true);
        ButtonsPanel.SetActive(false);
        Vibration.Vibrate(30);
    }
    public void Closebutton()
    {
        Time.timeScale = 1f;
        settingPanel.SetActive(false);
        ButtonsPanel.SetActive(true);
        Vibration.Vibrate(30);
    }

    public void UpdateReverseButton()
    {
        if(ReverseActive)
        {
            ChangeButtonColor(ReverseButton.image, Color.white, 1f);
            ChangeButtonColor(ReverseButton.transform.GetChild(0).GetComponent<Image>(), Color.white, .9f);
        }
        else
        {
            ChangeButtonColor(ReverseButton.image, Color.gray, 1f);
            ChangeButtonColor(ReverseButton.transform.GetChild(0).GetComponent<Image>(), Color.gray, .9f);
        }
    }


    
}
