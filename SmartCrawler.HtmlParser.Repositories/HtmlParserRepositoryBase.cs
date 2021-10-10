using HtmlAgilityPack;
using MongoDB.Bson;
using SmartCrawler.MongoDbEntities;
using SmartCrawler.ParseHtml.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartCrawler.HtmlParser.Repositories
{
    public class HtmlParserRepositoryBase : IHtmlParser
    {
        public List<ProductDatas> ParseHtml(ParseHtmlModel parseHtmlModel)
        {
            List<ProductDatas> productDatas = new List<ProductDatas>();

            if (!string.IsNullOrWhiteSpace(parseHtmlModel.HtmlString))
            {
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(parseHtmlModel.HtmlString);

                if (document != null)
                {
                    ProductDatas prData = new ProductDatas()
                    {
                        Id = ObjectId.GenerateNewId().ToString(),

                        StoreName = parseHtmlModel.StoreName,
                        ProductURL = parseHtmlModel.URL
                    };

                    #region Category
                    try
                    {
                        StoreHtmlMapping categoryMap = parseHtmlModel.StoreHtmlMappings.FirstOrDefault(x => x.ColumnHeader == "Category");

                        if (categoryMap != null)
                        {
                            if (categoryMap.IsAttributeCrawl)
                            {
                                prData.Category = document.DocumentNode.SelectSingleNode(categoryMap.XpathPattern).Attributes[categoryMap.AttributeName].Value;
                            }
                            else
                            {
                                prData.Category = document.DocumentNode.SelectNodes(categoryMap.XpathPattern)[0].InnerText;
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    #endregion

                    #region Brand
                    try
                    {
                        StoreHtmlMapping brandMap = parseHtmlModel.StoreHtmlMappings.FirstOrDefault(x => x.ColumnHeader == "Brand");

                        if (brandMap != null)
                        {
                            if (brandMap.IsAttributeCrawl)
                            {
                                prData.Brand = document.DocumentNode.SelectSingleNode(brandMap.XpathPattern).Attributes[brandMap.AttributeName].Value;
                            }
                            else
                            {
                                prData.Brand = document.DocumentNode.SelectNodes(brandMap.XpathPattern)[0].InnerText;
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    #endregion

                    #region SKUName
                    try
                    {
                        StoreHtmlMapping skuNameMap = parseHtmlModel.StoreHtmlMappings.FirstOrDefault(x => x.ColumnHeader == "SKUName");

                        if (skuNameMap != null)
                        {
                            if (skuNameMap.IsAttributeCrawl)
                            {
                                prData.SKUName = document.DocumentNode.SelectSingleNode(skuNameMap.XpathPattern).Attributes[skuNameMap.AttributeName].Value;
                            }
                            else
                            {
                                prData.SKUName = document.DocumentNode.SelectSingleNode(skuNameMap.XpathPattern).InnerText;
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    #endregion

                    #region ProductCode
                    try
                    {
                        StoreHtmlMapping productCodeMap = parseHtmlModel.StoreHtmlMappings.FirstOrDefault(x => x.ColumnHeader == "ProductCode");

                        if (productCodeMap != null)
                        {
                            if (productCodeMap.IsAttributeCrawl)
                            {
                                prData.ProductCode = document.DocumentNode.SelectSingleNode(productCodeMap.XpathPattern).Attributes[productCodeMap.AttributeName].Value;
                            }
                            else
                            {
                                prData.ProductCode = document.DocumentNode.SelectSingleNode(productCodeMap.XpathPattern).InnerText;
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    #endregion

                    #region Price
                    try
                    {
                        StoreHtmlMapping priceMap = parseHtmlModel.StoreHtmlMappings.FirstOrDefault(x => x.ColumnHeader == "Price");

                        if (priceMap != null)
                        {
                            if (priceMap.IsAttributeCrawl)
                            {
                                prData.Price = Convert.ToDouble(document.DocumentNode.SelectSingleNode(priceMap.XpathPattern).Attributes[priceMap.AttributeName].Value);
                            }
                            else
                            {
                                prData.Price = Convert.ToDouble(document.DocumentNode.SelectSingleNode(priceMap.XpathPattern).InnerText);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    #endregion

                    #region Discount Description
                    try
                    {
                        StoreHtmlMapping discountDescriptionMap = parseHtmlModel.StoreHtmlMappings.FirstOrDefault(x => x.ColumnHeader == "DiscountDescription");

                        if (discountDescriptionMap != null)
                        {
                            if (discountDescriptionMap.IsAttributeCrawl)
                            {
                                prData.DiscountDescription = document.DocumentNode.SelectSingleNode(discountDescriptionMap.XpathPattern).Attributes[discountDescriptionMap.AttributeName].Value;
                            }
                            else
                            {
                                prData.DiscountDescription = document.DocumentNode.SelectSingleNode(discountDescriptionMap.XpathPattern).InnerText;
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    #endregion

                    #region Product Photos
                    try
                    {
                        StoreHtmlMapping productPhotosMap = parseHtmlModel.StoreHtmlMappings.FirstOrDefault(x => x.ColumnHeader == "ProductPhotoUrls");

                        if (productPhotosMap != null)
                        {
                            List<string> productPhotoList = new List<string>();

                            if (productPhotosMap.IsAttributeCrawl)
                            {
                                HtmlNodeCollection nodes = document.DocumentNode.SelectNodes(productPhotosMap.XpathPattern);

                                if (nodes != null)
                                {
                                    foreach (HtmlNode node in nodes)
                                    {
                                        productPhotoList.Add(node.Attributes[productPhotosMap.AttributeName].Value);
                                    }
                                }
                            }
                            else
                            {

                            }

                            if (productPhotoList.Count > 0 )
                            {
                                prData.ProductPhotos = productPhotoList;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Html Parse Error: " + prData.ProductURL + " Error:" + ex.Message + " InnerException:" + ex.InnerException + " Stacktrace:" + ex.StackTrace);
                    }
                    #endregion

                    if (prData.Price > 0 && !string.IsNullOrWhiteSpace(prData.SKUName))
                    {
                        productDatas.Add(prData);
                    }
                    Console.WriteLine("Html Parse Success: " + prData.ProductURL);
                }
            }

            return productDatas.Distinct().ToList();
        }
    }
}
