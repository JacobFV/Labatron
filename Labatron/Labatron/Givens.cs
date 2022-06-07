using System;
using System.Collections.Generic;

namespace Labatron
{
    abstract class ReactionGiven
    {
        public virtual double moles { get; }
        public virtual string moleCalc { get; }

        public int numberGivenParams;
        public ReactionCompound givenForCompound;
        public double coefficentGiven;

        internal ReactionGiven(double coefficentGiven, ReactionCompound givenForCompound)
        {
            this.givenForCompound = givenForCompound;
            this.coefficentGiven = coefficentGiven;
        }
    }

    class GramsGiven : ReactionGiven
    {
        public double gramsGiven;
        public double gfwt;

        public override double moles
        {
            get
            {
                return gramsGiven / gfwt;
            }
        }

        public override string moleCalc
        {
            get
            {
                return "(" + gramsGiven + "g / "
                    + gfwt + "g/mol) " + givenForCompound.formula;
            }
        }

        public GramsGiven(double gramsGiven, double gfwt, double coefficentFrom,
            ReactionCompound forCompound) : base(coefficentFrom, forCompound)
        {
            this.gramsGiven = gramsGiven;
            this.gfwt = gfwt;

            numberGivenParams = 1;
        }
    }

    class MolarityGiven : ReactionGiven
    {
        public double molarity;
        public double liters;

        public override string moleCalc
        {
            get
            {
                if (liters < 0.01)
                {
                    return "(" + (liters * 1000) + "mL * " + molarity + "mol/L) * 1L / 1000mL"
                        + givenForCompound.formula;
                }
                else
                {
                    return "(" + liters + "L * " + molarity + "mol/L) "
                        + givenForCompound.formula;
                }
            }
        }

        public override double moles
        {
            get
            {
                return molarity * liters;
            }
        }

        public MolarityGiven(double molarity, double liters, double coefficentFrom,
            ReactionCompound compoundFrom) : base(coefficentFrom, compoundFrom)
        {
            this.molarity = molarity;
            this.liters = liters;

            numberGivenParams = 2;
        }
    }
}