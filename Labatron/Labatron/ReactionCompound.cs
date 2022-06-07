using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labatron
{
    class ReactionCompound
    {
        public string name;
        public string formula;

        public string state;
        public double coefficent;

        List<Element> composition;
        public ReactionGiven given;

        List<Tuple<Element, int>> amountElements
        {
            get
            {
                Dictionary<Element, int> amounts = new Dictionary<Element, int>();
                foreach (Element element in composition)
                {
                    if (amounts.ContainsKey(element))
                    {
                        amounts[element] = amounts[element] + 1;
                    }
                    else
                    {
                        amounts.Add(element, 1);
                    }
                }
                List<Tuple<Element, int>> elementsWithAmounts = new List<Tuple<Element, int>>();
                foreach (Element element in amounts.Keys)
                {
                    elementsWithAmounts.Add(new Tuple<Element, int>(element, amounts[element]));
                }
                return elementsWithAmounts;
            }
        }

        public double grams
        {
            get
            {
                return given.moles * gfwt;
            }
        }
        public double gfwt
        {
            get
            {
                double returnGfwt = 0;
                foreach (Tuple<Element, int> singleElement in amountElements)
                {
                    returnGfwt += ((double)singleElement.Item2) * singleElement.Item1.amu;
                }
                return returnGfwt;
            }
        }
        public double gasLiters
        {
            get
            {
                return moles * Element.gasConst;
            }
        }
        public double moles
        {
            get
            {
                return given.moles * coefficent / given.coefficentGiven;
            }
        }

        public Answer gramsString
        {
            get
            {
                if (given.givenForCompound == this && given is GramsGiven)
                {
                    GramsGiven gramsGiven = given as GramsGiven;
                    return new Answer(gramsGiven.gramsGiven + "g",
                        gramsGiven.gramsGiven + " grams given");
                }
                else
                {
                    return new Answer(grams.ToString(
                        Program.format, System.Globalization.CultureInfo.InvariantCulture)
                        + "g", moles.ToString(
                        Program.format, System.Globalization.CultureInfo.InvariantCulture)
                        + "mol * " + gfwt.ToString(
                        Program.format, System.Globalization.CultureInfo.InvariantCulture)
                        + "g/mol = " + grams.ToString(Program.format) + "g");
                }
            }
        }
        public Answer gfwtString
        {
            get
            {
                List<string> stringList = new List<string>();
                foreach (Tuple<Element, int> amountElement in amountElements)
                {
                    stringList.Add(amountElement.Item1.amu.ToString(
                        Program.format, System.Globalization.CultureInfo.InvariantCulture)
                        + " * " + amountElement.Item2.ToString("0")
                        + " " + amountElement.Item1.symbol);
                }
                string returnString = "";
                for (int sNum = 0; sNum < stringList.Count(); sNum++)
                {
                    returnString += stringList[sNum];
                    if (sNum < stringList.Count() - 1)
                    {
                        returnString += " + ";
                    }
                }
                returnString += " = " + gfwt.ToString(
                    Program.format, System.Globalization.CultureInfo.InvariantCulture);
                return new Answer(gfwt.ToString(
                    Program.format, System.Globalization.CultureInfo.InvariantCulture)
                    + "g/mol", returnString + "g/mol");
            }
        }
        public Answer molesString
        {
            get
            {
                if (given.givenForCompound == this && given is MolarityGiven)
                {
                    MolarityGiven molarityGiven = given as MolarityGiven;
                    return new Answer(moles.ToString(
                        Program.format, System.Globalization.CultureInfo.InvariantCulture)
                        + "mol", molarityGiven.liters.ToString(
                        Program.format, System.Globalization.CultureInfo.InvariantCulture)
                        + "L * " + molarityGiven.molarity.ToString(
                        Program.format, System.Globalization.CultureInfo.InvariantCulture)
                        + "mol/L = " + moles.ToString(Program.format) + "mol");
                }
                else
                {
                    return new Answer(moles.ToString(
                        Program.format, System.Globalization.CultureInfo.InvariantCulture),
                        given.moleCalc + " * " + formula + " / " + given.givenForCompound.formula
                        + " = " + moles.ToString(Program.format) + "mol");
                }
            }
        }
        public Answer gasLitersString
        {
            get
            {
                if (state == "g")
                {
                    return new Answer(gasLiters.ToString(
                        Program.format, System.Globalization.CultureInfo.InvariantCulture)
                        + "L @ STP.", moles.ToString(
                        Program.format, System.Globalization.CultureInfo.InvariantCulture)
                        + "mol * " + Element.gasConst.ToString(
                        Program.format, System.Globalization.CultureInfo.InvariantCulture)
                        + "L/mol = " + gasLiters.ToString(
                        Program.format, System.Globalization.CultureInfo.InvariantCulture)
                        + "L @ STP.");
                }
                else
                {
                    return new Answer("n/a", formula + " is not a gas");
                }
            }
        }
        public List<Answer> percentCompString
        {
            get
            {
                List<Answer> answers = new List<Answer>();
                foreach (Tuple<Element, int> element in amountElements)
                {
                    string answer =
                        ((int)((element.Item1.amu * (double)element.Item2 / gfwt) * 100))
                        .ToString() + "% " + element.Item1.symbol + " in " + formula;
                    string work = element.Item2.ToString() + element.Item1.symbol
                        + " * " + element.Item1.amu.ToString(
                        Program.format, System.Globalization.CultureInfo.InvariantCulture)
                        + "g/mol " + element.Item1.symbol + " / " + gfwt.ToString(
                        Program.format, System.Globalization.CultureInfo.InvariantCulture)
                        + "g/mol " + formula + " * 100% = " + answer;
                    answers.Add(new Answer(answer, work));
                }
                return answers;
            }
        }

        public ReactionCompound(string formula,
            string name, ReactionGiven given,
            string state)
        {
            this.name = name;
            this.given = given;
            this.state = state;
            this.coefficent = 1;
            this.formula = formula;
            postInitialization(formula);
        }

        public ReactionCompound(string formula)
        {
            this.formula = formula;
            postInitialization(formula);
        }

        private void postInitialization(string formula)
        {
            Tuple<List<Element>, int, string, string> processedStuff
                = elementsFromFormula(formula);
            composition = processedStuff.Item1;
            coefficent = processedStuff.Item2;
            state = processedStuff.Item3;
            formula = processedStuff.Item4;
        }

        static Tuple<List<Element>, int, string, string> elementsFromFormula(string formulaName)
        {
            string state = "";
            string properFormula = "";
            List<string> formula = new List<string>(); // EG: {"Na","2","O"}
            foreach (char singleChar in formulaName)
            {
                if (!Char.IsLower(singleChar))
                {
                    formula.Add("");
                }
                formula[formula.Count - 1] += singleChar;
            }
            int amount = number(ref formula);
            foreach(string piece in formula)
            {
                properFormula += piece;
            }
            if(formula.Last() == ")")
            {
                if (formula[formula.Count - 2][0] == '(')
                {
                    state = formula[formula.Count - 2].Substring(1);
                    formula.RemoveRange(formula.Count - 2, 2);
                }
                else
                {
                    state = formula[formula.Count - 2];
                    formula.RemoveRange(formula.Count - 3, 3);
                }
            }
            List<Element> elements = elementsIn(ref formula);
            return new Tuple<List<Element>, int, string, string>
                (elements, amount, state, properFormula);
        }

        static List<Element> elementsIn(ref List<string> formula)
        {
            string state = "";
            Element[] periodicTable = Element.PeriodicTable();
            List<Element> elements = new List<Element>();
            while (formula.Count > 0)
            {
                if (Char.IsLetter(formula[0][0]))
                {
                    Element elementFound = new Element();
                    int numberElement = 0;
                    foreach (Element element in periodicTable)
                    {
                        if (formula[0] == element.symbol)
                        {
                            elementFound = element;
                            formula.RemoveAt(0);
                            numberElement = number(ref formula);
                            for (int elementNum = 0; elementNum < numberElement; elementNum++)
                            {
                                elements.Add(elementFound);
                            }
                            break;
                            /*int numberDigits = 0;
                            for (int digitNum = 0;
                                formula.Count > 0 && Char.IsNumber(formula[digitNum][0]);
                                digitNum++)
                            {
                                numberDigits = digitNum;
                            }
                            if (numberDigits == 0)
                            {
                                numberElement = 1;
                            }
                            else
                            {
                                while (numberDigits > 0)
                                {
                                    numberElement +=
                                        (int)Math.Pow(10, numberDigits - 1) *
                                        (int)Double.Parse(formula[0]);
                                    numberDigits--;
                                    formula.RemoveAt(0);
                                }
                            }*/
                        }
                    }
                }
                else if (formula[0] == "(")
                {
                    formula.RemoveAt(0);
                    List<Element> newElements = new List<Element>();
                    newElements.AddRange(elementsIn(ref formula));
                    //TODO: look for coefficent following the ")" when it returns
                    int multiplier = number(ref formula);
                    foreach (Element element in newElements)
                    {
                        for (int iteration = 0; iteration < multiplier; iteration++)
                        {
                            elements.Add(element);
                        }
                    }
                }
                else if (formula.Count == 2 && formula[0].StartsWith("("))
                {
                    state = formula[0].Substring(1);
                    formula.RemoveAt(0);

                }
                else if (formula[0] == ")")
                {
                    formula.RemoveAt(0);
                    return elements;
                }
            }
            return elements;
        }

        static private int number(ref List<string> formula)
        {
            int numberDigits = 0;
            /*for (; formula.Count > 0 &&
                Char.IsNumber(formula[numberDigits][0]);
                numberDigits++) { }*/
            /*if (char.IsDigit(formula.First().First()) == false)
            {
                return 1;
            }
            for (bool loop = true; loop == true; numberDigits++)
            {
                if (numberDigits >= formula.Count())
                {
                    loop = false;
                }
                else if (char.IsDigit(formula[numberDigits], 0) == true)
                {
                    loop = true;
                }
            }*/
            for (; numberDigits < formula.Count &&
                char.IsDigit(formula[numberDigits], 0); numberDigits++) { }
            if (numberDigits == 0)
            {
                return 1;
            }
            else
            {
                int returnNumber = 0;
                while (numberDigits > 0)
                {
                    returnNumber +=
                        (int)Math.Pow(10, numberDigits - 1) *
                        (int)double.Parse(formula[0]);
                    numberDigits--;
                    formula.RemoveAt(0);
                }
                return returnNumber;
            }
        }

    }
}