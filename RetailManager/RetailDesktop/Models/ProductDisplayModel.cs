using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailDesktop.Models
{
    public class ProductDisplayModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal RetailPrice { get; set; }
        private int _quantityInStock;

        public int QuantityInStock
        {
            get { return _quantityInStock; }
            set { 
                _quantityInStock = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(QuantityInStock)));
            }
        }

        public bool IsTaxable { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
