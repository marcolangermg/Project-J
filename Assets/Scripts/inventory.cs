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
            if (equippedItem.item != null)
            {
                var attackPropertiesList = equippedItem.item.gameObject.GetComponents<AttackProperties>();

                if (attackPropertiesList != null && attackPropertiesList.Length > 0)
                {
                    foreach (var attackProperties in attackPropertiesList)
                    {
                        itemsOfType.Add(attackProperties);
                    }
                }
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
            // Se for uma arma de uma mão, verifica se há uma arma de duas mãos equipada e a remove
            if (newItem.itemType == itemType.ArmaUmaMao)
            {
                RemoveItemsOfType(itemType.ArmaDuasMaos);
            }
            else if (newItem.itemType == itemType.ArmaDuasMaos)
            {
                // Remover armas de uma mão antes de adicionar a arma de duas mãos
                RemoveItemsOfType(itemType.ArmaUmaMao);
                equippedItemsList.Add(new EquippedItem { type = newItem.itemType, item = newItem });
            }

            equippedItemsList.Add(new EquippedItem { type = newItem.itemType, item = newItem });
        }
        else if (newItem.itemType == itemType.ArmaUmaMao && CountItemsOfTypeArmaUmaMao() < 2)
        {
            equippedItemsList.Add(new EquippedItem { type = newItem.itemType, item = newItem });
        }
        // Verificar se é uma arma de duas mãos
        else
        {
            // Trocar o item se já houver um equipado
            item oldItem = existingItem.item;
            existingItem.item = newItem;
            RemoveItem(oldItem);
            oldItem.onGround = true;
            oldItem.timeToChange = 2.0f;
        }
    }

    public void RemoveItem(item item)
    {
        var itemToRemove = equippedItemsList.Find(equippedItem => equippedItem.item == item);
        if (itemToRemove != null)
        {
            equippedItemsList.Remove(itemToRemove);
            itemToRemove.item.onGround = true;
            itemToRemove.item.timeToChange = 2.0f;
        }
    }

    public void RemoveItemsOfType(itemType type)
    {
        var itemsToRemove = equippedItemsList.FindAll(equippedItem => equippedItem.type == type);

        foreach (var itemToRemove in itemsToRemove)
        {
            equippedItemsList.Remove(itemToRemove);
            itemToRemove.item.onGround = true;
            itemToRemove.item.timeToChange = 2.0f;
        }
    }

    private bool IsArmourType(itemType type)
    {
        return type == itemType.Peitoral || type == itemType.Capacete || type == itemType.Botas ||
               type == itemType.Luvas || type == itemType.Bracadeiras || type == itemType.Perneiras;
    }

    private int CountItemsOfTypeArmaUmaMao()
    {
        int count = 0;

        foreach (var equippedItem in equippedItemsList)
        {
            if (equippedItem.type == itemType.ArmaUmaMao)
            {
                count++;
            }
        }

        return count;
    }
}
