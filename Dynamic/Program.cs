using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

namespace ProviderDemo
{
    internal class CsvLine : DynamicObject
    {
        //public string Mrn { get; set; }
        //public string Name { get; set; }
        //public int Age { get; set; }
        //public string Location { get; set; }
        //public override string ToString()
        //{
        //    return $"{Mrn},{Name},{Age},{Location}";
        //}
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var headerFields = header.Split(',').ToList();
            var index = headerFields.IndexOf(binder.Name);
            if (index != -1)
            {
                var lineContents = lineContent.Split(',');
                result = lineContents[index];
                return true;
            }
            result = null;
            return false;
        }

        public string header;
        public string lineContent;
    }

    class CsvProvider
    {
        private string filePath;
        public CsvProvider(string filePath)
        {
            this.filePath = filePath;
        }
        public IEnumerable<CsvLine> GetCsvLines()
        {
            var lines = new List<CsvLine>();
            System.IO.StreamReader _r = new System.IO.StreamReader(filePath);
            try
            {
                string header = _r.ReadLine();
                while (!_r.EndOfStream)
                {
                    CsvLine line = new CsvLine();
                    line.header = header;
                    line.lineContent = _r.ReadLine();
                    lines.Add(line);
                }
            }
            finally
            {
                _r.Close();
            }
            return lines;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            var provider = new CsvProvider("..//..//Patients.csv");

            var lines = provider.GetCsvLines();
            var result = lines.Where((dynamic p) => p.Location == "blr");
            foreach (dynamic item in lines)
            {
                Console.WriteLine(item.Mrn);
            }
        }
    }

    class ElasticType : DynamicObject
    {
        Dictionary<string, object> _stateBag = new Dictionary<string, object>();

        //set accessor
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _stateBag[binder.Name] = value;
            return true;
        }

        //get accessor
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;
            if (this._stateBag.ContainsKey(binder.Name))
            {
                result = this._stateBag[binder.Name];
                return true;
            }
            return false;
        }

    }


}