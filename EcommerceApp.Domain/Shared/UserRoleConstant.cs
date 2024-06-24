namespace EcommerceApp.Domain.Shared
{
    public static class UserRoleConstant
    {
        public const string Customer = "Customer";
        public const string Admin = "Admin";

        public static string NormalizedAdmin
        {
            get => Admin.Normalize();
        }
        public static string NormalizedCustomer
        {
            get => Customer.Normalize();
        }

    }
}
