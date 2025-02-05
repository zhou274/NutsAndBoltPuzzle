using DG.Tweening;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class PlayerRingController : MonoBehaviour
{
    [Tooltip("Increase this to increase the amount of Rotaion when jumping")]
    [SerializeField] private float rotationAmount = 1f;
    [Tooltip(" Adjust this value to change the height of the jump")]
    [SerializeField] private float jumpHeight = 1f;

    [SerializeField] MeshRenderer _renderer;
   // public IEnumerator MoveRingToNextlevel(Transform target, GameObject currentBlastObj, float scale)
    public GameObject jumpFx;
    public GameObject[] obj;
    public IEnumerator MoveRingToNextlevel(Transform target, GameObject currentBlastObj, float scale)
    {
        yield return new WaitForSeconds(0.2f);
        foreach (GameObject go in obj)
        {
            go.transform.SetParent(null);
            go.SetActive(true);
        }
        // Calculate the jump height and number of jumps based on the distance to the target
        float distance = Vector3.Distance(transform.position, target.position);
        int numJumps = Mathf.CeilToInt(distance / (2 * jumpHeight)); // Increase the number of flips for longer distances

        // Generate a random color
        Color randomColor = new Color(Random.value, Random.value, Random.value);

        // Change the ring color
        _renderer.material.color = randomColor;

        // Perform the jump with multiple flips
        transform.DOJump(target.position, jumpHeight, numJumps, 1f);
        StartCoroutine(RotateChildern(target, scale));

        // Rotate the player ring during the movement
        Vector3 movementDirection = target.position - transform.position;
        float rotationAngle = (movementDirection.x >= 0) ? -180 * rotationAmount : 180 * rotationAmount; // If moving right, rotate clockwise; if moving left, rotate counterclockwise

        // Rotate the player ring during the movement
        transform.DORotate(new Vector3(0f, 0f, rotationAngle), 1f, RotateMode.FastBeyond360);

        // Wait for the movement to complete
        yield return new WaitForSeconds(1f);

     //   StartCoroutine(SqeezeObject(target, currentBlastObj, scale));
    }
    private IEnumerator RotateChildern(Transform target, float scale)
    {
        foreach (GameObject go in obj)
        {
            yield return new WaitForSeconds(0.1f);
            float distance = Vector3.Distance(go.transform.position, target.position);
            int numJumps = Mathf.CeilToInt(distance / (2 * jumpHeight));
            go.transform.DOJump(target.position, jumpHeight, numJumps, 1f).OnComplete(()=> go.SetActive(false));
            Vector3 movemenDirection = target.position - go.transform.position;
            float rotatioAngle = (movemenDirection.x >= 0) ? -180 * rotationAmount : 180 * rotationAmount; // If moving right, rotate clockwise; if moving left, rotate counterclockwise
            // Rotate the player ring during the movement
            go.transform.DORotate(new Vector3(0f, 0f, rotatioAngle), 1f, RotateMode.FastBeyond360);
        }
    }
   
}
