using UnityEngine;
[CreateAssetMenu(menuName = "DropConfig", fileName = "DropConfig")]
public class DropConfig : ScriptableObject
{
    [field:SerializeField] public GameObject[] Drops { get; private set; }
}
