using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleJSON;
using UnityEngine;

public class Example_Json : MonoBehaviour
{
    public string jsonPath;
    public GameObject CharacterPrefab;

    void Start()
    {
        CharacterRandom(jsonPath);
    }

    public void LoadLevelFromJSON(string path)
    {
        TextAsset data = Resources.Load(path) as TextAsset;

        var jsonData = JSON.Parse(data.text);

        Debug.Log(jsonData.Count);

        for (int i = 0; i < jsonData.Count; i++)
        {
            {
                Debug.Log(jsonData[i]["StageName"]);
            }
        }
    }

    public void CharacterRandom(string path)
    {
        TextAsset data = Resources.Load(path) as TextAsset;

        var jsonData = JSON.Parse(data.text);

        Debug.Log(jsonData.Count);

        for(int i = 0; i < 5; i++)
        {
            int rnd = Random.Range(0, jsonData.Count);
            GameObject character = Instantiate(CharacterPrefab);

            character.name = jsonData[rnd]["Name"].ToString();
            character.GetComponent<LOLCharacter>().characterName = jsonData[rnd]["Name"].ToString();
            character.GetComponent<LOLCharacter>().hp = float.Parse((jsonData[rnd]["HP"]).ToString());
            character.GetComponent<LOLCharacter>().mp = float.Parse((jsonData[rnd]["MP"]).ToString());
            character.GetComponent<LOLCharacter>().ad = float.Parse((jsonData[rnd]["AD"]).ToString());
            character.GetComponent<LOLCharacter>().asp = float.Parse((jsonData[rnd]["AS"]).ToString());
            character.GetComponent<LOLCharacter>().range = float.Parse((jsonData[rnd]["RANGE"]).ToString());
        }
    }
}
