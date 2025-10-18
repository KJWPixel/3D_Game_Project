using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class LoginManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField UserField;
    [SerializeField] private TMP_InputField PasswordField;
    [SerializeField] private Button LoginButton;

    [SerializeField] private TMP_Text LoginText;

    private string savePath;
    private const string SVAEFOLDER = "Save";//���̺� ���� �̸�
    private const string FILENAME = "UserData.json";//���� �̸�

    private List<UserData> UserList = new List<UserData>();

    private void Awake()
    {
        //Path.Combine("�������", "�����̸�.Ȯ����") 
        //Path.Combine("��Ʈ����", "��������", "�����̸�.Ȯ����")
        //(Window)Application.persistentDataPath: C:/Users/������̸�/AppData/LocalLow/ȸ���̸�/������Ʈ�̸�/UserData.json 
        //(Android)/storage/emulated/0/Android/data/com.ȸ���̸�.������Ʈ�̸�/files/UserData.json
        //savePath = Path.Combine(Application.persistentDataPath, SVAEFOLDER, FILENAME);
        savePath = Path.Combine(Application.dataPath, SVAEFOLDER, FILENAME);
        //Save������ ���ٸ� ����
        Debug.Log($"Save Path: {savePath}");
        LoadUserData();
    }

    private void LoadUserData()
    {
        if(File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            UserList = JsonUtility.FromJson<UserDataList>(json).Users;
            Debug.Log($"UsserData �ε� ����: {UserList.Count} ��");
        }
    }
    private void SaveUserData()
    {
        UserDataList Wrapper = new UserDataList { Users = UserList };
        string json = JsonUtility.ToJson(Wrapper, true);
        File.WriteAllText(savePath, json);
        Debug.Log($"UserData ���� {savePath}");
    }

    public void TryLogin()
    {
        string username = UserField.text.Trim();//Trim() ��������
        string password = PasswordField.text.Trim();

        if (string.IsNullOrEmpty(UserField.text) || string.IsNullOrEmpty(PasswordField.text))
        {
            ShowMessage("�����̸� �Ǵ� �н����带 �Է��Ͻñ� �ٶ��ϴ�.", Color.yellow);
            return;
        }

        UserData ExistUserData = UserList.Find(x => x.username == username);

        if (ExistUserData != null)//ExistUserData�� UserList�� �����ϰ�
        {
            if (ExistUserData.password == password)//�н����尡 ���ٸ� �α��μ���
            {
                ShowMessage("�α��� ����", Color.green);
                Shared.SceneManager.ChangeScene(SCENE.MAIN, false);
            }
            else
            {
                ShowMessage("�н����尡 ��ġ���� �ʽ��ϴ�.", Color.red);
            }
        }
        else//ExistUserData�� ������ �������� �ʴ´ٸ�, �� ����� ���
        {
            UserData NewUser = new UserData { username = username, password = password };
            UserList.Add(NewUser);
            SaveUserData();
            ShowMessage("�� ����� ��� �� �α��� ����", Color.green);
            Shared.SceneManager.ChangeScene(SCENE.MAIN, false);
        }      
    }

    private void ShowMessage(string _Message, Color _Color)
    { 
        if(LoginText != null)
        {
            LoginText.text = _Message;
            LoginText.color = _Color;
            LoginText.gameObject.SetActive(true);
            Invoke(nameof(HideMessage),3f);
        }
        Debug.Log(_Message);
    }

    private void HideMessage()
    {
        if (LoginText != null)
        {
            LoginText.gameObject.SetActive(false);
        }
    }
}
