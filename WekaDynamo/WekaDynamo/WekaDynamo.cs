using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using weka.classifiers.trees;
using weka.gui.treevisualizer;
using weka.core.converters;
using weka.core;
using Autodesk.DesignScript.Runtime;
using Autodesk.DesignScript.Interfaces;
namespace WekaDynamo
{
    public class WekaTrees
    {
        private Instances wekaData;

        public static string ByNominalAtttribute(string name, List<string> definition)
        {
            string Attribute = "@attribute " + name + " {";
            int numDef = definition.Count;
            if (numDef > 0)
            {
                Attribute += definition[0];
                for (int ii = 1; ii < numDef; ii++)
                    Attribute += "," + definition[ii];
            }
            Attribute += "}";
            return Attribute;
        }

        public static string ByNumericAtttribute(string name)
        {
            string Attribute = "@attribute " + name + " numeric";
            return Attribute;
        }
        [CanUpdatePeriodically(true)]
        [MultiReturn(new[] { "data", "count" })]
        public static Dictionary<string, string> CreateARFF(string name, List<string> attributes, List<string> data)
        {
            string ARFF = "@relation " + name + "\n";
            foreach (string attribute in attributes)
            {
                ARFF += attribute + "\n";
            }
            ARFF += "@data" + "\n";
            foreach (string datum in data)
            {
                ARFF += datum + "\n";
            }
            return new Dictionary<string, string>()
            {
                {"data", ARFF},
                {"count", data.Count.ToString() }
            };
        }
        public static J48 ByARFF(string ARFF, string count)
        {
            java.io.StringReader ArffReader = new java.io.StringReader(ARFF);
            ArffLoader.ArffReader ARFFData = new ArffLoader.ArffReader(ArffReader,Convert.ToInt32(count),true);
            Instances structure = ARFFData.getStructure();
            structure.setClassIndex(structure.numAttributes() - 1);
            Instance inst;
            while ((inst = ARFFData.readInstance(structure))!=null)
            {
                structure.add(inst);
            }
            //Instances data = ARFFData.getData();
            //J48 cls = new J48();
            //cls.buildClassifier(data);
            //String[] options = new String[1];
            //options[0] = "-U"; // unpruned tree 
            J48 tree = new J48(); // new instance of tree 
            //tree.setOptions(options); // set the options 
            tree.buildClassifier(structure); // build classifier

            return tree;
        }

        public static J48 ByHeaderAndData(string header, string dataARFF)
        {
            java.io.StringReader ArffReader = new java.io.StringReader(header);
            ArffLoader.ArffReader ARFFData = new ArffLoader.ArffReader(ArffReader, 100, false);
            Instances structure = ARFFData.getStructure();
            structure.setClassIndex(structure.numAttributes() - 1);

            ArffReader = new java.io.StringReader(dataARFF);
            Instances data = ARFFData.getData();

            Instance inst;
            while ((inst = ARFFData.readInstance(data)) != null)
            {
                structure.add(inst);
            }
            J48 tree = new J48(); // new instance of tree 
            tree.buildClassifier(structure); // build classifier

            return tree;
        }

        static List<string> sNumbers = new List<string>();
        static bool trueOrFalse = false;

        [CanUpdatePeriodically(true)]
        public static List<string> RunNumbers(bool tOrF)
        {
            if(trueOrFalse != tOrF)
            {
                sNumbers = new List<string>();
            }
            Random random = new Random();
            int randomNumber = random.Next(0, 100);
            int otherRandommNumber = random.Next(0, 100);
            string PeriodicNumber = randomNumber + "," + (randomNumber < otherRandommNumber ? "no" : "yes");
            if(sNumbers.Count == 100)
            {
                sNumbers.RemoveAt(0);
            }
            sNumbers.Add(PeriodicNumber);
            trueOrFalse = tOrF;
            return sNumbers;
        }

        public static string ReturnGraph(J48 j48data)
        {
            return j48data.graph();
        }
        public static string ReturnGlobalInfo(J48 j48data)
        {
            return j48data.globalInfo();
        }
        public static string ReturnPrefix(J48 j48data)
        {
            return j48data.prefix();
        }
        public static string ReturnIfThen(J48 j48data)
        {
            return j48data.toSource("wekaIf");
        }
        public static string ReturnToString(J48 j48data)
        {
            return j48data.toString();
        }
        public static string ReturnToSummaryString(J48 j48data)
        {
            return j48data.toSummaryString();
        }

        public static float getConfidenceFactor(J48 j48data)
        {
            return j48data.getConfidenceFactor();
        }
    }

}
