using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName ="NewItem", menuName ="Item")]
public class Item : ScriptableObject
{
    public string description;
    public Sprite icon;
    public GameObject droppedObject;

}
