using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public interface IAnalysis
    {
       ArrayList alys(string content, string path, string vid);
    }
}
