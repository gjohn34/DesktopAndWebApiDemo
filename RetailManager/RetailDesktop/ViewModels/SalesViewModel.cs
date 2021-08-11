using Caliburn.Micro;
using RetailDesktop.Library.Api;
using RetailDesktop.Library.Helpers;
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
        // Backers
        private BindingList<ProductModel> _products;
        private BindingList<CartItemModel> _cart;
        private int _quantity = 1;
        private ProductModel _selectedProduct;
        private CartItemModel _selectedRemove;
        IProductEndpoint _productEndpoint;
        IConfigHelper _configHelper;
        
        // Constructors
        public SalesViewModel(IProductEndpoint productEndpoint, IConfigHelper configHelper)
        {
            _productEndpoint = productEndpoint;
            _configHelper = configHelper;
        }

        // Props
        public CartItemModel SelectedRemove
        {
            get { return _selectedRemove; }
            set {
                _selectedRemove = value;
                NotifyOfPropertyChange(() => SelectedRemove);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
                }
        }
        public ProductModel SelectedProduct
        {
            get { return _selectedProduct; }
            set { 
                _selectedProduct = value;
                Quantity = 1;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => Quantity);
                NotifyOfPropertyChange(() => CanAddToCart);
                }
        }
        public BindingList<ProductModel> Products
        {
            get { return _products; }
            set { _products = value; NotifyOfPropertyChange(() => Products); }
        }
        public BindingList<CartItemModel> Cart
        {
            get { return _cart; }
            set { _cart = value; }
        }
        public int Quantity
        {
            get { return _quantity; }
            set { 
                _quantity = value; 
                NotifyOfPropertyChange(() => Quantity); 
                NotifyOfPropertyChange(() => CanAddToCart); }
        }
        public string SubTotal
        {
            get
            {

                return CalculateSubTotal().ToString("C");

            }
        }
        public string Tax
        {
            get
            {

                return CalculateTax().ToString("C");
            }
        }
        public string Total
        {
            get { return (CalculateSubTotal() + CalculateTax()).ToString("C"); }

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
        // functions
        private async Task LoadProducts()
        {
            var products = await _productEndpoint.GetAll();
            Products = new BindingList<ProductModel>(products);
        }
        private decimal CalculateSubTotal()
        {
            decimal subTotal = 0;

            if (Cart != null)
            {
                foreach (CartItemModel item in Cart)
                {
                    subTotal += (item.Product.RetailPrice * item.QuantityInCart);
                }
            }
            return subTotal;
        }
        private decimal CalculateTax()
        {
            decimal tax = 0;
            decimal taxRate = _configHelper.GetTaxRate()/100;
            if (Cart != null)
            {
                foreach (CartItemModel item in Cart)
                {
                    if (item.Product.IsTaxable)
                    {
                        tax += ((item.Product.RetailPrice * item.QuantityInCart) * taxRate);
                    }
                }
            }
            return tax;
        }

        // Events   
        public void AddToCart()
        {
            CartItemModel cartItem = Cart.FirstOrDefault(item => item.Product.Id == SelectedProduct.Id);
            if (cartItem != null)
            {
                cartItem.QuantityInCart += Quantity;
                // TODO Fix this vv
                Cart.Remove(cartItem);
                Cart.Add(cartItem);
            } else
            {
                Cart.Add(new CartItemModel(SelectedProduct, Quantity));

            }
            SelectedProduct.QuantityInStock -= Quantity;
            if (SelectedProduct.QuantityInStock <= 0)
            {
                Products.Remove(SelectedProduct);
                SelectedProduct = Products.FirstOrDefault();
            }
            Quantity = 1;
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => Cart);
        }
        public void RemoveFromCart()
        {
            ProductModel product = Products.FirstOrDefault(p => p.Id == SelectedRemove.Product.Id);
            if (product == null)
            {
                Products.Add(SelectedRemove.Product);
                // TODO - SORT
            }
            SelectedRemove.Product.QuantityInStock += SelectedRemove.QuantityInCart;
            Cart.Remove(SelectedRemove);
            SelectedRemove = Cart.FirstOrDefault();
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => Cart);

        }
        public void Checkout()
        {
            throw new NotImplementedException();
        }

        // Listeners
        public bool CanAddToCart
        {
            get
            {
                if (SelectedProduct != null)
                {
                    int result = 0;
                    try
                    {
                        Int32.TryParse(Quantity.ToString(), out result);
                        if (result <= SelectedProduct.QuantityInStock && Quantity > 0)
                        {
                            return true;
                        }
                    } catch (Exception error)
                    {
                        Console.WriteLine(error);
                        return false;
                    }
                }
                return false;
            }
        }
        public bool CanRemoveFromCart
        {
            get
            {
                if (SelectedRemove != null)
                {
                    return true;
                }
                return false;
            }
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            Cart = new BindingList<CartItemModel>();
            await LoadProducts();
        }
    }
}
