using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using System.Text.RegularExpressions;

public class StartEventManager : MonoBehaviour {

    public InputField inputText;
    public Button[] btns;

    public GameObject inputContainer;
    public GameObject outputContainer;

    public Text errorTextHolder;
    public Text kmapTextHolder;

    BooleanSimplifier bs = new BooleanSimplifier();

    void Start()
    {
        outputContainer.SetActive(false);
        inputText.enabled = false;
        foreach (Button btn in btns)
        {
            btn.onClick.AddListener(() => CheckInput(btn.GetComponentInChildren<Text>().text));
        }
    }

    public static string splitString(string value)
    {
        bool ThereIsDuplicate = true;
        string temporary = "";
        string finalString = "";
        string splitted = "";
        char delimiter = '+';

        string[] substrings = value.Split(delimiter);
        
        for(int i = 0; i < substrings.Length;i++)
        {
            splitted += Alphabetize(substrings[i]);
            splitted += "+";
        }
        finalString = splitted.Remove(splitted.Length - 1, 1);
        
        temporary = finalString;
        if (ThereIsDuplicate == true)
        {
            ThereIsDuplicate = OnlyOnceCheck(temporary);

            temporary = Regex.Replace(temporary, "A+", "A");
            finalString += "\n" + temporary;

            temporary = Regex.Replace(temporary, "B+", "B");
            finalString += "\n" + temporary;

            temporary = Regex.Replace(temporary, "C+", "C");
            finalString += "\n" + temporary;

            temporary = Regex.Replace(temporary, "D+", "D");
            finalString += "\n" + temporary;
        }
        else
        {
            finalString = "this is already simplified";
        }

        return finalString;

    }

    public static string Alphabetize(string s)
    {
        char[] a = s.ToCharArray();
        Array.Sort(a);
        return new string(a);
    }

    public static bool OnlyOnceCheck(string input)
    {
        var set = new HashSet<char>();
        for (int i = 0; i < input.Length; i++)
            if (!set.Add(input[i]))
                return true;
        return false;
    }

    public static string thereIsDuplicate(string solText, string holdText)
    {
        bool ThereIsDuplicate = true;
        string temporary = holdText;
        temporary = Alphabetize(temporary);
        int countA, countB, countC, countD;
        countA = temporary.TakeWhile(c => c == 'A').Count();
        countB = temporary.TakeWhile(c => c == 'B').Count();
        countC = temporary.TakeWhile(c => c == 'C').Count();
        countD = temporary.TakeWhile(c => c == 'D').Count();

        ThereIsDuplicate = OnlyOnceCheck(temporary);

        if (ThereIsDuplicate == true)
        {
            solText += temporary;

            temporary = Regex.Replace(temporary, "A+", "A");
            solText += "\n" + temporary;


            temporary = Regex.Replace(temporary, "B+", "B");
            solText += "\n" + temporary;

            temporary = Regex.Replace(temporary, "C+", "C");
            solText += "\n" + temporary;


            temporary = Regex.Replace(temporary, "D+", "D");
            solText += "\n" + temporary;

        }
        else
        {
            solText = "this is already simplified.";
        }
        return solText;
    }


    void CheckInput(string btnText)
    {
        
        string holdText = inputText.GetComponentInChildren<Text>().text;
        if (btnText == "<")
        {
            inputText.GetComponentInChildren<Text>().text = holdText.Remove(holdText.Length - 1, 1);
        }
        else if (btnText == "=")
        {
            if (holdText == "")
            {
               //Error Message here
                return;
            }
            try
            {
                /*Solution Code*/
                string solText = "";
                
                if (holdText.Contains('+'))
                {
                    if(holdText.Contains('\''))
                    {
                        solText = "this is simplified";
                    }
                    else
                    {
                        solText += splitString(holdText);
                    }


                    
                }
                else
                {

                    solText = thereIsDuplicate(solText, holdText);
                }






                    /* here is the output */
                outputContainer.GetComponentInChildren<Text>().text = "Original Expression is: " + holdText + "\nSolution is : " + solText + "\nSimplified Expression is: " + bs.simplify(holdText);
                string[] kmapTemp = bs.getKMAP();
                string temp = "";
                int div = 1;
                if (bs.getVars() >= 3) {
                    div = 4;
                }
                Debug.Log("Length => " + kmapTemp.Length + " Vars => " + bs.getVars());
                for (int i = 1; i <= kmapTemp.Length; i++) {
                    temp = temp + kmapTemp[i - 1] + " ";
                    if (i % div == 0 && div != 0)
                    {
                        temp = temp + Environment.NewLine;
                    }
                    //temp = temp + kmapTemp[i - 1] + " ";
                }
                kmapTextHolder.GetComponent<Text>().text = temp;

                inputContainer.SetActive(false);
                outputContainer.SetActive(true);
            }
            catch (System.Exception)
            {
                //UnityEditor.EditorUtility.DisplayDialog("Error", "There is something wrong with your expression", "Okay", "Cancel");
                //errorTextHolder.GetComponent<Text>().text = "There is something wrong with your code";
            }
            
        }else if(btnText == "Back"){
            inputContainer.SetActive(true);
            outputContainer.SetActive(false);
        }
        else if (btnText == "Or") {
            inputText.GetComponentInChildren<Text>().text += "+";
        }
        else if (btnText == "Not") { 
             inputText.GetComponentInChildren<Text>().text += "'";
        }
        else
        {
            inputText.GetComponentInChildren<Text>().text += btnText;
        }

    }
}
