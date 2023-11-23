using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Blade : MonoBehaviour
{
    MeshCollider coll;

    public enum Mode
    {
        normal,
        LR
    }
    [SerializeField] Mode mode = new Mode();
    [SerializeField] float moveDistance;
    [SerializeField] float turn_speed;

    private void Awake()
    {
        if (mode == Mode.LR)
        {
            transform.DOMoveX(-moveDistance / 2, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        }
        else if (mode == Mode.normal)
        {

        }
    }

    private void Start()
    {
        coll = transform.GetChild(0).GetComponent<MeshCollider>();
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, 0, turn_speed * Time.deltaTime));
    }

    private void OnCollisionEnter(Collision col)
    {
        switch (col.gameObject.layer)
        {
            case 7:
                GameManager.instance.g_state = GameManager.GameState.GameOver;
                col.transform.GetChild(0).GetComponent<Animator>().SetBool("GameOver", true);
                col.transform.GetComponent<PlayerMovement>().forwardSpeed = 0f;
                col.transform.GetComponent<PlayerMovement>().swipeSpeed = 0f;
                col.transform.GetComponent<PlayerMovement>().rifle.GetComponent<Gun>().SetFireRate(0f);
                col.transform.GetComponent<PlayerMovement>().rifle.SetActive(false);
                col.transform.GetComponent<Rigidbody>().isKinematic = true;
                break;

            default:
                break;
        }
    }
}
