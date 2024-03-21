using BackendMusica.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BackendMusica.Services
{
    public class BaseServices : Controller
    {
        public readonly BD_LOSS_SOUNDSEntities db = new BD_LOSS_SOUNDSEntities();
        public static readonly Logger logger = LogManager.GetCurrentClassLogger();
    }
}