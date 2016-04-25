using Bitcliq.BIR.Logs;
using Bitcliq.BIR.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcliq.BIR.Platform
{
    public class Issue_AssignedUsers
    {

        public static bool Save(int IssueID, string AssignedTo)
        {

            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Issue_AssignedUsers_Save");
                sql.AddInputParameter(false, "@IssueID", IssueID);

                sql.AddInputParameter(false, "@AssignedTo", AssignedTo);
              


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



        public static bool DeleteAssignedUsers(int IssueID)
        {

            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Issue_AssignedUsers_Delete");
                sql.AddInputParameter(false, "@IssueID", IssueID);

            


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
        public static DataSet ListAssignedUsers(int IssueID)
        {

            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Issue_AssignedUsers_List");
                sql.AddInputParameter(false, "@IssueID", IssueID);

                return sql.ExecuteDataSet();

            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return null;
            }
        }
    }
}
