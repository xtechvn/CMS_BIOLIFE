using Caching.RedisWorker;
using Entities.Models;
using Entities.ViewModels.Products;
using HuloToys_Service.ElasticSearch.NewEs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repositories.IRepositories;
using Utilities;
using Utilities.Contants;
using Utilities.Contants.ProductV2;
using WEB.Adavigo.CMS.Service;
using WEB.CMS.Customize;
using WEB.CMS.Models.Product;

namespace WEB.CMS.Controllers
{
    [CustomAuthorize]
    public class ProductController : Controller
    {
        private readonly ProductDetailMongoAccess _productV2DetailMongoAccess;
        private readonly ProductSpecificationMongoAccess _productSpecificationMongoAccess;
        private readonly IConfiguration _configuration;
        private readonly IGroupProductRepository _groupProductRepository;
        private readonly RedisConn _redisConn;
        private StaticAPIService _staticAPIService;
        private readonly int group_product_root = 31;
        private readonly int db_index = 9;
        public ProductController(IConfiguration configuration, RedisConn redisConn, IGroupProductRepository groupProductRepository)
        {
            _productV2DetailMongoAccess = new ProductDetailMongoAccess(configuration);
            _productSpecificationMongoAccess = new ProductSpecificationMongoAccess(configuration);
            _staticAPIService = new StaticAPIService(configuration);
            _redisConn = redisConn;
            _redisConn.Connect();
            _groupProductRepository = groupProductRepository;
            db_index = Convert.ToInt32(configuration["Redis:Database:db_search_result"]);
            _configuration = configuration;
        }
        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Detail_old(string id = "")
        {
            ViewBag.ProductId = id;
            return View();
        }
        public async Task<IActionResult> GroupProduct(int group_id = 1, int position = 0)
        {
            try
            {
                if (group_id > 0)
                    return Ok(new
                    {
                        is_success = true,
                        data = await _groupProductRepository.getCategoryByParentId(group_id),
                        position = position

                    });
            }
            catch
            {

            }
            return Ok(new
            {
                is_success = false
            });
        }
        public async Task<IActionResult> ProductListing(string keyword = "", int group_id = -1, int page_index = 1, int page_size = 10)
        {
            try
            {
                if (page_size <= 0) page_size = 10;
                if (page_index < 1) page_index = 1;
                var main_products = await _productV2DetailMongoAccess.Listing(keyword, group_id, page_index, page_size);
                return Ok(new
                {
                    is_success = true,
                    data = JsonConvert.SerializeObject(main_products),
                    subdata = JsonConvert.SerializeObject(await _productV2DetailMongoAccess.SubListing(main_products.Select(x => x._id)))
                });

            }
            catch
            {

            }
            return Ok(new
            {
                is_success = false
            });
        }
        public async Task<IActionResult> ProductSubListing(string parent_id)
        {
            try
            {
                return Ok(new
                {
                    is_success = true,
                    data = await _productV2DetailMongoAccess.SubListing(parent_id)
                });

            }
            catch
            {

            }
            return Ok(new
            {
                is_success = false
            });
        }
        public async Task<IActionResult> ProductDetail(string product_id)
        {
            try
            {
                var product = await _productV2DetailMongoAccess.GetByID(product_id);
                var group_string = "";
                if (product != null && product.group_product_id != null && product.group_product_id.Trim() != "")
                {
                    try
                    {
                        var split_value = product.group_product_id.Split(",");
                        for (int i = 0; i < split_value.Length; i++)
                        {
                            var group = await _groupProductRepository.GetById(Convert.ToInt32(split_value[i]));
                            if (group != null)
                                group_string += group.Name;
                            if (i < (split_value.Length - 1)) group_string += " > ";
                        }
                    }
                    catch { }

                }
                return Ok(new
                {
                    is_success = true,
                    data = JsonConvert.SerializeObject(await _productV2DetailMongoAccess.GetByID(product_id)),
                    product_group = group_string
                });
            }
            catch
            {

            }
            return Ok(new
            {
                is_success = false
            });
        }
        public async Task<IActionResult> Summit(ProductMongoDbSummitModel request)
        {
            var msg = "Cập nhật sản phẩm thành công";
            try
            {
                //ProductMongoDbSummitModel request = JsonConvert.DeserializeObject<ProductMongoDbSummitModel>(request_object);
                if (request == null ||
                    request.name == null || request.name.Trim() == ""
                    || request.images == null || request.images.Count <= 0
                    || request.avatar == null || request.avatar.Trim() == ""
                    )
                {
                    return Ok(new
                    {
                        is_success = false,
                        msg = "Dữ liệu sản phẩm không chính xác, vui lòng chỉnh sửa và thử lại",
                    });
                }
                string rs = "";
                var uploaded_image = new List<string>();

                //-- Add/Update product_main
                var product_main = JsonConvert.DeserializeObject<ProductMongoDbModel>(JsonConvert.SerializeObject(request));
                //-- Add / Update Sub product
                if (request.variations != null && request.variations.Count > 0)
                {
                    product_main.status = (int)ProductStatus.ACTIVE;
                    var amount_variations = request.variations.Select(x => x.amount);
                    product_main.amount_max = amount_variations.OrderByDescending(x => x).First();
                    product_main.amount_min = amount_variations.OrderBy(x => x).First();
                    product_main.quanity_of_stock = request.variations.Sum(x => x.quanity_of_stock);
                    //product_main.is_one_weight = request.is_one_weight;
                    //product_main.weight = request.weight;
                    //product_main.package_width = request.package_width;
                    //product_main.package_height = request.package_height;
                    //product_main.package_depth = request.package_depth;
                    

                }
                else
                {
                    product_main.amount_max = null;
                    product_main.amount_min = null;
                }
                product_main.parent_product_id = "";
                product_main.updated_last = DateTime.Now;
                if (product_main._id == null || product_main._id.Trim() == "")
                {

                    product_main.created_date = DateTime.Now;
                    msg = "Thêm mới sản phẩm thành công";
                    product_main.status = (int)ProductStatus.ACTIVE;
                    rs = await _productV2DetailMongoAccess.AddNewAsync(product_main);

                }
                else
                {
                    var old_product = await _productV2DetailMongoAccess.GetByID(product_main._id);
                    rs = await _productV2DetailMongoAccess.UpdateAsync(product_main);
                    await _productV2DetailMongoAccess.DeactiveByParentId(product_main._id);
                    //await _productV2DetailMongoAccess.DeleteInactiveByParentId(product_main._id);
                }

                //-- Add / Update Sub product
                if (request.variations != null && request.variations.Count > 0)
                {
                    foreach (var variation in request.variations)
                    {
                        var product_by_variations = JsonConvert.DeserializeObject<ProductMongoDbModel>(JsonConvert.SerializeObject(request));
                        product_by_variations.variation_detail = variation.variation_attributes;
                        product_by_variations.status = (int)ProductStatus.ACTIVE;
                        product_by_variations.parent_product_id = product_main._id;
                        product_by_variations.price = variation.price;
                        product_by_variations.profit = variation.profit;
                        product_by_variations.amount = variation.amount;
                        product_by_variations.quanity_of_stock = variation.quanity_of_stock;
                        product_by_variations.sku = variation.sku;
                        //product_by_variations.is_one_weight = product_main.is_one_weight;
                        //product_by_variations.weight = variation.weight;
                        //product_by_variations.package_depth = variation.package_depth;
                        //product_by_variations.package_height = variation.package_height;
                        //product_by_variations.package_width = variation.package_width;
                        product_by_variations.updated_last = DateTime.Now;
                        if (variation._id != null && variation._id != "")
                        {
                            product_by_variations._id = variation._id;
                            await _productV2DetailMongoAccess.UpdateAsync(product_by_variations);
                        }
                        else
                        {
                            await _productV2DetailMongoAccess.AddNewAsync(product_by_variations);
                        }
                    }

                }
                await _redisConn.DeleteCacheByKeyword(CacheName.PRODUCT_LISTING, db_index);
                await _redisConn.DeleteCacheByKeyword(CacheName.PRODUCT_DETAIL + product_main._id, db_index);
                if (rs != null)
                {
                    return Ok(new
                    {
                        is_success = true,
                        msg = msg,
                        data = rs
                    });
                }
                return Ok(new
                {
                    is_success = false,
                    msg = "Thêm mới / Cập nhật sản phẩm thất bại, vui lòng liên hệ bộ phận IT",
                    err=rs
                });
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Summit - ProductController: " + ex.ToString());
                return Ok(new
                {
                    is_success = false,
                    msg = "Thêm mới / Cập nhật sản phẩm thất bại, vui lòng liên hệ bộ phận IT",
                    err = ex.ToString(),
                });
            }
           
        }
        public async Task<IActionResult> SummitImages(string data_image)
        {
            try
            {
                if (
                    data_image == null || data_image.Trim() == ""
                    )
                {
                    return Ok(new
                    {
                        is_success = false,

                    });
                }
                var data_img = _staticAPIService.GetImageSrcBase64Object(data_image);
                if (data_img != null)
                {
                    var url = await _staticAPIService.UploadImageBase64(data_img);
                    return Ok(new
                    {
                        is_success = true,
                        data = url
                    });
                }

            }
            catch (Exception ex)
            {

            }
            return Ok(new
            {
                is_success = false,
            });
        }
        public async Task<IActionResult> SummitVideo(string data_video)
        {
            try
            {
                if (
                    data_video == null || data_video.Trim() == ""
                    )
                {
                    return Ok(new
                    {
                        is_success = false,

                    });
                }
                var data_img = _staticAPIService.GetVideoSrcBase64Object(data_video);
                if (data_img != null)
                {
                    var url = await _staticAPIService.UploadVideoBase64(data_img);
                    return Ok(new
                    {
                        is_success = true,
                        data = url
                    });
                }

            }
            catch (Exception ex)
            {

            }
            return Ok(new
            {
                is_success = false,
                msg = "Thêm mới / Cập nhật sản phẩm thất bại, vui lòng liên hệ bộ phận IT",
            });
        }
        public async Task<IActionResult> ProductDetailGroupProducts(string ids)
        {
            try
            {
                List<GroupProduct> groups = new List<GroupProduct>();
                foreach (var id in ids.Split(","))
                {
                    if (id != null && id.Trim() != "")
                    {
                        try
                        {
                            groups.Add( await _groupProductRepository.GetById(Convert.ToInt32(id)));
                        }
                        catch { }
                    }
                }
                return Ok(new
                {
                    is_success = true,
                    data = groups,
                });
            }
            catch
            {

            }
            return Ok(new
            {
                is_success = false
            });
        }
        public async Task<IActionResult> DeleteProductByID(string product_id)
        {
            try
            {
                if (product_id == null || product_id.Trim() == "")
                {
                    return Ok(new
                    {
                        is_success = false
                    });
                }
                await _productV2DetailMongoAccess.Delete(product_id);
                await _productV2DetailMongoAccess.DeactiveByParentId(product_id);

                return Ok(new
                {
                    is_success = true
                });
            }
            catch
            {

            }
            return Ok(new
            {
                is_success = false
            });
        }

        public async Task<IActionResult> AddProductSpecification(int type, string name)
        {
            try
            {
                if (name == null || name.Trim() == "")
                {

                }
                else
                {
                    var exists = await _productSpecificationMongoAccess.GetByNameAndType(type, name);
                    if (exists == null || exists._id == null)
                    {
                        var id = await _productSpecificationMongoAccess.AddNewAsync(new ProductSpecificationMongoDbModel()
                        {
                            attribute_name = name,
                            attribute_type = type,

                        });
                        return Ok(new
                        {
                            is_success = true,
                            data = id
                        });
                    }
                }

            }
            catch
            {

            }
            return Ok(new
            {
                is_success = false
            });
        }
        public async Task<IActionResult> GetSpecificationByName(int type, string name)
        {
            try
            {
                if (type <= 0)
                {

                }
                else
                {
                    var exists = await _productSpecificationMongoAccess.Listing(type, name);
                    return Ok(new
                    {
                        is_success = true,
                        data = exists
                    });
                }

            }
            catch
            {

            }
            return Ok(new
            {
                is_success = false
            });
        }
        public async Task<IActionResult> CopyProductByID(string product_id)
        {
            var msg = "Sao chép sản phẩm thành công";
            try
            {
                var product = await _productV2DetailMongoAccess.GetByID(product_id);
                var SubListing = await _productV2DetailMongoAccess.SubListing(product_id);

                product.created_date = DateTime.Now;
                product.updated_last = DateTime.Now;

                var rs = await _productV2DetailMongoAccess.AddNewAsync(product);

                if (rs != null)
                {
                    if (SubListing != null)
                    {
                        foreach (var item in SubListing)
                        {
                            item.parent_product_id = rs;
                            await _productV2DetailMongoAccess.AddNewAsync(item);
                        }
                    }
                    return Ok(new
                    {
                        is_success = true,
                        msg = msg,
                        data = rs
                    });
                }

            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("CopyProductByID - ProductController: " + ex.ToString());
                return Ok(new
                {
                    is_success = false,
                    msg = "sao chép sản phẩm thất bại, vui lòng liên hệ bộ phận IT",
                });
            }
            return Ok(new
            {
                is_success = false,
                msg = "sao chép sản phẩm thất bại",
            });
        }
        public async Task<IActionResult> UpdateProductStatus(string product_id, int status)
        {
            var msg = "Ẩn phẩm thành công";
            try
            {
                var product = await _productV2DetailMongoAccess.GetByID(product_id);
                product.updated_last = DateTime.Now;
                product.status = status;
                var rs = await _productV2DetailMongoAccess.UpdateAsync(product);
                if (rs != null)
                {
                    await _redisConn.DeleteCacheByKeyword(CacheName.PRODUCT_LISTING, db_index);
                    await _redisConn.DeleteCacheByKeyword(CacheName.PRODUCT_DETAIL + product._id, db_index);
                    return Ok(new
                    {
                        is_success = true,
                        msg = msg,

                    });
                }

            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("CopyProductByID - ProductController: " + ex.ToString());
                return Ok(new
                {
                    is_success = false,
                    msg = "Ẩn sản phẩm thất bại, vui lòng liên hệ bộ phận IT",
                });
            }
            return Ok(new
            {
                is_success = false,
                msg = "Ẩn sản phẩm thất bại",
            });
        }

        public async Task<IActionResult> Detail(string id = "")
        {
            ViewBag.Static = _configuration["API:StaticURL"];
            if (id == null || id.Trim() == "")
            {
                ViewBag.GroupProduct = "";
                ViewBag.Product = new ProductMongoDbModel();
                ViewBag.SubProduct = new List<ProductMongoDbModel>();
                ViewBag.ProductId = "";
                return View();

            }
            var product = await _productV2DetailMongoAccess.GetByID(id);
            if (product == null || product._id == null || product._id.Trim() == "")
            {
                ViewBag.GroupProduct = "";
                ViewBag.Product = new ProductMongoDbModel();
                ViewBag.SubProduct = new List<ProductMongoDbModel>();
                ViewBag.ProductId = "";
                return View();
            }
            var group_string = "";
            if (product != null && product.group_product_id != null && product.group_product_id.Trim() != "")
            {
                try
                {
                    var split_value = product.group_product_id.Split(",");
                    for (int i = 0; i < split_value.Length; i++)
                    {
                        var group = await _groupProductRepository.GetById(Convert.ToInt32(split_value[i]));
                        group_string += group.Name;
                        if (i < (split_value.Length - 1)) group_string += " > ";
                    }
                }
                catch { }

            }
            ViewBag.GroupProduct = group_string;
            ViewBag.Product = product;
            ViewBag.SubProduct = await _productV2DetailMongoAccess.SubListing(id);
            ViewBag.ProductId = id;

            return View();
        }
        public async Task<IActionResult> AttributesPrice(
            bool? is_one_weight, List<ProductAttributeMongoDbModel> attributes, List<ProductAttributeMongoDbModelItem> attributes_detail, List<ProductMongoDbModel> sub_product,
            string product_id = "")
        {
            ViewBag.IsOneWeight = is_one_weight;
            ViewBag.Attributes = attributes;
            ViewBag.AttributesDetail = attributes_detail;
            ViewBag.SubProduct = sub_product;
            try
            {
                var subs = sub_product;
                if (product_id != null && product_id.Trim() != "")
                {
                    var product = await _productV2DetailMongoAccess.GetByID(product_id);
                    if (product != null && product._id != null)
                    {
                        //ViewBag.IsOneWeight = product.is_one_weight;
                        ViewBag.Attributes = product.attributes;
                        ViewBag.AttributesDetail = product.attributes_detail;
                        subs.AddRange(await _productV2DetailMongoAccess.SubListing(product_id));

                    }
                }
            }
            catch
            {

            }
            return View();
        }
        public IActionResult AttributeDetail(int item_index)
        {
            ViewBag.Index = item_index;
            return View();
        }
    }

}