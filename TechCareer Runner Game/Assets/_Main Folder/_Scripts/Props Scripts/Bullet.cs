using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    [SerializeField] float bullet_speed;

    private void Start()
    {
        StartCoroutine(Destroy_After_Seconds(1.75f));
    }

    IEnumerator Destroy_After_Seconds(float sec)
    {
        yield return new WaitForSeconds(sec);

        Destroy(this.gameObject);
    }

    private void Update()
    {
        transform.position += new Vector3(0, 0, bullet_speed * Time.deltaTime);
    }
}
