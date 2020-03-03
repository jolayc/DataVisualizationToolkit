using System.Collections.Generic;
using System.IO;

/*
 * Class used for parsing CSV files
 */
namespace TimeSeriesExtension
{

    public class DataParser
    {
        private string DataString;

        public DataParser(string dataString)
        {
            DataString = dataString;
        }

        /**
         * @return List<float> of values at a given index
         * in a .csv file (represented by a string)
         */
        public List<float> GetListFromColumn(int index)
        {
            List<float> listOfValues = new List<float>();
            using (var reader = new StringReader(DataString))
            {
                while (true)
                {
                    var line = reader.ReadLine();
                    if (line != null)
                    {
                        var values = line.Split(',');
                        var value = float.Parse(values[index]);
                        listOfValues.Add(value);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return listOfValues;
        }
        public List<string> GetTimePoints(int index)
        {
            List<string> listOfValues = new List<string>();
            using (var reader = new StringReader(DataString))
            {
                while (true)
                {
                    var line = reader.ReadLine();
                    if (line != null)
                    {
                        var values = line.Split(',');

                        var value = values[index];

                        listOfValues.Add(value);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return listOfValues;
        }
    }
}
