using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveFollow : MonoBehaviour
{
    [SerializeField] int lerpSpeed;
    [SerializeField] Transform followTarget;
    [SerializeField] Vector3 offset;
    private void Start()
    {
        
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(0, followTarget.position.y, followTarget.position.z) + offset, lerpSpeed * Time.deltaTime);
    }

    public void ChangeFollowObject(GameObject _obj){ followTarget = _obj.transform; }
    public void SetFollowOffset(Vector3 _vec) { offset = _vec; }
}
