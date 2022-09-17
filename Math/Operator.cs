using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MathLibrary
{

    
    public class Operator
    {
        public string Class { get; set; }
        public int Precedence { get; set; }
        public string Symbol { get; set; }
    }

    //class a
    //{
    //    List<Operator> op = new List<Operator>();
    
    //    void test()
    //    {
    //        op.Where(x => x.Precedence == 1).First();
    //    }
    //}
}