using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;



public class Ring_Movement : MonoBehaviour
{
    public ColourEnum Colour;
    public ColourEnum SecondColourEnum;
    private Vector3 PolePos;
    [HideInInspector] public float JumpingPower = .5f;
   
    [HideInInspector] public float rotaionAngle = 180f;
   // public Transform topTransform;
    public GameObject SecondPole;
    public GameObject SparklingFx;
  
    public GameObject QuestainMark;

    public Transform child;
    public Material OriginalColour;
    public bool BLACKRING;
    public Transform ChildPolePosition;
    private void Awake()
    {
        OriginalColour = child.GetComponent<Renderer>().material;
        QuestainMark.SetActive(false);
    }
    void Start()
    {
      
    
        
    }
   
    void Update()
    {

    }
    public IEnumerator MoveRings(GameObject poleToMove, GameObject startPole)
    {
       
        PoleScript startPoleScript = startPole.GetComponent<PoleScript>();
        Vector3 startPoleTop = startPoleScript.StartingPos;
        SecondPole = poleToMove;
        PoleScript targetPoleScript = poleToMove.GetComponent<PoleScript>();
    
   
         

          
           
            float moveDur = 0.1f;
              transform.DOKill();
          if(transform.position.y < startPoleTop.y)
          {
              transform.DOMove(startPoleTop, moveDur).SetEase(Ease.Linear);
              transform.DORotate(new Vector3(0, 720, 0), moveDur, RotateMode.FastBeyond360).SetEase(Ease.Linear);
    
           }
         else
           {
            moveDur = 0;    
           }
     
        yield return new WaitForSeconds(moveDur);

        StartCoroutine(PerformJump(targetPoleScript));




        Vibration.Vibrate(15);
  

         
    }
   
    
   
    
   public IEnumerator PerformJump(PoleScript targetPoleScript)
    {
        
        Vector3 targetPoleTop = targetPoleScript.StartingPos;
        Vector3 targetMovePoint = ChildPolePosition.position;
        Vector3 start = transform.position;
        Vector3 end = targetPoleTop;
        transform.DOKill();
        transform.DOMove(end, .2f).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.position = end;
            transform.rotation = Quaternion.Euler(Vector3.zero);
        });
      
        SparklingFx.SetActive(false);
        yield return new WaitForSeconds(.2f);
    
        Vector3 startPos = transform.position;
        float duration = .5f;
        transform.DOKill();
      
        transform.DORotate(new Vector3(0, -720, 0), duration, RotateMode.FastBeyond360).SetEase(Ease.Linear);//.OnComplete(() =>
     ;
        float timeElapsed = 0;
        // targetPoleScript.Expanding = true;
        while (timeElapsed < duration)
        {
            // Calculate the interpolation value based on time elapsed
            float t = timeElapsed / duration;
            // Interpolate between the start position and the target position
            transform.position = Vector3.Lerp(startPos, targetMovePoint, t);
            // Increment time elapsed
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = ChildPolePosition.position;
        SparklingFx.SetActive(true);
       




    }
}
