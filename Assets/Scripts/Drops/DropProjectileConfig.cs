using UnityEngine;

[CreateAssetMenu(fileName = "DropProjectileConfig", menuName = "Scriptable Objects/DropProjectileConfig")]
public class DropProjectileConfig : ScriptableObject
{
    [field: SerializeField] public GameObject[] dropProjectiles { get; private set; }
}
