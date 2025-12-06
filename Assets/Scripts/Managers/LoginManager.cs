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
    [SerializeField] private TMP_Text LoginText;

    private string savePath;
    private const string SAVEFOLDER = "UserList";
    private const string FILENAME = "UserData.json";
    private const string AES_KEY = "MySecretKey12345"; // AES 키

    private List<UserData> UserList = new List<UserData>();
    public UserData CurrentUser { get; private set; } // 현재 로그인한 사용자

    private void Awake()
    {
        //savePath = Path.Combine(Application.persistentDataPath, SAVEFOLDER, FILENAME);
        savePath = Path.Combine(Application.dataPath, SAVEFOLDER, FILENAME);

        string directory = Path.GetDirectoryName(savePath);
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        LoadUserData();
    }

    private void LoadUserData()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            UserDataList wrapper = JsonUtility.FromJson<UserDataList>(json);
            UserList = wrapper?.Users ?? new List<UserData>();
            Debug.Log($"UserData 로드 성공: {UserList.Count} 명");
        }
        else
        {
            Debug.Log("UserData 파일 없음, 새 리스트 생성");
            UserList = new List<UserData>();
        }
    }

    private void SaveUserData()
    {
        UserDataList wrapper = new UserDataList { Users = UserList };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(savePath, json);
        Debug.Log("UserData 저장 성공");
    }

    public void TryLogin()
    {
        string username = UserField.text.Trim();
        string password = PasswordField.text.Trim();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ShowMessage("유저이름 또는 패스워드를 입력해주세요.", Color.yellow);
            return;
        }

        string encryptedPassword = AES.EncryptAES(password, AES_KEY);

        UserData existUser = UserList.Find(x => x.username == username);

        if (existUser != null)
        {
            if (existUser.password == encryptedPassword)
            {
                CurrentUser = existUser;
                ShowMessage("로그인 성공", Color.green);
                SceneMgr.Instance.ChangeScene(SCENE.MAIN, true);
            }
            else
            {
                ShowMessage("패스워드가 일치하지 않습니다.", Color.red);
            }
        }
        else
        {
            UserData newUser = new UserData
            {
                username = username,
                password = encryptedPassword
            };
            UserList.Add(newUser);
            SaveUserData();
            CurrentUser = newUser;
            ShowMessage("새 사용자 등록 및 로그인 성공", Color.green);
            SceneMgr.Instance.ChangeScene(SCENE.MAIN, true);
        }
    }

    private void ShowMessage(string message, Color color)
    {
        if (LoginText != null)
        {
            LoginText.text = message;
            LoginText.color = color;
            LoginText.gameObject.SetActive(true);
            Invoke(nameof(HideMessage), 3f);
        }
        Debug.Log(message);
    }

    private void HideMessage()
    {
        if (LoginText != null)
            LoginText.gameObject.SetActive(false);
    }
}
