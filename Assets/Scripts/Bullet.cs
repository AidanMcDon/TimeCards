using UnityEngine;

[CreateAssetMenu(fileName = "Bullet", menuName = "Scriptable Objects/Bullet")]
public class Bullet : ScriptableObject
{
    public float speed = 5;
    public float damage = 5;
    public float size = 1;
    public Color bulletColor = Color.white;
}
