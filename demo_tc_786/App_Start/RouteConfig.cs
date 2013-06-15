using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace demo_tc_786
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                /*
                 * 這種萬用寫法通常一定要放最下面，不然會吃掉所有的路由設定，
                 * 但是我們有加上限制只有 twMVC 和 Account 這兩個 Controller 才能進來，
                 * 所以才可以放最上面
                 */
                name: "FortwMVCAndAccount",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "demo", action = "Index", id = UrlParameter.Optional }
                //讀者可以試試看將下面的條件約束拿掉，連首頁都看不到會直接被導向去 demo/index
                ,constraints: new { controller = new OnlytwmvcAndAccountConstraint() }
                );

            routes.MapRoute(
                /*
                 * 此範例將 ID 限制必須為 Guid 的格式才可進入
                 */
                name: "ForGuid",
                url: "demo/{id}",
                defaults: new { controller = "demo", action = "Index", id = UrlParameter.Optional },
                constraints: new { id = new GuidConstraint() }
                );

            routes.MapRoute(
                /*
                 * 此範例將 ID 限制必須為可以正常轉換為時間格式的才可進入
                 */
                name: "ForDateTime",
                url: "demo/{id}",
                defaults: new { controller = "demo", action = "Dt", id = UrlParameter.Optional },
                constraints: new { id = new DateTimeConstraint() }
                );



            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                );
        }

    }
    public class OnlytwmvcAndAccountConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values,
                            RouteDirection routeDirection)
        {
            string[] constraintName = new string[] { "twMVC", "Account" };
            if (values.ContainsKey(parameterName))
            {
                var stringValue = values[parameterName] as string;
                //不考慮大小寫的比對方式
                return Array.Exists(constraintName,
                                    val => val.Equals(stringValue, StringComparison.InvariantCultureIgnoreCase));
            }
            return false;
        }
    }

    public class DateTimeConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values,
                            RouteDirection routeDirection)
        {
            var dateTime = values[parameterName] as DateTime?;
            if (dateTime.HasValue == false)
            {
                var stringValue = values[parameterName] as string;
                if (string.IsNullOrWhiteSpace(stringValue) == false)
                {
                    DateTime parsedDateTime;
                    DateTime.TryParse(stringValue, out parsedDateTime);
                    dateTime = parsedDateTime;
                }
            }
            return (dateTime.HasValue && dateTime.Value != default(DateTime));
        }
    }

    public class GuidConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values,
                            RouteDirection routeDirection)
        {
            if (values.ContainsKey(parameterName))
            {
                var guid = values[parameterName] as Guid?;
                if (guid.HasValue == false)
                {
                    var stringValue = values[parameterName] as string;
                    if (string.IsNullOrWhiteSpace(stringValue) == false)
                    {
                        Guid parsedGuid;
                        // .NET 4 新增的 Guid.TryParse
                        Guid.TryParse(stringValue, out parsedGuid);
                        guid = parsedGuid;
                    }
                }
                return (guid.HasValue && guid.Value != Guid.Empty);
            }
            return false;
        }
    }
}