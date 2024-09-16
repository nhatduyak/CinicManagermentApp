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
                    Controller = "Patient",
                    Action = "Index", 
                    Title = "thông tin Bệnh nhân",
                    AwesomeIcon = "fas fa-user-injured"
                });
            Items.Add(new SidebarItem() { 
                    Type = SidebarItemType.NavItem,
                    Controller = "Doctor",
                    Action = "Index", 
                    Title = "Thông tin Bác sĩ",
                    AwesomeIcon = "far fa-address-card"
                });
                   Items.Add(new SidebarItem() { 
                    Type = SidebarItemType.NavItem,
                    Controller = "Prescriptions",
                    Action = "Index", 
                    Title = "Quản lý đơn thuốc",
                    AwesomeIcon = "fas fa-file-medical-alt"
                });
            Items.Add(new SidebarItem() { Type = SidebarItemType.Divider});
            Items.Add(new SidebarItem() { Type = SidebarItemType.Heading, Title = "Quản lý Thuốc"});
            Items.Add(new SidebarItem() { 
                                Type = SidebarItemType.NavItem,
                                Controller = "Medicines",
                                Action = "Index", 
                                // Area = "Database",
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
                                Controller = "Manufacture",
                                Action = "Index", 
                                Title = "Nhà sản xuất",
                                AwesomeIcon = "fas fa-shipping-fast"
                            });   
            Items.Add(new SidebarItem() { 
                                Type = SidebarItemType.NavItem,
                                Controller = "",
                                Action = "Index", 
                                Title = "Lịch sử thuốc",
                                AwesomeIcon = "fas fa-glasses"
                            });                   
                Items.Add(new SidebarItem() { Type = SidebarItemType.Divider});
                
                Items.Add(new SidebarItem() { 
                    Type = SidebarItemType.NavItem,
                    Title = "Cấu hình chung",
                    AwesomeIcon = "fas fa-sitemap",
                    collapseID = "config",
                    Items = new List<SidebarItem>() {
                        new SidebarItem() { 
                                Type = SidebarItemType.NavItem,
                                Controller = "BloodGroup",
                                Action = "Index", 
                                Title = "Nhóm máu"                        
                        },
                         new SidebarItem() { 
                                Type = SidebarItemType.NavItem,
                                Controller = "Gender",
                                Action = "Index", 
                                Title = "Giới tính"                        
                        },   
                        new SidebarItem() { 
                                Type = SidebarItemType.NavItem,
                                Controller = "ClinicInfo",
                                Action = "Index", 
                                Title = "Thông tin phòng khám"                        
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