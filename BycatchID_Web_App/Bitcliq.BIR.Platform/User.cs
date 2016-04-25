using Bitcliq.BIR.Logs;
using Bitcliq.BIR.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcliq.BIR.Platform
{
    public class User
    {
        #region PRIVATE
        private int _UserID;
        private string _Name;
        private string _Email;
        private string _Pwd;
        private DateTime? _LastLogin;
        //private bool _IsAdmin;
        private bool _ChangedPassword;
        private int _CreatedBy;
        private DateTime _CreatedDate;
        private int? _ModifiedBy;
        private DateTime? _ModifiedDate;
        private bool _Active;
        private bool _ISAdmin;

        private int _ProfileID;
        private int _AccountID;

        private Guid _ActivationKey;
        private Guid _RecoverGuid;
        private bool _RecieveNotifications;
        #endregion

        #region PUBLIC
        public int UserID { get { return _UserID; } set { _UserID = value; } }
        public string Name { get { return _Name; } set { _Name = value; } }
        public string Email { get { return _Email; } set { _Email = value; } }
        public string Pwd { get { return _Pwd; } set { _Pwd = value; } }
        public DateTime? LastLogin { get { return _LastLogin; } set { _LastLogin = value; } }
        //public bool IsAdmin { get { return _IsAdmin; } set { _IsAdmin = value; } }
        public bool ChangedPassword { get { return _ChangedPassword; } set { _ChangedPassword = value; } }
        public int CreatedBy { get { return _CreatedBy; } set { _CreatedBy = value; } }
        public DateTime CreatedDate { get { return _CreatedDate; } set { _CreatedDate = value; } }
        public int? ModifiedBy { get { return _ModifiedBy; } set { _ModifiedBy = value; } }
        public DateTime? ModifiedDate { get { return _ModifiedDate; } set { _ModifiedDate = value; } }
        public bool Active { get { return _Active; } set { _Active = value; } }
        public bool ISAdmin { get { return _ISAdmin; } set { _ISAdmin = value; } }

        public int ProfileID { get { return _ProfileID; } set { _ProfileID = value; } }
        public int AccountID { get { return _AccountID; } set { _AccountID = value; } }

        public Guid ActivationKey { get { return _ActivationKey; } set { _ActivationKey = value; } }
        public Guid RecoverGuid { get { return _RecoverGuid; } set { _RecoverGuid = value; } }


        public bool RecieveNotifications { get { return _RecieveNotifications; } set { _RecieveNotifications = value; } }
        #endregion

        #region CONSTRUCTORS
        public User()
        {
            CleanValues();
        }

        public User(int p_ID)
        {
            this.UserID = p_ID;
            Get();
        }


        public User(DataRow dr)
        {
            FillValues(dr);
        }
        #endregion




        #region ################## PRIVATE METHODS

        #region GET
        private void Get()
        {
            try
            {
                if (_UserID > 0)
                {
                    SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "User_ListByID");
                    sql.AddInputParameter(false, "@ID", _UserID);

                    DataSet ds = sql.ExecuteDataSet();

                    if (UtilMethods.DataSetHasOnlyOneDataRow(ds))
                    {
                        DataRow dr = ds.Tables[0].Rows[0];

                        FillValues(dr);
                    }
                    else
                    {
                        CleanValues();
                    }
                }
                else
                {
                    CleanValues();
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                CleanValues();
            }
        }
        #endregion

        #region INSERT
        private bool Insert(int? p_UserID)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "User_Insert");


                sql.AddInputParameter(false, "@Name", _Name);
                sql.AddInputParameter(false, "@Email", _Email);

                

                //sql.AddInputParameter(false, "@Password", PasswordManager.GenerateMD5Password(_Pwd));
                //sql.AddInputParameter(false, "@IsAdmin", _IsAdmin);
                sql.AddInputParameter(false, "@CreatedBy", p_UserID);
                 sql.AddInputParameter(false, "@ProfileID", _ProfileID);
                 sql.AddInputParameter(false, "@AccountID", _AccountID);
           


                 _ActivationKey = Guid.NewGuid();

                 string pass = PasswordManager.GenerateRandomMD5Password();
                 sql.AddInputParameter(false, "@Password", pass);
                 sql.AddInputParameter(false, "ActivationKey", _ActivationKey);




                int ret = (int)UtilMethods.toInt(sql.ExecuteScalar(), 0);

                if (ret <= 0)
                {
                    return false;
                }

                _UserID = ret;

                User u = new User(_UserID);

                Account account = new Account();

                string content = UtilMethods.ReadTextFile(StaticKeys.BackofficeTemplatesPath + @"Invite.htm");

                content = UtilMethods.ReplaceTag(content, "Email", _Email);
                content = UtilMethods.ReplaceTag(content, "EntityName", u.Name);
                content = UtilMethods.ReplaceTag(content, "Name", _Name);
                content = UtilMethods.ReplaceTag(content, "ActivationKey", _ActivationKey + "");
                content = UtilMethods.ReplaceTag(content, "ImgUrl", StaticKeys.BackofficeUrl);
                content = UtilMethods.ReplaceTag(content, "UrlRegister", StaticKeys.BackofficeUrl + "ChangePassword.aspx?g=" + _ActivationKey);

              


                //bool mailSend = SendMail.Send(new Guid(StaticKeys.EmailsContextKey), _Email, StaticKeys.InviteSubject, content);

                SendMail sm = new SendMail(StaticKeys.MailFrom, _Email, StaticKeys.InviteSubject, content, null);

                return true;
            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return false;
            }
        }
        #endregion


        #region UPDATE
        private bool Update(int? p_UserID)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "User_Update");

                sql.AddInputParameter(false, "@ID", _UserID);
                sql.AddInputParameter(false, "@Name", _Name);
                sql.AddInputParameter(false, "@Email", _Email);
                //sql.AddInputParameter(false, "@Password", PasswordManager.GenerateMD5Password(_Pwd));
                // sql.AddInputParameter(false, "@LastLogin", _LastLogin);
                //sql.AddInputParameter(false, "@IsAdmin", _IsAdmin);
                sql.AddInputParameter(false, "@ModifiedBy", p_UserID);
                sql.AddInputParameter(false, "@ProfileID", _ProfileID);
                int ret = (int)UtilMethods.toInt(sql.ExecuteScalar(), 0);

                if (ret <= 0)
                {
                    return false;
                }



                return true;
            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return false;
            }
        }
        #endregion

        #endregion


        #region CLEAN VALUES
        private void CleanValues()
        {
            _UserID = 0;
            _Name = "";
            _Email = "";
            _Pwd = "";
            //_IsAdmin = false;
            _ChangedPassword = false;
            _CreatedBy = 0;
            _CreatedDate = DateTime.MinValue;
            _ModifiedBy = null;
            _ModifiedDate = null;
            _Active = false;
            _ProfileID = 0;
            _AccountID = 0;
            _RecieveNotifications = true;
        }
        #endregion

        #region FILL  VALUES
        private void FillValues(DataRow dr)
        {
            _UserID = (int)UtilMethods.toInt(dr["ID"], 0);
            _Name = dr["name"] + "";
            _Email = dr["Email"] + "";
            _Pwd = dr["Password"] + "";
            _LastLogin = UtilMethods.toDateTime(dr["LastLogin"], null);
           // _IsAdmin = (bool)UtilMethods.toBool(dr["IsAdmin"], false);
            _ChangedPassword = (bool)UtilMethods.toBool(dr["ChangedPassword"], false);
            _CreatedBy = (int)UtilMethods.toInt(dr["CreatedBy"], 0);
            _CreatedDate = (DateTime)UtilMethods.toDateTime(dr["CreatedDate"], DateTime.MinValue);
            _ModifiedBy = UtilMethods.toInt(dr["ModifiedBy"], null);
            _ModifiedDate = UtilMethods.toDateTime(dr["ModifiedDate"], null);
            _Active = (bool)UtilMethods.toBool(dr["Active"], false);

            _ProfileID = (int)UtilMethods.toInt(dr["ProfileID"], 0);
            _AccountID = (int)UtilMethods.toInt(dr["AccountID"], 0);

            _ISAdmin = (bool)UtilMethods.toBool(dr["IsAdmin"], false);

            _RecieveNotifications = (bool)UtilMethods.toBool(dr["ReceiveNotifications"], true);
        }
        #endregion




        #region UPDATE
        public bool UpdateMyProfile()
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "User_UpdateMyProfile");

                sql.AddInputParameter(false, "@ID", _UserID);
                sql.AddInputParameter(false, "@Name", _Name);
                sql.AddInputParameter(false, "@Email", _Email);
                sql.AddInputParameter(false, "@ReceiveNotifications", _RecieveNotifications);
                
                int ret = (int)UtilMethods.toInt(sql.ExecuteScalar(), 0);

                if (ret <= 0)
                {
                    return false;
                }



                return true;
            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return false;
            }
        }
        #endregion


        public static User Login(string userName, string p_Password)
        {

            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "User_Login");

                sql.AddInputParameter(false, "@Email", userName);
                sql.AddInputParameter(false, "@Password", PasswordManager.GenerateMD5Password(p_Password));
                //sql.AddInputParameter(false, "@Password", p_Password);


                DataSet ds = sql.ExecuteDataSet();


                if (UtilMethods.DataSetHasOnlyOneDataRow(ds))
                {
                    User user = new User(ds.Tables[0].Rows[0]);


                    return user;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return null;
            }

        }


        public static User ListByRecoverGuid(Guid p_RecoverGuid)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "User_ListByRecoverGuid");

                sql.AddInputParameter(false, "@RecoverGuid", p_RecoverGuid);

                int ret = (int)UtilMethods.toInt(sql.ExecuteScalar(), 0);

                if (ret > 0)
                    return new User(ret);

                return null;

            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return null;
            }
        }


        public static User ListByActivationKey(Guid ActivationKey)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "User_ListByActivationKey");

                sql.AddInputParameter(false, "@ActivationKey", ActivationKey);

                int ret = (int)UtilMethods.toInt(sql.ExecuteScalar(), 0);

                if (ret > 0)
                    return new User(ret);

                return null;

            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return null;
            }
        }


        #region PUBLIC STATIC METHODS
        public static List<User> List()
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "User_List");




                DataSet ds = sql.ExecuteDataSet();


                List<User> pl = new List<User>();

                if (UtilMethods.DataSetHasData(ds))
                {
                    DataRow[] drArr = ds.Tables[0].Select();

                    foreach (DataRow dr in drArr)
                        pl.Add(new User(dr));
                }
                return pl;

            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return new List<User>();
            }
        }

 

        public static bool SetLastLogin(int p_ID)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "User_SetLastLogin");

                sql.AddInputParameter(false, "@ID", p_ID);



                sql.ExecuteNoQuery();
                return true;

            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return false;
            }
        }


        public static bool ResetPassword(int p_ID, Guid p_RecoverGuid)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "User_ResetPassword");

                sql.AddInputParameter(false, "@ID", p_ID);

                string pass = PasswordManager.GenerateRandomMD5Password();
                sql.AddInputParameter(false, "@Password", PasswordManager.GenerateMD5Password(pass));
                sql.AddInputParameter(false, "@RecoverGuid", p_RecoverGuid);

                sql.ExecuteNoQuery();
                return true;

            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return false;
            }
        }


        public static User ListByEmail(string p_Email)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "User_ListByEmail");

                sql.AddInputParameter(false, "@Email", p_Email);

                int ret = (int)UtilMethods.toInt(sql.ExecuteScalar(), 0);

                if (ret > 0)
                    return new User(ret);

                return null;

            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return null;
            }
        }


      

        public static int CountLogin(int? p_ID, string p_Email)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "User_CountLogin");

                sql.AddInputParameter(false, "@ID", p_ID);


                sql.AddInputParameter(false, "@Email", p_Email);


                return Convert.ToInt32(sql.ExecuteScalar());

            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return -1;
            }
        }



        public static int SaveCategories(int? p_ID, string p_TypeList)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "User_Type_Save");

                sql.AddInputParameter(false, "@UserID", p_ID);


                sql.AddInputParameter(false, "@TypeList", p_TypeList);


                return Convert.ToInt32(sql.ExecuteScalar());

            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return -1;
            }
        }

        #endregion



        public DataSet ListTypesByUserID()
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "User_Type_ListByUserID");

                sql.AddInputParameter(false, "@UserID", UserID);
                sql.AddInputParameter(false, "@ISAdmin", ISAdmin);

                return sql.ExecuteDataSet();

            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return new DataSet();
            }
        }



     

        public bool ChangePassword(string p_Password)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "User_ChangePassword");

                sql.AddInputParameter(false, "@ID", UserID);

                sql.AddInputParameter(false, "@Password", PasswordManager.GenerateMD5Password(p_Password));

                sql.ExecuteNoQuery();
                return true;

            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return false;
            }
        }


        public bool ChangeMyPassword(string p_Password)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "User_ChangeMyPassword");

                sql.AddInputParameter(false, "@UserID", UserID);

                sql.AddInputParameter(false, "@Password", PasswordManager.GenerateMD5Password(p_Password));

                sql.ExecuteNoQuery();
                return true;

            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return false;
            }
        }



        public bool ChangePasswordAndActivate(string p_Password)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "User_ChangePasswordAndActivate");

                sql.AddInputParameter(false, "@ID", _UserID);
                sql.AddInputParameter(false, "@Password", PasswordManager.GenerateMD5Password(p_Password));


                sql.ExecuteNoQuery();


                return true;
            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return false;
            }
        }



        #region DELETE


        public bool Delete(int p_UserID)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "User_Delete");

                sql.AddInputParameter(false, "@ID", _UserID);
                sql.AddInputParameter(false, "@ModifiedBy", p_UserID);

                sql.ExecuteNoQuery();

                return true;
            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return false;
            }
        }
        #endregion

        #region SAVE
        public bool Save(int p_User)
        {
            if (_UserID > 0)
                return Update(p_User);
            else
                return Insert(p_User);

        }
        #endregion


        #region STATUS


        public bool ChangeStatus(int p_UserID)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "User_ChangeStatus");

                sql.AddInputParameter(false, "@ID", UserID);
                sql.AddInputParameter(false, "@Active", _Active);
                sql.AddInputParameter(false, "@ModifiedBy", p_UserID);

                sql.ExecuteNoQuery();

                return true;
            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return false;
            }
        }
        #endregion


        #region DATASET - FOR JSON
        public DataSet ListToDataSet(int userID, string p_Filters, int p_StartIndex, int p_NumRecords, string p_Order)
        {
            try
            {


                string whereclause = null;

                try
                {
                    if (p_Filters + "" != "")
                        whereclause = WhereClauseGenerator.ParseFilter(JsonConvert.DeserializeObject<Filter>(p_Filters)).ToString();
                }
                catch (Exception)
                {
                }

                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "User_ListToDataset");


                sql.AddInputParameter(false, "@UserID", userID);
                sql.AddInputParameter(false, "@p_Si", p_StartIndex);
                sql.AddInputParameter(false, "@p_iPage", p_NumRecords);

                if (p_Order.Trim() == "")
                    p_Order = "ID desc";
                sql.AddInputParameter(false, "@p_SortColumnName", p_Order);

                if (p_Filters + "" != "")
                    sql.AddInputParameter(false, "@p_filters", p_Filters);
                else
                    sql.AddInputParameter(false, "@p_filters", null);


                return sql.ExecuteDataSet();




            }
            catch (Exception ex)
            {

                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return null;
            }
        }
        #endregion


    
    }




}
