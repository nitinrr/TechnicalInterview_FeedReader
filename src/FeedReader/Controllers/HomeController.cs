using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Security;
using System.IO;
using System.Data;

namespace FeedReader.Controllers
{
    public class HomeController : Controller
    {
        //This is the default Index view
        public ActionResult Index()
        {
            //checks if the user is logged in and redirect to his home page
            if (Session["USERID"] == null)
            {
                ViewBag.Message = "Welcome to your Feed Reader.";
                return View();
            }
            else
            {
                return RedirectToAction("DisplayFeed", "Home");
            }
        }

        //This will display the updated list of RSS feeds by excluding the link which was unsubscribed 
        //Action: On clicking unsubscribe action link
        //To Do: Unsubscribe Logic
        [HttpPost]
        public ActionResult SubscribedFeeds(string feedLink)
        {
            //checks if the user is logged in else redirect to index page
            if (Session["USERID"] != null)
            {
                ViewBag.Message = "Your Subscribed Feeds are shown below.";
                return View(Models.FeedReader.GetSubscribedRss());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //This is the default view of the feeds subscribed.
        [HttpGet]
        public ActionResult SubscribedFeeds()
        {
            //checks if the user is logged in else redirect to index page
            if (Session["USERID"] != null)
            {
                ViewBag.Message = "Your Subscribed Feeds are shown below.";
                return View(Models.FeedReader.GetSubscribedRss());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
                
        //By default we will display the below RSS feed        
        public ActionResult DisplayFeed(string feedLink = "http://rss.cnn.com/rss/edition_world.rss")
        {
            //checks if the user is logged in else redirect to index page
            if (Session["USERID"] != null)
            {
                if (feedLink != "http://rss.cnn.com/rss/edition_world.rss")
                {
                    DataTable dtRssFeed = new DataTable();
                    dtRssFeed.Clear();
                    dtRssFeed = DataAccess.DAL.SubscribeFeed(Convert.ToInt32(Session["USERID"]), feedLink);
                    var model = new Models.FeedModel();
                    string strFeed = feedLink;
                    using (XmlReader reader = XmlReader.Create(strFeed))
                    {
                        SyndicationFeed rssData = SyndicationFeed.Load(reader); //using the SyndicationFeed for consuming feeds rss/atom
                        model.Feed = rssData; ;
                    }
                    return View(model);
                }
                else
                {
                    var model = new Models.FeedModel();
                    string strFeed = feedLink;
                    using (XmlReader reader = XmlReader.Create(strFeed))
                    {
                        SyndicationFeed rssData = SyndicationFeed.Load(reader);
                        model.Feed = rssData; ;
                    }
                    return View(model);
                }
            }
            else 
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
