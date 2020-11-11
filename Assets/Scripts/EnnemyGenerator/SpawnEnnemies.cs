using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnnemies : MonoBehaviour
{
    Score score;

    [Header("Spawner")]
    public bool canSpawn;
    [SerializeField] List<int> SpawnRates;
    [SerializeField] List<bool> Waves;
    [SerializeField] List<GameObject> ennemiesToSpawn;
    bool isSpawning;

    [Header("Ennemies control")]
    public Ennemy[] currentEnnemies;
    [SerializeField] int createdEnnemies;    
    public List<SpawnSlot> slotList;

    private void Awake()
    {
        slotList.AddRange(GetComponentsInChildren<SpawnSlot>());
        score = FindObjectOfType<Score>();
        Waves[0] = true;
    }

    public void Start()
    {
        StartCoroutine(Spawning());
    }

    IEnumerator Spawning()
    {
        while(true)
        {
            if (!isSpawning)
            {
                if (Waves[0]) 
                {
                    StartCoroutine(SpawnEnnemy(SpawnRates[0], 0));
                }
                else if (Waves[1]) 
                {
                    int newRandom01 = Random.Range(0, ennemiesToSpawn.Count - 1);
                    StartCoroutine(SpawnEnnemy(SpawnRates[1], newRandom01));                    
                }
                else if (Waves[2])
                {
                    int newRandom02 = Random.Range(1, ennemiesToSpawn.Count);
                    StartCoroutine(SpawnEnnemy(SpawnRates[2], newRandom02));
                }
                else
                {
                    if (!Waves[3])
                    {
                        canSpawn = false;
                        if (currentEnnemies.Length == 0)
                        {                            
                            Waves[3] = true;
                        }
                    }
                    else
                    {
                        canSpawn = true;
                        StartCoroutine(SpawnEnnemy(SpawnRates[3], 2));
                    }
                }
            }
            yield return null;
        }
    }

    IEnumerator SpawnEnnemy(float duration, int ennemyIndex)
    {
        isSpawning = true;
        yield return new WaitForSeconds(duration);
        if (canSpawn)
        {
            int random = Random.Range(0, slotList.Count);
            SpawnSlot pickedSlot = slotList[random];

            if (!pickedSlot.isOccupied && canSpawn)
            {
                GameObject mob = Instantiate(ennemiesToSpawn[ennemyIndex], transform.position, Quaternion.identity);
                Ennemy mobScript = mob.GetComponent<Ennemy>();

                pickedSlot.isOccupied = true;
                createdEnnemies++;
                mobScript.mySlot = pickedSlot;
                mobScript.StartCoroutine(mobScript.ReachPosition(pickedSlot.transform));
            }
        }
        isSpawning = false;
    }

    void CheckSpawn()
    {
        currentEnnemies = FindObjectsOfType<Ennemy>();
        Waves[0] = score.CurrentScore > score.maxScore - 5;
        Waves[1] = score.CurrentScore > 15 && score.CurrentScore <= score.maxScore - 5;
        Waves[2] = score.CurrentScore <= 15 && score.CurrentScore > 8;

        if (slotList.Count == 0 || createdEnnemies >= score.maxScore || currentEnnemies.Length == 5)
        {
            canSpawn = false;
        }
        else if (Waves[0] || Waves[1] || Waves[2])
        {
            canSpawn = true;
        }
    }

    private void Update()
    {
        CheckSpawn();
    }
}
