using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TwitterTestApp.Models
{
    public class TwitterCollectionModel
    {
        public IEnumerable<TwitterViewModel> Twitts { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }
    }
}