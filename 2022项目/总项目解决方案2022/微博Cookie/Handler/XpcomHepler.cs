using System.Data;
using System.Text;
using Gecko;
using 微博Cookie.Common;

namespace 微博Cookie.Handler
{
    class XpcomHepler
    {
        public static void RemoveCookie()
        {
            nsICookieManager CooKieMan;
            CooKieMan = Xpcom.GetService<nsICookieManager>("@mozilla.org/cookiemanager;1");
            CooKieMan = Xpcom.QueryInterface<nsICookieManager>(CooKieMan);
            CooKieMan.RemoveAll();
        }
        public static void RemoveHistory()
        {
            nsIBrowserHistory HistoryMan;
            HistoryMan = Xpcom.GetService<nsIBrowserHistory>(Gecko.Contracts.NavHistoryService);
            HistoryMan = Xpcom.QueryInterface<nsIBrowserHistory>(HistoryMan);
            HistoryMan.RemoveAllPages();
        }

        public static string ReadCookie()
        {
            DataTable dt = DbHelperSQLite.Query("SELECT [name],[value]FROM [moz_cookies] where baseDomain='zxxk.com'").Tables[0];
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb.AppendLine(dt.Rows[i][0].ToString() + "=" + dt.Rows[i][1].ToString() + ";");
            }
            return sb.ToString();
        }

    }
}
