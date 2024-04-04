using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    [SerializeField] List<Objective> objectives = new List<Objective>();
    List<Objective> completedObjectives = new List<Objective>();
    [SerializeField] Objective startingObjective;

    [SerializeField] Transform objectiveListContent;
    [SerializeField] GameObject objectivePrefab;

    [SerializeField] private float speed;

    public static Action<Objective> CompletedObjective;

    private int woodCounter;

    private void Start()
    {
        Entity.OnEntityDropItem += EntityDroppedItem;
        CraftMenuManager.ItemCrafted += ItemCrafted;
    }

    public void InitiateStartingObjective()
    {
        AddObjective(startingObjective);
    }

    public void AddObjective(Objective _objective)
    {
        if (completedObjectives.Contains(_objective)) return;
        else completedObjectives.Add(_objective);

        Objective objective = Objective.Instantiate(_objective);
        objectives.Add(objective);
        GameObject newObjectivePrefab = Instantiate(objectivePrefab, objectiveListContent);
        newObjectivePrefab.transform.localScale = Vector3.zero;
        StartCoroutine(CreateObjectiveObject(newObjectivePrefab));
        newObjectivePrefab.GetComponentInChildren<TextMeshProUGUI>().text = _objective.description;

        objective.gameObject = newObjectivePrefab;
    }

    void CompleteObjective(GameObject _gameobject, Objective _objective)
    {
        StartCoroutine(RemoveObjectiveObject(_gameobject));
        objectives.Remove(_objective);


        foreach(Objective nextObjective in _objective.nextObjective)
        {
            AddObjective(nextObjective);
        }
        CompletedObjective?.Invoke(_objective);
        Destroy(_objective);

    }

    private void EntityDroppedItem(Item item)
    {
        foreach(Objective _objective in objectives.ToList())
        {
            if(_objective.type == Objective.ObjectiveType.EntityDroppedItem && item.itemID == _objective.item.itemID)
            {
                _objective.amountNeeded--;
                if(_objective.amountNeeded <= 0) CompleteObjective(_objective.gameObject, _objective);
            }
        }
    }

    private void ItemCrafted(Item item)
    {
        foreach (Objective _objective in objectives.ToList())
        {
            if (_objective.type == Objective.ObjectiveType.CraftItem && item.itemID == _objective.item.itemID)
            {
                _objective.amountNeeded--;
                if (_objective.amountNeeded <= 0) CompleteObjective(_objective.gameObject, _objective);
            }
        }
    }

    IEnumerator RemoveObjectiveObject(GameObject _objective)
    {
        _objective.transform.localScale -= Vector3.one * Time.deltaTime * speed;
        yield return new WaitForFixedUpdate();

        if (_objective.transform.localScale.y > 0f)
            StartCoroutine(RemoveObjectiveObject(_objective));
        else
            Destroy(_objective);
    }

    IEnumerator CreateObjectiveObject(GameObject _objective)
    {
        _objective.transform.localScale += Vector3.one * Time.deltaTime * speed;
        yield return new WaitForFixedUpdate();

        if (_objective.transform.localScale.y >= 1f)
            _objective.transform.localScale = Vector3.one;
        else
            StartCoroutine(CreateObjectiveObject(_objective));
    }

}
