using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Http;
using System.Text;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using HtmlAgilityPack;
using System.Threading.Tasks;

namespace ArchiveProject
{
    public partial class ArchiveList : System.Web.UI.Page
    {
        List<Result> rs = new List<Result>();
        WebClient client = new WebClient();
        Uri rootRequestURI = null;
        //"http://web.archive.org/web/" + StartYeardate.Year + (StartYeardate.Month < 10 ? "0" + StartYeardate.Month.ToString() : StartYeardate.Month.ToString()) + (StartYeardate.Day < 10 ? "0" + StartYeardate.Day.ToString() : StartYeardate.Day.ToString()) + "/http://www.cnn.com/";
        string archiveAddress = "http://web.archive.org/web/";
        string Address = "/http://www.cnn.com/";
        string dateString = string.Empty;
        string URL = string.Empty;
        string rootResponsePage = string.Empty;// client.DownloadString(rootRequestURI);
        string rootPageSource = string.Empty;// WebUtility.HtmlDecode(rootResponsePage);
        HtmlDocument rootHtmlDocument = new HtmlDocument();
        List<string> imgListNodes = new List<string>();
        List<string> VideoNodes = new List<string>();
        List<string> AdListNodes = new List<string>();
        public class Result
        {
            public Result()
            {

            }
            public DateTime Date { get; set; }
            public string URL { get; set; }
            public int Images { get; set; }
            public int Videos { get; set; }
            public int Ads { get; set; }
            public int SocialMedia { get; set; }
          
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            LoadResults();

        }

        public void LoadResults()
        {
            try
            {
                
                DateTime StartYeardate =new DateTime(2016,01,01);
                DateTime EndYeardate = new DateTime(2016, 06, 01);
                StartYeardate =txtStartDate.Text!=string.Empty?Convert.ToDateTime(txtStartDate.Text):StartYeardate;
                EndYeardate = txtEndDate.Text != string.Empty ? Convert.ToDateTime(txtEndDate.Text) : EndYeardate;
                Address = txtURL.Text != null && txtURL.Text != string.Empty ? "/"+txtURL.Text : Address;
                while (StartYeardate<EndYeardate.AddMonths(1))
                   {
                       
                       dateString = StartYeardate.Year + (StartYeardate.Month < 10 ? "0" + StartYeardate.Month.ToString() : StartYeardate.Month.ToString()) + (StartYeardate.Day < 10 ? "0" + StartYeardate.Day.ToString() : StartYeardate.Day.ToString());
                       URL = archiveAddress + dateString + Address;
                        this.MakeList(StartYeardate);
                        StartYeardate = StartYeardate.AddMonths(1);
                       
			        }

                if (imgListNodes!=null && imgListNodes.Count>0)
                {
                    //GridView imgGrid= new GridView();
                    imgGrid.DataSource = imgListNodes;
                    imgGrid.DataBind();
                    //this.UpdatePanel1.Controls.Add(imgGrid);
                }

                if (VideoNodes != null && VideoNodes.Count > 0)
                {
                    //GridView videoGrid = new GridView();
                    videoGrid.DataSource = VideoNodes;
                    videoGrid.DataBind();
                   // this.UpdatePanel1.Controls.Add(videoGrid);
                }


                if (AdListNodes != null && AdListNodes.Count > 0)
                {
                    //GridView adGrid = new GridView();
                    adGrid.DataSource = AdListNodes;
                    adGrid.DataBind();
                   // this.UpdatePanel1.Controls.Add(adGrid);
                }
                grdList.DataSource=rs;
                grdList.DataBind();
                UpdatePanel1.Update();
                

            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
                
            }

        }
        //async Task
        public void  MakeList(DateTime date)
        {
            try
            {
                
                //HttpClient httpClient = new HttpClient();
                //http://web.archive.org/web/20100914233601/http://www.cnn.com/?refresh=1
                //Uri rootRequestURI = new Uri("http://web.archive.org/web/" + StartYeardate.Year + (StartYeardate.Month < 10 ? "0" + StartYeardate.Month.ToString() : StartYeardate.Month.ToString()) + (StartYeardate.Day < 10 ? "0" + StartYeardate.Day.ToString() : StartYeardate.Day.ToString()) + "/http://www.cnn.com/");
                //object temp 
                //byte[] rootResponsePage= await httpClient.GetByteArrayAsync(rootRequestURI);
                //byte[] rootResponsePage = ObjectToByteArray(temp);
                //String rootPageSource = Encoding.GetEncoding("utf-8").GetString(rootResponsePage);
                //rootRequestURI.AbsoluteUri = URL.ToString();//"http://web.archive.org/web/" + StartYeardate.Year + (StartYeardate.Month < 10 ? "0" + StartYeardate.Month.ToString() : StartYeardate.Month.ToString()) + (StartYeardate.Day < 10 ? "0" + StartYeardate.Day.ToString() : StartYeardate.Day.ToString()) + "/http://www.cnn.com/";
                //Response.Write("Path"+rootRequestURI.AbsolutePath);
                //Response.Write(" ....URI"+ rootRequestURI.AbsoluteUri);
                //string rootResponsePage = client.DownloadString(rootRequestURI);
                //String rootPageSource = WebUtility.HtmlDecode(rootResponsePage);
                // HtmlDocument rootHtmlDocument = new HtmlDocument();

                Result r = new Result();
                rootRequestURI = new Uri(URL.ToString());
                rootResponsePage=client.DownloadString(rootRequestURI.ToString());
                rootPageSource=WebUtility.HtmlDecode(rootResponsePage.ToString());
                rootHtmlDocument.LoadHtml(rootPageSource.ToString());
               
                
                var imgList = rootHtmlDocument.DocumentNode.Descendants("img").Where(d =>
                    (
                    (d.Attributes.Contains("href")
                    && !d.Attributes["id"].Value.Contains("sparklineImgId") && !d.Attributes["href"].Value.ToLower().Contains("weather") && !d.Attributes["href"].Value.ToLower().Contains("logo") && !d.Attributes["href"].Value.ToLower().Contains("toolbar") && !d.Attributes["href"].Value.ToLower().Contains("icon") && !d.Attributes["href"].Value.ToLower().Contains("footer") && !d.Attributes["href"].Value.ToLower().Contains("header") 
                    && (d.Attributes["href"].Value.Contains("jpg") || d.Attributes["href"].Value.Contains("png") || d.Attributes["href"].Value.Contains("bmp") 
                    //|| (d.FirstChild != null && d.FirstChild.Attributes.Contains("alt"))
                    ) && (!d.Attributes["href"].Value.ToUpper().Contains("AD")) && (!d.Attributes["href"].Value.ToUpper().Contains("/VIDEO")))
                    || (d.Attributes.Contains("src") && !d.Attributes["src"].Value.ToLower().Contains("weather") && !d.Attributes["src"].Value.ToLower().Contains("logo") && !d.Attributes["src"].Value.ToLower().Contains("toolbar") && !d.Attributes["src"].Value.ToLower().Contains("icon") && !d.Attributes["src"].Value.ToLower().Contains("footer") && !d.Attributes["src"].Value.ToLower().Contains("header") 
                    && (d.Attributes["src"].Value.Contains("jpg") || d.Attributes["src"].Value.Contains("png") || d.Attributes["src"].Value.Contains("bmp") 
                   // || (d.FirstChild != null && d.FirstChild.Attributes.Contains("alt"))
                    ) && (!d.Attributes["src"].Value.ToUpper().Contains("AD")) && (!d.Attributes["src"].Value.ToUpper().Contains("/VIDEO")))
                    //&& (((d.Attributes["href"].Value.Contains("ads")!=null &&!d.Attributes["href"].Value.Contains("ads")) && (d.Attributes["src"].Value.Contains("ads")!=null && !d.Attributes["src"].Value.Contains("ads"))))
                    )

                    &&

                      !(
                    (
                    (d.FirstChild != null) &&
                    (
                    (d.FirstChild.Attributes.Contains("href") && (d.FirstChild.Attributes["href"].Value.Contains("jpg") || d.FirstChild.Attributes["href"].Value.Contains("png") || d.FirstChild.Attributes["href"].Value.Contains("bmp") || d.FirstChild.Attributes["href"].Value.Contains("gif")))
                    || (d.FirstChild.Attributes.Contains("src") && (d.FirstChild.Attributes["src"].Value.Contains("jpg") || d.FirstChild.Attributes["src"].Value.Contains("png") || d.FirstChild.Attributes["src"].Value.Contains("bmp") || d.FirstChild.Attributes["src"].Value.Contains("gif")))
                   )
                   )
                    &&
                    (
                    (d.Attributes.Contains("href") && d.Attributes["href"].Value.ToUpper().Contains("/VIDEO") && (!d.Attributes["href"].Value.ToUpper().Contains("AD")) && (!(d.Attributes["href"].Value.Contains("jpg") || d.Attributes["href"].Value.Contains("png") || d.Attributes["href"].Value.Contains("bmp") || (d.FirstChild != null && d.FirstChild.Attributes.Contains("alt")))))
                    || (d.Attributes.Contains("src") && d.Attributes["src"].Value.ToUpper().Contains("/VIDEO") && (!d.Attributes["src"].Value.ToUpper().Contains("AD")) && (!(d.Attributes["src"].Value.Contains("jpg") || d.Attributes["src"].Value.Contains("png") || d.Attributes["src"].Value.Contains("bmp") || (d.FirstChild != null && d.FirstChild.Attributes.Contains("alt")))))
                        //&& (((d.Attributes["href"].Value.Contains("ads") != null && !d.Attributes["href"].Value.Contains("ads")) && (d.Attributes["src"].Value.Contains("ads") != null && !d.Attributes["src"].Value.Contains("ads"))))
                  )
                  )
                    );
                var liImageList = rootHtmlDocument.DocumentNode.Descendants("img").Where(d =>
                    (
                    (d.Attributes.Contains("href")
                    && !d.Attributes["id"].Value.ToLower().Contains("sparklineImgId") && !d.Attributes["href"].Value.ToLower().Contains("weather") && !d.Attributes["href"].Value.ToLower().Contains("logo") && !d.Attributes["href"].Value.ToLower().Contains("toolbar") && !d.Attributes["href"].Value.ToLower().Contains("icon") && !d.Attributes["href"].Value.ToLower().Contains("footer") && !d.Attributes["href"].Value.ToLower().Contains("header")
                    && (d.Attributes["href"].Value.Contains("jpg") || d.Attributes["href"].Value.Contains("png") || d.Attributes["href"].Value.Contains("bmp")
                        //|| (d.FirstChild != null && d.FirstChild.Attributes.Contains("alt"))
                    ) && (!d.Attributes["href"].Value.ToUpper().Contains("AD")) && (!d.Attributes["href"].Value.ToUpper().Contains("/VIDEO")))
                    || (d.Attributes.Contains("src") && !d.Attributes["src"].Value.ToLower().Contains("weather") && !d.Attributes["src"].Value.ToLower().Contains("logo") && !d.Attributes["src"].Value.ToLower().Contains("toolbar") && !d.Attributes["src"].Value.ToLower().Contains("icon") && !d.Attributes["src"].Value.ToLower().Contains("footer") && !d.Attributes["src"].Value.ToLower().Contains("header")
                    && (d.Attributes["src"].Value.Contains("jpg") || d.Attributes["src"].Value.Contains("png") || d.Attributes["src"].Value.Contains("bmp")
                        // || (d.FirstChild != null && d.FirstChild.Attributes.Contains("alt"))
                    ) && (!d.Attributes["src"].Value.ToUpper().Contains("AD")) && (!d.Attributes["src"].Value.ToUpper().Contains("/VIDEO")))
                        //&& (((d.Attributes["href"].Value.Contains("ads")!=null &&!d.Attributes["href"].Value.Contains("ads")) && (d.Attributes["src"].Value.Contains("ads")!=null && !d.Attributes["src"].Value.Contains("ads"))))
                    )

                    &&

                      !(
                    (
                    (d.FirstChild != null) &&
                    (
                    (d.FirstChild.Attributes.Contains("href") && (d.FirstChild.Attributes["href"].Value.Contains("jpg") || d.FirstChild.Attributes["href"].Value.Contains("png") || d.FirstChild.Attributes["href"].Value.Contains("bmp") || d.FirstChild.Attributes["href"].Value.Contains("gif")))
                    || (d.FirstChild.Attributes.Contains("src") && (d.FirstChild.Attributes["src"].Value.Contains("jpg") || d.FirstChild.Attributes["src"].Value.Contains("png") || d.FirstChild.Attributes["src"].Value.Contains("bmp") || d.FirstChild.Attributes["src"].Value.Contains("gif")))
                   )
                   )
                    &&
                    (
                    (d.Attributes.Contains("href") && d.Attributes["href"].Value.ToUpper().Contains("/VIDEO") && (!d.Attributes["href"].Value.ToUpper().Contains("AD")) && (!(d.Attributes["href"].Value.Contains("jpg") || d.Attributes["href"].Value.Contains("png") || d.Attributes["href"].Value.Contains("bmp") || (d.FirstChild != null && d.FirstChild.Attributes.Contains("alt")))))
                    || (d.Attributes.Contains("src") && d.Attributes["src"].Value.ToUpper().Contains("/VIDEO") && (!d.Attributes["src"].Value.ToUpper().Contains("AD")) && (!(d.Attributes["src"].Value.Contains("jpg") || d.Attributes["src"].Value.Contains("png") || d.Attributes["src"].Value.Contains("bmp") || (d.FirstChild != null && d.FirstChild.Attributes.Contains("alt")))))
                        //&& (((d.Attributes["href"].Value.Contains("ads") != null && !d.Attributes["href"].Value.Contains("ads")) && (d.Attributes["src"].Value.Contains("ads") != null && !d.Attributes["src"].Value.Contains("ads"))))
                  )
                  )
                    );
                var videoList = rootHtmlDocument.DocumentNode.Descendants("a").Where(d =>
                   (
                    (
                    (d.FirstChild != null) &&
                    (
                    (d.FirstChild.Attributes.Contains("href") && (d.FirstChild.Attributes["href"].Value.Contains("jpg") || d.FirstChild.Attributes["href"].Value.Contains("png") || d.FirstChild.Attributes["href"].Value.Contains("bmp") || d.FirstChild.Attributes["href"].Value.Contains("gif")))
                    || (d.FirstChild.Attributes.Contains("src") && (d.FirstChild.Attributes["src"].Value.Contains("jpg") || d.FirstChild.Attributes["src"].Value.Contains("png") || d.FirstChild.Attributes["src"].Value.Contains("bmp") || d.FirstChild.Attributes["src"].Value.Contains("gif")))
                   )
                   )
                    &&
                    (
                    (d.Attributes.Contains("href") && d.Attributes["href"].Value.ToUpper().Contains("/VIDEO") && (!d.Attributes["href"].Value.ToUpper().Contains("AD")) && (!(d.Attributes["href"].Value.Contains("jpg") || d.Attributes["href"].Value.Contains("png") || d.Attributes["href"].Value.Contains("bmp") || (d.FirstChild != null && d.FirstChild.Attributes.Contains("alt")))))
                    || (d.Attributes.Contains("src") && d.Attributes["src"].Value.ToUpper().Contains("/VIDEO") && (!d.Attributes["src"].Value.ToUpper().Contains("AD")) && (!(d.Attributes["src"].Value.Contains("jpg") || d.Attributes["src"].Value.Contains("png") || d.Attributes["src"].Value.Contains("bmp") || (d.FirstChild != null && d.FirstChild.Attributes.Contains("alt")))))
                  //&& (((d.Attributes["href"].Value.Contains("ads") != null && !d.Attributes["href"].Value.Contains("ads")) && (d.Attributes["src"].Value.Contains("ads") != null && !d.Attributes["src"].Value.Contains("ads"))))
                  )
                  )
                  );//.Where(a => a.Attributes.Contains("href") && (!(a.Attributes["href"].Value.Contains("img") || (a.FirstChild != null && a.FirstChild.Attributes.Contains("alt")))));

                var AdsList = rootHtmlDocument.DocumentNode.Descendants("a").Where(d =>

                  ((d.Attributes.Contains("href") && d.Attributes["href"].Value != null && d.Attributes["href"].Value.Contains("ad")) && (!(d.Attributes["href"].Value.Contains("jpg") || d.Attributes["href"].Value.Contains("png") || d.Attributes["href"].Value.Contains("bmp") || (d.FirstChild != null && d.FirstChild.Attributes.Contains("alt")))))
                  || (d.Attributes.Contains("src") && (d.Attributes["src"].Value != null && d.Attributes["src"].Value.Contains("ad")) && (!(d.Attributes["src"].Value.Contains("jpg") || d.Attributes["src"].Value.Contains("png") || d.Attributes["src"].Value.Contains("bmp") || (d.FirstChild != null && d.FirstChild.Attributes.Contains("alt")))))
                    //&& ((!d.Attributes["href"].Value.Contains("img") && (d.FirstChild != null && d.FirstChild.Attributes.Contains("alt"))))
                 );

                foreach (HtmlNode item in imgList)
                {
                    imgListNodes.Add(item.OuterHtml.ToString());
                }
                foreach (HtmlNode item in videoList)
                {
                    if (item.FirstChild!=null)
                    {
                        VideoNodes.Add("descendent.." + item.FirstChild.OuterHtml);
                        
                    }
                    VideoNodes.Add(item.OuterHtml.ToString());
                }
                foreach (HtmlNode item in AdsList)
                {
                    AdListNodes.Add(item.OuterHtml.ToString());
                }
                r.Images = imgList != null && imgList.ToList().Count() > 0 ? imgList.Count() : 0;
                r.Videos = videoList != null && videoList.ToList().Count() > 0 ? videoList.Count() : 0;
              r.Ads = AdsList != null && AdsList.ToList().Count() > 0 ? AdsList.Count() : 0;
                r.Date = date;
                r.URL = rootRequestURI.AbsoluteUri;
                rs.Add(r);
               
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
          return ;
            
             
        }
        //private byte[] ObjectToByteArray(object obj)
        //{
        //    if (obj == null)
        //        return null;
        //    BinaryFormatter bf = new BinaryFormatter();
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        bf.Serialize(ms, obj);
        //        return ms.ToArray();
        //    }
        //}

        protected void btnLoadResult_Click(object sender, EventArgs e)
        {
            try
            {
                LoadResults();
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }
    }
}