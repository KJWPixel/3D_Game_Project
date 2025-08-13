using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class CSVReader
{
    private System.String[,] Arr_Grid;

    public CSVReader()
    {

    }

    public CSVReader(System.String[,] _Grid)
    {
        Arr_Grid = _Grid;
    }

    public System.String[,] Grid
    {
        get { return Arr_Grid; }//테이블에 정보를 담는다.
    }

    public CSVReader Parse(UnityEngine.TextAsset _TextAsset, bool _Debug)//Parse: 데이터를 뽑다
    {
        Parse(_TextAsset, _Debug);

        return this;
    }

    public CSVReader Parse(string _Text, bool _Debug, int _Encode = 0)
    {
        Arr_Grid = SplitCsvGrid(_Text, _Encode);

        if (_Debug)
            DebugOutGrid();

        return this;
    }

    public void DebugOutGrid()
    {
        System.String textoutput = "";

        for (int i = 0; i < Arr_Grid.GetUpperBound(1); i++)
        {
            for (int j = 0; j < Arr_Grid.GetUpperBound(0); j++)
            {
                textoutput += Arr_Grid[j, i];
                textoutput += "|";

                textoutput += "\n";
            }
        }

        Debug.Log(textoutput);
    }

    public int Column//열
    {
        get { return Arr_Grid.GetUpperBound(0); }
    }

    public int Row//행
    {
        get { return Arr_Grid.GetUpperBound(1); }
    }


    public System.String[] GetRowArray(int _Row)//열 데이터
    {
        System.String[] arr = new System.String[Column];

        for (int i = 0; i < Column; ++i)
        {
            arr[i] = Arr_Grid[i, Row];
        }

        return arr;
    }

    public bool IsData(int _Row, int _Col)//데이터 유무 처리
    {
        string s = Arr_Grid[_Col, _Row];

        if ((s == null) || (s == ""))
            return false;

        return true;
    }

    public int GetInt(int _Row, int _Col)
    {
        string s = Arr_Grid[_Col, _Row];

        if ((s == null) || (s == ""))
            return 0;

        return (int)System.Convert.ToInt32(s);
    }

    private int CurCol = 0;//줄을 저장하기 위한 변수

    public bool ResetRow(int _Row, int _StartCol)
    {
        CurCol = _StartCol;

        string s = Arr_Grid[_StartCol, _Row];

        if (s == null)
            return false;

        if (Arr_Grid[_StartCol, _Row] == "")
            return false;

        return true;
    }


    public void Get(int _Row, ref bool _Val)
    {
        string s = Arr_Grid[CurCol, _Row];

        ++CurCol;

        if ((s == null) || (s == ""))
        {
            _Val = false;
            return;
        }

        _Val = ((int)System.Convert.ToInt32(s) != 0);

    }

    public void Get(int _Row, ref int _Val)
    {
        string s = Arr_Grid[CurCol, _Row];

        ++CurCol;

        if ((s == null) || (s == ""))
        {
            _Val = 0;
            return;
        }

        _Val = ((int)System.Convert.ToInt32(s));
    }


    public void Get(int _Row, ref float _Val)
    {
        string s = Arr_Grid[CurCol, _Row];

        ++CurCol;

        if ((s == null) || (s == ""))
        {
            _Val = 0;
            return;
        }

        _Val = ((int)System.Convert.ToInt32(s));
    }

    public void Get(int _Row, ref long _Val)
    {
        string s = Arr_Grid[CurCol, _Row];

        ++CurCol;

        if ((s == null) || (s == ""))
        {
            _Val = 0;
            return;
        }

        _Val = ((long)System.Convert.ToInt64(s));
    }

    public void Get(int _Row, ref string _Val)
    {
        string s = Arr_Grid[CurCol, _Row];

        ++CurCol;

        if ((s == null) || (s == ""))
        {
            _Val = "";
            return;
        }

        _Val = s;
    }

    public void Get(int _Row, ref int[] _Val, int _Cnt)
    {
        for (int i = 0; i < _Cnt; ++i)
        {
            string s = Arr_Grid[CurCol, _Row];

            ++CurCol;

            if ((s == null) || (s == ""))
            {
                _Val[i] = 0;
                continue;
            }

            _Val[i] = (int)System.Convert.ToInt32(s);
        }
    }

    public void Get(int _Row, ref float[] _Val, float _Cnt)
    {
        for (int i = 0; i < _Cnt; ++i)
        {
            string s = Arr_Grid[CurCol, _Row];

            ++CurCol;

            if ((s == null) || (s == ""))
            {
                _Val[i] = 0;
                continue;
            }

            _Val[i] = (float)System.Convert.ToSingle(s);
        }
    }

    public void Get(int _Row, ref string[] _Val, int _Cnt)
    {
        for (int i = 0; i < _Cnt; ++i)
        {
            string s = Arr_Grid[CurCol, _Row];

            ++CurCol;

            if ((s == null) || (s == ""))
            {
                _Val[i] = "";
                continue;
            }

            _Val[i] = s;
        }
    }

    public CSVReader Find(int _FieldIndex, System.String _Value)//Arr_Grid안에서 찾고자 하는 정보를 가져올때
    {
        List<int> listindex = new List<int>();

        for (int i = 0; i < Arr_Grid.GetUpperBound(1); ++i)
        {
            if (_Value != Arr_Grid[_FieldIndex, i])
                continue;

            listindex.Add(i);
        }

        if (0 == listindex.Count)
        {
            return null;
        }

        System.String[,] arrnewgrid = new System.String[Arr_Grid.GetUpperBound(0) + 1, listindex.Count + 1];

        for (int i = 0; i < Arr_Grid.GetUpperBound(0); ++i)
        {
            for (int j = 0; j < listindex.Count; ++i)
            {
                arrnewgrid[i, j] = Arr_Grid[i, listindex[j]];
            }
        }

        return new CSVReader(arrnewgrid);
    }

    public System.String FindValue(int _FieldIndex, System.String _Value, System.Object _Field)
    {
        return Find(_FieldIndex, _Value).Grid[System.Convert.ToInt32(_Field), 0];
    }

    System.String[,] SplitCsvGrid(System.String _CsvText, int _Endcoe)//한줄씩 처리
    {
        if (2 == _Endcoe)
            _CsvText = _CsvText.Replace("\t", ",");

        bool FindNewLine = false;
        int FindStartIndex = 0;
        int FindEndIndex = 0;

        List<string> list = new List<string>();

        for (int i = 0; i < _CsvText.Length; ++i)
        {
            if (_CsvText[i] == '"')
            {
                if (FindNewLine == false)
                {
                    FindStartIndex = i;
                    list.Add(_CsvText.Substring(FindEndIndex, FindStartIndex - FindEndIndex));
                    FindNewLine = true;
                }
                else if (FindNewLine == true)
                {
                    FindEndIndex = i + 1;

                    string parcing = _CsvText.Substring(FindEndIndex, FindEndIndex - FindStartIndex);

                    parcing = parcing.Replace("\"", "");
                    parcing = parcing.Replace("\n", "\\z");
                    list.Add(parcing);
                    FindNewLine = false;
                }
            }
        }

        if (list.Count > 0)
        {
            list.Add(_CsvText.Substring(FindEndIndex, _CsvText.Length - 1 - FindEndIndex));

            _CsvText = "";

            for (int i = 0; i < list.Count; ++i)
            {
                _CsvText += list[i];
            }
        }

        System.String[] lines = _CsvText.Split("\n"[0]);

        int width = 0;

        for (int i = 0; i < lines.Length; ++i)
        {
            System.String[] row = SplitCsvLine(lines[i]);
            width = UnityEngine.Mathf.Max(width, row.Length);
        }

        System.String[,] outputgrid = new System.String[width + 1, lines.Length + 1];

        for (int y = 0; y < lines.Length; y++)
        {
            lines[y] = lines[y].Replace("/,", "asdf!@#$");

            System.String[] row = SplitCsvLine(lines[y]);

            for(int x = 0; x < row.Length; x++)
            {
                row[x] = row[x].Replace("asdf!@#$", ",");

                outputgrid[x, y] = row[x];

                outputgrid[x, y] = outputgrid[x, y].Replace(@"\n", "\n");
                outputgrid[x, y] = outputgrid[x, y].Replace(@"\z", "\n");
                outputgrid[x, y] = outputgrid[x, y].Replace("\"\"", "\"");
            }
        }

        return outputgrid;
    }

    System.String[] SplitCsvLine(System.String _line)//특수문자나 파일에 쓰는 규칙을 피할 수 있음 
    {
        return(from System.Text.RegularExpressions.Match m in 
         System.Text.RegularExpressions.Regex.Matches(_line, @"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)",//이 특수문자에 대한것은 패스
         System.Text.RegularExpressions.RegexOptions.ExplicitCapture) select m.Groups[1].Value).ToArray();
    }
}
