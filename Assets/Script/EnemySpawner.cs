using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// this class is used to generate enemies with given datas in rounds
public class EnemySpawner : MonoBehaviour
{
    public Round[] Rounds;
    public Transform start;
    public float roundRate = 3;
    public static int enemyAliveCount = 0;
    public GameObject winWindow;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(5);
        PlayerDataController.PlayerData.addWave();
        int index = 0;
        foreach (Round round in Rounds)
        {
            index++;
            for (int i =0; i < round.count; i++)
            {
                GameObject.Instantiate(round.enemyPrefab, start.position, Quaternion.identity);
                enemyAliveCount++;
                if (i != round.count - 1)
                {
                    yield return new WaitForSeconds(round.timeStep);
                }
            }
            while (enemyAliveCount > 0)
            {
                yield return 0;
            }
            if (index< Rounds.Length)
            {
                yield return new WaitForSeconds(roundRate);
            }
            PlayerDataController.PlayerData.addWave();
        }
        while (enemyAliveCount > 0)
        {
            yield return 0;
        }
        // if reach here, Game Win
        winWindow.SetActive(true);
    }
}
