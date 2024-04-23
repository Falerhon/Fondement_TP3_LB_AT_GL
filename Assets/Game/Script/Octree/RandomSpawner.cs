using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RandomSpawner : MonoBehaviour
{
    [SerializeField] private GameObject PrefabToSpawn;
    [SerializeField] private float NbToSpawn;
    [SerializeField] private GameObject SpawnZone;

    public UnityEvent<GameObject> OnItemSpawned = new UnityEvent<GameObject>();
    public UnityEvent OnAllItemSpawned = new UnityEvent();
    public UnityEvent<Bounds> OnBoundsCalculated = new UnityEvent<Bounds>();


    private Collider spawnerCollider;

    // Start is called before the first frame update
    void Start()
    {
        spawnerCollider = SpawnZone.GetComponent<Collider>();
        if (spawnerCollider == null) print("BOX COLLIDER NULL");

        OnBoundsCalculated.Invoke(spawnerCollider.bounds);
        SpawnPrefab();
    }

    private void SpawnPrefab()
    {
        Vector3 boxSize = spawnerCollider.bounds.extents;
        for (int i = 0; i < NbToSpawn; i++)
        {
            Vector3 randomSpawnPos = new Vector3(
                    Random.Range(-boxSize.x, boxSize.x),
                    Random.Range(-boxSize.y, boxSize.y),
                    Random.Range(-boxSize.z, boxSize.z));

            GameObject go = Instantiate(PrefabToSpawn, SpawnZone.transform, false);

            go.transform.position = SpawnZone.transform.position + randomSpawnPos;
            go.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            Physics.SyncTransforms();     
            go.name = "Obstacle " + i;

            OnItemSpawned.Invoke(go);
        }
        OnAllItemSpawned.Invoke();
    }
}
