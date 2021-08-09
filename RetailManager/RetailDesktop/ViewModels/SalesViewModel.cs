using Caliburn.Micro;
using RetailDesktop.Library.Api;
using RetailDesktop.Library.Models;
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
        private BindingList<ProductModel> _products;
        private BindingList<ProductModel> _cart;
        private int _quantity;
        private IProductEndpoint _productEndpoint;
        public SalesViewModel(IProductEndpoint productEndpoint)
        {
            _productEndpoint = productEndpoint;
        }
        private async Task LoadProducts()
        {
            var products = await _productEndpoint.GetAll();
            Products = new BindingList<ProductModel>(products);
        }
        public BindingList<ProductModel> Products
        {
            get { return _products; }
            set { _products = value; NotifyOfPropertyChange(() => Products); }
        }
        public BindingList<ProductModel> Cart
        {
            get { return _cart; }
            set { _cart = value; }
        }
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

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
        }
    }
}
