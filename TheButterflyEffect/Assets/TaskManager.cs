using TMPro;
using UnityEngine;
using System.Linq;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private TextMeshPro[] tasks;
    [SerializeField] private TextMeshPro breedCountText;
    private TextMeshPro[] lines;

    private void Start()
    {
        lines = new TextMeshPro[tasks.Length];
        for (int i = 0; i < tasks.Length; i++)
        {
            lines[i] = tasks[i].transform.GetChild(0).GetComponent<TextMeshPro>();
            lines[i].enabled = false;
        }

        Inventory.Instance().onAddItem += Inventory_OnAddItem;
        FindAnyObjectByType<BreedingSystem>().onBreed += BreedingSystem_OnBreed;
    }

    public void FinishTask(int id)
    {
        if (id < 0 || id >= tasks.Length) { Debug.LogError("Tasks Doesn't Exist!"); return; }
        tasks[id].color = new Color(0, 0, 0, 0.5f);
        lines[id].enabled = true;
    }
    private void Inventory_OnAddItem(InventoryItem item, int index)
    {
        if(item.item.itemType == ItemType.Butterfly)
        {
            FinishTask(0);
            FinishTask(1);
            Inventory.Instance().onAddItem -= Inventory_OnAddItem;
        }
    }

    private void BreedingSystem_OnBreed(int breedCount)
    {
        FinishTask(2);
        if (breedCount >= 5) FinishTask(3);
        breedCountText.text = $"Breeded: {breedCount}/5";
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            FinishTask(0);
            Destroy(GetComponent<SphereCollider>());
        }
    }
}
