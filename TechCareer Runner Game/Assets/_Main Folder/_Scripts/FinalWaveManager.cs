using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalWaveManager : MonoBehaviour
{
    bool spawned = false;
    GameObject player;
    [SerializeField] GameObject Enemy;

    public int killed_enemy = 0, enemy_amount = 12;
    public static FinalWaveManager instance = new FinalWaveManager();

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);

        else
            instance = this;
    }

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        if (GameManager.instance.g_state == GameManager.GameState.Finished && !spawned)
        {
            StartCoroutine(EnemySpawner(enemy_amount));
            spawned = true;
        }

        else if (killed_enemy == enemy_amount && GameManager.instance.g_state != GameManager.GameState.Win)
        {
            GameManager.instance.g_state = GameManager.GameState.Win;
            player.transform.GetChild(0).GetComponent<Animator>().SetBool("Win", true);
            player.GetComponent<PlayerMovement>().rifle.GetComponent<Gun>().canFire = false;
            player.GetComponent<PlayerMovement>().swipeSpeed = 0f;
            player.transform.GetChild(1).position += new Vector3(0, 0, -4);
            Camera.main.GetComponent<CameraMoveFollow>().ChangeFollowObject(player);
            Camera.main.GetComponent<CameraMoveFollow>().SetFollowOffset(new Vector3(0, 7, -7));
        }
    }


    IEnumerator EnemySpawner(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            float rand_x_val = Random.Range(-10f, 10f);
            Vector3 spawn_pos = new Vector3(rand_x_val, 2f, 550);
            GameObject _obj = Instantiate(Enemy, spawn_pos, Quaternion.identity);
            yield return StartCoroutine(Wait(2.5f));
        }
    }

    IEnumerator Wait(float _sec)
    {
        yield return new WaitForSeconds(_sec);
    }
}
