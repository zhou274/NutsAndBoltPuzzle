using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using JetBrains.Annotations;
using System;

using UnityEngine.UIElements;
using UnityEditor.Rendering.BuiltIn;



public class PoleScript : MonoBehaviour
{
    public int PoleCount;
    public int FilledPoleCount;
    public int NumberOfRingMoving = 0;
    public int numberOfPoleAvailable = 0;
    public List<GameObject> Rings;

    public Transform Top;
    public List<Transform> ChildPolePos;

    [HideInInspector] public float HightOffset = .2f;
    public Vector3 StartingPos;
    private Vector3 offset;
    [HideInInspector] public float Yoffset = .2f;

    public bool moving;
    [HideInInspector] public float Duration = .25f;

    public bool completed;

    public GameObject Tick;
    public GameObject Cross;
    public GameObject Full;
    public bool training;

    public GameObject Confetti;
    public GameObject StarAura;

    private bool hiidenRings = false;
    public Material black;
    // public bool Expanding;
    public bool receving;
    public int index;
    public int receivingCount;

    public CapsuleCollider PoleCollider;     // for newPole PowerUp
    public bool CheckReceving;
    public bool CheckUpward;
    // Start is called before the first frame update
    List<GameObject> RingReadyToMoveList = new List<GameObject>();
    public bool isExpanding;
    public GameObject FogNewPole;
    public GameObject Base;
    public Material BaseMaterial;
    [Header("Blocked Pole")]
    public bool Blocked;
    public int blocked_index = -1;
    public GameObject BlockConnectedPole;
    public GameObject Block;
    private void Awake()
    {
        PoleCollider = transform.GetComponent<CapsuleCollider>();
        Base.GetComponent<Renderer>().material = BaseMaterial;  

    }
    void Start()
    {
        
        CheckReceving = false;
        hiidenRings = GameManager.instance.hiidenRings;

        Confetti.SetActive(false);
        StarAura.SetActive(false);
        // Expanding = false;
        training = GameManager.instance.Training;
        offset = new Vector3(0, -Yoffset, 0);

        moving = false;
        completed = false;

        StartingPos = Top.position + Vector3.up * HightOffset;

        FilledPoleCount = Rings.Count;
        PoleCount = ChildPolePos.Count;

        if (GameManager.instance.Training)
        {
            Tick.transform.parent = null;
            Cross.transform.parent = null;
            Full.transform.parent = null;
        }

        // Debug.Log("Rings Count -"+Rings.Count +"filled pole-" + FilledPoleCount);
        if (FilledPoleCount <= 0) return;

        for (int i = 0; i < FilledPoleCount; i++)
        {
            Rings[i].transform.position = ChildPolePos[i].position;
            Ring_Movement ring_Movement = Rings[i].GetComponent<Ring_Movement>();
            ring_Movement.ChildPolePosition = ChildPolePos[i];


        }
        if (hiidenRings)
        {
            for (int i = 0; i < FilledPoleCount - 1; i++)
            {
                HideRings(Rings[i]);
            }
        }



    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
       
        if (Input.touchCount > 1) { return; }
        else
        {
            
            //bool TransferWorking = GameManager.instance.TransferWorking;
            if (completed)
            {
                if (!moving && GameManager.instance.Upward)
                {
                    PoleScript poleScript = GameManager.instance.FirstPole.GetComponent<PoleScript>();
                    StartCoroutine(poleScript.ReturnRing());
                    Debug.Log("the pole is sorted");
                }
            }
            else
            {
                if (!GameManager.instance.Upward)// && GameManager.instance.nextClick )//&& !GameManager.instance.RingExpanding)
                {
                    if (!moving && !receving)
                    {


                        if (!isExpanding)
                        {
                            if (training)
                            {
                                TrainerScript.instance.SignInPole(this.gameObject);
                            }

                            Debug.Log("clicked");
                            RingReadyToMoveList = checkRings();
                            if (RingReadyToMoveList != null && RingReadyToMoveList.Count > 0)
                            {
                                Vibration.Vibrate(15);
                                GameManager.instance.MovingRingList = RingReadyToMoveList;
                                StopReturnRing();
                                SoundManager.instance.clickSound.PlayOneShot(SoundManager.instance.UpwardClickSound);
                                //SoundManager.instance.clickSound.Play();

                                GameManager.instance.Upward = true;
                                Debug.Log("upward true first click");
                                moving = true;
                                isExpanding = true;
                                StartCoroutine(UpwardAnimation());

                                //  GameManager.instance.RingExpanding = true;
                                GameManager.instance.FirstPole = this.gameObject;

                                // GameManager.instance.nextClick = false;

                            }
                            else
                            {
                                Debug.Log("No rings to move");
                                //Expanding = false;
                                //moving = false;
                            }
                        }
                    }
                   
                    else
                    {
                        return;
                      
                    }

                }
                else
                {
                    if (moving && !receving)
                    {
                        //if(!isExpanding)
                        StartCoroutine(ReturnRing());

                    }
                    else if (!moving && !receving)
                    {
                        Debug.Log("second");
                        int availableSpot = CheckAvailable();
                        GameManager.instance.NumberOfRingsCanBeMoved = availableSpot;

                        StartCoroutine(RingsSelected());
                    }

                    else if (!moving && receving)
                    {
                        int availableSpot = CheckAvailable();
                        GameManager.instance.NumberOfRingsCanBeMoved = availableSpot;

                        StartCoroutine(RingsSelected());

                    }
                    else
                    {
                        return;
                    }
                    
                }
            }

        }
    }

    public List<GameObject> checkRings()
    {
        if(Blocked && Rings.Count-1 == blocked_index)
        {
            return null;
        }

       else if (FilledPoleCount == 0  )
        {
            //  Debug.Log("no rings to move -- checks");
            return null;
        }

        else
        {
            int k = 0;


            List<GameObject> RingMovingList = new List<GameObject>();

            RingMovingList.Add(Rings[FilledPoleCount - 1]);
            if (Rings.Count > 1)
            {
                Ring_Movement lastRingColor = Rings[Rings.Count - 1].GetComponent<Ring_Movement>();
                int index = 0;
                if (Blocked)
                {
                    index = blocked_index+1;
                }

                for (int i = Rings.Count - 2; i >= index; i--)
                {
                    GameObject NextRing = Rings[i];
                    Ring_Movement NextRingColor = NextRing.GetComponent<Ring_Movement>();
                    if (NextRingColor.Colour == lastRingColor.Colour)
                    {
                        k++;
                        RingMovingList.Add(NextRing);

                    }
                    else
                    {
                        break;
                    }
                }
            }
            NumberOfRingMoving = RingMovingList.Count;

            GameManager.instance.NumberOfRingsAvailable = RingMovingList.Count;

            return RingMovingList;
        }
    }

    public void StopReturnRing()
    {
        List<GameObject> movingRingList = GameManager.instance.MovingRingList;
        StopCoroutine(ReturnRing());

        for (int i = movingRingList.Count - 1; i >= 0; i--)
        {
            StopCoroutine(DownWardMovement(movingRingList[i]));
            Ring_Movement ring_Movement = movingRingList[i].GetComponent<Ring_Movement>();
            Transform child = ring_Movement.child;
            Vector3 startPos = child.position;
            child.DOKill();
           
        }
    }
    //   yield
    public IEnumerator ReturnRing()
    {
        StopUpwardMovement();
        SoundManager.instance.clickSound.PlayOneShot(SoundManager.instance.DownwardClickSound);
        // isExpanding = true;
        GameManager.instance.FirstPole = null;
        List<GameObject> movingRingList = GameManager.instance.MovingRingList;
        if (movingRingList.Count <= 0) yield break;

        //Ring_Movement ring_Movement = movingRingList[0].GetComponent<Ring_Movement>();
        //Transform child = ring_Movement.child;

        StartCoroutine(DownWardMovement(movingRingList[0]));
       
        if(CheckUpward )
        {
            CheckUpward=false;
            yield return new WaitForSeconds(.3f);
            StartCoroutine(LevelManager.instance.FirstClickNewPOle());
        }
      
    }


    public int CheckAvailable()
    {
        int numberOfPoleAvailable = 0;

        if (PoleCount == FilledPoleCount)
        {
            return numberOfPoleAvailable;
        }
        else if (FilledPoleCount == 0)
        {
            numberOfPoleAvailable = PoleCount;

            return numberOfPoleAvailable;
        }
        else
        {
            if (Blocked  && blocked_index == Rings.Count-1)
            {
                numberOfPoleAvailable = PoleCount - FilledPoleCount;
                return numberOfPoleAvailable;
            }
            else
            {
                Ring_Movement UpperRing = Rings[Rings.Count - 1].GetComponent<Ring_Movement>();


                if (GameManager.instance.MovingRingList.Count > 0)
                {
                    Ring_Movement CommingRing = GameManager.instance.MovingRingList[0].GetComponent<Ring_Movement>();

                    if (UpperRing.Colour == CommingRing.Colour)
                    {
                        numberOfPoleAvailable = PoleCount - FilledPoleCount;
                        // Debug.Log("test--"+numberOfPoleAvailable);
                        return numberOfPoleAvailable;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
           

            return 0;
        }
    }


    public IEnumerator RingsSelected()
    {
        PoleScript FirstPoleScript = null;
        int availableRing = 0;
        int RingWhichCanMove = 0;
       
        availableRing = GameManager.instance.NumberOfRingsAvailable;

        RingWhichCanMove = GameManager.instance.RingsNeedToMove();

        if (GameManager.instance.FirstPole)
        { FirstPoleScript = GameManager.instance.FirstPole.GetComponent<PoleScript>(); }
        else yield break;
        List<GameObject> RingsMoving = null;
        RingsMoving = GameManager.instance.MovingRingList;
        //}
       


        if (RingWhichCanMove < 1)
        {

            StartCoroutine(FirstPoleScript.ReturnRing());
           

        }
        else if (RingWhichCanMove >= availableRing)
        {
            FirstPoleScript.isExpanding = false;
            index = TransferData.instance.stackList.Count;
            GameObject firstPole = null;

            firstPole = GameManager.instance.FirstPole;

            TransferData.instance.Push(firstPole, this.gameObject, RingsMoving);

            RemoveRings(firstPole, RingsMoving);
            AddRings(RingsMoving);
            GameManager.instance.RingTransfer(this.gameObject, index);
            GameManager.instance.FirstPole = null;
            receving = true;
            receivingCount++;
            // GameManager.instance.RingExpanding = false;
            GameManager.instance.Upward = false;
            if (FirstPoleScript.Blocked)
            {
              FirstPoleScript.CheckBlockedStatus();
            }
            //  Debug.Log("upward false RingSelcted full transfer");
            FirstPoleScript.moving = false;
            if (!SoundManager.instance.ReverseActive)
            {
                SoundManager.instance.ReverseActive = true;
                SoundManager.instance.UpdateReverseButton();
            }

        }
        else
        {
            FirstPoleScript.isExpanding = false;

           
            index = TransferData.instance.stackList.Count;
            GameObject firstPole = null;

           

            firstPole = GameManager.instance.FirstPole;

            GameManager.instance.UpdateRingList();
            //}


            TransferData.instance.Push(firstPole, this.gameObject, RingsMoving);
            RemoveRings(firstPole, RingsMoving);
            AddRings(RingsMoving);
            GameManager.instance.RingTransfer(this.gameObject, index);
            GameManager.instance.FirstPole = null;
            receving = true;
            receivingCount++;
            // GameManager.instance.RingExpanding = false;
            GameManager.instance.Upward = false;
            if (FirstPoleScript.Blocked)
            {
               FirstPoleScript.CheckBlockedStatus();
            }
            //  Debug.Log("upward false Ringselected partial transfer");
            if (!SoundManager.instance.ReverseActive)
            {
                SoundManager.instance.ReverseActive = true;
                SoundManager.instance.UpdateReverseButton();
            }
            yield return new WaitForSeconds((RingWhichCanMove - availableRing) * 0.3f);
            FirstPoleScript.moving = false;

        }

    }
    public void StopUpwardMovement()
    {
        StopCoroutine(UpwardAnimation());
        Ring_Movement ring_Movement = RingReadyToMoveList[0].GetComponent<Ring_Movement>();
        ring_Movement.child.DOKill();
        //for (int i = 0; i < ChildRings.Count; i++)
        //{
        //    ChildRings[i].DOKill();
        //}
        GameManager.instance.Upward = false;
        isExpanding = false;
        moving = false;
    }
    
    public IEnumerator UpwardAnimation()
    {

        Vector3 position = StartingPos;


        if (RingReadyToMoveList.Count > 0)
        {
            Transform child = RingReadyToMoveList[0].transform;
            child.DOMove(position, .5f).SetEase(Ease.Linear);
            child.DORotate(new Vector3(0,720, 0), .5f, RotateMode.FastBeyond360).SetEase(Ease.Linear).OnComplete(() =>
            {
                
                 isExpanding = false;
            });


        }
        Vibration.Vibrate(30);

        yield return null;
    }
    public bool CheckWinningCondition()
    {
        bool SingleColor = true;
        if (GameManager.instance.NumberOfChildInPole == PoleCount)
        {
            if (Rings.Count == PoleCount)
            {
                Ring_Movement ring_Movement = Rings[0].GetComponent<Ring_Movement>();
                ColourEnum color = ring_Movement.Colour;
                for (int i = 0; i < Rings.Count; i++)
                {
                    Ring_Movement Next_ring_Movement = Rings[i].GetComponent<Ring_Movement>();
                    if (color != Next_ring_Movement.Colour)
                    {
                        SingleColor = false;
                        return SingleColor;

                    }
                }
            }
            else
            {
                SingleColor = false;
                return SingleColor;
            }

        }
        else
        {
            SingleColor = false;
            return SingleColor;
        }
        return SingleColor;
    }



    public IEnumerator DownWardMovement(GameObject ring)
    {
            Ring_Movement ring_Movement = ring.GetComponent<Ring_Movement>();
            Vector3 targetMovePoint = ring_Movement.ChildPolePosition.position;
            Transform child = ring.transform;
            child.DOKill();

      
            child.DOMove(targetMovePoint, 0.3f).SetEase(Ease.Linear);
            child.DORotate(new Vector3(0, -720, 0), .3f, RotateMode.FastBeyond360).SetEase(Ease.Linear);

            yield return new WaitForSeconds(.3f);
            child.position = targetMovePoint;
       

    }

    public void HideRings(GameObject Ring)
    {



        Ring_Movement ring_Movement = Ring.GetComponent<Ring_Movement>();
        ring_Movement.QuestainMark.SetActive(true);
        ring_Movement.child.GetComponent<Renderer>().material = black;
      
        var k = ring_Movement.Colour;
        ring_Movement.Colour = ring_Movement.SecondColourEnum;
        ring_Movement.SecondColourEnum = k;
        ring_Movement.BLACKRING = true;
    }

    public void RevealRings(GameObject Ring)
    {
        
        Ring_Movement ring_Movement = Ring.GetComponent<Ring_Movement>();
        if(ring_Movement.BLACKRING)
        {
            ring_Movement.BLACKRING = false;
            ring_Movement.QuestainMark.SetActive(false);
            Ring.transform.DOScale(0, .05f).SetEase(Ease.Linear).OnComplete(() => {

                ring_Movement.child.GetComponent<Renderer>().material = ring_Movement.OriginalColour;
                ring_Movement.Colour = ring_Movement.SecondColourEnum;
                Ring.transform.DOScale(1, .05f).SetEase(Ease.Linear).OnComplete(() => { Ring.transform.DOKill(); });

            });
        }
       
       
    }


    public IEnumerator MoveRingsWithDelay()
    {

        List<GameObject> RingsMoving = new List<GameObject>();
        RingsMoving = TransferData.instance.stackList[index].RingsMoved;

        GameObject firstPole_ = TransferData.instance.stackList[index].firstPole;
        //GameManager.instance.MoovingStarted();

        foreach (GameObject ring in RingsMoving)
        {
            Ring_Movement ringMovement = ring.GetComponent<Ring_Movement>();

            //ringMovement.MoovingPoles(Pole);
            StartCoroutine(ringMovement.MoveRings(this.gameObject, firstPole_));

            PoleScript FirstPoleScript = firstPole_.gameObject.GetComponent<PoleScript>();

            if (FirstPoleScript.Rings.Count > 0 && hiidenRings)
            {
                FirstPoleScript.RevealRings(FirstPoleScript.Rings[FirstPoleScript.Rings.Count - 1]);
            }


            yield return new WaitForSeconds(.2f);
        }

        SoundManager.instance.RingFalling.Play();

        // GameManager.instance.nextClick = true;
        float RingMovingTime = (RingsMoving.Count * .25f);

        yield return new WaitForSeconds(RingMovingTime);
       

        receving = false;
        receivingCount--;
        float CheckWinTime = 0;



        if (receivingCount == 0)
        {
            if (CheckReceving)
            {
                StartCoroutine(LevelManager.instance.FirstClickNewPOle());
                CheckReceving = false;
            }
            if (CheckWinTime > RingMovingTime)
            {
                CheckWinTime -= RingMovingTime;
            }
            else
            {
                CheckWinTime = 0;
            }
            yield return new WaitForSeconds(CheckWinTime);
            if (CheckWinningCondition())
            {
                GameManager.instance.FinishedPoleList.Add(this.gameObject);
                Ring_Movement ring_Movement = Rings[0].GetComponent<Ring_Movement>();
               
                TransferData.instance.RemoveTravalData();
                StartCoroutine(GameManager.instance.WaveForSinglePole(this));
                completed = true;
                Confetti.SetActive(true);
                StarAura.SetActive(true);
                yield return new WaitForSeconds(.5f);
                SoundManager.instance.poleCompletedSound.Play();
                StartCoroutine(GameManager.instance.PoleCompleted());
                yield return new WaitForSeconds(3);

                StarAura.SetActive(false);
            }



            //}

        }
    }

    public void RemoveRings(GameObject FirstPole, List<GameObject> RingReadyToMoveList)
    {
        PoleScript poleScript = FirstPole.GetComponent<PoleScript>();

        foreach (GameObject ring in RingReadyToMoveList)
        {
            poleScript.Rings.Remove(ring);
        }


        poleScript.FilledPoleCount -= RingReadyToMoveList.Count;
    }

    public void AddRings(List<GameObject> RingMooving)
    {
        foreach (GameObject _rings in RingMooving)
        {
            Rings.Add(_rings);
            FilledPoleCount++;
            Ring_Movement ring_Movement = _rings.GetComponent<Ring_Movement>();
            //  Debug.Log("checked--" + Rings.Count );
            ring_Movement.ChildPolePosition = ChildPolePos[Rings.Count - 1];
        }
    }

    public List<GameObject> UpdateRingList(int listcount, List<GameObject> MovingRingList)
    {
        // int listcount = RingsNeedToMove();

        for (int i = MovingRingList.Count - 1; i >= listcount; i--)
        {
            MovingRingList.RemoveAt(i);
        }
        return MovingRingList;
        // Debug.Log(MovingRingList.Count);
    }

  public bool IsThereNutsAboveBlocker()
    {
        if(Rings.Count -1 == blocked_index)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void CheckBlockedStatus()
    {
        if (Rings.Count - 1 == blocked_index)
        {
            Debug.Log("checkBlock");

            if (BlockConnectedPole )
            {
                FogNewPole.SetActive(false);
                PoleScript poleScript = BlockConnectedPole.GetComponent<PoleScript>();
                Debug.Log("Block");
                if (!poleScript.IsThereNutsAboveBlocker())
                {
                    Blocked = false;
                    poleScript.Blocked = false;
                    Block.transform.DOMoveY(StartingPos.y, .5f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        FogNewPole.transform.position = Block.transform.position;
                        FogNewPole.SetActive(true);
                        Block.SetActive(false);
                    });
                }
            }
           
            
            
        }
    }


}
