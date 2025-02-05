using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferData : MonoBehaviour
{
    public static TransferData instance;
    public class GameObjectData
    {
        public GameObject firstPole;
        public GameObject secondPole;
        public List<GameObject> RingsMoved;

        public GameObjectData(GameObject first, GameObject second, List<GameObject> rings)
        {
            firstPole = first;
            secondPole = second;
            RingsMoved = rings;
        }
    }

    public List<GameObjectData> stackList = new List<GameObjectData>();

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void Push(GameObject firstPole, GameObject secondPole, List<GameObject> RingsMoved)
    {
        stackList.Add(new GameObjectData(firstPole, secondPole, RingsMoved));
    }

    public GameObjectData PopData()
    {
        if (stackList.Count == 0)
        {
            Debug.LogError("Stack is empty!");
            return null;
        }

        int lastIndex = stackList.Count - 1;
        GameObjectData topData = stackList[lastIndex];
        stackList.RemoveAt(lastIndex);
        if(stackList.Count == 0)
        {
            SoundManager.instance.ReverseActive = false;
            SoundManager.instance.UpdateReverseButton();
        }
        return topData;
    }

    public GameObjectData PeekData()
    {
        if (stackList.Count == 0)
        {
            Debug.LogError("Stack is empty!");
            return null;
        }

        return stackList[stackList.Count - 1];
    }

    public void RemoveTravalData()
    {
       stackList.Clear();
       SoundManager.instance.ReverseActive = false;
        SoundManager.instance.UpdateReverseButton();

    }
}
