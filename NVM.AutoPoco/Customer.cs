using System;

namespace NVM.AutoPoco
{
    public class Customer
    {
        #region Fields

        public string Password { get; private set; }

        #endregion

        #region Methods


        public void SetPassword(string password)
        {
            Password = password;
        }

        #endregion

        #region Properties

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ShippingAddress { get; set; }
        public string BillingAddress { get; set; }
        public string EmailAddress { get; set; }
        public string Telephone { get; set; }

        public Order[] Orders { get; set; }

        #endregion
    }

    public class Order
    {
        public string Number { get; set; }
        public string Product { get; set; }
        public decimal Total { get; set; }
    }
}