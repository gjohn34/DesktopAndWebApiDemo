using AutoMapper;
using Caliburn.Micro;
using RetailDesktop.Library.Api;
using RetailDesktop.Library.Helpers;
using RetailDesktop.Library.Models;
using RetailDesktop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RetailDesktop.ViewModels
{
    public class SalesViewModel : Screen
    {
        // Backers
        private BindingList<ProductDisplayModel> _products;
        private BindingList<CartItemDisplayModel> _cart;
        private int _quantity = 0;
        private ProductDisplayModel _selectedProduct;
        private CartItemDisplayModel _selectedRemove;
        readonly IProductEndpoint _productEndpoint;
        readonly ISaleEndpoint _saleEndpoint;
        readonly IConfigHelper _configHelper;
        readonly IMapper _mapper;
        private readonly StatusInfoViewModel _status;
        private readonly IWindowManager _window;
        
        // Constructors
        public SalesViewModel(IProductEndpoint productEndpoint, IConfigHelper configHelper, 
            ISaleEndpoint saleEndpoint, IMapper mapper, StatusInfoViewModel status, IWindowManager window)
        {
            _productEndpoint = productEndpoint;
            _saleEndpoint = saleEndpoint;
            _configHelper = configHelper;
            _mapper = mapper;
            _status = status;
            _window = window;
        }

        // Props
        public CartItemDisplayModel SelectedRemove
        {
            get { return _selectedRemove; }
            set {
                _selectedRemove = value;
                NotifyOfPropertyChange(() => SelectedRemove);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
                }
        }
        public ProductDisplayModel SelectedProduct
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
        public BindingList<ProductDisplayModel> Products
        {
            get { return _products; }
            set { _products = value; NotifyOfPropertyChange(() => Products); }
        }
        public BindingList<CartItemDisplayModel> Cart
        {
            get { return _cart; }
            set { 
                _cart = value;
                NotifyOfPropertyChange(() => SubTotal);
                NotifyOfPropertyChange(() => Tax);
                NotifyOfPropertyChange(() => Total);
            }
        }
        public int Quantity
        {
            get { return _quantity; }
            set { 
                _quantity = value; 
                NotifyOfPropertyChange(() => Quantity); 
                NotifyOfPropertyChange(() => CanAddToCart);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
            }
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
        // functions
        private async Task LoadProducts()
        {
            var productsList = await _productEndpoint.GetAll();
            var products = _mapper.Map<List<ProductDisplayModel>>(productsList);
            Products = new BindingList<ProductDisplayModel>(products);
            NotifyOfPropertyChange(() => Products);
        }
        private decimal CalculateSubTotal()
        {
            decimal subTotal = 0;

            if (Cart != null)
            {
                foreach (CartItemDisplayModel item in Cart)
                {
                    subTotal += (item.Product.RetailPrice * item.QuantityInCart);
                }
            }
            return subTotal;
        }
        private decimal CalculateTax()
        {
            decimal tax = 0;
            decimal taxRate = _configHelper.GetTaxRate() / 100;
            if (Cart != null)
            {
                foreach (CartItemDisplayModel item in Cart)
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
            CartItemDisplayModel cartItem = Cart.FirstOrDefault(item => item.Product.Id == SelectedProduct.Id);
            if (cartItem != null)
            {
                cartItem.QuantityInCart += Quantity;
            } else
            {
                Cart.Add(new CartItemDisplayModel(SelectedProduct, Quantity));

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
            NotifyOfPropertyChange(() => CanCheckOut);
        }
        public void RemoveFromCart()
        {
            ProductDisplayModel product = Products.FirstOrDefault(p => p.Id == SelectedRemove.Product.Id);
            if (product == null)
            {
                Products.Add(SelectedRemove.Product);
                // TODO - SORT
            }
            SelectedRemove.Product.QuantityInStock += (Quantity > SelectedRemove.QuantityInCart ? SelectedRemove.QuantityInCart : Quantity);
            SelectedRemove.QuantityInCart -= (Quantity > SelectedRemove.QuantityInCart ? SelectedRemove.QuantityInCart : Quantity);
            if (SelectedRemove.QuantityInCart <= 0)
            {
                Cart.Remove(SelectedRemove);
            }
            SelectedRemove = Cart.FirstOrDefault();
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => Cart);
            NotifyOfPropertyChange(() => CanCheckOut);
        }
        public async Task CheckOut()
        {
            SaleModel sale = new SaleModel();

            foreach (CartItemDisplayModel item in Cart)
            {
                sale.SaleDetails.Add(new SaleDetailModel(item.Product.Id, item.QuantityInCart));
            }
            await _saleEndpoint.PostSale(sale);
            Cart = new BindingList<CartItemDisplayModel>();
            await LoadProducts();

            NotifyOfPropertyChange(() => Cart);
            // update products
            //await LoadProducts();
            //Cart = new BindingList<CartItemDisplayModel>();
            //throw new NotImplementedException();
        }

        // Listeners
        public bool CanCheckOut
        {
            get { return Cart.Count > 0; }

        }
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
                if (SelectedRemove == null || Quantity <= 0)
                {
                    return false;
                }
                return true;
            }
        }
        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            Cart = new BindingList<CartItemDisplayModel>();
            try
            {
                await LoadProducts();
            }
            catch (Exception ex)
            {
                dynamic settings = new ExpandoObject();
                settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                settings.ResizeMode = ResizeMode.NoResize;
                settings.Title = "An error has occurred";

                _status.UpdateMessage("Unauthorized", "Cashiers Only");

                await TryCloseAsync();
                // open up dashboard page
                
                await _window.ShowDialogAsync(_status, null, settings);

            }
        }
    }
}
