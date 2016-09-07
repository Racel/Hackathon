using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using weka.classifiers.trees.j48;
using weka.gui.treevisualizer;
using weka.core;

namespace Weka
{
    public class WekaTrees
    {
        private Instances wekaData;

        public static string ByNominalAtttribute(string name, List<string> definition)
        {
            string Attribute = "@attribute " + name + " {" + definition + "}";
            return Attribute;
        }

        public static string ByNumericAtttribute(string name, List<string> definition)
        {
            string Attribute = "@attribute " + name + " numeric";
            return Attribute;
        }

    }
}
