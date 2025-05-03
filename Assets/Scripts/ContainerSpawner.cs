using Unity.VisualScripting;
using UnityEngine;

public class ContainerSpawner : MonoBehaviour
{
    [SerializeField] GameObject containerPrefab;
    public Transform spawner;
    private void Start()
    {
        SpawnContainer();
    }

    private void SpawnContainer()
    {
        GameObject Container = Instantiate(containerPrefab, transform.position, Quaternion.identity);
        Container.transform.SetParent(transform);
        BulletContainer bulletContainer = Container.GetComponent<BulletContainer>();
        bulletContainer.spawner = spawner;
        Destroy(this);
    }
}
