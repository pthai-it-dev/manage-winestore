﻿
using ManageWineStore.app.BussinessClasses;
using ManageWineStore.app.Controllers.ModelControllers;
using ManageWineStore.app.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManageWineStore.app.Controllers
{
    class LoginController
    {
        private AccountController accountController;
        private AdminController adminController = null;
        private EmployeeController employeeController = null;
        private AccountModel accountModel = null;

        public LoginController()
        {
            this.accountController = new AccountController();
            this.adminController = new AdminController();
            this.employeeController = new EmployeeController();
        }

        internal AccountModel AccountModel { get => accountModel; set => accountModel = value; }

        public Object login(string username, string password)
        {
            string hashPassword = MD5Helper.GetHash(password);
            DataTable dataTable = accountController.findByUsernameAndPassword(username, hashPassword);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                this.AccountModel = new AccountModel(dataRow);
                if (dataRow["role_id"].ToString() == "2")
                {
                    return new EmployeeModel(employeeController.find("account_id", dataRow["id"].ToString()).Rows[0]);
                }

                return new AdminModel(adminController.find("account_id", dataRow["id"].ToString()).Rows[0]);
            }

            return null;
        }
    }
}
