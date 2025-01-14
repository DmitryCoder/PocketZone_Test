//тестовое задание. Дмитрий Малахаев, nazgul7@yandex.ru
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class InventoryManager : MonoBehaviour
{
    public Canvas mainCanvas;
    public GameObject prefabRemoveConfirmation;
    public GameObject prefabDropElement;
    public GameObject prefabInventoryElement;
    public GameObject inventoryRoot;
    public Dictionary<string, InventoryElement> activeItems = new Dictionary<string, InventoryElement>();
    public static InventoryManager instance;
    [NonSerialized] public string itemToDelete;
    public string currentAmmoId;
    public Dictionary<string, DropElement> availableDrop = new Dictionary<string, DropElement>();
    private void Start()
    {
        inventoryRoot = this.gameObject;
        if (instance == null) instance = this;
        AvailableDrop_LoadConfig();
        
    }

    private void Update()
    {
        if(DataManager.instance.loadInventory)
        {
            if (DataManager.instance.respawnInventory)
            {
                DataManager.instance.respawnInventory = false;
                ReSpawnInventory();
            }
            else
            {
                InventoryValues_Init();
            }
            InventoryValues_RefreshUI();
            DataManager.instance.loadInventory = false;
        }
    }

    private void ReSpawnInventory()
    {
        activeItems.Clear();
        ClearChilds(inventoryRoot);
        foreach (string itemId in DataManager.session.inventory.Keys.ToList())
        {
            Item_PickUp(itemId, DataManager.session.inventory[itemId]);
        }
        InventoryValues_RefreshUI();
    }

    private void InventoryValues_Init()
    {
        //для теста берем с готового набора со сцены
        foreach (Transform child in this.transform)
        {
            string itemId = child.name.Split("=")[1];
            activeItems.Add(itemId, new InventoryElement()
            {
                image = child.GetChild(0).GetComponent<Image>().sprite,
                elementObject = child.gameObject,
                amount = Int32.Parse(child.GetChild(1).GetComponent<Text>().text)
            });
            DataManager.instance.UpdateItemInInventory(itemId, activeItems[itemId].amount);
        }
    }

    private void AvailableDrop_LoadConfig()
    {
        availableDrop.Add("Hfkd", new DropElement() { title = "AK-47", amount = 1 });
        availableDrop.Add("Lfjg", new DropElement() { title = "Ammo 5.45x39", amount = 10 });
        availableDrop.Add("Lfmn", new DropElement() { title = "PM", amount = 1 });
        availableDrop.Add("GBfd", new DropElement() { title = "Sidor", amount = 1 });
        availableDrop.Add("Gdfkf", new DropElement() { title = "Item 1", amount = 1 });
        availableDrop.Add("HdlK", new DropElement() { title = "Item 2", amount = 1 });
        availableDrop.Add("IdmH", new DropElement() { title = "Helmet", amount = 1 });
        availableDrop.Add("JNmJm", new DropElement() { title = "Item 3", amount = 1 });
        availableDrop.Add("Ydmc", new DropElement() { title = "Item 4", amount = 1 });
        
        foreach (Sprite sprite in Resources.LoadAll("items", typeof(Sprite)).Cast<Sprite>().ToArray())
        {
            if (availableDrop.ContainsKey(sprite.name))
            {
                availableDrop[sprite.name].image = sprite;
            }
        }
    }
    public  void ClearChilds(GameObject victim)
    {
        if (victim == null) return;
        for (int x=victim.transform.childCount-1; x>=0; x--)
        {
            DestroyImmediate(victim.transform.GetChild(x).gameObject);
        }
    }
    public void InventoryValues_RefreshUI()
    {
        foreach (string elId in activeItems.Keys)
        {
            if (activeItems[elId].amount > 1)
            {
                activeItems[elId].elementObject.transform.GetChild(1).GetComponent<Text>().text =
                    activeItems[elId].amount.ToString();
            }
            else
            {
                activeItems[elId].elementObject.transform.GetChild(1).GetComponent<Text>().text = "";
            }
        }
    }

    public bool CurrentAmmoFired(int shots)
    {
        if (currentAmmoId == "") return false; //нет установлены патроны
        if (!activeItems.ContainsKey(currentAmmoId)) return false; //по какой-то причине нет в справочнике
        if (activeItems[currentAmmoId].amount == 0) return false; //нет патронов
        
        activeItems[currentAmmoId].amount = Math.Max(activeItems[currentAmmoId].amount-1, 0);
        DataManager.instance.UpdateItemInInventory(currentAmmoId, activeItems[currentAmmoId].amount);
        if (activeItems[currentAmmoId].amount <= 0)
        {
            //патроны кончились, удаляем из карманов
            itemToDelete = currentAmmoId;
            currentAmmoId = "";
            ItemRemove_Remove();
        }

        return true;
    }

    public void SpawnDrop(Vector3 coords)
    {
        GameObject drop = Instantiate(prefabDropElement, coords, Quaternion.identity);
        UnityEngine.Random.InitState(seed: DateTime.UtcNow.GetHashCode());
        Random rnd = new Random();
        int randomDropNum = rnd.Next(0, availableDrop.Count);
        string itemId = availableDrop.Keys.ToList()[randomDropNum];
        DropElement element = availableDrop[itemId];
        drop.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite =element.image;
        drop.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = element.title;
        drop.name = itemId;
    }

    public void Item_PickUp(string itemId, int amount)
    {
        if (amount == 0) amount = availableDrop[itemId].amount;
        if (activeItems.ContainsKey(itemId))
        {
            //есть такое в карманах. Увеличиваем количчество
            int newAmount = activeItems[itemId].amount+ amount;
            activeItems[itemId].amount = newAmount;
            DataManager.instance.UpdateItemInInventory(itemId, newAmount);
        }
        else
        {
            //еще нет такого. Добавляем
            GameObject element = Instantiate(prefabInventoryElement, inventoryRoot.transform);
            element.name = "InventoryElement=" + itemId;
            Image img = element.transform.GetChild(0).GetComponent<Image>();
            img.sprite = availableDrop[itemId].image;
            img.SetNativeSize();
            int newAmount = amount;
            element.transform.GetChild(1).GetComponent<Text>().text =newAmount.ToString();
            
            activeItems.Add(itemId, new InventoryElement()
            {
                elementObject = element,
                amount = newAmount,
                image=availableDrop[itemId].image
                
            });
            DataManager.instance.UpdateItemInInventory(itemId, newAmount);
        }

        InventoryValues_RefreshUI();
    }

    public void ItemRemove_ShowConfirmationWindow(string itemId)
    {
        GameObject window = Instantiate(prefabRemoveConfirmation, mainCanvas.transform);
        window.transform.GetChild(0).GetComponent<Image>().sprite = activeItems[itemId].image;
        itemToDelete = itemId;
    }

    public void ItemRemove_Remove()
    {
        if (itemToDelete == currentAmmoId) currentAmmoId = ""; //если удаляются текущие патроны
        Destroy(activeItems[itemToDelete].elementObject);
        activeItems.Remove(itemToDelete);
        DataManager.instance.RemoveItemFromInventory(itemToDelete);
    }

    public class InventoryElement
    {
        public Sprite image { get; set; }
        public GameObject elementObject { get; set; }
        public int amount { get; set; }
    }

    public class DropElement
    {
        public Sprite image { get; set; }
        public string title { get; set; }
        public int amount { get; set; }
    }
}
