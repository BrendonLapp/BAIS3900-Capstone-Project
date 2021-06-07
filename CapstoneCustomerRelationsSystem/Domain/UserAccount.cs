using System.Collections.Generic;

namespace CapstoneCustomerRelationsSystem.Domain 
{
    public class UserAccount 
    {
        public int UserAccountNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public int DCINumber { get; set; }
        public List<string> Roles { get; set; }

        public UserAccount() {

        }//end Constructor
        
    }//end class
}//end namespace