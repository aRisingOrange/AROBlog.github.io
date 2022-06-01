//using AROBlog.Data.Models;
//using Markdig;
//using Markdig.Renderers.Normalize;
//using Markdig.Syntax;
//using Markdig.Syntax.Inlines;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace AROBlog.BLL
//{
//    public class PostProcessor
//    {
//        private readonly Article _article;
//        private readonly string _importPath;
//        private readonly string _assetsPath;
//        public PostProcessor(Article article, string importPath, string assetsPath)
//        {
//            _article = article;
//            _importPath = importPath;
//            _assetsPath = assetsPath;
//        }
//        /// <summary>
//        /// Markdown内容解析，复制图片 & 替换图片链接
//        /// </summary>
//        /// <returns></returns>
//        public string MarkdownParse()
//        {
//            if (_article.Content == null)
//            {
//                return string.Empty;
//            }

//            var document = Markdown.Parse(_article.Content);

//            foreach (var node in document.AsEnumerable())
//            {
//                if (node is not ParagraphBlock { Inline: { } } paragraphBlock) continue;
//                foreach (var inline in paragraphBlock.Inline)
//                {
//                    if (inline is not LinkInline { IsImage: true } linkInline) continue;

//                    if (linkInline.Url == null) continue;
//                    if (linkInline.Url.StartsWith("http")) continue;

//                    // 路径处理
//                    var imgPath = Path.Combine(_importPath, _article.StoragePath, linkInline.Url);
//                    var imgFilename = Path.GetFileName(linkInline.Url);
//                    var destDir = Path.Combine(_assetsPath, _article.Id.ToString());
//                    if (!Directory.Exists(destDir)) Directory.CreateDirectory(destDir);
//                    var destPath = Path.Combine(destDir, imgFilename);
//                    if (File.Exists(destPath))
//                    {
//                        // 图片重名处理
//                        var imgId = GuidUtils.GuidTo16String();
//                        imgFilename = $"{Path.GetFileNameWithoutExtension(imgFilename)}-{imgId}.{Path.GetExtension(imgFilename)}";
//                        destPath = Path.Combine(destDir, imgFilename);
//                    }

//                    // 替换图片链接
//                    linkInline.Url = imgFilename;
//                    // 复制图片
//                    File.Copy(imgPath, destPath);

//                    Console.WriteLine($"复制 {imgPath} 到 {destPath}");
//                }
//            }


//            using var writer = new StringWriter();
//            var render = new NormalizeRenderer(writer);
//            render.Render(document);
//            return writer.ToString();
//        }

//        /// <summary>
//        /// 从文章正文提取前 <paramref name="length"/> 字的梗概
//        /// </summary>
//        /// <param name="length"></param>
//        /// <returns></returns>
//        public string GetSummary(int length)
//        {
//            return _article.Content == null
//                ? string.Empty
//                : Markdown.ToPlainText(_article.Content).Limit(length);
//        }
//    }
//}
