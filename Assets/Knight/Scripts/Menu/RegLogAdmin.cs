using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tables;
using Assets.Scripts;
using System.Linq;
using UnityEngine.UI;

public enum Roles
{
    user,
    admin,
    forreg
}
public class RegLogAdmin : MonoBehaviour {

    Roles role = Roles.user;
    public GameObject loginPanel;
    public GameObject mainPanel;
    public GameObject persInfo;
    public GameObject createPers;
    public GameObject choosePers;

    public Text buttonText;
    public InputField login;
    public InputField password;

    public void ChangeRole(string role1)
    {
        switch(role1)
        {
            case "User":
                role = Roles.user;
                buttonText.text = "Login";
                break;

            case "Admin":
                role = Roles.admin;
                buttonText.text = "LoginAdm";
                break;

            case "Register":
                role = Roles.forreg;
                buttonText.text = "Register";
                break;
        }
        
        loginPanel.SetActive(true);
        mainPanel.SetActive(false);
    }
    public void Back()
    {
        login.text = "";
        password.text = "";
        loginPanel.SetActive(false);
        mainPanel.SetActive(true);
    }
    public void PersInfo()
    {
        persInfo.SetActive(true);
        loginPanel.SetActive(false);
    }

    public bool createPersFlag = false;
    public void CreatePers()
    {
        createPersFlag = true;
        persInfo.SetActive(false);
        createPers.SetActive(true);
    }

    public Text persName;
    public void InsertPers()
    {
        if (persName.text != "")
        {
            using (DatabaseManager manager = new DatabaseManager("gamedata.db"))
            {
                if (manager.ConnectToDatabase())
                {

                    manager.InsertRecord<Pers>(new Pers() { Name = persName.text, RaceId = raceId, UserId = DataBaseInfo.currentUserId });
                    Pers pers = (Pers)((List<Pers>)manager.ReadAll<Pers>()).Single(x => x.Name == persName.text && x.RaceId == raceId && x.UserId == DataBaseInfo.currentUserId);
                    if (pers != null)
                    {
                        Debug.Log("Find");
                        manager.InsertRecord<Progress>(new Progress() { CurrentLevel = 0, Money = 0, PersId = pers.Id, Score = 0 });
                    }
                }
            }
            createPersFlag = false;
            persInfo.SetActive(true);
            createPers.SetActive(false);
        }
    }

    bool isOpen;
    string raceStr = "Race";
    int raceId = 0;
    Vector2 scrollPos;
    bool choosePersFlag;

    List<Pers> p;
    List<Progress> prog;
    void GetAllPers()
    {
        using (DatabaseManager manager = new DatabaseManager("gamedata.db"))
        {
            if (manager.ConnectToDatabase())
            {
                p = (List<Pers>)manager.ReadAll<Pers>();
                prog = (List<Progress>)manager.ReadAll<Progress>();
            }
        }
    }
    string persStr ="Choose pers";
    void OnGUI()
    {
        #region
        if (createPersFlag)
        {
            GUILayout.BeginArea(new Rect(Screen.width/2 - 100, Screen.height /2 - 20, 200, 150));
            {
                if (GUILayout.Button(raceStr))
                {
                    isOpen = !isOpen;
                }
                if (isOpen)
                {
                    scrollPos = GUILayout.BeginScrollView(scrollPos);
                    {
                        for (int i = 1; i < DataBaseInfo.races.Count;i++ )
                            if (GUILayout.Button(DataBaseInfo.races[i].RaceName, GUILayout.ExpandWidth(true)))
                            {
                                raceStr = DataBaseInfo.races[i].RaceName;
                                raceId = DataBaseInfo.races[i].Id;
                                isOpen = false;
                            }                       
                    }
                    GUILayout.EndScrollView();
                }
            }
            GUILayout.EndArea();
        }
        #endregion
        if (choosePersFlag)
        {
            GUILayout.BeginArea(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 20, 200, 150));
            {
                if (GUILayout.Button(persStr))
                {
                    isOpen = !isOpen;
                }
                if (isOpen)
                {
                    scrollPos = GUILayout.BeginScrollView(scrollPos);
                    {
                        for (int i = 0; i < p.Count; i++)
                        {
                            Race rac = DataBaseInfo.races.Single(x => x.Id == p[i].RaceId);
                            if (GUILayout.Button("Name: " + p[i].Name + " Race:" + rac.RaceName, GUILayout.ExpandWidth(true)))
                            {
                                persStr = p[i].Name;
                                DataBaseInfo.currentPersId = p[i].Id;
                                DataBaseInfo.progress = prog.Single(x => x.PersId == p[i].Id);
                                isOpen = false;
                            }
                        }
                    }
                    GUILayout.EndScrollView();
                }
            }
            GUILayout.EndArea();
        }

    }

    public void ChoosePers()
    {
        GetAllPers();
        choosePersFlag = true;
        persInfo.SetActive(false);
        choosePers.SetActive(true);
    }
    public void Login()
    {
        switch(role)
        {
            case Roles.user:
                User user = DataBaseInfo.users.FirstOrDefault(x => x.Login == login.text && x.Password == password.text);
                if (user != null)
                {
                    DataBaseInfo.currentUserId = user.Id;
                    DataBaseInfo.isAdmin = false;
                    Debug.Log("Login" + user.Id);
                    PersInfo();
                }
                break;
            case Roles.admin:
                Admin admin = DataBaseInfo.admins.FirstOrDefault(x => x.Login == login.text && x.Password == password.text);
                if (admin != null)
                {
                    DataBaseInfo.currentUserId = admin.UserId;
                    DataBaseInfo.isAdmin = true;
                    Debug.Log("LoginAdmin " + admin.UserId);
                    PersInfo();
                }
                break;
            case Roles.forreg:

                using (DatabaseManager manager = new DatabaseManager("gamedata.db"))
                {
                    if (manager.ConnectToDatabase())
                    {
                        User user1 = DataBaseInfo.users.FirstOrDefault(x => x.Login == login.text);
                        if (user1 == null)
                        {
                            manager.InsertRecord<User>(new User() { Login = login.text, Password = password.text });
                            Debug.Log("Success!");
                            Back();
                        }
                        else
                            Debug.Log("Already registered, change name");
                    }
                }              
                break;
        }
    }

   /* void Start()
    {
        using (DatabaseManager manager = new DatabaseManager("gamedata.db"))
        {
            if (manager.ConnectToDatabase())
            {
               // manager.InsertRecord<Item>(new Item() { Name = "Small Sword", Stats = 5, SpeedAttack = 3f, Description = "", Image = "1_25", ItemTypeId = 1, BuyPrice = 10, SellPrice = 8, ConditionId = 1, AttackDistance = 0});
               /* manager.InsertRecord<Item>(new Item() { Name = "Middle Sword", Stats = 10, SpeedAttack = 3f, Description = "", Image = "2_25", ItemTypeId = 1, BuyPrice = 15, SellPrice = 12, ConditionId = 1, AttackDistance = 0 });
                manager.InsertRecord<Item>(new Item() { Name = "Big Sword", Stats = 15, SpeedAttack = 3f, Description = "", Image = "3_25", ItemTypeId = 1, BuyPrice = 18, SellPrice = 15, ConditionId = 1, AttackDistance = 0 });
                manager.InsertRecord<Item>(new Item() { Name = "Magic Sword", Stats = 20, SpeedAttack = 3f, Description = "", Image = "4_25", ItemTypeId = 1, BuyPrice = 20, SellPrice = 15, ConditionId = 2, AttackDistance = 0 });


                manager.InsertRecord<Item>(new Item() { Name = "Small Bow", Stats = 5, SpeedAttack = 3f, Description = "", Image = "1_19", ItemTypeId = 2, BuyPrice = 10, SellPrice = 8, ConditionId = 1, AttackDistance = 5 });
                manager.InsertRecord<Item>(new Item() { Name = "Middle Bow", Stats = 10, SpeedAttack = 3f, Description = "", Image = "2_19", ItemTypeId = 2, BuyPrice = 15, SellPrice = 12, ConditionId = 1, AttackDistance = 5 });
                manager.InsertRecord<Item>(new Item() { Name = "Big Bow", Stats = 15, SpeedAttack = 3f, Description = "", Image = "3_19", ItemTypeId = 2, BuyPrice = 18, SellPrice = 15, ConditionId = 1, AttackDistance = 5 });
                manager.InsertRecord<Item>(new Item() { Name = "Magic Bow", Stats = 20, SpeedAttack = 3f, Description = "", Image = "4_19", ItemTypeId = 2, BuyPrice = 20, SellPrice = 15, ConditionId = 3, AttackDistance = 5 });



                manager.InsertRecord<Item>(new Item() { Name = "Rag Armor", Stats = 5, Description = "", Image = "1_17", ItemTypeId = 3, BuyPrice = 10, SellPrice = 2, ConditionId = 1, AttackDistance = 0 });
                manager.InsertRecord<Item>(new Item() { Name = "Silk Armor", Stats = 10, Description = "", Image = "2_17", ItemTypeId = 3, BuyPrice = 15, SellPrice = 10, ConditionId = 1, AttackDistance = 0 });
                manager.InsertRecord<Item>(new Item() { Name = "Leather Armor", Stats = 15, Description = "", Image = "3_17", ItemTypeId = 3, BuyPrice = 20, SellPrice = 15, ConditionId = 3, AttackDistance = 0 });
               / manager.InsertRecord<Progress>(new Progress() { CurrentLevel = 0, Money = 0, PersId = 1, Score = 0 });
            }
        }
    } */
}
