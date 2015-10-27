using System.Collections.Generic;
using System.Web.Mvc;
using Dao.Model;

namespace Martin.Utility
{
    public static class StaticContentHelper
    {
        public static SelectList SelectList()
        {
            var items = new List<TypeStaticContentForListBox>();
            items.Add(new TypeStaticContentForListBox
            {
                Value = (int)TypeStaticContent.About,
                Name = "About"
            });
            items.Add(new TypeStaticContentForListBox
            {
                Value = (int)TypeStaticContent.Donate,
                Name = "Donate"
            });
    
            return new SelectList(items, "Value", "Name");
        }
    }

    public class TypeStaticContentForListBox
    {
        public int Value { get; set; }

        public string Name { get; set; }
    }
}