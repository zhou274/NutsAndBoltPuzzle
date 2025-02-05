using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrainerScript : MonoBehaviour
{
    public static TrainerScript instance;
    public LevelManager levelManager;
    [SerializeField] private float Time = 1f;
    // Start is called before the first frame update
    [SerializeField] private List<GameObject> poles;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        // levelManager = GetComponent<LevelManager>();
        poles = levelManager.Poles;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SignInPole(GameObject SelectedPole)
    {

        PoleScript SelectedPoleScript = SelectedPole.GetComponent<PoleScript>();
        GameObject selectedRing = SelectedPoleScript.Rings[SelectedPoleScript.Rings.Count - 1];
        Ring_Movement SelectedRing_movement = selectedRing.GetComponent<Ring_Movement>();
        foreach (GameObject pole in poles)
        {

            if (pole.activeSelf)
            {
                if (pole == SelectedPole)
                {
                    continue;
                }
                else
                {
                    PoleScript poleScript = pole.GetComponent<PoleScript>();
                    if (poleScript.FilledPoleCount == poleScript.PoleCount)
                    {
                        GameObject sign = poleScript.Full;
                        StartCoroutine(ActivateAndDeactivate(sign));
                    }
                    else if (poleScript.FilledPoleCount == 0)
                    {
                        GameObject sign = poleScript.Tick;
                        StartCoroutine(ActivateAndDeactivate(sign));
                    }
                    else
                    {
                        GameObject Rings = poleScript.Rings[poleScript.Rings.Count - 1];
                        if (poleScript.Blocked && poleScript.Rings.Count - 1 == poleScript.blocked_index)
                        {
                            GameObject sign = poleScript.Tick;
                            StartCoroutine(ActivateAndDeactivate(sign));
                        }
                        else
                        {
                            Ring_Movement ring_Movement = Rings.GetComponent<Ring_Movement>();
                            if (SelectedRing_movement.Colour == ring_Movement.Colour)
                            {
                                GameObject sign = poleScript.Tick;
                                StartCoroutine(ActivateAndDeactivate(sign));
                            }
                            else
                            {
                                GameObject sign = poleScript.Cross;
                                StartCoroutine(ActivateAndDeactivate(sign));
                            }
                        }
                     
                    }
                }
            }

        }
    }

    IEnumerator ActivateAndDeactivate(GameObject Helper)
    {

        Helper.SetActive(true);
        yield return new WaitForSeconds(Time);
        Helper.SetActive(false);
    }
}
