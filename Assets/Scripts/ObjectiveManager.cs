using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Yarn;
using Yarn.Unity;

public class ObjectiveManager : MonoBehaviour
{
    [SerializeField]public static List<Objective> objectives = new List<Objective>();

    [SerializeField] Transform objectiveListContent;
    [SerializeField] GameObject objectivePrefab;

    [SerializeField] private float speed;

    public static Action<Objective> NewObjective;
    public static Action<Objective> CompletedObjective;
    [SerializeField] DialogueRunner dialogueRunner;

    private int woodCounter;

    private void Start()
    {
        Entity.OnEntityDropItem += EntityDroppedItem;
        CraftMenuManager.ItemCrafted += ItemCrafted;
        dialogueRunner.onDialogueComplete.AddListener(delegate { DialogueComplete(); });
        SaveLoadJSON.worldLoaded += LoadObjectives;
    }

    private void LoadObjectives(WorldData worldData)
    {
        objectives.Clear();

        foreach(Objective objective in Resources.LoadAll<Objective>("ScriptableObjects/Objectives/"))
        {
            if (worldData.objectives.Contains(objective.name))
                objectives.Add(objective);
        }

        ResetObjectivesList();
    }

    private void ResetObjectivesList()
    {
        foreach(Transform child in objectiveListContent)
        {
            if(child.name != "Title")
                Destroy(child.gameObject);
        }

        foreach(Objective _objective in objectives)
        {
            CreateObjectiveObject(_objective);

            if (_objective.type == Objective.ObjectiveType.CompleteDialogue)
                dialogueRunner.StartDialogue("Tutorial1");
        }
    }

    private void CreateObjectiveObject(Objective _objective)
    {
        GameObject newObjectivePrefab = Instantiate(objectivePrefab, objectiveListContent);
        newObjectivePrefab.transform.localScale = Vector3.zero;
        StartCoroutine(CreateObjectiveObject(newObjectivePrefab));
        newObjectivePrefab.GetComponentInChildren<TextMeshProUGUI>().text = _objective.description;

        _objective.gameObject = newObjectivePrefab;
    }

    public void AddObjective(Objective _objective)
    {
        if (objectives.Contains(_objective)) return;

        Objective objective = Objective.Instantiate(_objective);
        objectives.Add(objective);

        CreateObjectiveObject(objective);

        NewObjective?.Invoke(_objective);

        if (_objective.type == Objective.ObjectiveType.CompleteDialogue)
            dialogueRunner.StartDialogue("Tutorial1");
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

    private void EntityDroppedItem(ItemData item)
    {
        foreach(Objective _objective in objectives.ToList())
        {
            if(_objective.type == Objective.ObjectiveType.EntityDroppedItem && item.name == _objective.item.data.name)
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
            if (_objective.type == Objective.ObjectiveType.CraftItem && item.data.name == _objective.item.data.name)
            {
                _objective.amountNeeded--;
                if (_objective.amountNeeded <= 0) CompleteObjective(_objective.gameObject, _objective);
            }
        }
    }

    private void DialogueComplete()
    {
        foreach (Objective _objective in objectives.ToList())
        {
            if (_objective.type == Objective.ObjectiveType.CompleteDialogue)
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
