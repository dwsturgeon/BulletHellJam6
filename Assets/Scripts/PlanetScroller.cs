using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlanetScroller : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 0.25f;
    [SerializeField] private float buffer = 0f;
    [SerializeField] private float minY = -10f;
    private float resetY = 10f;


    [Header("Spawn Area")]
    [SerializeField] private float minX = -8f;
    [SerializeField] private float maxX = 8f;
    [SerializeField] private float verticalSpacing = 3f;
    [SerializeField] private float horizontalSpacing = 3f;
    [SerializeField] private float maxOffset;
    [SerializeField] private int numVerticalRows = 3;
    


    [Header("Prefabs")]
    [SerializeField] private GameObject[] Prefabs;
    [SerializeField] private int maxPrefabs = 3;

    private List<Vector2> spawnSlots = new List<Vector2>();
    private Queue<Vector2> availableSlots = new Queue<Vector2>();
    private List<GameObject> activePrefabs = new List<GameObject>();
    private Queue<GameObject> shuffledPrefabQueue = new Queue<GameObject>();

    private void Start()
    {
        GenerateSpawnSlots();
        ReshufflePlanetPrefabs();
        SpawnInitialPrefabs();
        resetY = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y + buffer;
    }


    private void Update()
    {
        for(int i = 0; i < activePrefabs.Count; i++)
        {
            if(activePrefabs[i] != null)
            {
                activePrefabs[i].transform.Translate(Vector2.down * scrollSpeed * Time.deltaTime);

                if (activePrefabs[i].transform.position.y <= minY)
                {
                    ResetPrefab(activePrefabs[i]);
                }
            }
        }
    }

    private void GenerateSpawnSlots()
    {
        spawnSlots.Clear();
        availableSlots.Clear();

        for (float x = minX; x <= maxX; x += horizontalSpacing)
        {
            for (float y = resetY; y <= resetY + verticalSpacing * numVerticalRows; y += verticalSpacing)
            {
                spawnSlots.Add(new Vector2(x, y));
            }
        }

        Shuffle(spawnSlots);
        foreach (var slot in spawnSlots) availableSlots.Enqueue(slot);           
    }

    private void SpawnInitialPrefabs()
    {
        for(int i = 0; i < maxPrefabs && availableSlots.Count > 0; i++)
        {
            Vector2 baseSlot = availableSlots.Dequeue();
            Vector2 offsetPos = GetOffsetPosition(baseSlot);
            GameObject _prefab = InstantiateRandomPrefab(offsetPos);
            _prefab.AddComponent<PrefabSlotData>().slot = baseSlot;
            activePrefabs.Add(_prefab);
        }

    }

    GameObject InstantiateRandomPrefab(Vector2 position)
    {
        if (shuffledPrefabQueue.Count == 0) ReshufflePlanetPrefabs();


        GameObject prefab = shuffledPrefabQueue.Dequeue();
        GameObject targetPrefab = Instantiate(prefab, position, Quaternion.identity, transform);
        targetPrefab.transform.localScale = Vector3.one * Random.Range(0.5f, 1.5f);
        return targetPrefab;
    }

    void ReshufflePlanetPrefabs()
    {
        List<GameObject> tempList = new List<GameObject>(Prefabs);
        Shuffle(tempList);
        foreach (GameObject p in tempList) shuffledPrefabQueue.Enqueue(p);
    }
    

    void ResetPrefab(GameObject _prefab)
    {
        PrefabSlotData data = _prefab.GetComponent<PrefabSlotData>();
        if (data != null)
        {
            availableSlots.Enqueue(data.slot);
        }

        if (availableSlots.Count == 0) return;
        
        Vector2 newSlot = availableSlots.Dequeue();
        _prefab.transform.position = GetOffsetPosition(newSlot);
        _prefab.transform.localScale = Vector3.one * Random.Range(0.5f, 1.5f);

        if (data == null)
        {
            data = _prefab.AddComponent<PrefabSlotData>();
        }
        data.slot = newSlot;

    }

    Vector2 GetOffsetPosition(Vector2 baseSlot)
    {
        float offsetX = Random.Range(-maxOffset, maxOffset);
        float offsetY = Random.Range(-maxOffset, maxOffset);
        return new Vector2(baseSlot.x + offsetX, baseSlot.y + offsetY);
    }

    void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    private class PrefabSlotData : MonoBehaviour
    {
        public Vector2 slot;
    }

}
