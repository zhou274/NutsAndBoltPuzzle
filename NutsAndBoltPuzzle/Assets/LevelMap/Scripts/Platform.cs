using DG.Tweening;
using TMPro;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private TextMeshPro _levelIdx;
    [SerializeField] private BlastObjectAnimations _animation;
    [SerializeField] private Transform _blastObjectPos;
    [SerializeField] private GameObject checkMark;
    [SerializeField] private MeshRenderer platformMeshRenderer;
    public Transform BlastObjectTransform { get { return _blastObjectPos; } }
    public TextMeshPro _LevelIdx { get { return _levelIdx; } set { _levelIdx = value; } }

    public  Transform plateTransform;
    public GameObject questionMark;
    public void SetLevelIdx(int levelIdx)
    {
        _levelIdx.text = levelIdx.ToString();
    }

    public void ToggleCheckMark(bool val)
    {
        checkMark.SetActive(val);
        questionMark.SetActive(!val);
        transform.parent.root.GetComponent<LevelMapController>().PlayTickSound();
    }
    public void ToggleAnimation(bool val)
    {
        _animation.enabled = val;
    }

    public void ChangePlatformColor(Material mat)
    {
        platformMeshRenderer.material = mat;
    }
}
