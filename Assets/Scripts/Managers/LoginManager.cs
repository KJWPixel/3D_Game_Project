using System.Collections;
using System.Collections.Generic;
using System.IO;
using Autodesk.Fbx;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UserData
{
    public string username;
    public string password;
}

public class LoginManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField UserField;
    [SerializeField] private TMP_InputField PasswordField;
    [SerializeField] private Button LoginButton;

    [SerializeField] private GameObject LoginText;

    private string savepath;

    private void Start()
    {
        savepath = Path.Combine(Application.dataPath, "UserData.json");
    }

    //UserData UserName �Է�
    //UserName or Password null�̸� return
    //UserData�� �����ͼ� UserName�� �ߺ��̸� return
    //UserData���� UserName�� �������� �ʴٸ� NewUserData Create
    //UserData�� ���� UserName�� �����Ѵٸ� �ش� Data�� Login

    public void OnClickLogin()
    {
        string username = UserField.text;
        string password = PasswordField.text;

        if (string.IsNullOrEmpty(UserField.text) || string.IsNullOrEmpty(PasswordField.text))
        {
            Debug.Log("UserName Field or PasswordField Empty");
            return;
        }

        UserData ExistsUserData = null;

        if(File.Exists(savepath))
        {
            string saveJson = File.ReadAllText(savepath);
            ExistsUserData = JsonUtility.FromJson<UserData>(saveJson);
        }

        if(ExistsUserData != null || ExistsUserData.username == UserField.text)
        {
            Debug.Log("���� UserName ���� �ٸ� UserName ���ٶ��ϴ�.");
            return;
        }


        if(ExistsUserData != null || ExistsUserData.username == UserField.text)
        {

        }

        UserData userData = new UserData
        {
            username = username,
            password = password
        };

        string json = JsonUtility.ToJson(userData, true);

        File.WriteAllText(savepath, json);
        Debug.Log($"User data saved to: {savepath}");
        Debug.Log(json);
    }

    private void CreateUserData()
    {
        
    }


}
