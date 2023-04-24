using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.Exceptions.Dtos;
using LockthreatCompliance.Sessions;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Linq.Extensions;

using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq;
using Stripe;
using LockthreatCompliance.Authorization.Roles;
using Abp.Authorization.Users;
using Microsoft.JSInterop;
using System.Runtime.InteropServices.WindowsRuntime;
using LockthreatCompliance.Exceptions.Pdf;
using Abp.Dependency;

namespace LockthreatCompliance.Exceptions.ExceptionPdf
{
 public class ExceptionPdfAppService : LockthreatComplianceAppServiceBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        //private readonly ApplicationSession _appSession;
        //private readonly IRepository<Exception> _exceptionRepository;
        //public ExceptionPdfAppService(IWebHostEnvironment webHostEnvironment, ApplicationSession appSession, IRepository<Exception> exceptionRepository)
        //{
        //    _webHostEnvironment = webHostEnvironment;
        //    _appSession = appSession;
        //    _exceptionRepository = exceptionRepository;
        //}
        private readonly ApplicationSession _appSession;
        private readonly IRepository<Exception> _exceptionRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly RoleManager _roleManager;
        
        public ExceptionPdfAppService(IWebHostEnvironment webHostEnvironment, ApplicationSession appSession, IRepository<Exception> exceptionRepository, IRepository<User, long> userRepository, RoleManager roleManager,
            IRepository<UserRole, long> userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
            _roleManager = roleManager;
            _userRepository = userRepository;
            _webHostEnvironment = webHostEnvironment;
            _appSession = appSession;
            _exceptionRepository = exceptionRepository;
        }

    

       
    }
  

}

