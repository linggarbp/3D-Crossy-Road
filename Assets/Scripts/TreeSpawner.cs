using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    [SerializeField] GameObject treePrefab;
    [SerializeField] GameObject treePrefab2;
    [SerializeField] GameObject treePrefab3;
    [SerializeField] TerrainBlock terrain;
    [SerializeField] int count = 3;

    List<GameObject> treePrefabList = new List<GameObject>();

    private void Start()
    {
        treePrefabList.Add(treePrefab);
        treePrefabList.Add(treePrefab2);
        treePrefabList.Add(treePrefab3);

        int prefabIndex = Random.Range(0, 3);

        List<Vector3> emptyPos = new List<Vector3>();
        for (int x = -terrain.Extent; x <= terrain.Extent; x++)
        {
            if (transform.position.z == 0 && x == 0)
                continue;

            emptyPos.Add(transform.position + Vector3.right * x);
        }
        for (int i = 0; i < count; i++)
        {
            var index = Random.Range(0, emptyPos.Count);
            var spawnPos = emptyPos[index];
            Instantiate(
                treePrefabList[prefabIndex],
                spawnPos,
                Quaternion.identity,
                this.transform);
            emptyPos.RemoveAt(index);
        }
        Instantiate(
            treePrefabList[prefabIndex],
            transform.position + Vector3.right * -(terrain.Extent + 1),
            Quaternion.identity,
            this.transform);
        Instantiate(
            treePrefabList[prefabIndex],
            transform.position + Vector3.right * (terrain.Extent + 1),
            Quaternion.identity,
            this.transform);
    }
}
