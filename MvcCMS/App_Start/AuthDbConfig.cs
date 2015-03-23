﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MvcCMS.Data;
using MvcCMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcCMS.App_Start {
    public class AuthDbConfig {
        public static void RegisterAdmin() {
            using (var context = new CmsContext()) {
                using (var userStore = new UserStore<CmsUser>(context)) {
                    using (var userManager = new UserManager<CmsUser>(userStore)) {
                        var user = userStore.FindByNameAsync("admin").Result;

                        if (user == null) {
                            var adminUser = new CmsUser {
                                UserName = "admin",
                                Email = "admin@cms.com",
                                DisplayName = "Administrator"
                            };

                            userManager.Create(adminUser, "defender");
                        }
                    }
                }
            }
        }
    }
}