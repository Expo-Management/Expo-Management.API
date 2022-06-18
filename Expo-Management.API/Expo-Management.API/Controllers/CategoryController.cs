using Expo_Management.API.Entities;
using Expo_Management.API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expo_Management.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        [HttpGet]
        [Route("category")]
        public async Task<IActionResult> GetCategoriesAvailable()
        {

           // var category = await categoryRepository.GetCategoriesAvailable();
            List<Category> category =
                new List<Category>
                {
                new Category{Id = 1, Description = "Robotica"},
                new Category{Id = 2, Description = "Software"},
                new Category{Id = 3, Description = "Mecatronica"},

                };

            if (category != null)
            {
                var domainCategory = new List<Category>();



                foreach (var items in category)
                {
                    domainCategory.Add(new Category()
                    {
                        Id = items.Id,
                        Description = items.Description
                    });
                }
                return Ok(domainCategory);
            }

            /* List<SelectListItem> list = new List<SelectListItem>(); //inicia la variable

                foreach (var item in category)
                {
                    list.Add(new SelectListItem
                    {
                        Value = item.Id.ToString(), //id
                        Text = item.Description.ToString() //Description
                    });
                }
            }*/

            return BadRequest("There was an error.");
        }

    }

}


