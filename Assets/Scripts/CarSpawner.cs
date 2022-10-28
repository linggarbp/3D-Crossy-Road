using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] GameObject animalPrefab;
    [SerializeField] GameObject animalPrefab2;
    [SerializeField] TerrainBlock terrain;
    [SerializeField] float minSpawnDuration = 2;
    [SerializeField] float maxSpawnDuration = 4;

    List<GameObject> animalPrefabList = new List<GameObject>();
    bool isRight;
    float timer;

    private void Start()
    {
        isRight = Random.value > 0.5f ? true : false;
        timer = Random.Range(minSpawnDuration, maxSpawnDuration);

        animalPrefabList.Add(animalPrefab);
        animalPrefabList.Add(animalPrefab2);
    }

    private void Update()
    {
        int prefabIndex = Random.Range(0, 2);

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }

        timer = Random.Range(minSpawnDuration, maxSpawnDuration);

        var spawnPos = this.transform.position +
            Vector3.right * (isRight ? -(terrain.Extent + 1) : terrain.Extent + 1);

        var go = Instantiate(
            original: animalPrefabList[prefabIndex],
            position: spawnPos,
            rotation: Quaternion.Euler(0, isRight ? 90 : -90, 0),
            parent: this.transform);

        var car = go.GetComponent<Car>();
        car.setUp(terrain.Extent);
    }
}
