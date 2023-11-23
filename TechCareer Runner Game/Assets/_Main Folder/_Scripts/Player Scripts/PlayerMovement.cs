using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject rifle;

    private bool isSwiping = false;

    public float swipeSpeed = 5f, forwardSpeed = 5f, lerp_speed;
    float minX, maxX;

    private void Start()
    {
        minX = -2.75f; maxX = 2.75f;
    }

    private void Update()
    {
        switch (GameManager.instance.g_state)
        {
            case GameManager.GameState.Running:
                break;

            case GameManager.GameState.GameOver:
                swipeSpeed = 0f; forwardSpeed = 0f;
                break;

            case GameManager.GameState.Finished:
                forwardSpeed = 0f;
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, 488.5f), lerp_speed * Time.deltaTime);
                break;

            default:
                break;
        }

        ForwardMove();
        HandleSwipeInput();
    }

    private void HandleSwipeInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isSwiping = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isSwiping = false;
        }

        if (isSwiping)
        {
            Vector2 swipeDirection = new Vector2(Input.GetAxis("Mouse X"), 0f);
            swipeDirection.Normalize();

            LR_Move(swipeDirection);
        }
    }

    private void LR_Move(Vector2 direction)
    {
        transform.position += new Vector3(direction.x, 0f, 0f) * Time.deltaTime * swipeSpeed;
        Vector3 newPos = transform.position;
        newPos.x = Mathf.Clamp(transform.position.x, minX, maxX);
        transform.position = newPos;
    }

    private void ForwardMove()
    {
        transform.position += Vector3.forward * forwardSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == 8)
        {
            GameManager.instance.g_state = GameManager.GameState.Finished;
            Camera.main.transform.GetComponent<CameraMoveFollow>().SetFollowOffset(new Vector3(0, 16, -16));
            transform.GetChild(1).position += new Vector3(0, 0, 4f);
            swipeSpeed = 20;
            ChangeMoveClampValues(4.5f);
            transform.GetChild(0).GetComponent<Animator>().SetBool("Finish", true);
            rifle.transform.localPosition = new Vector3(-0.217f, -0.094f, -0.105f);
            rifle.transform.localRotation = Quaternion.Euler(-39f, 62.4f, 94.28f);

            Sequence sequence = DOTween.Sequence();
            
            foreach (GameObject obj in GameManager.instance.GetCoinList())
            {
                sequence.Append(obj.transform.DOMove(new Vector3(0f, 2f, 488.5f), 0.15f));
                sequence.Append(obj.transform.DOScale(0f, 0.2f).OnComplete(() =>
                {
                    ParticleSystem _particle = Instantiate(GameManager.instance.coin_poof_effect, obj.transform.position, Quaternion.identity);
                    _particle.Play();
                    float duration = _particle.main.duration + _particle.main.startLifetime.constant;
                    Destroy(_particle.gameObject, duration);
                    Destroy(obj);
                }));
            }
            sequence.Play();
        }
    }

    public void ChangeMoveClampValues(float _val)
    {
        if (_val < 0)
        {
            minX = _val; maxX = Mathf.Abs(_val);
        }

        else if (_val > 0)
        {
            minX = -_val; maxX = _val;
        }
    }
}
