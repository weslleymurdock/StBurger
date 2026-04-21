namespace StBurger.Application.Core.Constants;

public static class Routes
{
    public static class Base
    {
        public const string Api = "/api";
        public const string Version = "/v1";
    }

    public static class Menu
    {
        public const string Base = $"{Routes.Base.Api}{Routes.Base.Version}/menu";
        public const string Tag = "Catalog";
    }

    public static class Order
    {
        public const string Base = $"{Routes.Base.Api}{Routes.Base.Version}/order";
        public const string Tag = "Orders";
    }
}
