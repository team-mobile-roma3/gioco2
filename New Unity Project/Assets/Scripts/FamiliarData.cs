
using UnityEngine;
[CreateAssetMenu(fileName = "Familiar.asset", menuName ="Familiar/FamiliarObject")]
public class FamiliarData : ScriptableObject
{
    public string familiarType;
    public float speed;
    public float fireDelay;
    public GameObject bulletPrefab;
}
