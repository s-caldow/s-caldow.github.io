using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Spatial;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Homepage.Models;
using Microsoft.SyndicationFeed;

namespace Homepage.NewsAggregator
{
    public class RSSReader
    {
		private List<string> xmlUrls = new List<string>() { "http://thehill.com/rss/syndicator/19110", "http://feeds.bbci.co.uk/news/video_and_audio/world/rss.xml", "http://feeds.bbci.co.uk/news/world/us_and_canada/rss.xml" };
	    public List<RssItem> articles;

		public RSSReader()
	    {
		    articles = new List<RssItem>();
		}

	    public List<RssItem> GetNewsFeed()
	    {
		   
		    foreach (string url in xmlUrls)
		    {
			    string rawXml = ReadRssXml(url);
				ProcessRssXml(rawXml);
			}
		
		    return articles.OrderBy(o => o.DatePublished).ToList();
	    }

	    public void ProcessRssXml(string fileReadout)
	    {
		    var doc = XDocument.Parse(fileReadout);
		    var nav = doc.CreateNavigator();
		    XPathNodeIterator nodes = nav.Select("/rss/channel/item");
		    foreach (XPathNavigator node in nodes)
		    {
			    var article = new RssItem();
			    article.Title = node.SelectSingleNode("title").Value;
			    article.Description = node.SelectSingleNode("description").Value;
			    article.DatePublished = Convert.ToDateTime(node.SelectSingleNode("pubDate").Value);
			    article.Url = node.SelectSingleNode("link").Value;
				articles.Add(article);
			}
	    }

	    public string ReadRssXml(string url)
	    {
		    string posts = "";
		    try
		    { 
			    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

			    if (response.StatusCode == HttpStatusCode.OK)
			    {
				    Stream receiveStream = response.GetResponseStream();
				    StreamReader readStream = null;
				    if (response.CharacterSet == null)
					    readStream = new StreamReader(receiveStream);
				    else
					    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
				    posts = readStream.ReadToEnd();
				    response.Close();
				    readStream.Close();
			    }
		    }
		    catch (NullReferenceException e)
		    {
		    }
		    return posts;
	    }

	    public IEnumerable<string> GetRssUrlsList()
	    {
			IEnumerable<string> fullList = new List<string>();



		    return fullList;
	    }

	    public List<string> GetRssUrl(string urlAddress)
	    {
		    List<string> feedUrls = new List<string>();
			
		    try
		    {
			    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
			    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

				if (response.StatusCode == HttpStatusCode.OK)
			    {
				    Stream receiveStream = response.GetResponseStream();
				    StreamReader readStream = null;
				    if (response.CharacterSet == null)
					    readStream = new StreamReader(receiveStream);
				    else
					    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
				    string data = readStream.ReadToEnd();
				    response.Close();
				    readStream.Close();

				    System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(data,
					    "<link rel=\"alternate\" type=\"application/rss\\+xml\"(.+?) href=\"(.+?)\"",
					    System.Text.RegularExpressions.RegexOptions.IgnoreCase);

				    while (match.Success)
				    {
					    feedUrls.Add(match.Groups[2].Value);
					    match = match.NextMatch();
				    }
			    }
		    }
		    catch (Exception e)
		    {
			    

		    }
		    
		    return feedUrls;
		}
    }
}
