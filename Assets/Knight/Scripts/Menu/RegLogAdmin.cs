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
    void GetAllPers()
    {
        using (DatabaseManager manager = new DatabaseManager("gamedata.db"))
        {
            if (manager.ConnectToDatabase())
            {
                p = (List<Pers>)manager.ReadByFieldName<Pers>("UserId",DataBaseInfo.currentUserId);
            }
        }
    }
    string persStr ="Choose pers";

    public void StartGame()
    {
        if (persStr != "Choose pers")
            Application.LoadLevel(1);
    }
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
                                DataBaseInfo.progress = DataBaseInfo.allProgress.Single(x => x.PersId == p[i].Id);
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

    public void CancelPers()
    {
        choosePersFlag = false;
        persInfo.SetActive(true);
        choosePers.SetActive(false);
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
                    PersInfo();
                }
                break;
            case Roles.admin:
                Admin admin = DataBaseInfo.admins.FirstOrDefault(x => x.Login == login.text && x.Password == password.text);
                if (admin != null)
                {
                    DataBaseInfo.currentUserId = admin.UserId;
                    DataBaseInfo.isAdmin = true;
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
}
