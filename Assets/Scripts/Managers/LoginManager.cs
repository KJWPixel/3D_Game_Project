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
    private const string SVAEFOLDER = "Save";//세이브 폴더 이름
    private const string FILENAME = "UserData.json";//파일 이름

    private List<UserData> UserList = new List<UserData>();

    private void Awake()
    {
        //Path.Combine("폴더경로", "파일이름.확장자") 
        //Path.Combine("루트폴더", "서브폴더", "파일이름.확장자")
        //(Window)Application.persistentDataPath: C:/Users/사용자이름/AppData/LocalLow/회사이름/프로젝트이름/UserData.json 
        //(Android)/storage/emulated/0/Android/data/com.회사이름.프로젝트이름/files/UserData.json
        //savePath = Path.Combine(Application.persistentDataPath, SVAEFOLDER, FILENAME);
        savePath = Path.Combine(Application.dataPath, SVAEFOLDER, FILENAME);
        //Save폴더가 없다면 생성
        Debug.Log($"Save Path: {savePath}");
        LoadUserData();
    }

    private void LoadUserData()
    {
        if(File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            UserList = JsonUtility.FromJson<UserDataList>(json).Users;
            Debug.Log($"UsserData 로드 성공: {UserList.Count} 명");
        }
    }
    private void SaveUserData()
    {
        UserDataList Wrapper = new UserDataList { Users = UserList };
        string json = JsonUtility.ToJson(Wrapper, true);
        File.WriteAllText(savePath, json);
        Debug.Log($"UserData 저장 {savePath}");
    }

    public void TryLogin()
    {
        string username = UserField.text.Trim();//Trim() 공백제거
        string password = PasswordField.text.Trim();

        if (string.IsNullOrEmpty(UserField.text) || string.IsNullOrEmpty(PasswordField.text))
        {
            ShowMessage("유저이름 또는 패스워드를 입력하시기 바랍니다.", Color.yellow);
            return;
        }

        UserData ExistUserData = UserList.Find(x => x.username == username);

        if (ExistUserData != null)//ExistUserData가 UserList에 존재하고
        {
            if (ExistUserData.password == password)//패스워드가 같다면 로그인성공
            {
                ShowMessage("로그인 성공", Color.green);
                Shared.SceneManager.ChangeScene(SCENE.MAIN, false);
            }
            else
            {
                ShowMessage("패스워드가 일치하지 않습니다.", Color.red);
            }
        }
        else//ExistUserData가 기존에 존재하지 않는다면, 새 사용자 등록
        {
            UserData NewUser = new UserData { username = username, password = password };
            UserList.Add(NewUser);
            SaveUserData();
            ShowMessage("새 사용자 등록 및 로그인 성공", Color.green);
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
