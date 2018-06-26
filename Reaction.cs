using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CUI;
using System.IO;
using System.Windows.Forms;

namespace Labatron
{
    class Reaction
    {
        public List<ReactionCompound> products;
        public List<ReactionCompound> reactants;
        public List<ReactionCompound> allCompounds
        {
            get
            {
                List<ReactionCompound> combined = new List<ReactionCompound>();
                foreach (ReactionCompound compound in reactants)
                {
                    combined.Add(compound);
                }
                foreach (ReactionCompound compound in products)
                {
                    combined.Add(compound);
                }
                return combined;
            }
        }
        //ReactionHeatType reactionHeatType;
        //string reactionType;
        ReactionGiven given;

        public Reaction()
        {
            CUI.CUI.DataListItem equationItem
                = new CUI.CUI.DataListItem("Equation",
                CUI.CUI.DataListItem.DataType.tString);
            CUI.CUI.DataListItem givenItem
                = new CUI.CUI.DataListItem("Limiting Given",
                CUI.CUI.DataListItem.DataType.tString);
            /*CUI.CUI.DataListItem ReactionTypeItem
                = new CUI.CUI.DataListItem("Reaction Type",
                CUI.CUI.DataListItem.DataType.tString);
            CUI.CUI.DataListItem ReactionHeatTypeItem
                = new CUI.CUI.DataListItem("Reaction Heat Type",
                CUI.CUI.DataListItem.DataType.tString);*/
            CUI.CUI.getDataList("Labatron v1 by Jacob Valdez", new CUI.CUI.DataListItem[2]{
                equationItem, givenItem, //ReactionTypeItem, ReactionHeatTypeItem
            });
            string equation = equationItem.getValue as string;
            parseEquationFromString(equation);
            given = parseGiven(givenItem.getValue as string);
            //reactionType = reactionTypeFunc(ReactionTypeItem.getValue as string);
            //reactionHeatType = parseReactionHeatType(ReactionHeatTypeItem.getValue as string);
            setupCompounds(ref reactants, "Reactants");
            setupCompounds(ref products, "Products");
            renderToCSVFile();
        }

        private void renderToCSVFile()
        {
            string output = "";
            //gfwt
            foreach(ReactionCompound compound in allCompounds)
            {
                output += "\"" + compound.gfwtString.work + "\",";
            }
            output.TrimEnd(',');
            output += "\n";
            //mass
            foreach (ReactionCompound compound in allCompounds)
            {
                output += "\"" + compound.gramsString.work + "\",";
            }
            output.TrimEnd(',');
            output += "\n";
            //moles
            foreach (ReactionCompound compound in allCompounds)
            {
                output += "\"" + compound.molesString.work + "\",";
            }
            output.TrimEnd(',');
            output += "\n";
            //gas liters
            foreach (ReactionCompound compound in allCompounds)
            {
                output += "\"" + compound.gasLitersString.work + "\",";
            }
            output.TrimEnd(',');
            output += "\n";
            //% comp
            foreach (ReactionCompound compound in allCompounds)
            {
                output += "\"";
                foreach(Answer answer in compound.percentCompString)
                {
                    output += answer.work + " ";
                }
                output += "\",";

            }
            output.TrimEnd(',');

            var dialog =
                new SaveFileDialog();

            dialog.Filter = "CSV Files|*.csv";
            dialog.FilterIndex = 1;
            dialog.Title = "Save CSV Stochiometry";

            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            File.WriteAllText(dialog.FileName, output);
        }

        private string reactionTypeFunc(string reactionTypeInput)
        {
            switch (reactionTypeInput)
            {
                case "S.":
                case "S":
                    return "synthesis";
                case "D.":
                case "D":
                    return "decomposition";
                case "S.R.":
                case "SR.":
                case "SR":
                    return "single replacement";
                case "D.R.":
                case "DR.":
                case "DR":
                    return "double replacement";
                default:
                    return reactionTypeInput;
            }
        }

        private ReactionHeatType parseReactionHeatType(string input)
        {
            switch (input.Trim().ToLower())
            {
                case "endothermic":
                case "endo":
                    return ReactionHeatType.endothermic;
                case "exothermic":
                case "exo":
                    return ReactionHeatType.exothermic;
                default:
                    return ReactionHeatType.neither;
            }
        }

        private ReactionGiven parseGiven(string input)
        {
            /* FORMAT
             * Xg AbCd2(s)
             * X g AB(s)
             * 5mL 6M AcId(aq)
             * 5 mL 6 M AcId(aq)
             * 1L 1M HCl(aq)
             */

            /*List<string> components = new List<string>();
            //1.02g 2HCl(aq) -> 1.02, g, 2, HCl(aq)
            //1mL 2M H2SO4 -> 1, mL, 2, M, H, 2, SO, 4
            input.Trim();

            for (int index = 0; index < input.Length; index++)
            {
                if (components.Count > 0)
                {
                    if ((char.IsDigit(input, index) || input[index] == '.') &&
                        !char.IsDigit(components.Last().Last()) ||
                        !char.IsDigit(input, index) &&
                        (char.IsDigit(components.Last().Last()) || input[index] == '.'))
                    {
                        components.Add("" + input[index]);
                    }
                    else if (string.IsNullOrWhiteSpace("" + input[index]))
                    {
                        if (components.Last() != " ")
                        {
                            components.Add(" ");
                        }
                    }
                    else
                    {
                        components[components.Count - 1] += input[index];
                    }
                }
                else
                {
                    components.Add("" + input[index]);
                }
            }
            for (int index = 0; index < components.Count(); index++)
            {
                components[index] = components[index].Trim();
            }
            Func<Reaction, int, List<string>, ReactionCompound> getCompound =
                (reaction, numberParams, inputComponents) =>
                {
                    string formula = "";
                    for(int index = 0; index < inputComponents.Count(); index++)
                    {
                        if (string.IsNullOrWhiteSpace(inputComponents[index]))
                        {
                            inputComponents.RemoveAt(index);
                        }
                    }
                    for (int index = numberParams * 2; index < inputComponents.Count; index++)
                    {
                        formula += inputComponents[index];
                    }
                    foreach (ReactionCompound possibleCompound in
                    reaction.reactants.Concat(reaction.products))
                    {
                        if (possibleCompound.formula == formula)
                        {
                            return possibleCompound;
                        }
                    }
                    throw new Exception("Given compound not found in reaction");
                };

            switch (components[1].ToLower())
            {
                case "gg":
                case "g":
                    ReactionCompound gramsCompound = getCompound(this, 1, components);
                    double grams = 0.0;
                    double.TryParse(components[0], out grams);
                    return new GramsGiven(grams, gramsCompound.gfwt,
                                          gramsCompound.coefficent, gramsCompound);
                case "ml":
                case "l":
                case "m":
                case "molar":
                    ReactionCompound molarityCompound = getCompound(this, 2, components);
                    int molarityLitersIndex = 0;
                    int molarityMolarityIndex = 2;
                    if(components[1].ToLower() == "m")
                    {
                        molarityLitersIndex = 2;
                        molarityMolarityIndex = 0;
                    }
                    else
                    {
                        molarityLitersIndex = 0;
                        molarityMolarityIndex = 2;
                    }
                    double molarityLiters = 0.0;
                    double.TryParse(components[molarityLitersIndex], out molarityLiters);
                    double molarity = 0.0;
                    double.TryParse(components[molarityMolarityIndex], out molarity);
                    if (components[molarityLitersIndex + 1].ToLower() == "ml")
                    {
                        molarityLiters /= 1000;
                    }
                    return new 
                        MolarityGiven(molarity, molarityLiters,
                        molarityCompound.coefficent, molarityCompound);
                default:
                    throw new Exception("given is imparsable!");
            }*/

            switch (input.ToLower())
            {
                case "grams":
                case "grams given":
                case "gg":
                case "gg.":
                case "g.g.":
                    CUI.CUI.DataListItem grams = new CUI.CUI.DataListItem(
                        "grams", CUI.CUI.DataListItem.DataType.tDouble);
                    CUI.CUI.DataListItem compound = new CUI.CUI.DataListItem(
                        "compound formula", CUI.CUI.DataListItem.DataType.tString);
                    CUI.CUI.getDataList("Given", new CUI.CUI.DataListItem[2]
                    {
                        grams,compound
                    });
                    string formula = compound.getValue as string;
                    foreach (ReactionCompound possibleCompound in allCompounds)
                    {
                        if (possibleCompound.formula == formula)
                        {
                            return new GramsGiven(
                                (double)grams.getValue,
                                possibleCompound.gfwt,
                                possibleCompound.coefficent,
                                possibleCompound);
                        }
                    }
                    break;
                case "aqueous solution":
                case "aqueous sol":
                case "aq sol":
                case "aqueous sol.":
                case "aq sol.":
                case "aq. sol.":
                case "mol":
                case "moles":
                case "molarity":
                case "liters":
                case "liters given":
                case "molarity given":
                    CUI.CUI.DataListItem molarityItem = new CUI.CUI.DataListItem(
                        "molarity (M)", CUI.CUI.DataListItem.DataType.tDouble);
                    CUI.CUI.DataListItem litersItem = new CUI.CUI.DataListItem(
                        "liters (L)", CUI.CUI.DataListItem.DataType.tDouble);
                    CUI.CUI.DataListItem molarityCompoundFormulaItem = new CUI.CUI.DataListItem(
                        "compound formula", CUI.CUI.DataListItem.DataType.tString);
                    CUI.CUI.getDataList("Given", new CUI.CUI.DataListItem[3]
                    {
                        molarityItem,litersItem,molarityCompoundFormulaItem
                    });
                    string molarityCompoundFormula
                        = molarityCompoundFormulaItem.getValue as string;
                    foreach (ReactionCompound possibleCompound in allCompounds)
                    {
                        if (possibleCompound.formula == molarityCompoundFormula)
                        {
                            return new MolarityGiven(
                                (double)molarityItem.getValue,
                                (double)litersItem.getValue,
                                possibleCompound.coefficent,
                                possibleCompound);
                        }
                    }
                    break;
            }
            throw new Exception("Given not parsed");
        }

        private void parseEquationFromString(string equation)
        {
            List<string> reactantsString = new List<string>();
            List<string> productsString = new List<string>();
            bool hasFoundYeildSign = false;
            bool newCompound = true;
            foreach (char character in equation)
            {
                if (char.IsLetterOrDigit(character) ||
                    character == '(' || character == ')')
                {
                    if (newCompound == true)
                    {
                        newCompound = false;
                        if (hasFoundYeildSign == true)
                        {
                            productsString.Add(new string(new char[1] { character }));
                        }
                        else
                        {
                            reactantsString.Add(new string(new char[1] { character }));
                        }
                    }
                    else
                    {
                        if (hasFoundYeildSign == true)
                        {
                            productsString[productsString.Count - 1] = productsString.Last() + character;
                        }
                        else
                        {
                            reactantsString[reactantsString.Count - 1] = reactantsString.Last() + character;
                        }
                    }
                }
                else
                {
                    newCompound = true;
                    switch (character)
                    {
                        case ' ':
                            break;
                        case '+':
                            break;
                        case '-':
                            break;
                        case '>':
                        case '→':
                        case '↔':
                            hasFoundYeildSign = true;
                            break;
                        default:
                            break;
                    }
                }
            }
            reactants = processCompounds(reactantsString);
            products = processCompounds(productsString);
        }

        private void setupCompounds(ref List<ReactionCompound> compounds, string compoundsName)
        {
            bool everyCompoundHasSpecifiedState = true;
            foreach(ReactionCompound compound in compounds)
            {
                if (compound.state == "")
                {
                    everyCompoundHasSpecifiedState = false;
                }
            }
            if (everyCompoundHasSpecifiedState == false)
            {
                for(int index = 0; index < compounds.Count; index++)
                {
                    CUI.CUI.DataListItem state =
                       new CUI.CUI.DataListItem("state", CUI.CUI.DataListItem.DataType.tString);
                    state.consoleValue = compounds[index].state;
                    /*CUI.CUI.DataListItem name =
                       new CUI.CUI.DataListItem("name", CUI.CUI.DataListItem.DataType.tString);*/
                    CUI.CUI.getDataList(compounds[index].formula + " Properties",
                        new CUI.CUI.DataListItem[1] { //name,
                            state });
                    /*compounds[index] = new ReactionCompound(
                        compounds[index].formula, name.getValue as string,
                        given, state.getValue as string);*/
                    //compounds[index].name = name.getValue as string;
                    compounds[index].given = given;
                    compounds[index].state = state.getValue as string;
                }
            }
            else
            {
                List<CUI.CUI.DataListItem> compoundNames
                    = new List<CUI.CUI.DataListItem>(compounds.Count);
                foreach(ReactionCompound compound in compounds)
                {
                    compoundNames.Add(new CUI.CUI.DataListItem
                        (compound.formula, CUI.CUI.DataListItem.DataType.tString));
                }
                CUI.CUI.getDataList(compoundsName + " Names", compoundNames.ToArray());
                for(int compoundIndex = 0; compoundIndex < compounds.Count; compoundIndex++)
                {
                    /*compounds[compoundIndex] = new ReactionCompound(
                        compounds[compoundIndex].formula,
                        compoundNames[compoundIndex].getValue as string,
                        given,
                        compounds[compoundIndex].state);*/
                    compounds[compoundIndex].name
                        = compoundNames[compoundIndex].getValue as string;
                    compounds[compoundIndex].given = given;
                    
                }
            }
        }

        private List<ReactionCompound> processCompounds(List<string> compounds)
        {
            List<ReactionCompound> finalCompounds = new List<ReactionCompound>();
            foreach(string compound in compounds)
            {
                finalCompounds.Add(new ReactionCompound(compound));
            }
            return finalCompounds;
        }

        enum ReactionHeatType { neither, exothermic, endothermic }
    }
}