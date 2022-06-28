using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace ClinicManagement.Menu {
    public class AdminSidebarService
    {
        private readonly IUrlHelper UrlHelper;
        public List<SidebarItem> Items { get; set; } = new List<SidebarItem>();


        public AdminSidebarService(IUrlHelperFactory factory, IActionContextAccessor action)
        {
            UrlHelper = factory.GetUrlHelper(action.ActionContext);
            // Khoi tao cac muc sidebar

            Items.Add(new SidebarItem() { Type = SidebarItemType.Divider});
            Items.Add(new SidebarItem() { Type = SidebarItemType.Heading, Title = "Quản lý Phòng khám"});
        
            Items.Add(new SidebarItem() { 
                    Type = SidebarItemType.NavItem,
                    Controller = "DbManage",
                    Action = "Index", 
                    Area = "Database",
                    Title = "thông tin Bệnh nhân",
                    AwesomeIcon = "fas fa-user-injured"
                });
            Items.Add(new SidebarItem() { 
                    Type = SidebarItemType.NavItem,
                    Controller = "Contact",
                    Action = "Index", 
                    Area = "Contact",
                    Title = "Thông tin Bác sĩ",
                    AwesomeIcon = "far fa-address-card"
                });
                   Items.Add(new SidebarItem() { 
                    Type = SidebarItemType.NavItem,
                    Controller = "Contact",
                    Action = "Index", 
                    Area = "Contact",
                    Title = "Quản lý đơn thuốc",
                    AwesomeIcon = "fas fa-file-medical-alt"
                });
            Items.Add(new SidebarItem() { Type = SidebarItemType.Divider});
            Items.Add(new SidebarItem() { Type = SidebarItemType.Heading, Title = "Quản lý Thuốc"});
            Items.Add(new SidebarItem() { 
                                Type = SidebarItemType.NavItem,
                                Controller = "DbManage",
                                Action = "Index", 
                                Area = "Database",
                                Title = "Quản lý Thuốc",
                                AwesomeIcon = "fas fa-capsules"
                            });
             Items.Add(new SidebarItem() { 
                                Type = SidebarItemType.NavItem,
                                Controller = "Unit",
                                Action = "Index", 
                                Title = "Quản lý đơn vị",
                                AwesomeIcon = "fas fa-sort-amount-down-alt"
                            });               
            Items.Add(new SidebarItem() { 
                                Type = SidebarItemType.NavItem,
                                Controller = "Category",
                                Action = "Index", 
                                Title = "Danh mục thuốc",
                                AwesomeIcon = "fas fa-list"
                            });               
            Items.Add(new SidebarItem() { 
                                Type = SidebarItemType.NavItem,
                                Controller = "DbManage",
                                Action = "Index", 
                                Area = "Database",
                                Title = "Nhà sản xuất",
                                AwesomeIcon = "fas fa-shipping-fast"
                            });   
            Items.Add(new SidebarItem() { 
                                Type = SidebarItemType.NavItem,
                                Controller = "DbManage",
                                Action = "Index", 
                                Area = "Database",
                                Title = "Lịch sử thuốc",
                                AwesomeIcon = "fas fa-glasses"
                            });                   
                Items.Add(new SidebarItem() { Type = SidebarItemType.Divider});
                
                Items.Add(new SidebarItem() { 
                    Type = SidebarItemType.NavItem,
                    Title = "Quản lý bài viết",
                    AwesomeIcon = "far fa-folder",
                    collapseID = "blog",
                    Items = new List<SidebarItem>() {
                        new SidebarItem() { 
                                Type = SidebarItemType.NavItem,
                                Controller = "Category",
                                Action = "Index", 
                                Area = "Blog",
                                Title = "Các chuyên mục"                        
                        },
                         new SidebarItem() { 
                                Type = SidebarItemType.NavItem,
                                Controller = "Category",
                                Action = "Create", 
                                Area = "Blog",
                                Title = "Tạo chuyên mục"                        
                        },   
                        new SidebarItem() { 
                                Type = SidebarItemType.NavItem,
                                Controller = "Post",
                                Action = "Index", 
                                Area = "Blog",
                                Title = "Các bài viết"                        
                        }, 
                        new SidebarItem() { 
                                Type = SidebarItemType.NavItem,
                                Controller = "Post",
                                Action = "Create", 
                                Area = "Blog",
                                Title = "Tạo bài viết"                        
                        },                                   
                    },
                });
                Items.Add(new SidebarItem() { Type = SidebarItemType.Divider});
                Items.Add(new SidebarItem() { 
                    Type = SidebarItemType.NavItem,
                    Title = "Phân quyền & thành viên",
                    AwesomeIcon = "far fa-folder",
                    collapseID = "role",
                    Items = new List<SidebarItem>() {
                        new SidebarItem() { 
                                Type = SidebarItemType.NavItem,
                                Controller = "Role",
                                Action = "Index", 
                                Area = "Identity",
                                Title = "Các vai trò (role)"                        
                        },
                         new SidebarItem() { 
                                Type = SidebarItemType.NavItem,
                                Controller = "Role",
                                Action = "Create", 
                                Area = "Identity",
                                Title = "Tạo role mới"                        
                        },
                        new SidebarItem() { 
                                Type = SidebarItemType.NavItem,
                                Controller = "User",
                                Action = "Index", 
                                Area = "Identity",
                                Title = "Danh sách thành viên"                        
                        },
                    },
                });


        }


        public string renderHtml()
        {
            var html = new StringBuilder();

            foreach (var item in Items)
            {
                html.Append(item.RenderHtml(UrlHelper));
            }


            return html.ToString();
        }

        public void SetActive(string Controller, string Action, string Area)
        {
            foreach (var item in Items)
            {
                if (item.Controller == Controller && item.Action == Action && item.Area == Area)
                {
                    item.IsActive =  true;
                    return;
                }
                else
                {
                    if (item.Items != null)
                    {
                        foreach (var childItem in item.Items)
                        {
                            if (childItem.Controller == Controller && childItem.Action == Action && childItem.Area == Area)
                            {
                                childItem.IsActive = true;
                                item.IsActive = true;
                                return;

                            }
                        }
                    }
                }



            }
        }


    }
}