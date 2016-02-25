namespace FillTheDocDb.Utils
{
    using System.Collections.Generic;
    using System.IO;
    using Models;

    public static class Utils
    {
        public static List<string> ReadFileToList(string filename)
        {
            var lines = new List<string>();
            using (var reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }
            return lines;
        }

        public static List<IndexModel> GetModelList(List<string> rawDataList)
        {
            var datalist = new List<IndexModel>();
            foreach (var row in rawDataList)
            {
              datalist.Add(IndexModel.NewIndexModel(row));
            }
            return datalist;
        }
    }
}
