﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RetailManager.Library.Internal.DataAccess;
using RetailManager.Library.Models;

namespace RetailManager.Library.DataAccess
{
    public class UserData
    {
        public UserModel GetUserById(string Id)
        {
            SqlDataAccess sql = new SqlDataAccess();

            // Anonymous object, no declared Type
            var p = new { Id };

            // LOGGING IN WRONG THROWS ERROR
            return sql.LoadData<UserModel, dynamic>("dbo.spUserLookup", p, "RetailManager").First();
                
        }
    } 
}
