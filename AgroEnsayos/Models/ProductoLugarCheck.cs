using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgroEnsayos.Models
{
   public class ProductoLugarCheck
    {
        public int ProductoId { get; set; }
        public int LugarId { get; set; }
        public string Region { get; set; }
        public bool IsChecked { get; set; }

        public ProductoLugarCheck(int prodId, int lugId, string region, bool isChecked)
        {
            this.ProductoId = prodId;
            this.LugarId = lugId;
            this.Region = region;
            this.IsChecked = isChecked;
        }
       
       public ProductoLugarCheck()
        {

        }
    }
}
