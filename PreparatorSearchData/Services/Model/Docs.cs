using System;
using System.Collections.Generic;
using System.Text;

namespace PreparatorSearchData.Services.Model
{
    public class Docs
    {
        public string DocumentName { get; set; }
        public double VectorLength { get; set; }
        public List<TfIdfWord> ThIdfWords { get; set; }
    }
}
