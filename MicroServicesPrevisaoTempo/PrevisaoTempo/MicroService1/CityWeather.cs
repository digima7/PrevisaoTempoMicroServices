using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroService1
{
    [Table("CityWeather")]
    public class CityWeather
    {
        [Key]
        public string Name { get; set; }
        public string JSON { get; set; }
        public DateTime Date { get; set; }
    }
}