using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BlackMesa.MyStack.Main.Models
{
    [Table("Identity_Users")]
    public class User : IdentityUser
    {

    }

}