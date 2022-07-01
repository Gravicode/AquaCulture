using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AquaCulture.Web.Helpers
{
    public class LogicEvaluator
    {/// <summary>
     /// Evaluates a dynamically built logical expression 
     /// Implement try catch when calling this function : if an exception is thrown, the logical expression is not valid
     /// </summary>
     /// <param name="logicalExpression">True AND False OR True</param>
     /// <returns></returns>
        public static bool EvaluateLogicalExpression(string logicalExpression)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            table.Columns.Add("", typeof(bool));
            table.Columns[0].Expression = logicalExpression;

            System.Data.DataRow r = table.NewRow();
            table.Rows.Add(r);
            bool result = (Boolean)r[0];
            return result;
        }
    }
}
