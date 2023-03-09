using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Consts
{
    public class Consts
    {
        public static class OrderBy
        {
            public const string Ascending = "ASC";
            public const string Descending = "DESC";
        }


        public static class ICType
        {
            public const string Equity = "Equity";
            public const string MoneyMarket = "MoneyMarket";
        }


        public static class UserRoles
        {
            public const string Admin = "Admin";
            public const string User = "User";
        }

    }
}