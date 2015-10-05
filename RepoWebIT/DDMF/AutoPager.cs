using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace RepoWebIT
{
    public class AutoPager<T> where T : new()
    {
        private string _curFile;
        private string table;
        private string field = "KID";

        public AutoPager(string curFile)
        {
            _curFile = curFile;
            table = typeof(T).Name;
        }


        public AutoPager(string curFile, string relatedField)
        {
            _curFile = curFile;
            table = typeof(T).Name;
            field = relatedField;

        }

        public string GetNavigation(int cPage, int PageSize, int KID)
        {
            int currentPage;

            if (cPage == 0)
            {
                currentPage = 1;
            }
            else
            {
                currentPage = cPage;
            }

            return GetPagingLinks(currentPage, PageSize, KID) + "<br/>";

        }


        public List<T> GetPagedData(int page, int PageSize, int KID)
        {
            int itemFrom = 1;
            int itemTo = PageSize;

            if (page <= 1)
            {
                itemFrom = ((page * PageSize) - PageSize);
                itemTo = (page * PageSize);
            }
            else
            {
                itemFrom = ((page * PageSize) - PageSize) + 1;
                itemTo = (page * PageSize);
            }

            List<T> listData = GetDataPaging(itemFrom, itemTo, KID);

            return listData;
        }



        private string GetPagingLinks(int cPage, int PageSize, int KID)
        {
            string sql = "select ID from " + table;

            if (KID > 0)
            {
                sql = "select ID from " + table + " where " + field + "=@ID";
            }

            SqlCommand CMD = new SqlCommand(sql);

            if (KID > 0)
            {
                CMD.Parameters.AddWithValue("@ID", KID);
            }

            int totalRecords = CountRecords(KID);

            int totalPages;

            if (totalRecords > PageSize)
            {

                // Tjekker via modelus om der er et skevt antal side eks. 5,3
                if (totalRecords % PageSize == 0)
                {
                    totalPages = totalRecords / PageSize;
                }
                else
                {
                    totalPages = (totalRecords / PageSize) + 1;
                }


                int i;
                string pageTxt = "";
                string t = "?";

                if (_curFile.IndexOf('?') > 0)
                {
                    t = "&";
                }

                //Hvis siden vi står på ikke er den første laver vi her et tilbage link
                if (cPage > 1)
                {
                    pageTxt += "<a href=\"" + _curFile + t + "page=" + (cPage - 1).ToString() + "\" class=\"prevLink\">Previous</a> &nbsp;&nbsp;&nbsp;";
                }

                //Her udskriver vi tallene for siderne som links
                for (i = 1; i < totalPages + 1; i++)
                {
                    //Hvis siden er den vi står på skal der ikke laves noget link men kun udskrives tallet for siden.
                    if (cPage == i)
                    {
                        pageTxt += "<span class=\"activLink\">" + i + "</span> &nbsp;&nbsp;&nbsp;";
                    }
                    else
                    {
                        pageTxt += "<a href=\"" + _curFile + t + "page=" + i + "\" class=\"numberLinks\">" + i + "</a> &nbsp;&nbsp;&nbsp;";
                    }

                }

                //Hvis siden vi står på ikke er den sidste udskriver vi et næste link.
                if (cPage < totalPages)
                {
                    pageTxt += "<a href=\"" + _curFile + t + "page=" + (cPage + 1).ToString() + "\" class=\"nextLink\">Next</a> &nbsp;&nbsp;&nbsp;";
                }
                return pageTxt;
            }
            else
            {
                return "";
            }


        }

        private List<T> GetDataPaging(int itemFrom, int itemTo, int KID)
        {

            string newTable = table;

            if (KID > 0)
            {
                newTable = table + " where " + field + "=@ID";
            }

            string strSQL = "Select * from (select row_number() over(ORDER BY ID DESC) as row_number, * from " + newTable + ") T where row_number BETWEEN " + itemFrom + " AND " + itemTo;

            using (var cmd = new SqlCommand(strSQL, Conn.CreateConnection()))
            {
                if (KID > 0)
                {
                    cmd.Parameters.AddWithValue("@ID", KID);
                }

                var mapper = new Mapper<T>();
                List<T> list = mapper.MapList(cmd.ExecuteReader());
                cmd.Connection.Close();
                return list;
            }

        }


        private int CountRecords(int KID)
        {

            string strSQL = "Select count(ID) as Antal from " + table;
            

            if (KID > 0)
            {
                strSQL = "Select count(ID) as Antal from " + table + " where " + field + "=" + KID;
            }

            using (var cmd = new SqlCommand(strSQL, Conn.CreateConnection()))
            {

                if (KID > 0)
                {
                    cmd.Parameters.AddWithValue("@ID", KID);
                }

                var mapper = new Mapper<T>();
                IDataReader IDR = cmd.ExecuteReader();
                int Antal = 0;

                if (IDR.Read())
                {
                    Antal = int.Parse(IDR["Antal"].ToString());
                }
                cmd.Connection.Close();
                return Antal;
            }

        }

       
    }
}
