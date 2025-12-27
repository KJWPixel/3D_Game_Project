//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;

//public class Example_CSV : MonoBehaviour
//{
//    public string filePath;
//    public GameObject characterPrefab;

//    private void Start()
//    {
//        var data = CSVReader_.Read(filePath);

//        Debug.Log($"Data: Count :: {data.Count}");
        
//        for(int i = 0; i < 5; i++)
//        {
//            int rnd = Random.Range(0, data.Count);
//            GameObject character = Instantiate(characterPrefab);

//            character.name = data[rnd]["Name"].ToString();
//            character.GetComponent<LOLCharacter>().characterName = data[rnd]["Name"].ToString();
//            character.GetComponent<LOLCharacter>().hp = float.Parse((data[rnd]["HP"]).ToString());
//            character.GetComponent<LOLCharacter>().mp = float.Parse((data[rnd]["MP"]).ToString());
//            character.GetComponent<LOLCharacter>().ad = float.Parse((data[rnd]["AD"]).ToString());
//            character.GetComponent<LOLCharacter>().asp = float.Parse((data[rnd]["AS"]).ToString());
//            character.GetComponent<LOLCharacter>().range = float.Parse((data[rnd]["RANGE"]).ToString());

//            Debug.Log(data[rnd]["Name"]);
//        }

//        //int rnd = Random.Range(0, data.Count); // 0번째에서 데이터
//        //Debug.Log(data[0]["StageName"]);     // 헤더를 제외한 [0번째] 의 [칼럼명(StageName)]
//        //Debug.Log(data[rnd]["Name"]);          // data[행][칼럼이름]


        
//    }
//}
