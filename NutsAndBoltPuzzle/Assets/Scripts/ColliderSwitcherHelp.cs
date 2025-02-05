using UnityEngine;

public class ColliderSwitcherHelp : MonoBehaviour
{
    public static ColliderSwitcherHelp Instance;
    public GameObject firstPole;
    public GameObject secondPole;
    public GameObject Hand1;
    public GameObject Hand2;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Enable the collider of the first object
        firstPole.GetComponent<CapsuleCollider>().enabled = true;
        secondPole.GetComponent<CapsuleCollider>().enabled = false;
        Hand2.SetActive(false);
        Hand1.SetActive(true);
    }

    void Update()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            PoleScript poleScript =firstPole.GetComponent<PoleScript>();
            if (poleScript.moving)
            {
                firstPole.GetComponent<CapsuleCollider>().enabled = false;
                secondPole.GetComponent<CapsuleCollider>().enabled = true;
                Hand2.SetActive(true);
                Hand1.SetActive(false);
            }
            
        }
    }
    public void LevelComplete()
    {
        Hand2.SetActive(false);
    }
}
