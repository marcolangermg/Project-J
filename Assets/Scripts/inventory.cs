using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory : MonoBehaviour
{
    [System.Serializable]
    public class EquippedItem
    {
        public itemType type;
        public item item;
    }

    [SerializeField]
    private List<EquippedItem> equippedItemsList = new List<EquippedItem>();

    // Usado apenas para visualização no Editor
    public EquippedItem[] EquippedItemsArray => equippedItemsList.ToArray();

    public void Update()
    {
        List<AttackProperties> itemsOfType = new List<AttackProperties>();

        float totalfireResistance = 0.0f;
        float totalwaterResistance = 0.0f;
        float totalearthResistance = 0.0f;
        float totalairResistance = 0.0f;
        float totalenergyResistance = 0.0f;
        float totalphysicalResistance = 0.0f;

        foreach (var equippedItem in equippedItemsList)
        {
            if (equippedItem.item != null && equippedItem.item.gameObject.GetComponent<AttackProperties>() != null)
            {
                itemsOfType.Add(equippedItem.item.gameObject.GetComponent<AttackProperties>());
            }
            totalfireResistance += equippedItem.item.fireResistance;
            totalwaterResistance += equippedItem.item.waterResistance;
            totalearthResistance += equippedItem.item.earthResistance;
            totalairResistance += equippedItem.item.airResistance;
            totalenergyResistance += equippedItem.item.energyResistance;
            totalphysicalResistance += equippedItem.item.physicalResistance;

            this.gameObject.GetComponent<Health>().fireResistance = totalfireResistance;
            this.gameObject.GetComponent<Health>().waterResistance = totalwaterResistance;
            this.gameObject.GetComponent<Health>().earthResistance = totalearthResistance;
            this.gameObject.GetComponent<Health>().airResistance = totalairResistance;
            this.gameObject.GetComponent<Health>().energyResistance = totalenergyResistance;
            this.gameObject.GetComponent<Health>().physicalResistance = totalphysicalResistance;

        }

        GetComponent<PlayerAttack>().attackProperties = itemsOfType.ToArray();
    }

    public void AddItem(item newItem)
    {
        var existingItem = equippedItemsList.Find(equippedItem => equippedItem.type == newItem.itemType);

        if (existingItem == null)
        {
            equippedItemsList.Add(new EquippedItem { type = newItem.itemType, item = newItem });
            Debug.Log(newItem.itemName + " foi equipado.");
        }
        else
        {
            // Trocar o item se já houver um equipado
            item oldItem = existingItem.item;
            existingItem.item = newItem;
            RemoveItem(oldItem);
            Debug.Log(newItem.itemName + " foi equipado, substituindo " + oldItem.itemName + ".");
        }
    }

    public void RemoveItem(item item)
    {
        var itemToRemove = equippedItemsList.Find(equippedItem => equippedItem.item == item);
        if (itemToRemove != null)
        {
            equippedItemsList.Remove(itemToRemove);
            Debug.Log(item.itemName + " foi removido do inventário.");
        }
    }

    private bool IsArmourType(itemType type)
    {
        return type == itemType.Peitoral || type == itemType.Capacete || type == itemType.Botas ||
               type == itemType.Luvas || type == itemType.Bracadeiras || type == itemType.Perneiras;
    }

    private int CountItemsOfType(itemType type)
    {
        return equippedItemsList.Count;
    }
}
