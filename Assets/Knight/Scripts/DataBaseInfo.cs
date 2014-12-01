using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tables;
using Assets.Scripts;

namespace Tables
{
    #region Tables
    public class ItemType
    {
        [PrimaryKeyField(true)]
        public int Id { get; set; }
        public string Type { get; set; }
        public bool CanUse { get; set; }
        public bool CanWear { get; set; }
        public bool CanTake { get; set; }
        public ItemType()
        {
            Id = 0;
            Type = "";
            CanUse = false;
            CanWear = false;
            CanTake = false;
        }
    }

    public class Race
    {
        [PrimaryKeyField(true)]
        public int Id { get; set; }
        public string RaceName { get; set; }
        public Race()
        {
            Id = 0;
            RaceName = "";
        }
    }

    public class Condition
    {
        [PrimaryKeyField(true)]
        public int Id { get; set; }
        public int Level { get; set; }
        [ForeignKeyField(true, "Race", "Id")]
        public int RaceId { get; set; }
        public Condition()
        {
            Id = 0;
            Level = 0;
            RaceId = 0;
        }
    }

    public class Item
    {
        [PrimaryKeyField(true)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float SellPrice { get; set; }
        public float BuyPrice { get; set; }
        public float Stats { get; set; }
        public string Image { get; set; }
        [ForeignKeyField(true, "ItemType", "Id")]
        public int ItemTypeId { get; set; }
        [ForeignKeyField(true, "Condition", "Id")]
        public int ConditionId { get; set; }
        public float SpeedAttack { get; set; }
        public float AttackDistance { get; set; }
        public Item()
        {
            Id = 0;
            Name = "";
            Stats = 0;
            SellPrice = 1f;
            BuyPrice = 2f;
            ConditionId = 0;
            SpeedAttack = 0;
            Description = "";
            Image = "";
            ItemTypeId = 0;
        }
        public class SortByName : IComparer<Item>
        {
            public int Compare(Item x, Item y)
            {
                return string.Compare(x.Name, y.Name);
            }
        }
        public class SortByBuyPrice : IComparer<Item>
        {
            public int Compare(Item x, Item y)
            {
                if (x.BuyPrice > y.BuyPrice)
                    return 1;
                if (x.BuyPrice < y.BuyPrice)
                    return -1;
                return 0;
            }
        }
        public class DontSort : IComparer<Item>
        {
            public int Compare(Item x, Item y)
            {
                if (x.Id > y.Id)
                    return 1;
                if (x.Id < y.Id)
                    return -1;
                return 0;
            }
        }
        public class SortByType : IComparer<Item>
        {
            public int Compare(Item x, Item y)
            {
                if (x.ItemTypeId > y.ItemTypeId)
                    return 1;
                if (x.ItemTypeId < y.ItemTypeId)
                    return -1;
                return 0;
            }
        }
        public class SortBySellPrice : IComparer<Item>
        {
            public int Compare(Item x, Item y)
            {
                if (x.SellPrice > y.SellPrice)
                    return 1;
                if (x.SellPrice < y.SellPrice)
                    return -1;
                return 0;
            }
        }
    }

    public class Shop
    {
        [PrimaryKeyField(true)]
        public int Id { get; set; }
        [ForeignKeyField(true, "Item", "Id")]
        public int ItemId { get; set; }
        public int Amount { get; set; }
        public Shop()
        {
            Id = 0;
            ItemId = 0;
            Amount = 0;
        }
    }

    public class User
    {
        [PrimaryKeyField(true)]
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public User()
        {
            Id = 0;
            Login = "";
            Password = "";
        }
    }

    public class Admin
    {
        [PrimaryKeyField(true)]
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        [ForeignKeyField(true, "User", "Id")]
        public int UserId { get; set; }
    }

    public class Pers
    {
        [PrimaryKeyField(true)]
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKeyField(true, "Race", "Id")]
        public int RaceId { get; set; }
        [ForeignKeyField(true, "User", "Id")]
        public int UserId { get; set; }
        public Pers()
        {
            Id = 0;
            Name = "";
            RaceId = 0;
        }
    }

    public class Inventory
    {
        [PrimaryKeyField(true)]
        public int Id { get; set; }
        [ForeignKeyField(true, "Pers", "Id")]
        public int PersId { get; set; }
        [ForeignKeyField(true, "Item", "Id")]
        public int ItemId { get; set; }
        public Inventory()
        {
            Id = 0;
            PersId = 0;
            ItemId = 0;
        }
    }

    public class Progress
    {
        [PrimaryKeyField(true)]
        public int Id { get; set; }
        public int Score { get; set; }
        public int CurrentLevel { get; set; }
        public float Money { get; set; }
        [ForeignKeyField(true, "Pers", "Id")]
        public int PersId { get; set; }
        public Progress()
        {
            Id = 0;
            Score = 0;
            CurrentLevel = 0;
            Money = 0;
            PersId = 0;
        }
    }
    #endregion
}
public class DataBaseInfo : MonoBehaviour {       
    //for current session
    public static int currentUserId;
    public static int currentPersId;
    public static Progress progress;
    public static bool isAdmin = false;
    public static List<Inventory> inventory;
    //for all users
    public static List<Shop> shop;
    public static List<Item> items;
    public static List<Race> races;
    public static List<Condition> conditions;
    public static List<ItemType> types;       
    //for log- reg- in
    public static List<Admin> admins;
    public static List<User> users;
    void Start () {
        using (DatabaseManager manager = new DatabaseManager("gamedata.db"))
        {
            if (manager.ConnectToDatabase())
            {
                users = (List<User>)manager.ReadAll<User>();
                admins = (List<Admin>)manager.ReadAll<Admin>();

                items = (List<Item>)manager.ReadAll<Item>();
                conditions = (List<Condition>)manager.ReadAll<Condition>();
                types = (List<ItemType>)manager.ReadAll<ItemType>();
                shop = (List<Shop>)manager.ReadAll<Shop>();
                races = (List<Race>)manager.ReadAll<Race>();
            }
        }   
        DontDestroyOnLoad(gameObject);
    }
}
