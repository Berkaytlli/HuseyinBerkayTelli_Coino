
using System.ComponentModel.DataAnnotations;

namespace ViewModel.Authentication
{
    public class UserEditVM
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        public string PhoneNumber { get; set; }
        public string UserImage { get; set; }
        public string Email { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }



    }
}
