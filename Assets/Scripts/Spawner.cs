using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Spawner : MonoBehaviour
{
    public List<GameObject> prefab;
    
    public List<GameObject> spawnerMovePoints;

    public float spawnTime = 1;

    private int counter;

    public float difficultyMultiplier = 1;

    private void Start()
    {
        StartCoroutine(Spawn(spawnTime));
        counter = 0;
    }

    private float difficulty = 100;
    private void Update()
    {
        if (spawnerMovePoints.Count != 0)
        {
            MoveSpawnerToPoints(100);
        }

        if (CompareTag("EnemySpawner"))
        {
            difficultyMultiplier += Time.deltaTime/difficulty;
        }
    }

    private void MoveSpawnerToPoints(float speed)
    {
        transform.position = Vector2.MoveTowards(transform.position, spawnerMovePoints[counter].transform.position, speed * Time.deltaTime);
        if (transform.position != spawnerMovePoints[counter].transform.position)
            return;
        counter++;
        if (counter == spawnerMovePoints.Count)
        {
            counter = 0;
        }
    }

    private IEnumerator ScaleUp(GameObject spawned)
    {
        yield return new WaitForSeconds(0.1f);
        spawned.transform.localScale = new Vector2(2f, 2f);
    }

    private IEnumerator Spawn(float time)
    {
        UnityEngine.Random.Range(0, prefab.Count);
        while (true)
        {
            yield return new WaitForSeconds(time / difficultyMultiplier);
            //Debug.Log(time / difficultyMultiplier);
            GameObject spawned = Instantiate(prefab[UnityEngine.Random.Range(0, prefab.Count)], transform.position, Quaternion.identity);
            //Debug.Log(spawned);

            if (spawned.CompareTag("Ball"))
            {
                spawned.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(50, 0);
                StartCoroutine(ScaleUp(spawned));
            }
        }
    }
}
