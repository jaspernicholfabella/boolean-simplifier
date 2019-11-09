using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Text;
using System.ComponentModel;

public class BooleanSimplifier{
    private BoolExpr expression1;
    private BoolExpr expression2;
    private bool error1 = false;
    private bool error2 = false;
    private bool errors = false;

    string[] theTruthTable;

    int[] m = new int[16]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};

    private void GenBtn()
    {
        //if (!validateInputs()) return;

        // clear the previous table:
        //TruthTableView.Columns.Clear();
        //TruthTableView.Items.Clear();

        List<char> boolVars;

        boolVars = expression1.GetBoolVars();

        bool[,] inputTable = GenInputTable(boolVars.Count);

        VARIABLE = boolVars.Count;
        //foreach (char c in boolVars)
        //{
        //    TruthTableView.Columns.Add(c.ToString());
        //}

        // solve:
        bool[] answer1 = new bool[inputTable.GetLength(0)];
        bool[] answer2 = new bool[inputTable.GetLength(0)]; ;
        for (int i = 0; i < inputTable.GetLength(0); i++)
        {
            for (int j = 0; j < inputTable.GetLength(1); j++)
            {
                expression1.SetValue(boolVars[j], inputTable[i, j]);
                //if (isInput2Enabled()) expression2.SetValue(boolVars[j], inputTable[i, j]);
            }
            answer1[i] = expression1.Solve();
            //if (isInput2Enabled()) answer2[i] = expression2.Solve();
        }

        bool equal = false;
        //if (isInput2Enabled()) equal = answer1.SequenceEqual(answer2);

        //TruthTableView.Columns.Add("#1");
        //if (isInput2Enabled() && !equal) TruthTableView.Columns.Add("#2");
        string temp = "";
        theTruthTable = new string[inputTable.GetLength(0)];
        for (int i = 0; i < inputTable.GetLength(0); i++)
        {
            temp = "";
            //ListViewItem L = TruthTableView.Items.Add(getBoolStr(inputTable[i, 0]));
            temp = temp + getBoolStr(inputTable[i, 0]);
            for (int j = 1; j < inputTable.GetLength(1); j++)
            {
                //L.SubItems.Add(getBoolStr(inputTable[i, j]));
                temp = temp + " : " + getBoolStr(inputTable[i, j]);
            }
            //L.SubItems.Add(getBoolStr(answer1[i]));
            temp = temp + "  :  a=> " + getBoolStr(answer1[i]);
            theTruthTable[i] = getBoolStr(answer1[i]);
            m[i] = Convert.ToInt32(getBoolStr(answer1[i]));
            //if (isInput2Enabled() && !equal) L.SubItems.Add(getBoolStr(answer2[i])); ;
            Debug.Log(temp);
        }
    }

    private string getBoolStr(bool b)
    {
        return b ? "1" : "0";
    }

    //private bool validateInputs()
    //{
    //    if (Input1.Text == "")
    //    {
    //        //I can't generate a truth table without any input.
    //        return false;
    //    }

    //    if (errors)
    //    {
    //        //Please make sure there are no errors with your input before generate the truth table.
    //        return false;
    //    }

    //    return true;
    //}

    private bool[,] GenInputTable(int col)
    {
        bool[,] table;
        int row = (int)Math.Pow(2, col);

        table = new bool[row, col];

        int divider = row;

        // iterate by column
        for (int c = 0; c < col; c++)
        {
            divider /= 2;
            bool cell = false;
            // iterate every row by this column's index:
            for (int r = 0; r < row; r++)
            {
                table[r, c] = cell;
                if ((divider == 1) || ((r + 1) % divider == 0))
                {
                    cell = !cell;
                }
            }
        }

        return table;
    }

    //=======================================
    private string[] variable4 = new string[] { "0000", "0001", "0010", "0011", "0100", "0101", "0110", "0111", "1000", "1001", "1010", "1011", "1100", "1101", "1110", "1111" };
    private string[,] variable4KMap = new string[,] { { "0000", "0100", "1100", "1000" }, { "0001", "0101", "1101", "1001" }, { "0011", "0111", "1111", "1011", }, { "0010", "0110", "1110", "1010" } };
    private string[] truth4 = new string[] { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };
    private string[,] kmap4 = new string[,] { { "0", "0", "0", "0" }, { "0", "0", "0", "0" }, { "0", "0", "0", "0", },
			{ "0", "0", "0", "0" } };
    private string[] VARIABLE_NAME = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "L", "M", "N" };
    private int VARIABLE;
    private bool ALLOW_IGNORANCE = false;

    private string simplify(string[] aValueTruthTable, string[] theTruthTable, string[][] aValueKMapTable, string[][] theKMapTable)
    {
        string functionOutput = "";
        int i;
        bool isAllZeros = true, isAllOnes = true, isAllIgnorances = true;

        functionOutput = "F(" + VARIABLE_NAME[0] + ", " + VARIABLE_NAME[1] + ", " + VARIABLE_NAME[2] + ", " + VARIABLE_NAME[3] + ") = ";

        // check if all 0s or all 1s
        for (i = 0; i < theTruthTable.Length; i++)
        {
            if (theTruthTable[i].Equals("1"))
            {
                isAllZeros = false;
            }
            if (theTruthTable[i].Equals("0"))
            {
                isAllOnes = false;
            }
            if (!theTruthTable[i].Equals("x"))
            {
                isAllIgnorances = false;
            }
        }

        if (isAllZeros || isAllIgnorances)
        {
            functionOutput += "0";
        }
        else if (isAllOnes || isAllIgnorances)
        {
            functionOutput += "1";
        }
        // do the simplification here
        else
        {
            Simplifier simplifier = new Simplifier();
            String functionSimplify = "";

            int[] arrMinTerms = getSubArray(theTruthTable, 1);
            int[] arrIgnorances = getSubArray(theTruthTable, 2);

            printArray(arrMinTerms);
            printArray(arrIgnorances);

            //functionOutput += getFunctionOutput(aValueKMapTable, theKMapTable);

            functionSimplify = simplifier.getSimplifier(arrMinTerms, arrIgnorances, VARIABLE);
            functionOutput += functionSimplify;
        }

        return functionOutput;
    }

    private void printArray (int[] anArray)
	{
        string temp = "";
		for (int i = 0; i < anArray.GetLength(0); i++)
		{
			temp = temp + anArray[i] + " ";
		}
        //Debug.Log("\n");
	}

    private int[] getSubArray(string[] theTruthTable, int subArrayType)
    {
        int[] anArray = new int[theTruthTable.GetLength(0) + 1];
        int i, arrayIndex = 0;

        for (i = 0; i < anArray.Length; i++)
        {
            anArray[i] = -1;
        }
        // for the 1s
        if (subArrayType == 1)
        {
            for (i = 0, arrayIndex = 0; i < theTruthTable.GetLength(0); i++)
            {
                if (theTruthTable[i].Equals("1"))
                {
                    anArray[arrayIndex] = i;
                    arrayIndex++;
                }
            }
            //anArray[arrayIndex] = -1;
        }
        // for don't care
        else if (subArrayType == 2)
        {
            for (i = 0, arrayIndex = 0; i < theTruthTable.GetLength(0); i++)
            {
                if (theTruthTable[i].Equals("x"))
                {
                    anArray[arrayIndex] = i;
                    arrayIndex++;
                }
            }
            //anArray[arrayIndex] = -1;
        }

        return anArray;
    }

    private string getFunctionOutput(string[,] aValueKMapTable, string[,] theKMapTable)
    {
        string functionOutput = "";
        string theCellValue = "";
        string theValue = "";
        string oneTerm = "";
        int index = 0;

        // loop through the matrix vertically
        for (int j = 0; j < theKMapTable.GetLength(1); j++)
        {
            for (int i = 0; i < theKMapTable.GetLength(0); i++)
            {
                if (!theKMapTable[i,j].Equals("0"))
                {
                    theCellValue = aValueKMapTable[i,j];
                    index = 0;
                    oneTerm = "";

                    if (functionOutput != "")
                    {
                        functionOutput += "+";
                    }

                    while (theCellValue.Length > 0)
                    {
                        theValue = theCellValue.Substring(0, 1);
                        theCellValue = theCellValue.Substring(1);

                        if (theValue.Equals("0"))
                        {
                            oneTerm += VARIABLE_NAME[index] + "'";
                        }
                        else
                        {
                            oneTerm += VARIABLE_NAME[index];
                        }
                        index++;
                    }
                    functionOutput += oneTerm;
                }
            }
        }

        return functionOutput;
    }

    private string[] getRowOrColumn(string[] aValueTruthTable, int anIndex, string[,] aValueKMapTable, int row, int column, int direction)
    {
        string[] theReturn = new string[2];
        string theValue;
        int i, j;

        switch (direction)
        {
            case 1:
                theValue = aValueTruthTable[anIndex];
                for (i = 0; i < aValueKMapTable.GetLength(0); i++)
                {
                    for (j = 0; j < aValueKMapTable.GetLength(1); j++)
                    {
                        if (theValue.ToLower().Equals(aValueKMapTable[i,j].ToLower()))
                        {
                            theReturn[0] = "" + i;
                            theReturn[1] = "" + j;

                            return theReturn;
                        }
                    }
                }
                break;

            case 2:
                theValue = aValueKMapTable[row,column];
                for (i = 0; i < aValueTruthTable.Length; i++)
                { 
                    if (theValue.ToLower().Equals(aValueTruthTable[i].ToLower()))
                    {
                        theReturn[0] = "" + i;
                        theReturn[1] = "-1";

                        return theReturn;
                    }
                }

                break;
        }

        return theReturn;
    }

    public BooleanSimplifier() {
        //simplify(variable4, truth4, variable4KMap, kmap4);
    }
    public int getVars() {
        return VARIABLE;
    }
    public string[] getKMAP() {
        return theTruthTable;
    } 
    public string simplify(string formula) {
        expression1 = new BoolExpr(formula);
        GenBtn();


        string functionOutput = "";

        Simplifier simplifier = new Simplifier();
		string functionSimplify = "";
		
		int[] arrMinTerms = getSubArray(theTruthTable, 1);
		int[] arrIgnorances = getSubArray(theTruthTable, 2);

        //Debug.Log("Print:" + arrMinTerms.Length);
        //for (int a = 0; a < arrMinTerms.Length; a++)
        //{
        //    Debug.Log("Printed => s" + arrMinTerms[a]);
        //}

        //Debug.Log("Minterms Here:");
		printArray(arrMinTerms);
        //Debug.Log("Ignorances Here:");
		printArray(arrIgnorances);

        //Debug.Log(arrMinTerms.Length + " " + arrIgnorances.Length);
		functionSimplify = simplifier.getSimplifier(arrMinTerms, arrIgnorances, VARIABLE);			
		functionOutput += functionSimplify;

        //Debug.Log(functionOutput);
        return functionOutput;
    } 
}
