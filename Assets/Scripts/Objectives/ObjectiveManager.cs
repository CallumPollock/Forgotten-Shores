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

    public void AddObjective(Objective _objective)
    {
        objectives.Add(_objective);
        GameObject newObjectivePrefab = Instantiate(objectivePrefab, objectiveListContent);
        newObjectivePrefab.GetComponent<TextMeshProUGUI>().text = _objective.description;
    }

    public void CompleteObjective(Objective _objective)
    {
        objectives.Remove(_objective);
    }

    private void Test(Item item)
    {
        Debug.Log("Entity has dropped a " + item.name);
    }

}
