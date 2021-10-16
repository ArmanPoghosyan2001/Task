using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using Task.Models;

namespace Task.Controllers
{
    public class CacheController : Controller
    {
        private readonly MemoryCache _cache;
        public CacheController(MemoryCache cache)
        {
            _cache = cache;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(User user)
        {
            var cacheItemPolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(60.0)
            };

            var _cacheItemName = new CacheItem("Name", user.Name);
            var _cacheItemSurName = new CacheItem("SurName", user.SurName);

            var resultofName = _cache.Add(_cacheItemName, cacheItemPolicy);
            var resultofSurName = _cache.Add(_cacheItemSurName, cacheItemPolicy);

            return RedirectToAction("index");
        }

        [HttpGet]
        public IActionResult GetData(string key)
        {
            ViewBag.key = key;
            var value = _cache.Get($"{key}");
            return View(value);
        }

        public IActionResult Delete(string key)
        {
            var result = _cache.Remove($"{key}");
            return RedirectToAction("add");
        }

        [HttpGet]
        public IActionResult Update(string parameterName)
        {
            ViewBag.parameterName = parameterName;
            var value = _cache.Get($"{parameterName}");
            return View(value);
        }

        [HttpPost]
        public IActionResult Update(string parameterName, object value)
        {
            var cacheItem = new CacheItem($"{parameterName}", value);

            var cacheItemPolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(60.0)
            };
            var result = _cache.Add(cacheItem, cacheItemPolicy);
            return RedirectToAction("getData");
        }
    }
}
