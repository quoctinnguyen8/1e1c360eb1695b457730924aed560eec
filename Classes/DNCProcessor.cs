using DNCTool.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace DNCTool.Classes
{
	public class DNCProcessor
	{
		const string DEFAULT_USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.122 Safari/537.36";
		const string BASE_URL = "http://student.nctu.edu.vn";
		const string TARGET_URL = BASE_URL + "/tra-cuu-thong-tin.html";
		const string ADDITIONAL_PARAM = "&X-Requested-With=XMLHttpRequest";

		public static async Task<string> Process(string @params, string userAgenr = DEFAULT_USER_AGENT)
		{
			// Call api để lấy dữ liệu
			HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Add("user-agent", userAgenr);
			client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
			client.BaseAddress = new Uri(TARGET_URL);
			var content = new StringContent(@params + ADDITIONAL_PARAM);
			content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

			var response = await client.PostAsync("", content);
			var responseText = await response.Content.ReadAsStringAsync();

			// Trích xuất dữ liệu cần thiết
			var ids = GetIds(responseText);
			var fullNames = GetFullNames(responseText);
			var links = GetViewScoreLinks(responseText);

			// Đóng gói vào model để convert thành json
			var resultObj = new List<ResponseDataModel>();
			for (int i = 0; i < ids.Count; i++)
			{
				resultObj.Add(new ResponseDataModel
				{
					Id = ids[i],
					FullName = HttpUtility.HtmlDecode(fullNames[i]),
					ViewScoreLink = BASE_URL + links[i]
				});
			}
			var json = JsonConvert.SerializeObject(resultObj);
			Console.WriteLine(json);
			return json;
		}

		// Trích xuất mssv
		private static List<string> GetIds(string html)
		{
			string regex = @"(?<=style=""width:50px"">\r\n)\d+(?=\s+<\/td>)";
			return GetList(html, regex);
		}

		// Trích xuất họ tên
		private static List<string> GetFullNames(string html)
		{
			string regex = @"(?<=<td valign=""middle"" style=""width:150px"">).+(?=<\/td>)";
			return GetList(html, regex);
		}

		// Trích xuất link xem điểm
		private static List<string> GetViewScoreLinks(string html)
		{
			string regex = @"(?<=href="").+(?="" lang=""tc-xemdiem"")";
			return GetList(html, regex);
		}

		private static List<string> GetList(string input, string regex)
		{
			List<string> listKetQua = Regex.Matches(input, regex)
										.Cast<Match>()
										.Select(m => m.Value)
										.ToList();
			return listKetQua;
		}
	}
}
