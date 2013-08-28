using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Data;

namespace FeedReader.Models
{
    public class FeedReader
    {
        public static string _blogURL = "http://rss.cnn.com/rss/edition.rss";
        public static ArrayList urls = new ArrayList();
        

        public static IEnumerable<Rss> GetSubscribedRss()
        {
            DataTable dtSubscribedFeed = new DataTable();
            dtSubscribedFeed.Clear();
            dtSubscribedFeed = DataAccess.DAL.ViewSubscribedFeed(Convert.ToInt32(HttpContext.Current.Session["USERID"]));
           
                var tankReadings = new List<Rss>();
                if (dtSubscribedFeed.Rows.Count > 0)
                {
                    foreach (DataRow row in dtSubscribedFeed.Rows)
                    {
                        var tankReading = new Rss
                        {
                            RssFeed = Convert.ToString(row["rsslink"]),
                            Status = Convert.ToString(row["isactive"]),
                        };
                        tankReadings.Add(tankReading);
                    }
                }
                return tankReadings.AsEnumerable();          
        }
    }
}