using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemDrop
{
    public item item;
    [Range(0f, 1f)] public float dropChance;
    public int minAmount;
    public int maxAmount;
    public List<item> excludedItems; // Lista de itens excluídos para este ItemDrop
}

public class dropItems : MonoBehaviour
{
    public List<ItemDrop> itemsToDrop = new List<ItemDrop>();
    public bool allowMultipleDrops;

    public void Drop()
    {
        if (allowMultipleDrops)
        {
            DropMultipleItems();
        }
        else
        {
            DropSingleItem();
        }
    }

    private void DropSingleItem()
    {
        ItemDrop selectedItem = GetItemWithMinDropChance(itemsToDrop);
        if (selectedItem != null)
        {
            int amountToDrop = Random.Range(selectedItem.minAmount, selectedItem.maxAmount + 1);

            for (int i = 0; i < amountToDrop; i++)
            {
                Instantiate(selectedItem.item, transform.position, Quaternion.identity);
            }
        }
    }

    private void DropMultipleItems()
    {
        List<ItemDrop> availableItems = new List<ItemDrop>(itemsToDrop);

        while (availableItems.Count > 0)
        {
            ItemDrop selectedDrop = GetItemWithMinDropChance(availableItems);

            if (selectedDrop != null)
            {
                int amountToDrop = Random.Range(selectedDrop.minAmount, selectedDrop.maxAmount + 1);

                for (int i = 0; i < amountToDrop; i++)
                {
                    Instantiate(selectedDrop.item, transform.position, Quaternion.identity);
                }

                // Remove os itens excluídos do sorteio
                availableItems.RemoveAll(item => selectedDrop.excludedItems.Contains(item.item));
                availableItems.Remove(selectedDrop);
            }
            else
            {
                // Todos os itens restantes têm uma chance de drop 0, então saímos do loop
                break;
            }
        }
    }

    private ItemDrop GetItemWithMinDropChance(List<ItemDrop> itemList)
    {
        float lowestDropChance = 1f;
        ItemDrop selectedItem = null;

        foreach (ItemDrop itemDrop in itemList)
        {
            if (itemDrop.dropChance < lowestDropChance && Random.value <= itemDrop.dropChance)
            {
                lowestDropChance = itemDrop.dropChance;
                selectedItem = itemDrop;
            }
        }

        return selectedItem;
    }
}
