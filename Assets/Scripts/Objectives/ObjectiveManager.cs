using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    List<Objective> objectives = new List<Objective>();

    [SerializeField] Transform objectiveListContent;
    [SerializeField] GameObject objectivePrefab;

    [Header("Objectives")]
    [SerializeField] GameObject obj_dropWood, obj_craftStick, obj_gatherWood;

    private void Start()
    {
        Entity.OnEntityDropItem += EntityDroppedItem;
    }

    public void AddObjective(Objective _objective)
    {
        objectives.Add(_objective);
        GameObject newObjectivePrefab = Instantiate(objectivePrefab, objectiveListContent);
        newObjectivePrefab.GetComponent<TextMeshProUGUI>().text = _objective.description;
    }

    void CompleteObjective(GameObject _objective)
    {
        Destroy(_objective);
    }

    private void EntityDroppedItem(Item item)
    {
        if (item.name == "Wood")
            CompleteObjective(obj_dropWood);
    }

}
