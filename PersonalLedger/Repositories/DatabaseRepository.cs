using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PersonalLedger.DomainModels;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Web.Security.AntiXss;
using PersonalLedger.Controllers;
using NLog;

namespace PersonalLedger.Repositories
{
    public class DatabaseRepository : IDatabaseRepository
    {
        #region Fields
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static ConnectionStringSettings cnxnString = ConfigurationManager.ConnectionStrings["LedgerConnectionDev"];
        private int user;
        #endregion
        public DatabaseRepository()
        {
            try
            {
                int? userIdFromSession = (int?)HttpContext.Current.Session["UserId"];
                if(userIdFromSession == null)
                {
                    throw new ArgumentNullException("userIdFromSession");
                }
                else
                {
                    user = (int)userIdFromSession;
                }
            }
            catch (Exception e)
            {
                HandleException(e, "constructor", "");
            }
        }  //constructor
        #region Accounts
        public List<Account> Accounts()
        {
            List<Account> accounts = new List<Account>();
            int id = 0;
            DateTime? dateClosed = null;
            DateTime dateOpened = DateTime.Today;
            string institution = "";
            decimal? interestRate = null;
            decimal? limit = null;
            string name = "";
            string number = "";
            decimal openingBalance = 0;
            int type = 0;
            string cmdString = "SELECT * FROM Accounts WHERE UserId = " + user;
            using (SqlConnection cnxn = new SqlConnection(cnxnString.ConnectionString))
            {
                try
                {
                    cnxn.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdString, cnxn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                id = (int)reader["ID"];
                            }

                            if (!reader.IsDBNull(1))
                            {
                                dateClosed = (DateTime)reader["DateClosed"];
                            }

                            if (!reader.IsDBNull(2))
                            {
                                dateOpened = (DateTime)reader["DateOpened"];
                            }

                            if (!reader.IsDBNull(3))
                            {
                                institution = SanitizeHTML((string)reader["Institution"]).Trim();
                            }

                            if (!reader.IsDBNull(4))
                            {
                                interestRate = (decimal)reader["InterestRate"];
                            }

                            if (!reader.IsDBNull(5))
                            {
                                limit = (decimal)reader["Limit"];
                            }

                            if (!reader.IsDBNull(6))
                            {
                                name = SanitizeHTML((string)reader["Name"]).Trim();
                            }

                            if (!reader.IsDBNull(7))
                            {   
                                number = SanitizeHTML((string)reader["Number"]).Trim();
                            }

                            if (!reader.IsDBNull(8))
                            {
                                openingBalance = (decimal)reader["OpeningBalance"];
                            }

                            if (!reader.IsDBNull(9))
                            {
                                type = (int)reader["Type"];
                            }

                            List<Transaction> transactions = TransactionsForAccount(id);
                            accounts.Add(new Account(id, dateClosed, dateOpened, institution, interestRate, limit, name, number, openingBalance, type, transactions));
                        }
                    }  //using command
                }
                catch (Exception e)
                {
                    HandleException(e, "Accounts", null);
                }
                finally
                {
                    if (cnxn.State == ConnectionState.Open)
                    {
                        cnxn.Close();
                    }
                }
                return accounts;
            }  //using connection
        }  //Accounts

         public Account Account(int id)
        {
            Account account = new Account();
            List<Account> accounts = new List<Account>();
            DateTime? dateClosed = null;
            DateTime dateOpened = DateTime.Today;
            string institution = "";
            decimal? interestRate = null;
            decimal? limit = null;
            string name = "";
            string number = "";
            decimal openingBalance = 0;
            int type = 0;
            string cmdString = "SELECT * FROM Accounts WHERE ID = " + id;
            using (SqlConnection cnxn = new SqlConnection(cnxnString.ConnectionString))
            {
                try
                {
                    cnxn.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdString, cnxn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(1))
                            {
                                dateClosed = (DateTime)reader["DateClosed"];
                            }

                            if (!reader.IsDBNull(2))
                            {
                                dateOpened = (DateTime)reader["DateOpened"];
                            }

                            if (!reader.IsDBNull(3))
                            {
                                institution = SanitizeHTML((string)reader["Institution"]).Trim();
                            }

                            if (!reader.IsDBNull(4))
                            {
                                interestRate = (decimal)reader["InterestRate"];
                            }

                            if (!reader.IsDBNull(5))
                            {
                                limit = (decimal)reader["Limit"];
                            }

                            if (!reader.IsDBNull(6))
                            {
                                name = SanitizeHTML((string)reader["Name"]).Trim();
                            }

                            if (!reader.IsDBNull(7))
                            {
                                number = SanitizeHTML((string)reader["Number"]).Trim();
                            }

                            if (!reader.IsDBNull(8))
                            {
                                openingBalance = (decimal)reader["OpeningBalance"];
                            }

                            if (!reader.IsDBNull(9))
                            {
                                type = (int)reader["Type"];
                            }

                            List<Transaction> transactions = TransactionsForAccount(id);
                            return new Account(id, dateClosed, dateOpened, institution, interestRate, limit, name, number, openingBalance, type, transactions);
                        }
                    }  //using command
                }
                catch (Exception e)
                {
                    HandleException(e, "Account", null);
                }
                finally
                {
                    if (cnxn.State == ConnectionState.Open)
                    {
                        cnxn.Close();
                    }
                }
                return account;
            }  //using connection
        }  //Account

        public Dictionary<int, string> AccountDictionary()
        {
            Dictionary<int, string> accounts = new Dictionary<int, string>();
            int id = 0;
            DateTime? dateClosed = null;
            DateTime dateOpened = DateTime.Today;
            string institution = "";
            decimal? interestRate = null;
            decimal? limit = null;
            string name = "";
            string number = "";
            decimal openingBalance = 0;
            int type = 0;
            string cmdString = "SELECT * FROM Accounts WHERE UserId = " + user;
            using (SqlConnection cnxn = new SqlConnection(cnxnString.ConnectionString))
            {
                try
                {
                    cnxn.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdString, cnxn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                id = (int)reader["ID"];
                            }

                            if (!reader.IsDBNull(1))
                            {
                                dateClosed = (DateTime)reader["DateClosed"];
                            }

                            if (!reader.IsDBNull(2))
                            {
                                dateOpened = (DateTime)reader["DateOpened"];
                            }

                            if (!reader.IsDBNull(3))
                            {
                                institution = SanitizeHTML((string)reader["Institution"]).Trim();
                            }

                            if (!reader.IsDBNull(4))
                            {
                                interestRate = (decimal)reader["InterestRate"];
                            }

                            if (!reader.IsDBNull(5))
                            {
                                limit = (decimal)reader["Limit"];
                            }

                            if (!reader.IsDBNull(6))
                            {
                                name = SanitizeHTML((string)reader["Name"]).Trim();
                            }

                            if (!reader.IsDBNull(7))
                            {
                                number = SanitizeHTML((string)reader["Number"]).Trim();
                            }

                            if (!reader.IsDBNull(8))
                            {
                                openingBalance = (decimal)reader["OpeningBalance"];
                            }

                            if (!reader.IsDBNull(9))
                            {
                                type = (int)reader["Type"];
                            }

                            accounts.Add(id, name);
                        }
                    }  //using command
                }
                catch (Exception e)
                {
                    HandleException(e, "AccountDictionary", null);
                }
                finally
                {
                    if (cnxn.State == ConnectionState.Open)
                    {
                        cnxn.Close();
                    }
                }
                return accounts;
            }  //using connection
        }  //AccountDictionary

        public int AddAccount(Account a)
        {
            int insertedId = -1;
            string cmdString = "INSERT INTO Accounts (DateClosed, DateOpened, Institution, InterestRate, Limit, Name, Number, OpeningBalance, Type, UserId) OUTPUT INSERTED.ID VALUES (@dateClosed, @dateOpened, @institution, @interestRate, @limit, @name, @number, @openingBalance, @type, @userId);";
            using (SqlConnection cnxn = new SqlConnection(cnxnString.ConnectionString))
            {
                try
                {
                    cnxn.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdString, cnxn))
                    {
                        if(a.DateClosed == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("dateClosed", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("dateClosed", a.DateClosed));
                        }
                        cmd.Parameters.Add(new SqlParameter("dateOpened", a.DateOpened));
                        cmd.Parameters.Add(new SqlParameter("institution", a.Institution));
                        if (a.InterestRate == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("interestRate", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("interestRate", a.InterestRate));
                        }
                        if (a.Limit == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("limit", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("limit", a.Limit));
                        }
                        cmd.Parameters.Add(new SqlParameter("name", a.Name));
                        cmd.Parameters.Add(new SqlParameter("number", a.Number));
                        cmd.Parameters.Add(new SqlParameter("openingBalance", a.OpeningBalance));
                        cmd.Parameters.Add(new SqlParameter("type", a.TypeId));
                        cmd.Parameters.Add(new SqlParameter("userId", user));
                        insertedId = (int)cmd.ExecuteScalar();
                    }  //using command
                }
                catch (Exception e)
                {
                    HandleException(e, "AddAccount", null);
                }
                finally
                {
                    if (cnxn.State == ConnectionState.Open)
                    {
                        cnxn.Close();
                    }
                }
                return insertedId;
            }  //using connection
        }  //AddAccount

        public void DeleteAccount(int acctId)
        {
            string cmdString = "DELETE FROM Accounts WHERE ID = " + acctId;
            using (SqlConnection cnxn = new SqlConnection(cnxnString.ConnectionString))
            {
                try
                {
                    cnxn.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdString, cnxn))
                    {
                        cmd.ExecuteNonQuery();
                    }  //using command
                }
                catch (Exception e)
                {
                    HandleException(e, "DeleteAccount", null);
                }
            }  //using connection
        }  //DeleteAccount

        public void UpdateAccount(Account a)
        {
            string cmdString = "UPDATE Accounts SET DateClosed = @dateClosed, DateOpened = @dateOpened, Institution = @institution, InterestRate = @interestRate, Limit = @limit, Name = @name, Number = @number, OpeningBalance = @openingBalance, Type = @type, UserId = @userId WHERE ID = @id;";
            using (SqlConnection cnxn = new SqlConnection(cnxnString.ConnectionString))
            {
                try
                {
                    cnxn.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdString, cnxn))
                    {
                        cmd.Parameters.Add(new SqlParameter("id", a.Id));
                        if (a.DateClosed == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("dateClosed", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("dateClosed", a.DateClosed));
                        }
                        cmd.Parameters.Add(new SqlParameter("dateOpened", a.DateOpened));
                        cmd.Parameters.Add(new SqlParameter("institution", a.Institution));
                        if (a.InterestRate == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("interestRate", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("interestRate", a.InterestRate));
                        }
                        if (a.Limit == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("limit", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("limit", a.Limit));
                        }
                        cmd.Parameters.Add(new SqlParameter("name", a.Name));
                        cmd.Parameters.Add(new SqlParameter("number", a.Number));
                        cmd.Parameters.Add(new SqlParameter("openingBalance", a.OpeningBalance));
                        cmd.Parameters.Add(new SqlParameter("type", a.TypeId));
                        cmd.Parameters.Add(new SqlParameter("userId", user));
                        cmd.ExecuteNonQuery();
                    }  //using command
                }
                catch (Exception e)
                {
                    HandleException(e, "UpdateAccount", null);
                }
                finally
                {
                    if (cnxn.State == ConnectionState.Open)
                    {
                        cnxn.Close();
                    }
                }
            }  //using connection
        }  //UpdateAccount
        #endregion
        #region Categories
        public List<Category> Categories()
        {
            List<Category> categories = new List<Category>();
            int id = 0;
            string name = "";
            bool tax = false;
            int type = 0;
            string cmdString = "SELECT * FROM Categories WHERE UserId = " + user;
            using (SqlConnection cnxn = new SqlConnection(cnxnString.ConnectionString))
            {
                try
                {
                    cnxn.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdString, cnxn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                id = (int)reader["ID"];
                            }
                            if (!reader.IsDBNull(1))
                            {
                                name = SanitizeHTML((string)reader["Name"]).Trim();
                            }
                            if (!reader.IsDBNull(2))
                            {
                                tax = (bool)reader["Tax"];
                            }
                            if (!reader.IsDBNull(3))
                            {
                                type = (int)reader["Type"];
                            }

                            categories.Add(new Category(id, name, tax, type));
                        }
                    }  //using command
                }
                catch (Exception e)
                {
                    HandleException(e, "Categories", null);
                }
                finally
                {
                    if(cnxn.State == ConnectionState.Open)
                    {
                        cnxn.Close();
                    }
                }
                return categories.OrderBy(x => x.Type).ThenBy(x => x.Name).ToList();
            }  //using connection
        }  //Categories

        public int AddCategory(Category c)
        {
            int insertedId = -1;
            string cmdString = "INSERT INTO Categories (Name, Tax, Type, UserId) OUTPUT INSERTED.ID VALUES (@name, @tax, @type, @userId);";
            using (SqlConnection cnxn = new SqlConnection(cnxnString.ConnectionString))
            {
                try
                {
                    cnxn.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdString, cnxn))
                    {
                        cmd.Parameters.Add(new SqlParameter("name", c.Name));
                        cmd.Parameters.Add(new SqlParameter("tax", c.Tax));
                        cmd.Parameters.Add(new SqlParameter("type", c.Type));
                        cmd.Parameters.Add(new SqlParameter("userId", user));
                        insertedId = (int)cmd.ExecuteScalar();
                    }  //using command
                }
                catch (Exception e)
                {
                    HandleException(e, "AddCategory", null);
                }
                finally
                {
                    if (cnxn.State == ConnectionState.Open)
                    {
                        cnxn.Close();
                    }
                }
                return insertedId;
            }  //using connection
        }  //AddCategory

        public void AddCategories(List<Category> categories)
        {
            if(categories != null)
            {
                if(categories.Count > 0)
                {
                    using (SqlConnection cnxn = new SqlConnection(cnxnString.ConnectionString))
                    {
                        foreach (Category c in categories)
                        {
                            string cmdString = "INSERT INTO Categories (Name, Tax, Type, UserId) OUTPUT INSERTED.ID VALUES (@name, @tax, @type, @userId);";
                            try
                            {
                                cnxn.Open();
                                using (SqlCommand cmd = new SqlCommand(cmdString, cnxn))
                                {
                                    cmd.Parameters.Add(new SqlParameter("name", c.Name));
                                    cmd.Parameters.Add(new SqlParameter("tax", c.Tax));
                                    cmd.Parameters.Add(new SqlParameter("type", c.Type));
                                    cmd.Parameters.Add(new SqlParameter("userId", user));
                                    cmd.ExecuteScalar();
                                }  //using command
                            }
                            catch (Exception e)
                            {
                                HandleException(e, "AddCategories", null);
                            }
                            finally
                            {
                                if (cnxn.State == ConnectionState.Open)
                                {
                                    cnxn.Close();
                                }
                            }
                        }  //category iteration
                    }  //using connection
                }  //if count>0
            }  //if not null
        }  //AddCategories

        public void DeleteCategory(int id)
        {
            string cmdString = "DELETE FROM Categories WHERE ID = " + id;
            using (SqlConnection cnxn = new SqlConnection(cnxnString.ConnectionString))
            {
                try
                {
                    cnxn.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdString, cnxn))
                    {
                        cmd.ExecuteNonQuery();
                    }  //using command
                }
                catch (Exception e)
                {
                    HandleException(e, "DeleteCategory", null);
                }
                finally
                {
                    if (cnxn.State == ConnectionState.Open)
                    {
                        cnxn.Close();
                    }
                }
            }  //using connection
        }  //DeleteCategory

        public void UpdateCategory(Category c)
        {
            string cmdString = "UPDATE Categories SET Name = @name, Tax = @tax, Type = @type, UserId = @userId WHERE ID = @id;";
            using (SqlConnection cnxn = new SqlConnection(cnxnString.ConnectionString))
            {
                try
                {
                    cnxn.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdString, cnxn))
                    {
                        cmd.Parameters.Add(new SqlParameter("id", c.Id));
                        cmd.Parameters.Add(new SqlParameter("name", c.Name));
                        cmd.Parameters.Add(new SqlParameter("tax", c.Tax));
                        cmd.Parameters.Add(new SqlParameter("type", c.Type));
                        cmd.Parameters.Add(new SqlParameter("userId", user));
                        cmd.ExecuteNonQuery();
                    }  //using command
                }
                catch (Exception e)
                {
                    HandleException(e, "UpdateCategory", null);
                }
                finally
                {
                    if (cnxn.State == ConnectionState.Open)
                    {
                        cnxn.Close();
                    }
                }
            }  //using connection
        }  //UpdateCategory
        #endregion
        #region Payees
        public List<Payee> Payees()
        {
            List<Payee> payees = new List<Payee>();
            int id = 0;
            string name = "";
            string cmdString = "SELECT * FROM Payees WHERE UserId = " + user;
            using (SqlConnection cnxn = new SqlConnection(cnxnString.ConnectionString))
            {
                try
                {
                    cnxn.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdString, cnxn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                id = (int)reader["ID"];
                            }

                            if (!reader.IsDBNull(1))
                            {
                                name = SanitizeHTML((string)reader["Name"]).Trim();
                            }

                            List<Transaction> transactions = TransactionsForPayee(id);
                            payees.Add(new Payee(id, name, transactions));
                        }
                    }  //using command
                }
                catch (Exception e)
                {
                    HandleException(e, "Payees", null);
                }
                finally
                {
                    if (cnxn.State == ConnectionState.Open)
                    {
                        cnxn.Close();
                    }
                }
                return payees;
            }  //using connection
        }  //Payees

        public Dictionary<int, string> PayeeDictionary()
        {
            Dictionary<int, string> payees = new Dictionary<int, string>();
            int id = 0;
            string name = "";
            string cmdString = "SELECT * FROM Payees WHERE UserId = " + user;
            using (SqlConnection cnxn = new SqlConnection(cnxnString.ConnectionString))
            {
                try
                {
                    cnxn.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdString, cnxn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                id = (int)reader["ID"];
                            }

                            if (!reader.IsDBNull(1))
                            {
                                name = SanitizeHTML((string)reader["Name"]).Trim();
                            }
                            payees.Add(id, name);
                        }
                    }  //using command
                }
                catch (Exception e)
                {
                    HandleException(e, "Payees", null);
                }
                finally
                {
                    if (cnxn.State == ConnectionState.Open)
                    {
                        cnxn.Close();
                    }
                }
                return payees;
            }  //using connection
        }  //PayeeDictionary

        public int AddPayee(Payee p)
        {
            int insertedId = -1;
            string cmdString = "INSERT INTO Payees (Name, UserId) OUTPUT INSERTED.ID VALUES (@name, @userId);";
            using (SqlConnection cnxn = new SqlConnection(cnxnString.ConnectionString))
            {
                try
                {
                    cnxn.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdString, cnxn))
                    {
                        cmd.Parameters.Add(new SqlParameter("name", p.Name));
                        cmd.Parameters.Add(new SqlParameter("userId", user));
                        insertedId = (int)cmd.ExecuteScalar();
                    }  //using command
                }
                catch (Exception e)
                {
                    HandleException(e, "AddPayee(payee)", null);
                }
                finally
                {
                    if (cnxn.State == ConnectionState.Open)
                    {
                        cnxn.Close();
                    }
                }
                return insertedId;
            }  //using connection
        }  //AddPayee(payee)

        public int AddPayee(string name)
        {
            int insertedId = -1;
            string cmdString = "INSERT INTO Payees (Name, UserId) OUTPUT INSERTED.ID VALUES (@name, @userId);";
            using (SqlConnection cnxn = new SqlConnection(cnxnString.ConnectionString))
            {
                try
                {
                    cnxn.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdString, cnxn))
                    {
                        cmd.Parameters.Add(new SqlParameter("name", name));
                        cmd.Parameters.Add(new SqlParameter("userId", user));
                        insertedId = (int)cmd.ExecuteScalar();
                    }  //using command
                }
                catch (Exception e)
                {
                    HandleException(e, "AddPayee(name)", null);
                }
                finally
                {
                    if (cnxn.State == ConnectionState.Open)
                    {
                        cnxn.Close();
                    }
                }
                return insertedId;
            }  //using connection
        }  //AddPayee(name)

        public void DeletePayee(int id)
        {
            string cmdString = "DELETE FROM Payees WHERE ID = " + id;
            using (SqlConnection cnxn = new SqlConnection(cnxnString.ConnectionString))
            {
                try
                {
                    cnxn.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdString, cnxn))
                    {
                        cmd.ExecuteNonQuery();
                    }  //using command
                }
                catch (Exception e)
                {
                    HandleException(e, "DeletePayee", null);
                }
                finally
                {
                    if (cnxn.State == ConnectionState.Open)
                    {
                        cnxn.Close();
                    }
                }
            }  //using connection
        }  //DeletePayee

        public void UpdatePayee(Payee p)
        {
            string cmdString = "UPDATE Payees SET Name = @name, UserId = @userId WHERE ID = @id;";
            using (SqlConnection cnxn = new SqlConnection(cnxnString.ConnectionString))
            {
                try
                {
                    cnxn.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdString, cnxn))
                    {
                        cmd.Parameters.Add(new SqlParameter("name", p.Name));
                        cmd.Parameters.Add(new SqlParameter("userId", user));
                        cmd.ExecuteNonQuery();
                    }  //using command
                }
                catch (Exception e)
                {
                    HandleException(e, "UpdatePayee", null);
                }
                finally
                {
                    if (cnxn.State == ConnectionState.Open)
                    {
                        cnxn.Close();
                    }
                }
            }  //using connection
        }  //UpdatePayeeName
        #endregion
        #region Reports
        public List<Report> Reports()
        {
            List<Report> reports = new List<Report>();
            int id = 0;
            string dbAccounts = "";
            List<int> accounts = new List<int>();
            string dbCategories = "";
            List<int> categories = new List<int>();
            int? dayScope = null;
            decimal? minAmount = null;
            decimal? maxAmount = null;
            DateTime? minDate = null;
            DateTime? maxDate = null;
            string name = "";
            bool tax = false;
            int type = 0;
            string cmdString = "SELECT * FROM Reports WHERE UserId = " + user;
            using (SqlConnection cnxn = new SqlConnection(cnxnString.ConnectionString))
            {
                try
                {
                    cnxn.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdString, cnxn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                id = (int)reader["ID"];
                            }

                            if (!reader.IsDBNull(1))
                            {
                                dbAccounts = (string)reader["Accounts"];
                                if (!String.IsNullOrEmpty(dbAccounts))
                                {
                                    accounts = dbAccounts.Split(',').Select(x => Int32.Parse(x)).ToList();
                                }
                            }
                            if (!reader.IsDBNull(2))
                            {
                                dbCategories = (string)reader["Categories"];
                                if (!String.IsNullOrEmpty(dbCategories))
                                {
                                    categories = dbCategories.Split(',').Select(x => Int32.Parse(x)).ToList();
                                }
                            }
                            if (!reader.IsDBNull(3))
                            {
                                dayScope = (int)reader["DayScope"];
                            }

                            if (!reader.IsDBNull(4))
                            {
                                minAmount = (decimal)reader["MinAmount"];
                            }

                            if (!reader.IsDBNull(5))
                            {
                                maxAmount = (decimal)reader["MaxAmount"];
                            }

                            if (!reader.IsDBNull(6))
                            {
                                minDate = (DateTime)reader["MinDate"];
                            }

                            if (!reader.IsDBNull(7))
                            {
                                maxDate = (DateTime)reader["MaxDate"];
                            }

                            if (!reader.IsDBNull(8))
                            {
                                name = SanitizeHTML((string)reader["Name"]).Trim();
                            }

                            if (!reader.IsDBNull(9))
                            {
                                tax = (bool)reader["Tax"];
                            }

                            if (!reader.IsDBNull(10))
                            {
                                type = (int)reader["Type"];
                            }

                            reports.Add(new Report(id, accounts, categories, dayScope, maxAmount, maxDate, minAmount, minDate, name, tax, type));
                        }
                    }  //using command
                }
                catch (Exception e)
                {
                    HandleException(e, "Reports", null);
                }
                finally
                {
                    if (cnxn.State == ConnectionState.Open)
                    {
                        cnxn.Close();
                    }
                }
                return reports;
            }  //using connection
        }  //Reports

        public Report Report(int id)
        {
            Report report = new Report();
            string dbAccounts = "";
            List<int> accounts = new List<int>();
            string dbCategories = "";
            List<int> categories = new List<int>();
            int? dayScope = null;
            decimal? minAmount = null;
            decimal? maxAmount = null;
            DateTime? minDate = null;
            DateTime? maxDate = null;
            string name = "";
            bool tax = false;
            int type = 0;
            string cmdString = "SELECT * FROM Reports WHERE ID = " + id;
            using (SqlConnection cnxn = new SqlConnection(cnxnString.ConnectionString))
            {
                try
                {
                    cnxn.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdString, cnxn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                             if (!reader.IsDBNull(1))
                            {
                                dbAccounts = (string)reader["Accounts"];
                                if (!String.IsNullOrEmpty(dbAccounts))
                                {
                                    accounts = dbAccounts.Split(',').Select(x => Int32.Parse(x)).ToList();
                                }
                            }
                            if (!reader.IsDBNull(2))
                            {
                                dbCategories = (string)reader["Categories"];
                                if (!String.IsNullOrEmpty(dbCategories))
                                {
                                    categories = dbCategories.Split(',').Select(x => Int32.Parse(x)).ToList();
                                }
                            }
                            if (!reader.IsDBNull(3))
                            {
                                dayScope = (int)reader["DayScope"];
                            }

                            if (!reader.IsDBNull(4))
                            {
                                minAmount = (decimal)reader["MinAmount"];
                            }

                            if (!reader.IsDBNull(5))
                            {
                                maxAmount = (decimal)reader["MaxAmount"];
                            }

                            if (!reader.IsDBNull(6))
                            {
                                minDate = (DateTime)reader["MinDate"];
                            }

                            if (!reader.IsDBNull(7))
                            {
                                maxDate = (DateTime)reader["MaxDate"];
                            }

                            if (!reader.IsDBNull(8))
                            {
                                name = SanitizeHTML((string)reader["Name"]).Trim();
                            }

                            if (!reader.IsDBNull(9))
                            {
                                tax = (bool)reader["Tax"];
                            }

                            if (!reader.IsDBNull(10))
                            {
                                type = (int)reader["Type"];
                            }

                            return new Report(id, accounts, categories, dayScope, maxAmount, maxDate, minAmount, minDate, name, tax, type);
                        }
                    }  //using command
                }
                catch (Exception e)
                {
                    HandleException(e, "Report", null);
                }
                finally
                {
                    if (cnxn.State == ConnectionState.Open)
                    {
                        cnxn.Close();
                    }
                }
                return report;
            }  //using connection
        }  //Report

        public int AddReport(Report r)
        {
            int insertedId = -1;
            string cmdString = "INSERT INTO Reports (Accounts, Categories, DayScope, MinAmount, MaxAmount, MinDate, MaxDate, Name, Tax, Type, UserId) OUTPUT INSERTED.ID VALUES (@accountIds, @categoryIds, @dayScope, @minAmount, @maxAmount, @minDate, @maxDate, @name, @tax, @type, @userId);";
            using (SqlConnection cnxn = new SqlConnection(cnxnString.ConnectionString))
            {
                try
                {
                    cnxn.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdString, cnxn))
                    {
                        cmd.Parameters.Add(new SqlParameter("accountIds", IntegersToCSV(r.AccountIds)));
                        cmd.Parameters.Add(new SqlParameter("categoryIds", IntegersToCSV(r.CategoryIds)));
                        cmd.Parameters.Add(new SqlParameter("dayScope", r.DayScope));
                        if (r.MinAmount == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("minAmount", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("minAmount", r.MinAmount));
                        }
                        if (r.MaxAmount == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("maxAmount", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("maxAmount", r.MaxAmount));
                        }
                        if (r.MinDate == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("minDate", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("minDate", r.MinDate));
                        }
                        if (r.MaxDate == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("maxDate", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("maxDate", r.MaxDate));
                        }
                        cmd.Parameters.Add(new SqlParameter("name", r.Name));
                        cmd.Parameters.Add(new SqlParameter("tax", r.TaxOnly));
                        cmd.Parameters.Add(new SqlParameter("type", r.TypeId));
                        cmd.Parameters.Add(new SqlParameter("userId", user));
                        insertedId = (int)cmd.ExecuteScalar();
                    }  //using command
                }
                catch (Exception e)
                {
                    HandleException(e, "AddReport", null);
                }
                finally
                {
                    if (cnxn.State == ConnectionState.Open)
                    {
                        cnxn.Close();
                    }
                }
                return insertedId;
            }  //using connection
        }  //AddReport

        public void DeleteReport(int id)
        {
            string cmdString = "DELETE FROM Reports WHERE ID = " + id;
            using (SqlConnection cnxn = new SqlConnection(cnxnString.ConnectionString))
            {
                try
                {
                    cnxn.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdString, cnxn))
                    {
                        cmd.ExecuteNonQuery();
                    }  //using command
                }
                catch (Exception e)
                {
                    HandleException(e, "DeleteReport", null);
                }
                finally
                {
                    if (cnxn.State == ConnectionState.Open)
                    {
                        cnxn.Close();
                    }
                }
            }  //using connection
        }  //DeleteReport

        public void UpdateReport(Report r)
        {
            string cmdString = "UPDATE Reports SET Accounts = @accountIds, Categories = @categoryIds, DayScope = @dayScope, MinAmount = @minAMount, MaxAmount = @maxAmount, MinDate = @minDate, MaxDate = @maxDate, Name = @name, Tax = @tax, Type = @type, UserId = @userId WHERE ID = @Id;";
            using (SqlConnection cnxn = new SqlConnection(cnxnString.ConnectionString))
            {
                try
                {
                    cnxn.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdString, cnxn))
                    {
                        cmd.Parameters.Add(new SqlParameter("id", r.Id));
                        cmd.Parameters.Add(new SqlParameter("accountIds", IntegersToCSV(r.AccountIds)));
                        cmd.Parameters.Add(new SqlParameter("categoryIds", IntegersToCSV(r.CategoryIds)));
                        cmd.Parameters.Add(new SqlParameter("dayScope", r.DayScope));
                        if (r.MinAmount == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("minAmount", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("minAmount", r.MinAmount));
                        }
                        if (r.MaxAmount == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("maxAmount", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("maxAmount", r.MaxAmount));
                        }
                        if (r.MinDate == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("minDate", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("minDate", r.MinDate));
                        }
                        if (r.MaxDate == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("maxDate", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("maxDate", r.MaxDate));
                        }
                        cmd.Parameters.Add(new SqlParameter("name", r.Name));
                        cmd.Parameters.Add(new SqlParameter("tax", r.TaxOnly));
                        cmd.Parameters.Add(new SqlParameter("type", r.TypeId));
                        cmd.Parameters.Add(new SqlParameter("userId", user));
                        cmd.ExecuteNonQuery();
                    }  //using command
                }
                catch (Exception e)
                {
                    HandleException(e, "UpdateReport", null);
                }
                finally
                {
                    if (cnxn.State == ConnectionState.Open)
                    {
                        cnxn.Close();
                    }
                }
            }  //using connection
        }  //UpdateReport
        #endregion
        #region Transactions
        public List<Transaction> Transactions()
        {
            List<Transaction> transactions = new List<Transaction>();
            int id = 0;
            decimal amount = 0;
            int category = 0;
            int? checkno = null;
            string flags = "";
            int? fromAccount = null;
            string memo = "";
            DateTime onDate = DateTime.Today;
            int? payee = null;
            bool reconciled = false;
            bool tax = false;
            int? toAccount = null;
            int userId = 0;
            string cmdString = "SELECT * FROM Transactions WHERE UserId = " + user;
            using (SqlConnection cnxn = new SqlConnection(cnxnString.ConnectionString))
            {
                try
                {
                    cnxn.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdString, cnxn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                id = (int)reader["ID"];
                            }

                            if (!reader.IsDBNull(1))
                            {
                                amount = (decimal)reader["Amount"];
                            }

                            if (!reader.IsDBNull(2))
                            {
                                category = (int)reader["Category"];
                            }

                            if (!reader.IsDBNull(3))
                            {
                                checkno = (int)reader["CheckNo"];
                            }

                            if (!reader.IsDBNull(4))
                            {
                                flags = SanitizeHTML((string)reader["Flags"]).Trim();
                            }

                            if (!reader.IsDBNull(5))
                            {
                                fromAccount = (int)reader["FromAccount"];
                            }

                            if (!reader.IsDBNull(6))
                            {
                                memo = SanitizeHTML((string)reader["Memo"]).Trim();
                            }

                            if (!reader.IsDBNull(7))
                            {
                                onDate = (DateTime)reader["OnDate"];
                            }

                            if (!reader.IsDBNull(8))
                            {
                                payee = (int)reader["Payee"];
                            }

                            if (!reader.IsDBNull(9))
                            {
                                reconciled = (bool)reader["Reconciled"];
                            }

                            if (!reader.IsDBNull(10))
                            {
                                tax = (bool)reader["Tax"];
                            }

                            if (!reader.IsDBNull(11))
                            {
                                toAccount = (int)reader["ToAccount"];
                            }

                            if (!reader.IsDBNull(12))
                            {
                                userId = (int)reader["UserId"];
                            }

                            transactions.Add(new Transaction(id, amount, category, checkno, flags, fromAccount, memo, onDate, payee, reconciled, tax, toAccount));
                        }  //while Read
                    }  //using command
                }
                catch (Exception e)
                {
                    HandleException(e, "Transactions", null);
                }
                finally
                {
                    if (cnxn.State == ConnectionState.Open)
                    {
                        cnxn.Close();
                    }
                }
                return transactions;
            }  //using connection
        }  //Transactions

        public List<Transaction> Transactions(int? acctId, int? catId, int? payeeId, DateTime? fromDt, DateTime? toDt, decimal? fromAmt, decimal? toAmt)
        {
            List<Transaction> transactions = new List<Transaction>();
            int id = 0;
            decimal amount = 0;
            int category = 0;
            int? checkno = null;
            string flags = "";
            int? fromAccount = null;
            string memo = "";
            DateTime onDate = DateTime.Today;
            int? payee = null;
            bool reconciled = false;
            bool tax = false;
            int? toAccount = null;
            int userId = 0;
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM Transactions WHERE UserId = " + user);
            if (acctId != null)
            {
                sb.Append(" AND FromAccount = " + acctId + " OR ToAccount = " + acctId);
            }
            if (catId != null)
            {
                sb.Append(" AND Category = " + catId);
            }
            if (fromDt != null)
            {
                sb.Append(" AND OnDate >= " + fromDt);
            }
            if (payeeId != null)
            {
                sb.Append(" AND Payee = " + payeeId);
            }
            if (toDt != null)
            {
                sb.Append(" AND OnDate <= " + toDt);
           }
            if (fromAmt != null)
            {
                sb.Append(" AND Amount >= " + fromAmt);
           }
            if (toAmt != null)
            {
                sb.Append(" AND Amount <= " + toAmt);
           }

            string cmdString = sb.ToString();
            using (SqlConnection cnxn = new SqlConnection(cnxnString.ConnectionString))
            {
                try
                {
                    cnxn.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdString, cnxn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                id = (int)reader["ID"];
                            }

                            if (!reader.IsDBNull(1))
                            {
                                amount = (decimal)reader["Amount"];
                            }

                            if (!reader.IsDBNull(2))
                            {
                                category = (int)reader["Category"];
                            }

                            if (!reader.IsDBNull(3))
                            {
                                checkno = (int)reader["CheckNo"];
                            }

                            if (!reader.IsDBNull(4))
                            {
                                flags = SanitizeHTML((string)reader["Flags"]).Trim();
                            }

                            if (!reader.IsDBNull(5))
                            {
                                fromAccount = (int)reader["FromAccount"];
                            }

                            if (!reader.IsDBNull(6))
                            {
                                memo = SanitizeHTML((string)reader["Memo"]).Trim();
                            }

                            if (!reader.IsDBNull(7))
                            {
                                onDate = (DateTime)reader["OnDate"];
                            }

                            if (!reader.IsDBNull(8))
                            {
                                payee = (int)reader["Payee"];
                            }

                            if (!reader.IsDBNull(9))
                            {
                                reconciled = (bool)reader["Reconciled"];
                            }

                            if (!reader.IsDBNull(10))
                            {
                                tax = (bool)reader["Tax"];
                            }

                            if (!reader.IsDBNull(11))
                            {
                                toAccount = (int)reader["ToAccount"];
                            }

                            if (!reader.IsDBNull(12))
                            {
                                userId = (int)reader["UserId"];
                            }

                            transactions.Add(new Transaction(id, amount, category, checkno, flags, fromAccount, memo, onDate, payee, reconciled, tax, toAccount));
                        }  //while Read
                    }  //using command
                }
                catch (Exception e)
                {
                    HandleException(e, "Transactions(params)", null);
                }
                finally
                {
                    if (cnxn.State == ConnectionState.Open)
                    {
                        cnxn.Close();
                    }
                }
                return transactions;
            }  //using connection
        }  //Transactions(params)

        public List<Transaction> TransactionsForAccount(int accountId)
        {
            List<Transaction> transactions = new List<Transaction>();
            int id = 0;
            decimal amount = 0;
            int category = 0;
            int? checkno = null;
            string flags = "";
            int? fromAccount = null;
            string memo = "";
            DateTime onDate = DateTime.Today;
            int? payee = null;
            bool reconciled = false;
            bool tax = false;
            int? toAccount = null;
            int userId = 0;
            string cmdString = "SELECT * FROM Transactions WHERE UserId = " + user + " AND FromAccount = " + accountId + " OR ToAccount = " + accountId;
            using (SqlConnection cnxn = new SqlConnection(cnxnString.ConnectionString))
            {
                try
                {
                    cnxn.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdString, cnxn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                id = (int)reader["ID"];
                            }

                            if (!reader.IsDBNull(1))
                            {
                                amount = (decimal)reader["Amount"];
                            }

                            if (!reader.IsDBNull(2))
                            {
                                category = (int)reader["Category"];
                            }

                            if (!reader.IsDBNull(3))
                            {
                                checkno = (int)reader["CheckNo"];
                            }

                            if (!reader.IsDBNull(4))
                            {
                                flags = SanitizeHTML((string)reader["Flags"]).Trim();
                            }

                            if (!reader.IsDBNull(5))
                            {
                                fromAccount = (int)reader["FromAccount"];
                            }

                            if (!reader.IsDBNull(6))
                            {
                                memo = SanitizeHTML((string)reader["Memo"]).Trim();
                            }

                            if (!reader.IsDBNull(7))
                            {
                                onDate = (DateTime)reader["OnDate"];
                            }

                            if (!reader.IsDBNull(8))
                            {
                                payee = (int)reader["Payee"];
                            }

                            if (!reader.IsDBNull(9))
                            {
                                reconciled = (bool)reader["Reconciled"];
                            }

                            if (!reader.IsDBNull(10))
                            {
                                tax = (bool)reader["Tax"];
                            }

                            if (!reader.IsDBNull(11))
                            {
                                toAccount = (int)reader["ToAccount"];
                            }

                            if (!reader.IsDBNull(12))
                            {
                                userId = (int)reader["UserId"];
                            }

                            transactions.Add(new Transaction(id, amount, category, checkno, flags, fromAccount, memo, onDate, payee, reconciled, tax, toAccount));
                        }  //while Read
                    }  //using command
                }
                catch (Exception e)
                {
                    HandleException(e, "TransactionsForAccount", null);
                }
                finally
                {
                    if (cnxn.State == ConnectionState.Open)
                    {
                        cnxn.Close();
                    }
                }
                return transactions;
            }  //using connection
        }  //TransactionsForAccount

        public List<Transaction> TransactionsForPayee(int payeeId)
        {
            List<Transaction> transactions = new List<Transaction>();
            int id = 0;
            decimal amount = 0;
            int category = 0;
            int? checkno = null;
            string flags = "";
            int? fromAccount = null;
            string memo = "";
            DateTime onDate = DateTime.Today;
            int? payee = null;
            bool reconciled = false;
            bool tax = false;
            int? toAccount = null;
            int userId = 0;
            string cmdString = "SELECT * FROM Transactions WHERE UserId = " + user + " AND Payee = " + payeeId;
            using (SqlConnection cnxn = new SqlConnection(cnxnString.ConnectionString))
            {
                try
                {
                    cnxn.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdString, cnxn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                id = (int)reader["ID"];
                            }

                            if (!reader.IsDBNull(1))
                            {
                                amount = (decimal)reader["Amount"];
                            }

                            if (!reader.IsDBNull(2))
                            {
                                category = (int)reader["Category"];
                            }

                            if (!reader.IsDBNull(3))
                            {
                                checkno = (int)reader["CheckNo"];
                            }

                            if (!reader.IsDBNull(4))
                            {
                                flags = SanitizeHTML((string)reader["Flags"]).Trim();
                            }

                            if (!reader.IsDBNull(5))
                            {
                                fromAccount = (int)reader["FromAccount"];
                            }

                            if (!reader.IsDBNull(6))
                            {
                                memo = SanitizeHTML((string)reader["Memo"]).Trim();
                            }

                            if (!reader.IsDBNull(7))
                            {
                                onDate = (DateTime)reader["OnDate"];
                            }

                            if (!reader.IsDBNull(8))
                            {
                                payee = (int)reader["Payee"];
                            }

                            if (!reader.IsDBNull(9))
                            {
                                reconciled = (bool)reader["Reconciled"];
                            }

                            if (!reader.IsDBNull(10))
                            {
                                tax = (bool)reader["Tax"];
                            }

                            if (!reader.IsDBNull(11))
                            {
                                toAccount = (int)reader["ToAccount"];
                            }

                            if (!reader.IsDBNull(12))
                            {
                                userId = (int)reader["UserId"];
                            }

                            transactions.Add(new Transaction(id, amount, category, checkno, flags, fromAccount, memo, onDate, payee, reconciled, tax, toAccount));
                        }  //while Read
                    }  //using command
                }
                catch (Exception e)
                {
                    HandleException(e, "TransactionsForPayee", null);
                }
                finally
                {
                    if (cnxn.State == ConnectionState.Open)
                    {
                        cnxn.Close();
                    }
                }
                return transactions;
            }  //using connection
        }  //TransactionsForPayee

        public Transaction Transaction(int tranId)
        {
            Transaction transaction = new Transaction();
            int id = 0;
            decimal amount = 0;
            int category = 0;
            int? checkno = null;
            string flags = "";
            int? fromAccount = null;
            string memo = "";
            DateTime onDate = DateTime.Today;
            int? payee = null;
            bool reconciled = false;
            bool tax = false;
            int? toAccount = null;
            int userId = 0;
            string cmdString = "SELECT * FROM Transactions WHERE ID = " + tranId;
            using (SqlConnection cnxn = new SqlConnection(cnxnString.ConnectionString))
            {
                try
                {
                    cnxn.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdString, cnxn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                id = (int)reader["ID"];
                            }

                            if (!reader.IsDBNull(1))
                            {
                                amount = (decimal)reader["Amount"];
                            }

                            if (!reader.IsDBNull(2))
                            {
                                category = (int)reader["Category"];
                            }

                            if (!reader.IsDBNull(3))
                            {
                                checkno = (int)reader["CheckNo"];
                            }

                            if (!reader.IsDBNull(4))
                            {
                                flags = SanitizeHTML((string)reader["Flags"]).Trim();
                            }

                            if (!reader.IsDBNull(5))
                            {
                                fromAccount = (int)reader["FromAccount"];
                            }

                            if (!reader.IsDBNull(6))
                            {
                                memo = SanitizeHTML((string)reader["Memo"]).Trim();
                            }

                            if (!reader.IsDBNull(7))
                            {
                                onDate = (DateTime)reader["OnDate"];
                            }

                            if (!reader.IsDBNull(8))
                            {
                                payee = (int)reader["Payee"];
                            }

                            if (!reader.IsDBNull(9))
                            {
                                reconciled = (bool)reader["Reconciled"];
                            }

                            if (!reader.IsDBNull(10))
                            {
                                tax = (bool)reader["Tax"];
                            }

                            if (!reader.IsDBNull(11))
                            {
                                toAccount = (int)reader["ToAccount"];
                            }

                            if (!reader.IsDBNull(12))
                            {
                                userId = (int)reader["UserId"];
                            }

                            return new Transaction(id, amount, category, checkno, flags, fromAccount, memo, onDate, payee, reconciled, tax, toAccount);
                        }  //while Read
                    }  //using command
                }
                catch (Exception e)
                {
                    HandleException(e, "Transaction", null);
                }
                finally
                {
                    if (cnxn.State == ConnectionState.Open)
                    {
                        cnxn.Close();
                    }
                }
                return transaction;
            }  //using connection
        }  //Transaction

        public int AddTransaction(Transaction t)
        {
            int insertedId = -1;
            string cmdString = "INSERT INTO Transactions (Amount, Category, CheckNo, Flags, FromAccount, Memo, OnDate, Payee, Reconciled, Tax, ToAccount, UserId) OUTPUT INSERTED.ID VALUES (@amount, @category, @checkNo, @flags, @fromAccount, @memo, @onDate, @payee, @reconciled, @tax, @toAccount, @userId);";
            using (SqlConnection cnxn = new SqlConnection(cnxnString.ConnectionString))
            {
                try
                {
                    cnxn.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdString, cnxn))
                    {
                        cmd.Parameters.Add(new SqlParameter("amount", t.Amount));
                        cmd.Parameters.Add(new SqlParameter("category", t.CategoryId));
                        cmd.Parameters.Add(new SqlParameter("checkNo", t.CheckNo));
                        cmd.Parameters.Add(new SqlParameter("flags", TransactionFlagsToString(t)));
                        cmd.Parameters.Add(new SqlParameter("fromAccount", t.FromAccountId));
                        cmd.Parameters.Add(new SqlParameter("memo", t.Memo));
                        cmd.Parameters.Add(new SqlParameter("onDate", t.OnDate));
                        cmd.Parameters.Add(new SqlParameter("payee", t.PayeeId));
                        cmd.Parameters.Add(new SqlParameter("reconciled", t.Reconciled));
                        cmd.Parameters.Add(new SqlParameter("tax", t.Tax));
                        cmd.Parameters.Add(new SqlParameter("toAccount", t.ToAccountId));
                        cmd.Parameters.Add(new SqlParameter("userId", user));
                        insertedId = (int)cmd.ExecuteScalar();
                    }  //using command
                }
                catch (Exception e)
                {
                    HandleException(e, "AddTransaction", null);
                }
                finally
                {
                    if (cnxn.State == ConnectionState.Open)
                    {
                        cnxn.Close();
                    }
                }
                return insertedId;
            }  //using connection
        }  //AddTransaction

        public void DeleteTransaction(int tranId)
        {
            string cmdString = "DELETE FROM Transactions WHERE ID = " + tranId;
            using (SqlConnection cnxn = new SqlConnection(cnxnString.ConnectionString))
            {
                try
                {
                    cnxn.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdString, cnxn))
                    {
                        cmd.ExecuteNonQuery();
                    }  //using command
                }
                catch (Exception e)
                {
                    HandleException(e, "DeleteTransaction", null);
                }
                finally
                {
                    if (cnxn.State == ConnectionState.Open)
                    {
                        cnxn.Close();
                    }
                }
            }  //using connection
        }  //DeleteTransaction

        public void UpdateTransaction(Transaction t)
        {
            string cmdString = "UPDATE Transactions SET Amount = @amount, Category = @category, CheckNo = @checkNo, Flags = @flags, FromAccount = @fromAccount, Memo = @memo, OnDate = @onDate, Payee = @payee, Reconciled = @reconciled, Tax = @tax, ToAccount = @toAccount, UserId = @userId WHERE ID = @id;";
            using (SqlConnection cnxn = new SqlConnection(cnxnString.ConnectionString))
            {
                try
                {
                    cnxn.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdString, cnxn))
                    {
                        cmd.Parameters.Add(new SqlParameter("id", t.Id));
                        cmd.Parameters.Add(new SqlParameter("amount", t.Amount));
                        cmd.Parameters.Add(new SqlParameter("category", t.CategoryId));
                        cmd.Parameters.Add(new SqlParameter("checkNo", t.CheckNo));
                        cmd.Parameters.Add(new SqlParameter("flags", TransactionFlagsToString(t)));
                        cmd.Parameters.Add(new SqlParameter("fromAccount", t.FromAccountId));
                        cmd.Parameters.Add(new SqlParameter("memo", t.Memo));
                        cmd.Parameters.Add(new SqlParameter("onDate", t.OnDate));
                        cmd.Parameters.Add(new SqlParameter("payee", t.PayeeId));
                        cmd.Parameters.Add(new SqlParameter("reconciled", t.Reconciled));
                        cmd.Parameters.Add(new SqlParameter("tax", t.Tax));
                        cmd.Parameters.Add(new SqlParameter("toAccount", t.ToAccountId));
                        cmd.Parameters.Add(new SqlParameter("userId", user));
                        cmd.ExecuteNonQuery();
                    }  //using command
                }
                catch (Exception e)
                {
                    HandleException(e, "UpdateTransaction", null);
                }
                finally
                {
                    if (cnxn.State == ConnectionState.Open)
                    {
                        cnxn.Close();
                    }
                }
            }  //using connection
        }  //UpdateTransaction
        #endregion
        #region Infrastructure
        private string IntegersToCSV(List<int> integers)
        {
            StringBuilder sb = new StringBuilder();
            if (integers != null)
            {
                if (integers.Count > 0)
                {
                    foreach (int item in integers)
                    {
                        sb.Append(item.ToString() + ",");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    return sb.ToString();
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }  //IntegersToCSV

        private string TransactionFlagsToString(Transaction t)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(t.Flag0 ? "1" : "0");
            sb.Append(t.Flag1 ? "1" : "0");
            sb.Append(t.Flag2 ? "1" : "0");
            sb.Append(t.Flag3 ? "1" : "0");
            sb.Append(t.Flag4 ? "1" : "0");
            sb.Append(t.Flag5 ? "1" : "0");
            sb.Append(t.Flag6 ? "1" : "0");
            sb.Append(t.Flag7 ? "1" : "0");
            return sb.ToString();
        }  //TransactionFlagsToString

        private string ToChar12(bool[] a)
        {
            int l = a.Length;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < l; i++)
            {
                sb.Append(a[i]? '1' : '0');
            }
            if(l < 12)
            {
                for(int i = sb.Length; i < 13; i++)
                {
                    sb.Append('0');
                }
            }
            if(l > 12)
            {
                sb = sb.Remove(12, (sb.Length - 12));
            }
            return sb.ToString();
        }  //ToBooleanArray

        private bool[] ToBooleanArray(string s)
        {
            return s.Select(x => x.Equals('1')).ToArray();
        }  //ToBooleanArray

        private string SanitizeHTML (string input)
        {
            input = input.Replace("<", " less than ").Replace("<=", " less or equal to ").Replace(">", " greater than ").Replace(">=", " greater or equal to ").Replace("\\", " [backslash] ");
            return AntiXssEncoder.HtmlEncode(input, true);
        }  //SanitizeHTML

        private void HandleException (Exception e, string method, string userMessage)
        {
            Debug.WriteLine("An error occurred in DatabaseRepository." + method + ".\n" + e.Message + ".\n" + userMessage + ".\n");
            Debug.WriteLine("Inner Exception: " + e.InnerException + "\n");
            Debug.WriteLine("Stack: " + e.StackTrace);
            logger.Error("An error occurred in DatabaseRepository." + method + ".\n" + e.Message + ".\n" + userMessage);
        }  //HandleException
        #endregion
    }  //class
}  //namespace