using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailDesktop.ViewModels
{
    public class SalesViewModel : Screen
    {
        private BindingList<string> _products;
        private BindingList<string> _cart;
        public BindingList<string> Products
        {
            get { return _products; }
            set { Products = value; }
        }
        public BindingList<string> Cart
        {
            get { return _cart; }
            set { Cart = value; }
        }

        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; NotifyOfPropertyChange(() => Quantity); }
        }
        public string SubTotal
        {
            get { return "$0.00"; }
        }
        public string Tax
        {
            get { return "$0.00"; }
        }
        public string Total
        {
            get { return "$0.00"; }

        }
        public bool CanAddToCart
        {
            get
            {
                bool output = false;
                if (Quantity > 0) //And item selected in products )
                {
                    output = true;
                }
                return output;
            }
        }

        public bool CanRemoveFromCart
        {
            get
            {
                bool output = false;
                if (Quantity > 0) // And item selected from Cart )
                {
                    output = true;
                }
                return output;
            }
        }

        public bool CanCheckOut
        {
            get
            {
                bool output = false;
                if (true) // Cart.Length > 0 ??
                {
                    output = true;
                }
                return output;
            }

        }

        // Events

        public void AddToCart()
        {
            throw new NotImplementedException();
        }

        public void RemoveFromCart()
        {
            throw new NotImplementedException();
        }

        public void Checkout()
        {
            throw new NotImplementedException();
        }


        //public BindingList<string> Products
        //{
        //    get { return _products; }
        //    set { _products = value; NotifyOfPropertyChange(() => Products);}
        //}





    }
}
