using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labatron
{
    struct Element
    {
        public static double gasConst = 22.4;

        public string name;
        public string symbol;
        public double amu;
        public int atomicNumber;

        public Element(string name, string symbol, double amu, int atomicNumber)
        {
            this.name = name;
            this.symbol = symbol;
            this.amu = amu;
            this.atomicNumber = atomicNumber;
        }

        public static Element[] PeriodicTable()
        {
            return new Element[72] {
                new Element("hydrogen", "H",   01.01,   1),
                new Element("nameless", "He",  4.00,   2),
                new Element("nameless", "Li",  6.94,   3),
                new Element("nameless", "Be",  9.01,   4),
                new Element("nameless", "B",   10.81,   5),
                new Element("nameless", "C",   12.01,   6),
                new Element("nameless", "N",   14.01,   7),
                new Element("nameless", "O",   16.00,   8),
                new Element("nameless", "F",   19.00,   9),
                new Element("nameless", "Ne",  20.18,   10),
                new Element("nameless", "Na",  22.99,   11),
                new Element("nameless", "Mg",  24.31,   12),
                new Element("nameless", "Al",  26.98,   13),
                new Element("nameless", "Si",  28.09,   14),
                new Element("nameless", "P",   30.97,   15),
                new Element("nameless", "S",   32.07,   16),
                new Element("nameless", "Cl",  35.45,   17),
                new Element("nameless", "Ar",  39.95,   18),
                new Element("nameless", "K",   39.10,   19),
                new Element("nameless", "Ca",  40.08,   20),
                new Element("nameless", "Sc",  44.96,   21),
                new Element("nameless", "Ti",  47.87,   22),
                new Element("nameless", "V",   50.94,   23),
                new Element("nameless", "Cr",  52.00,   24),
                new Element("nameless", "Mn",  54.94,   25),
                new Element("nameless", "Fe",  55.85,   26),
                new Element("nameless", "Co",  58.93,   27),
                new Element("nameless", "Ni",  58.69,   28),
                new Element("nameless", "Cu",  63.55,   39),
                new Element("nameless", "Zn",  65.41,   30),
                new Element("nameless", "Ga",  69.72,   31),
                new Element("nameless", "Ge",  72.64,   32),
                new Element("nameless", "As",  74.92,   33),
                new Element("nameless", "Se",  78.96,   34),
                new Element("nameless", "Br",  79.90,   35),
                new Element("nameless", "Kr",  83.80,   36),
                new Element("nameless", "Rb",  85.47,   37),
                new Element("nameless", "Sr",  87.62,   38),
                new Element("nameless", "Y",   88.91,   39),
                new Element("nameless", "Zr",  91.22,   10),
                new Element("nameless", "Nb",  92.90,   41),
                new Element("nameless", "Mo",  95.94,   42),
                new Element("nameless", "Tc",  98.00,   43),
                new Element("nameless", "Ru",  101.07,   44),
                new Element("nameless", "Rh",  102.91,   45),
                new Element("nameless", "Pd",  106.42,   46),
                new Element("nameless", "Ag",  107.87,   47),
                new Element("nameless", "Cd",  112.41,   48),
                new Element("nameless", "In",  114.82,   49),
                new Element("nameless", "Sn",  118.71,   50),
                new Element("nameless", "Sb",  121.76,   51),
                new Element("nameless", "Te",  127.60,   52),
                new Element("nameless", "I",   126.90,   53),
                new Element("nameless", "Xe",  131.29,   54),
                new Element("nameless", "Cs",  132.91,   55),
                new Element("nameless", "Ba",  137.33,   56),
                new Element("nameless", "La",  138.91,   57),
                new Element("nameless", "Hf",  178.49,   72),
                new Element("nameless", "Ta",  180.95,   73),
                new Element("nameless", "W",   183.84,   74),
                new Element("nameless", "Re",  186.21,   75),
                new Element("nameless", "Os",  190.23,   76),
                new Element("nameless", "Ir",  192.22,   77),
                new Element("nameless", "Pt",  195.08,   78),
                new Element("nameless", "Au",  196.97,   79),
                new Element("nameless", "Hg",  200.59,   80),
                new Element("nameless", "Tl",  204.38,   81),
                new Element("nameless", "Pb",  207.21,   82),
                new Element("nameless", "Bi",  208.98,   83),
                new Element("nameless", "Po",  209.00,   84),
                new Element("nameless", "At",  210.00,   85),
                new Element("nameless", "Rn",  222.00,   86)};
        }
    }

    enum State
    {
        solid, liquid, aquaeous, gas
    }
}
