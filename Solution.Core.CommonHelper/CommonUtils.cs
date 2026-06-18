
using System.Web;

namespace Solution.Core.CommonHelper
{
    public class CommonUtils
    {
        /// <summary>
        /// 获取枚举列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="minValue"></param>
        /// <returns></returns>
        public static async Task<List<Dictionary<string, object>>> GetEnumKeyValueAsync<T>(int? minValue = null) where T : Enum
        {
            return await Task.Run(() =>
            {
                return Enum.GetValues(typeof(T))
                    .Cast<T>()
                    .Where(value => !minValue.HasValue || Convert.ToInt32(value) > minValue.Value)
                    .Select(value => new Dictionary<string, object>
                    {
                { "Key", value.ToString() },
                { "Value", Convert.ToInt32(value) }
                    })
                    .ToList();
            });
        }
        /// <summary>
        /// 反射实现两个类的对象之间相同属性的值的复制
        /// 适用于初始化新实体
        /// </summary>
        /// <typeparam name="D">返回的实体</typeparam>
        /// <typeparam name="S">数据源实体</typeparam>
        /// <param name="s">数据源实体</param>
        /// <returns>返回的新实体</returns>
        public static D Mapper<D, S>(S s)
        {
            D d = Activator.CreateInstance<D>(); //构造新实例
            try
            {
                var Types = s.GetType();//获得类型  
                var Typed = typeof(D);
                foreach (PropertyInfo sp in Types.GetProperties())//获得类型的属性字段  
                {
                    foreach (PropertyInfo dp in Typed.GetProperties())
                    {
                        if (dp.Name == sp.Name && dp.PropertyType == sp.PropertyType && dp.Name != "Error" && dp.Name != "Item")//判断属性名是否相同  
                        {
                            dp.SetValue(d, sp.GetValue(s, null), null);//获得s对象属性的值复制给d对象的属性  
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return d;
        }
        /// <summary>
        /// 反射实现两个类的对象之间相同属性的值的复制
        /// 适用于初始化新实体
        /// </summary>
        /// <typeparam name="D">返回的实体</typeparam>
        /// <typeparam name="S">数据源实体</typeparam>
        /// <param name="s">数据源实体</param>
        /// <returns>返回的新实体</returns>
        public static D Mapper<D>(object s)
        {
            D d = Activator.CreateInstance<D>(); //构造新实例
            try
            {
                var Types = s.GetType();//获得类型  
                var Typed = typeof(D);
                foreach (PropertyInfo sp in Types.GetProperties())//获得类型的属性字段  
                {
                    foreach (PropertyInfo dp in Typed.GetProperties())
                    {
                        if (dp.Name == sp.Name && dp.PropertyType == sp.PropertyType && dp.Name != "Error" && dp.Name != "Item")//判断属性名是否相同  
                        {
                            dp.SetValue(d, sp.GetValue(s, null), null);//获得s对象属性的值复制给d对象的属性  
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return d;
        }
        /// <summary>
        /// 生成哈希值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Hash(string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }
        public static List<T> ToTreeData<T>(long? id, List<T> models) where T : IToTreeModel
        {
            var dtoMap = new Dictionary<long, T>();
            var list = id.HasValue ? models.Where(x => x.Id == id.Value).ToList() : models;
            foreach (var item in list)
            {
                dtoMap.Add(item.Id, item);
            }
            List<T> result = new List<T>();
            foreach (var item in dtoMap.Values)
            {
                if (item.ParentId == 0)
                {
                    result.Add(item);
                }
                else
                {
                    if (dtoMap.ContainsKey(item.ParentId))
                    {
                        dtoMap[item.ParentId].AddChilrden(item);
                    }
                }
            }
            //foreach (var item in dtoMap.Values)
            //{
            //    item.Children.AddRange(ToTreeData(item.Id, models));
            //    cmbTreeList.Add(treeModel);
            //}
            return result;
        }
        /// <summary> 
        /// 格式化显示时间为几个月,几天前,几小时前,几分钟前,或几秒前 
        /// </summary> 
        /// <param name="dt">要格式化显示的时间</param> 
        /// <returns>几个月,几天前,几小时前,几分钟前,或几秒前</returns> 
        public static string DateStringFromNow(DateTime dt)
        {
            TimeSpan span = DateTime.Now - dt;
            if (span.TotalDays > 60)
            {
                return dt.ToShortDateString();
            }
            else if (span.TotalDays > 30)
            {
                return "1个月前";
            }
            else if (span.TotalDays > 14)
            {
                return "2周前";
            }
            else if (span.TotalDays > 7)
            {
                return "1周前";
            }
            else if (span.TotalDays > 1)
            {
                return string.Format("{0}天前", (int)Math.Floor(span.TotalDays));
            }
            else if (span.TotalHours > 1)
            {
                return string.Format("{0}小时前", (int)Math.Floor(span.TotalHours));
            }
            else if (span.TotalMinutes > 1)
            {
                return string.Format("{0}分钟前", (int)Math.Floor(span.TotalMinutes));
            }
            else if (span.TotalSeconds >= 1)
            {
                return string.Format("{0}秒前", (int)Math.Floor(span.TotalSeconds));
            }
            else
            {
                return "1秒前";
            }
        }

        private static object obj = new object();
        /// <summary>
        ///  生成单号
        /// </summary>
        public static long GenerateNumber()
        {
            lock (obj)
            {
                int rand;
                char code;
                string orderId = string.Empty;
                Random random = new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));
                for (int i = 0; i < 5; i++)
                {
                    rand = random.Next();
                    code = (char)('0' + (char)(rand % 10));
                    orderId += code.ToString();
                }
                //long timestamp = DateTime.UtcNow.Ticks;
                //return long.Parse(timestamp.ToString() + orderId);
                return long.Parse(DateTime.Now.ToString("yyyyMMddfff") + orderId);
            }
        }

        public static string GenerateRandomCode(int length)
        {
            var result = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                var r = new Random(Guid.NewGuid().GetHashCode());
                result.Append(r.Next(0, 10));
            }
            return result.ToString();
        }

        private static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        public static void CreateFile(string fileName, string dir, Stream stream)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            if (!File.Exists(dir))
            {
                FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                //将内容写入文件流
                var fileContent = StreamToBytes(stream);
                fs.Write(fileContent, 0, fileContent.Length);
                //必须关闭文件流，否则得到的文本什么内容都没有
                fs.Close();//必须关闭
            }
        }

        private string GetPhysicalPath(string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                if (fileName.StartsWith("http://") || fileName.StartsWith("https://"))
                    fileName = fileName.Substring(fileName.IndexOf("/", fileName.IndexOf("//") + 2) + 1);

                var index = fileName.LastIndexOf("@");
                if (index > 0)
                    fileName = fileName.Substring(0, index);
            }

            return GetMapPath(fileName);
        }

        public static string GetMapPath(string path)
        {
            string root = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            if (!string.IsNullOrWhiteSpace(path))
            {
                path = path.Replace("/", "\\");
                if (!path.StartsWith("\\"))
                    path = "\\" + path;
                path = path.Substring(path.IndexOf('\\') + (root.EndsWith("\\") ? 1 : 0));
            }
            return root + path;
        }

        public static List<T> RandomSort<T>(List<T> list)
        {
            var random = new System.Random();
            var newList = new List<T>();
            foreach (var item in list)
            {
                newList.Insert(random.Next(newList.Count), item);

            }
            return newList;
        }

        public static string SendMessage(string appkey, string channel, string content)
        {
            string message = "";
            try
            {
                //string appkey = "BC-4a48910f91474ecdb013c23308d4686d";
                string url = "https://rest-hz.goeasy.io/v2/pubsub/publish";
                string postDataStr = "appkey=" + appkey + "&channel=" + channel + "&content=" + content;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
                Stream myRequestStream = request.GetRequestStream();
                byte[] data = Encoding.UTF8.GetBytes(postDataStr);
                myRequestStream.Write(data, 0, data.Length);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                message = "true";
            }
            catch (Exception ex)
            {
                message = ex.Message.ToString();
            }
            return message;
        }
        /// <summary>
        /// 获取一个新的用户名
        /// </summary>
        /// <returns></returns>
        public static string GetNewUserName()
        {
            string result = "";
            result = "wx";
            Random rnd = new Random();
            string[] seeds = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            int seedlen = seeds.Length;
            result += seeds[rnd.Next(0, seedlen)];
            result += seeds[rnd.Next(0, seedlen)];
            result += seeds[rnd.Next(0, seedlen)];
            result += seeds[rnd.Next(0, seedlen)];
            result += seeds[rnd.Next(0, seedlen)];
            result += seeds[rnd.Next(0, seedlen)];
            return result;
        }
        public static string SavaImg(string icob, string baseDir,string dir, string fileName, string picSuffix)
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            var strSp = icob.Split(',');
            string fullPath = string.Empty;
            if (strSp.Length > 1)
            {
                byte[] b = Convert.FromBase64String(strSp[1]);
                Random ra = new Random();
                if (string.IsNullOrEmpty(fileName))
                {
                    if (!string.IsNullOrEmpty(picSuffix))
                    {
                        fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ra.Next(1000, 9999) + picSuffix;
                    }
                    else
                    {
                        fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ra.Next(1000, 9999) + ".png";
                    }
                    fullPath = $"{dir}/{fileName}";
                }
                else
                {
                    fullPath = $"{dir}/{fileName}{picSuffix}";
                }            
                System.IO.File.WriteAllBytes($"{baseDir}{fullPath}", b);
                return $"{fullPath}";
            }
            else
            {
                return icob;
            }
        }
        public static async Task<string> HttpGetAsync(string url, string token = "" ,string customToken="")
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    if (string.IsNullOrEmpty(customToken))
                    {
                        if (!string.IsNullOrEmpty(token))
                        {
                            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                        }
                    }
                    else
                    {
                        client.DefaultRequestHeaders.Add(customToken, token);
                    }
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException e)
                {
                    return $"Request error: {e.Message}";
                }
            }
        }
        public static async Task<string> HttpPostAsync(string url, string jsonData, string token="", string customToken = "")
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    if (string.IsNullOrEmpty(customToken))
                    {
                        if (!string.IsNullOrEmpty(token))
                        {
                            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                        }
                    }
                    else
                    {
                        client.DefaultRequestHeaders.Add(customToken, token);
                    }
                    HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException e)
                {
                    return $"Request error: {e.Message}";
                }
            }
        }
        public static async Task<string> HttpPostAsync(string url, string jsonData)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                { 
                    HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException e)
                {
                    return $"Request error: {e.Message}";
                }
            }
        }
        /// <summary>
        /// 执行HTTP POST请求。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="charset">编码字符集</param>
        /// <returns>HTTP响应</returns>
        public static string DoPost(string url, IDictionary<string, string> parameters, string charset)
        {
            HttpWebRequest req = GetWebRequest(url, "POST");
            req.ContentType = "application/x-www-form-urlencoded;charset=" + charset;
            req.Accept = "application/json";
            byte[] postData = Encoding.GetEncoding(charset).GetBytes(BuildQuery(parameters, charset));
            Stream reqStream = req.GetRequestStream();
            reqStream.Write(postData, 0, postData.Length);
            reqStream.Close();

            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
            Encoding encoding = Encoding.GetEncoding(charset);
            return GetResponseAsString(rsp, encoding);
        }
        /// <summary>
        /// 把响应流转换为文本。
        /// </summary>
        /// <param name="rsp">响应流对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>响应文本</returns>
        public static string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            StringBuilder result = new StringBuilder();
            Stream stream = null;
            StreamReader reader = null;

            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);

                // 按字符读取并写入字符串缓冲
                int ch = -1;
                while ((ch = reader.Read()) > -1)
                {
                    // 过滤结束符
                    char c = (char)ch;
                    if (c != '\0')
                    {
                        result.Append(c);
                    }
                }
            }
            finally
            {
                // 释放资源
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();
            }

            return result.ToString();
        }
        public static HttpWebRequest GetWebRequest(string url, string method)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.ServicePoint.Expect100Continue = false;
            req.Method = method;
            req.KeepAlive = true;
            req.UserAgent = "Zmop4Net";
            req.Timeout = 100000;
            return req;
        }
        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <returns>URL编码后的请求数据</returns>
        public static string BuildQuery(IDictionary<string, string> parameters, string charset)
        {
            return BuildQuery(parameters, true, charset);
        }

        public static string BuildQuery(IDictionary<string, string> parameters, bool encode, string charset)
        {
            StringBuilder postData = new StringBuilder();
            bool hasParam = false;

            IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                string name = dem.Current.Key;
                string value = dem.Current.Value;
                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                {
                    if (hasParam)
                    {
                        postData.Append("&");
                    }

                    postData.Append(name);
                    postData.Append("=");

                    if (encode)
                    {
                        value = HttpUtility.UrlEncode(value, Encoding.GetEncoding(charset));
                    }

                    postData.Append(value);
                    hasParam = true;
                }
            }

            return postData.ToString();

        }

    }

    /// <summary>
    /// 长整形转字符串
    /// </summary>
    public class IntToStringConverter : System.Text.Json.Serialization.JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
                if (Utf8Parser.TryParse(span, out int number, out int bytesConsumed) && span.Length == bytesConsumed)
                {
                    return number;
                }

                if (int.TryParse(reader.GetString(), out number))
                {
                    return number;
                }
            }

            return reader.GetInt32();
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }

    
    }



}
