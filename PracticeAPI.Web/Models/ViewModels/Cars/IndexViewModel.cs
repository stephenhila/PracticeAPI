using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticeAPI.Web.Models.ViewModels.Cars
{
    public class IndexViewModel
    {
        public IEnumerable<Car> Cars { get; set; }
    }

    public class Car
    {
        public string Name { get; set; }
    }
}