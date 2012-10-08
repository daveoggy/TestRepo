using MegaProject.Data.Entities;

namespace MegaProject.Utilities
{
    public static class EntitiesHelpers
    {
        public static void Update(this Customer customer, CustomerAudit change)
        {
            customer.CompanyName = change.CompanyName;
            customer.ContactName = change.ContactName;
            customer.ContactTitle = change.ContactTitle;
            customer.Address = change.Address;
            customer.City = change.City;
            customer.Region = change.Region;
            customer.Phone = change.Phone;
            customer.PostalCode = change.PostalCode;
            customer.Country = change.Country;
            customer.Fax = change.Fax;
            customer.Bool = change.Bool;
        }
    }
}
