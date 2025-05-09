using UnityEngine;

public class RogueLikeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] lowLevelEnemies;
    [SerializeField] private GameObject[] mediumLevelEnemies;
    [SerializeField] private GameObject[] highLevelEnemies;
    [SerializeField] private GameObject[] bossLevelEnemies;
    private PlayerController player;

    private void Start()
    {
        player = GetComponent<PlayerController>(); // if we want to scale off player stats
    }



}
