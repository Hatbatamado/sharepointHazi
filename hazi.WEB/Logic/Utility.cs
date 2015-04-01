using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace hazi.WEB.Logic
{
    public class Utility
    {
        /// <summary>
        /// GridView sor értékék kiolvasásához
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static IOrderedDictionary GetValues(GridViewRow row)
        {
            IOrderedDictionary values = new OrderedDictionary();
            foreach (DataControlFieldCell cell in row.Cells)
            {
                if (cell.Visible)
                {
                    // Extract values from the cell.
                    cell.ContainingField.ExtractValuesFromCell(values, cell, row.RowState, true);
                }
            }
            return values;
        }

        public static string EkezetEltavolitas(string szoveg)
        {
            //ékezet eltávolítása, mivel db-ben is úgy van
            string seged = szoveg;
            byte[] temp;
            temp = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(seged);
            seged = System.Text.Encoding.UTF8.GetString(temp);

            return seged;
        }
    }
}