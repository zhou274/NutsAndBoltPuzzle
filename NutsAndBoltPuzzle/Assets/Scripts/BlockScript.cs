using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    public GameObject FirstPole;
    public GameObject SecondPole;
    public int index;

    private void Awake()
    {
        PoleScript FirstPoleScriprt = FirstPole.GetComponent<PoleScript>();
        PoleScript SecondPoleScript = SecondPole.GetComponent<PoleScript>();
        FirstPoleScriprt.Block =transform.gameObject;
        SecondPoleScript.Block =transform.gameObject;
        FirstPoleScriprt.Blocked = true;
        SecondPoleScript.Blocked = true;
        FirstPoleScriprt.blocked_index = index;
        SecondPoleScript.blocked_index = index;
        FirstPoleScriprt.BlockConnectedPole = SecondPole;
        SecondPoleScript.BlockConnectedPole = FirstPole;
    }
}
